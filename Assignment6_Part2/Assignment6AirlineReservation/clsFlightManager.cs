using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    public class clsFlightManager
    {
        #region Attributes
        /// <summary>
        /// Create an instance of the data access class
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// Creates a list of flights
        /// </summary>
        public List<clsFlight> lstFlight { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// clsFlightManager constructor
        /// </summary>
        public clsFlightManager()
        {
            try
            {
                //just creating the list
                lstFlight = new List<clsFlight>();
            }
            catch (Exception ex)
            {
                //throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        #endregion

        #region Methods

        #region GetFlights()
        /// <summary>
        /// creates a list for the flight number + aircraft + returns it
        /// </summary>
        /// <returns></returns>
        public List<clsFlight> GetFlights()
        {
            try
            {
                //database
                db = new clsDataAccess();
                //holds sql statements
                string sSQL; 

                //return values
                int iRet = 0;   
                //holds the return values
                DataSet ds = new DataSet(); 

                //flight instance
                clsFlight Flight;

                //sql to get flight info
                sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";

                //put passengers in dataset
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                //for loop to build list
                for (int i = 0; i < iRet; i++)
                {
                    Flight = new clsFlight();
                    Flight.sFlightID = ds.Tables[0].Rows[i][0].ToString();
                    Flight.sFlightNumber = ds.Tables[0].Rows[i]["Flight_Number"].ToString();
                    Flight.sAircraft = ds.Tables[0].Rows[i]["Aircraft_Type"].ToString();

                    lstFlight.Add(Flight);
                }
                //return the list
                return lstFlight;
            }
            catch (Exception ex)
            {
                //throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        }

            #endregion

        #endregion
    }
