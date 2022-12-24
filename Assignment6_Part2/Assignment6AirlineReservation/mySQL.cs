using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    public class mySQL
    {
        public string sSQL;    //Holds an SQL statement
        private MainWindow mainWnd;

        public string FlightCbo()
        {
            string sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";

            return sSQL;
        }

        //public string PassengerCbo()
        public string PassengerCbo(string sFlightID)////////////////////////////////////////////////////I added the flight ID
        {
            mainWnd = new MainWindow();

            string sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                          "FROM Passenger, Flight_Passenger_Link FPL " +
                          "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                          "Flight_ID = " + sFlightID;

            return sSQL;
        }
    }
}