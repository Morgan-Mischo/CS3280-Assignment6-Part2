using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    public class clsPassenger
    {
        #region Attributes
        /// <summary>
        /// String for the passenger ID
        /// </summary>
        public string sPassengerID;

        /// <summary>
        /// String for the first name
        /// </summary>
        public string sFirstName;

        /// <summary>
        /// String for the last name
        /// </summary>
        public string sLastName;

        /// <summary>
        /// String for the seat number
        /// </summary>
        public string sSeat;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for clsPassenger
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        public clsPassenger(string firstname, string lastname)
        {
            try
            {
                //set the class variables
                sFirstName = firstname;
                sLastName = lastname;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Constructor 2 for class passenger
        /// </summary>
        public clsPassenger()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Overrides tostring to properly display first and last name
        /// </summary>
        public override string ToString()
        {
            try
            {
                return sFirstName + " " + sLastName;
            }
            catch (Exception ex)
            {
                //throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion
    }
}