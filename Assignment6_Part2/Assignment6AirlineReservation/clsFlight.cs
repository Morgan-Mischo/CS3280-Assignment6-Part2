using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    public class clsFlight
    {
        /// <summary>
        /// Grabs the Flight ID
        /// </summary>
        public string sFlightID;

        /// <summary>
        /// Grabs the Flight Number information
        /// </summary>
        public string sFlightNumber;

        /// <summary>
        /// Grabs the Aircraft information
        /// </summary>
        public string sAircraft;

        public override string ToString()
        {
                       try
            {
                return sFlightNumber + " " + sAircraft;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}