using System;
namespace PassengerAirplane
{
    public class Person
    {
        string _name;
        int _ID;

        public Person(string Name, int ID)
        {
            this.Name = Name;
            this.ID = ID;
        }

        public string Name { get => _name; set => _name = value; }
        public int ID { get => _ID; set => _ID = value; }
    }
}
