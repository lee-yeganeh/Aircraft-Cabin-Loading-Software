using System;
using System.Collections.Generic;
namespace PassengerAirplane
{
    public class Passenger
    {
        private List<Person> _people;
        private int _numPassenger;
        private Seat _passengerSeat;


        public Passenger()
        {
        }

        public Passenger(List<Person> people, int numPassenger, Seat passengerSeat)
        {
            _people = people;
            _numPassenger = numPassenger;
            _passengerSeat = passengerSeat;
        }

        public int NumPassenger { get => _numPassenger; set => _numPassenger = value; }
        public Seat PassengerSeat { get => _passengerSeat; set => _passengerSeat = value; }
        public List<Person> People { get => _people; set => _people = value; }

        public override string ToString()
        {
            string str = "";
            for(int i=0;i<People.Count;i++)
            {
                str += $"{People[i].Name}, { People[i].ID}, {PassengerSeat.SeatRow+1}-{(char)(PassengerSeat.SeatCol +i)}";
                str += "\n";
            }
            return str;

        }
    }
}
