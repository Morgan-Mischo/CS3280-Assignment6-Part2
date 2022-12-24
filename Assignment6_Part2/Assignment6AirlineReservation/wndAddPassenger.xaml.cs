using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for wndAddPassenger.xaml
    /// </summary>
    public partial class wndAddPassenger : Window
    {
        #region Attributes

        /// <summary>
        /// Save button boolean
        /// </summary>
        public bool save = false;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public wndAddPassenger()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                //throw an exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion Constructor

        #region Methods

        #region TxtLetterInput

        /// <summary>
        /// Only allows letters to be input
        /// </summary>
        /// <param name="sender">sent object</param>
        /// <param name="e">key argument</param>
        private void txtLetterInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //only allow letters to be entered
                if (!(e.Key >= Key.A && e.Key <= Key.Z))
                {
                    //allow the user to use the backspace, delete, tab and enter
                    if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter))
                    {
                        //no other keys allowed besides numbers, backspace, delete, tab, and enter
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //throw an error
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion TxtLetterInput

        #region Save Click

        /// <summary>
        /// Action for when the save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                save = true;
                this.Close();
            }
            catch (Exception ex)
            {
                //throw an error
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion Save Click

        #region HandleError

        /// <summary>
        /// Exception handler that shows the error
        /// </summary>
        /// <param name="sClass">the class</param>
        /// <param name="sMethod">the method</param>
        /// <param name="sMessage">the error message</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }

        #endregion HandleError

        #endregion Methods
    }
}