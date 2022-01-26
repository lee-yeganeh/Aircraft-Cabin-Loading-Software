using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PassengerAirplane
    
{
    public class Airplane
    {
        private int rows;
        private int seatsPerRow;
        private List<Passenger> passengers;
        private Seat[,] _seatList;


        public Airplane(int rows, int seatsPerRow)
        {
            this.rows = rows;
            this.seatsPerRow = seatsPerRow;
            _seatList = new Seat[rows,seatsPerRow];
            for (int i =0; i<rows;i++)
            {
                for (int j = 0; j < seatsPerRow; j++)
                    _seatList[i,j] = new Seat(i, (char)('A' + j),0);
            }
            passengers = new List<Passenger>();
        }

        // add a new passenger or a group of passengers to the airplane.
        // Re-arranges the passengers to be banalnced.
        // Reverts to the previous state if failed.
        // Returns true if operation succeeded.
        public bool addPassenger(List<Person> people,int countOfSeats)
        {
            var copySeats = deepCloneSeats();
            var copyPassengers = deepClonePassengers();
            Passenger p = new()
            {
                People = people,
                NumPassenger = countOfSeats
            };
            passengers.Add(p);
            var seatIndex = (0, 0);
            if (countOfSeats == 1)
            {
                seatIndex = freeSeatIndex(countOfSeats);
                _seatList[seatIndex.Item1, seatIndex.Item2].SeatStatus = 1;
                p.PassengerSeat = _seatList[seatIndex.Item1, seatIndex.Item2];
            }
            else if (countOfSeats > 1)
            {
                if (countOfSeats > seatsPerRow)
                    return false;
                var temp = passengers.OrderByDescending(x => x.NumPassenger).ToList();
                resetAllStatus();
                foreach(var item in temp)
                {
                    seatIndex = freeSeatIndex(item.NumPassenger);
                    if (seatIndex.Item1 != -1 && seatIndex.Item2 != -1)
                    {
                        item.PassengerSeat = _seatList[seatIndex.Item1, seatIndex.Item2];
                        for (int i = 0; i < item.NumPassenger; i++)
                        {
                            _seatList[seatIndex.Item1,seatIndex.Item2 + i].SeatStatus = 1;
                        }
                    }
                    else
                    {
                        _seatList = copySeats;
                        passengers = copyPassengers;
                        passengers.Remove(p);
                        return false;
                    }
                }
            }

            return true;
        }
        // remove a passenger or a group of passengers to the airplane.
        // Re-arranges the passengers to be banalnced.
        public void removePassenger(Passenger p)
        {
            passengers.Remove(p);

            List<Passenger> tmp = new List<Passenger>();

            foreach (Passenger item in passengers)
            {
                tmp.Add(item);
            }

            passengers.Clear();
            resetAllStatus();
            foreach (Passenger item in tmp)
            {

                addPassenger(item.People, item.NumPassenger);
            }

        }
        // Find the location of new seat in a way that the airplane is balanced.
        // Compare 4 sides of the plane to find the balanced location.
        // Returns the row and column of the seat.
        public (int,int) freeSeatIndex(int countOfSeats)
        {


            var forwardRow = availableSeats(0, rows / 2, 0, seatsPerRow, countOfSeats);
            var aftRow = availableSeats(rows / 2, rows, 0, seatsPerRow, countOfSeats);


            int startRow, endRow;
            int startColLeft = 0;
            int endColLeft = seatsPerRow / 2;
            int startColRight = seatsPerRow / 2;
            int endColRight = seatsPerRow;

            var result=(0,0);
            var leftSeats = (0, 0, 0);
            var rightSeats = (0, 0, 0);

            if (aftRow.Item1 > forwardRow.Item1)
            {
                result = (aftRow.Item2, aftRow.Item3);
                startRow = rows / 2;
                endRow = rows;
                
            }
            else
            {
                result = (forwardRow.Item2, forwardRow.Item3);
                startRow = 0;
                endRow = rows / 2;
            }

            if (countOfSeats <= seatsPerRow / 2)
            {
                leftSeats = availableSeats(startRow, endRow, startColLeft, endColLeft, countOfSeats);
                rightSeats = availableSeats(startRow, endRow, startColRight, endColRight, countOfSeats);

                if (rightSeats.Item1 > leftSeats.Item1)
                    return (rightSeats.Item2, rightSeats.Item3);
                else
                    return (leftSeats.Item2, leftSeats.Item3);
            }
            else
                return result;
        }
        // Find available seats in speicific part of the plane.
        // Check if there are free seats for a group to be added.
        // Returns the number of free seats and the location of first acceptable free seat.
        public (int, int, int) availableSeats(int startRow, int endRow, int startCol, int endCol, int countOfSeats)
        {
            int count = 0;
            bool flag = false;
            int row = -1;
            int col = -1;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startCol; j < endCol; j++)
                {
                    if (_seatList[i, j].SeatStatus==0)
                    {
                        if (endCol - j >= countOfSeats && !flag)
                        {
                            bool flag2 = true;
                            for (int k = j; k < countOfSeats; k++)
                            {
                                if (_seatList[i, k].SeatStatus != 0)
                                    flag2 = false;
                            }
                            if (flag2)
                            {
                                row = i;
                                col = j;
                                flag = true;
                            }
                        }
                        count++;
                    }
                }
            }
            return (count, row, col);
        }
        // Make a hard copy of the list of seats.
        // Returns a list of seats.
        public Seat [,] deepCloneSeats()
        {
            Seat [,] tmp = new Seat[rows, seatsPerRow];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < seatsPerRow; j++)
                    tmp[i, j] = new Seat(i, (char)('A' + j), _seatList[i,j].SeatStatus);
            }
            return tmp;
        }
        // Make a hard copy of the list of passengers.
        // Returns a list of passenegers.
        public List<Passenger> deepClonePassengers()
        {
            List<Passenger> tmp = new List<Passenger>();
            foreach(Passenger p in passengers)
            {
                tmp.Add(new Passenger(p.People, p.NumPassenger, p.PassengerSeat));
            }
            return tmp;
        }
        // Change status of those seats that were allocated to free.
        // Keep those seats that were unavailbe by user, still unavailabe.
        public void resetAllStatus()
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < seatsPerRow; j++)
                    if(_seatList[i, j].SeatStatus!=2)
                        _seatList[i,j].SeatStatus = 0;
        }
        // Find a passenger by his/her ID.
        // Return the founded passenger.
        public Passenger searchPassenger(int ID)
        {
            foreach(Passenger item in passengers)
            {
                foreach(Person p in item.People)
                {
                    if (p.ID == ID)
                        return item;
                }
            }
            return null;
        }
        // Print status of plane and its seats.
        // Print the list of passengers.
        public override string ToString()
        {
            string str = "";
            str += $"Rows: {rows}, Seats per row: {seatsPerRow}";
            str += "\n";
            str += "  ";
            for(int i =0; i< seatsPerRow;i++)
            {
                str += (char)('A' + i);
                str += " ";
                if (i == (seatsPerRow / 2) - 1)
                    str += "  ";
            }
            str += "\n";
            for (int i=0;i<rows;i++)
            {
                str += i + 1;
                str += " ";
                for(int j=0;j<seatsPerRow;j++)
                {
                    if (_seatList[i, j].SeatStatus!=0)
                        str += "x";
                    else
                        str += "-";
                    str += " ";
                    if (j == (seatsPerRow / 2) - 1)
                        str += "  ";
                }
                str += "\n";

            }
            str += "List of Passengers:";
            str += "\n";
            foreach (Passenger p in passengers)
            {
                str += p.ToString();
            }

            return str;
        }
        // Change status of a seat based on its location.
        // Return true if the operation was succeded.
        public bool changeSeatStatus(int row, int col)
        {
            if(_seatList[row,col].SeatStatus==0)
            {
                _seatList[row, col].SeatStatus = 2;
                return true;
            }
            return false;
        }
        // Achive the status of seats.
        // Retrun the number of free and allocated seats.
        public (int,int) getSeatsStatus()
        {
            int freeSeat = 0;
            int allocatedSeat=0;
            foreach(Seat s in _seatList)
            {
                if (s.SeatStatus==0)
                    freeSeat++;
                allocatedSeat++;
            }
            return (freeSeat, allocatedSeat);
        }


    }
}
