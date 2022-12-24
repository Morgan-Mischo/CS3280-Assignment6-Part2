using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    public class clsPassengerManager
    {
        #region Attributes

        /// <summary>
        /// Instance of the data access class
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// Passenger list
        /// </summary>
        public List<clsPassenger> lstPassengers { get; set; }

        /// <summary>
        /// Holds a SQL statement
        /// </summary>
        public string sSQL;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public clsPassengerManager()
        {
            try
            {
                //make an instance of the passenger list
                lstPassengers = new List<clsPassenger>();
            }
            catch (Exception ex)
            {
                //throw an exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region Methods

        #region GetPassengers

        /// <summary>
        /// Creates the list and runs the SQL statement 
        /// </summary>
        /// <param name="sFlightID"></param>
        /// <returns></returns>
        public List<clsPassenger> GetPassengers(string sFlightID)
        {
            try
            {
                db = new clsDataAccess();

                int iRet = 0;   
                DataSet ds = new DataSet(); 
                clsPassenger Passenger; 

                //SQL statement to get passengers
                sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, Flight_ID, Seat_Number " +
                "FROM Passenger, Flight_Passenger_Link " +
                "WHERE Passenger.Passenger_ID = Flight_Passenger_Link.Passenger_ID AND " +
                "Flight_id = " + sFlightID;

                //put passengers in dataset
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                //create the list
                lstPassengers = new List<clsPassenger>();

                //create the list of passengers
                for (int i = 0; i < iRet; i++)
                {
                    Passenger = new clsPassenger();
                    Passenger.sFirstName = ds.Tables[0].Rows[i]["First_Name"].ToString();
                    Passenger.sLastName = ds.Tables[0].Rows[i]["Last_Name"].ToString();
                    Passenger.sSeat = ds.Tables[0].Rows[i]["Seat_Number"].ToString();
                    Passenger.sPassengerID = ds.Tables[0].Rows[i][0].ToString();

                    lstPassengers.Add(Passenger);
                }
                //return the list
                return lstPassengers;
            }
            catch (Exception ex)
            {
                //throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region UpdateChangeSeat

         /// <summary>
        /// Updates the new list to add all the new passengers and updates seat numbers
        /// </summary>
        /// <param name="passenger"></param>
        /// <param name="flight"></param>
        public void UpdateChangeSeat(clsPassenger passenger, clsFlight flight)
        {
            try
            {
                //SQL statement
                string sSQL = "UPDATE FLIGHT_PASSENGER_LINK SET Seat_Number =  '" + passenger.sSeat + "'" +
               "WHERE FLIGHT_ID = " + flight.sFlightID.ToString() + " And Passenger_ID = " + passenger.sPassengerID.ToString();

                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                //throw an exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region UpdatePassengerSeat

        /// <summary>
        /// Updates the passenger seat after the add passenger button click
        /// </summary>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sSeat"></param>
        /// <param name="flight"></param>
        public void updatePassengerSeat(string sFirstName, string sLastName, string sSeat, clsFlight flight)
        {
            try
            {
                //SQL statement
                string sSQL2 = "SELECT Passenger_ID FROM Passenger WHERE First_Name = '" + sFirstName + "' AND Last_Name = '" + sLastName + "'";

                //execute sql
                string sPassengerID = db.ExecuteScalarSQL(sSQL2);

                //SQL statement
                string sSQL = "UPDATE FLIGHT_PASSENGER_LINK SET Seat_Number =  '" + sSeat + "'" +
               "WHERE FLIGHT_ID = " + flight.sFlightID.ToString() + " And Passenger_ID = " + sPassengerID;

                //execute sql
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                //throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region InsertPassenger

        /// <summary>
        /// Inserts a new passenger into the database.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="flightID"></param>
        public void InsertPassenger(string firstName, string lastName, string flightID)
        {
            try
            {
                //data access instance
                db = new clsDataAccess();

                //sql statement
                sSQL = "INSERT INTO PASSENGER(First_Name, Last_Name) VALUES('" + firstName + "','" + lastName + "')";

                //perform sql statement
                db.ExecuteNonQuery(sSQL);

                //sql statement
                string sSQL2 = "SELECT Passenger_ID FROM Passenger WHERE First_Name = '" + firstName + "' AND Last_Name = '" + lastName + "'";

                //execute sql statement
                string passenger_id = db.ExecuteScalarSQL(sSQL2);

                //sql statement
                string sSQL3 = "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID) " +
                    "VALUES(" + flightID + " , " + passenger_id + ")";

                //execute sql statement
                db.ExecuteNonQuery(sSQL3);
            }
            catch (Exception ex)
            {
                //throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region DeletePassenger

        /// <summary>
        /// Deletes a passenger from the database.
        /// </summary>
        /// <param name="seatNumber"></param>
        /// <param name="sFlightID"></param>
        /// <param name="sPassengerID"></param>
        public void DeletePassenger(string seatNumber, string sFlightID, string sPassengerID)
        {
            try
            {
                //data access class
                db = new clsDataAccess();
                DataSet ds;
                int iRet = 0;
                
                //sql statement
                string sSQL = "SELECT PASSENGER.PASSENGER_ID " +
                              "FROM FLIGHT_PASSENGER_LINK, FLIGHT, PASSENGER " +
                              "WHERE FLIGHT.FLIGHT_ID = FLIGHT_PASSENGER_LINK.FLIGHT_ID AND " +
                              "FLIGHT_PASSENGER_LINK.PASSENGER_ID = PASSENGER.PASSENGER_ID AND " +
                              "FLIGHT.FLIGHT_ID = " + sFlightID.ToString() + " AND " +
                              "SEAT_NUMBER = \"" + seatNumber + "\"";

                //execute sql statement
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                //sql statement
                string sSQL2 = "DELETE FROM FLIGHT_PASSENGER_LINK " +
                    "WHERE FLIGHT_ID = " + sFlightID.ToString() +
                    " AND PASSENGER_ID = " + sPassengerID;

                //sql statement
                string sSQL3 = "DELETE FROM PASSENGER " +
                                "WHERE PASSENGER_ID = " + sPassengerID;

                //execute sql statement
                db.ExecuteNonQuery(sSQL2);
                db.ExecuteNonQuery(sSQL3);
            }
            catch (Exception ex)
            {
                //throw an exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #endregion
    }
}