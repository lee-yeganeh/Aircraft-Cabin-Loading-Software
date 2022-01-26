using System;
namespace PassengerAirplane
{
    public class Seat
    {
        private int _seatRow;
        private char _seatCol;
        private int _seatStatus;

        public int SeatStatus
        {
            get
            {
                return _seatStatus;
            }

            set
            {
                _seatStatus = value;
            }
        }
        public int SeatRow
        {
            get
            {
                return _seatRow;
            }

            set
            {
                _seatRow = value;
            }
        }
        public char SeatCol
        {
            get
            {
                return _seatCol;
            }

            set
            {
                _seatCol = value;
            }
        }

        public Seat(int seatRow, char seatCol, int seatStatus)
        {
            this.SeatRow = seatRow;
            this.SeatCol = seatCol;
            this.SeatStatus = seatStatus;
        }



    }
}
