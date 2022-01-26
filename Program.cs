using System;
using System.Collections.Generic;
namespace PassengerAirplane
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please insert the number of rows: ");
                var rows = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please insert the number of seats per row: ");
                int seats = Convert.ToInt32(Console.ReadLine());
                Airplane airplane = new Airplane(rows, seats);
                Console.WriteLine($"This Airplane has {rows} rows and {seats} seats in each row.");
                Console.WriteLine("All seats are available");
                Console.WriteLine("______________________________");
                while (true)
                {
                    Console.WriteLine("If you want to add new passengers press 1");
                    Console.WriteLine("If you want to remove new passengers press 2");
                    Console.WriteLine("If you want to query all seats in the plane press 3");
                    Console.WriteLine("If you want to change status of a seat press 4");
                    Console.WriteLine("If you want to get number of free and allocated seats press 5");
                    Console.WriteLine("If you want to exit the program press 0");
                    int op = Convert.ToInt32(Console.ReadLine());
                    if (op == 1)
                    {
                        addPassenger(airplane);
                    }
                    else if (op == 2)
                    {
                        removePassenger(airplane);
                    }
                    else if (op == 3)
                    {
                        Console.WriteLine(airplane);
                    }
                    else if (op == 4)
                    {
                        changeStatus(airplane);
                    }
                    else if (op == 5)
                    {
                        getSeatsStatus(airplane);
                    }
                    else if (op == 0)
                    {
                        break;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong:(. Please try again.");
            }

        }

        static void addPassenger(Airplane airplane)
        {
            Console.WriteLine("Please enter number of passengers:");
            int numberOfSeats = Convert.ToInt32(Console.ReadLine());
            List<Person> people = new List<Person>();

            for (int i = 0; i < numberOfSeats; i++)
            {
                Console.WriteLine($"Passenger #{i+1}");
                Console.WriteLine("Please enter passenger name:");
                string name = Console.ReadLine();
                Console.WriteLine("Please enter passenger ID number:");
                int ID = Convert.ToInt32(Console.ReadLine());
                Person p = new Person(name, ID);
                people.Add(p);
            }

            bool isAdded = airplane.addPassenger(people, numberOfSeats);
            if (isAdded)
            {
                Console.WriteLine("Success! Passengers are added to the plane");
            }
            else
                Console.WriteLine("Error! Passengers are not added to the plane. Please try again.");
        }

        static void removePassenger(Airplane airplane)
        {
            Console.WriteLine("Please enter the ID of the person you want to remove:");
            int ID = Convert.ToInt32(Console.ReadLine());
            Passenger p = airplane.searchPassenger(ID);
            if (p == null)
            {
                Console.WriteLine("Error! Passenger not found. Please try again");
            }
            else
            {
                airplane.removePassenger(p);
                Console.WriteLine($"Success! {p.NumPassenger} passengers are removed from the plane");
            }
        }
        static void changeStatus(Airplane airplane)
        {
            Console.WriteLine("Please enter the row number you want to change:");
            int row = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter the seat charachter you want to cahnge:");
            char col = Char.ToUpper(Convert.ToChar(Console.ReadLine()));
            int c = col - 'A';
            bool isCahnged = airplane.changeSeatStatus(row-1, c);
            if(isCahnged)
                Console.WriteLine("Success! Seat is allocated");
            else
                Console.WriteLine("Error! Seat has been allocated before!");
        }
        static void getSeatsStatus(Airplane airplane)
        {
            var res = airplane.getSeatsStatus();
            Console.WriteLine($"There are {res.Item1} free seats and {res.Item2} allocated seats.");
        }


    }
}

