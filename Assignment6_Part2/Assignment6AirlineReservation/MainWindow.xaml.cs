using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region variables

        /// <summary>
        /// Creates and instance of the add passengers window
        /// </summary>
        wndAddPassenger wndAddPassenger;


        /// <summary>
        /// Creates and instance of the flight manager class
        /// </summary>
        clsFlightManager FlightManager;

        /// <summary>
        /// Creates and instance of the passenger manager class
        /// </summary>
        clsPassengerManager PassengerManager;

        /// <summary>
        /// Puts the Passengers name into a list
        /// </summary>
        public List<clsPassenger> lstData;

        /// <summary>
        /// Creates an instance of the clsFlight class
        /// </summary>
        clsFlight clsMyFlight;

        /// <summary>
        /// Canvas to access the canvas for to change colors
        /// </summary>
        Canvas curGUI;

        /// <summary>
        /// Checks to see if the change seat button was clicked
        /// </summary>
        private bool changeSeatMode = false;

        /// <summary>
        /// Checks to see if the add passenger button was clicked
        /// </summary>
        private bool addpassMode = false;

        #endregion variables

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                wndAddPassenger = new wndAddPassenger();
                FlightManager = new clsFlightManager();
                PassengerManager = new clsPassengerManager();

                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                //////bind to the combo box
                cbChooseFlight.ItemsSource = FlightManager.GetFlights();

                cbChoosePassenger.IsEnabled = false;

                cmdAddPassenger.IsEnabled = false;
                cmdChangeSeat.IsEnabled = false;
                cmdDeletePassenger.IsEnabled = false;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion constructor

        #region methods

        /// <summary>
        /// Action for when they select a flight from the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                clsMyFlight = (clsFlight)cbChooseFlight.SelectedItem;

                updatePassenger();
                fillPass();


                lblPassengersSeatNumber.Content = "";
                lblWarning.Content = "";
                if (cbChooseFlight.SelectedItem == cbChooseFlight.Items[0])
                {
                    CanvasA380.Visibility = Visibility.Visible;
                    Canvas767.Visibility = Visibility.Hidden;
                    curGUI = CanvasA380;

                    UpdateColors();

                    cmdChangeSeat.IsEnabled = false;
                    cmdDeletePassenger.IsEnabled = false;
                }
                else if (cbChooseFlight.SelectedItem == cbChooseFlight.Items[1])
                {
                    Canvas767.Visibility = Visibility.Visible;
                    CanvasA380.Visibility = Visibility.Hidden;
                    curGUI = Canvas767;
                    UpdateColors();

                    cmdChangeSeat.IsEnabled = false;
                    cmdDeletePassenger.IsEnabled = false;
                }


                //bind to the combo box
                cbChoosePassenger.ItemsSource = PassengerManager.GetPassengers(clsMyFlight.sFlightID);
                cbChoosePassenger.IsEnabled = true;
                cmdAddPassenger.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

               ///// <summary>
        ///// Used to change the label green when they select a seat in flight 2
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        private void SeatA1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                clsMyFlight = (clsFlight)cbChooseFlight.SelectedItem;
                clsPassenger Passenger;

                //Extract the selected Passenger object from the combo box
                //Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;
                Label MyLabel = (Label)sender;  //Get the label that the user clicked
                string sSeatNumber; //The seat number

                //Check to see if the seat backcolor is read
                if (MyLabel.Background == Brushes.Red)
                {
                    //Turn the seat green
                    MyLabel.Background = Brushes.LimeGreen;

                    //Get the seat number
                    sSeatNumber = MyLabel.Content.ToString();

                    //Loop through the items in the combo box
                    for (int i = 0; i < cbChoosePassenger.Items.Count; i++)
                    {
                        //Extract the passenger from the combo box
                        Passenger = (clsPassenger)cbChoosePassenger.Items[i];

                        //If the seat number matches then select the passenger in the combo box
                        if (sSeatNumber == Passenger.sSeat)
                        {
                            cbChoosePassenger.SelectedIndex = i;
                        }
                    }
                }
                //clsPassenger Passenger;

                ////Extract the selected Passenger object from the combo box
                Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;
                //clsMyFlight = (clsFlight)cbChooseFlight.SelectedItem;


                if (changeSeatMode == true)
                {
                    if (MyLabel.Background == Brushes.Blue && MyLabel.Background != Brushes.Red)
                    {

                        MyLabel.Background = Brushes.LimeGreen;
                    }



                    List<string> seatList = new List<string>();

                    foreach (clsPassenger p in lstData)
                    {
                        seatList.Add(p.sSeat);
                    }

                    if (!seatList.Contains(MyLabel.Content.ToString()))
                    {
                        Passenger.sSeat = MyLabel.Content.ToString();
                        PassengerManager.UpdateChangeSeat(Passenger, clsMyFlight);
                        UpdatePassSeat();

                        updatePassenger();
                        fillPass();
                        MyLabel.Background = Brushes.LimeGreen;

                        changeSeatMode = false;

                        lblWarning.Content = "";
                        cbChoosePassenger.IsEnabled = true;
                        cbChooseFlight.IsEnabled = true;
                        cmdAddPassenger.IsEnabled = true;
                        cmdChangeSeat.IsEnabled = true;
                        cmdDeletePassenger.IsEnabled = true;
                    }
                }

                if (addpassMode == true)
                {
                    PassengerManager.InsertPassenger(wndAddPassenger.txtFirstName.Text.ToString(), wndAddPassenger.txtLastName.Text.ToString(), clsMyFlight.sFlightID);

                    updatePassenger();
                    fillPass();
                    MyLabel.Background = Brushes.Green;

                    PassengerManager.updatePassengerSeat(wndAddPassenger.txtFirstName.Text.ToString(), wndAddPassenger.txtLastName.Text.ToString(), MyLabel.Content.ToString(), clsMyFlight);

                    UpdatePassSeat();
                    updatePassenger();
                    fillPass();
                    lblPassengersSeatNumber.Content = MyLabel.Content;
                    addpassMode = false;
                }

                if (addpassMode == false)
                {
                    cbChoosePassenger.IsEnabled = true;
                    cbChooseFlight.IsEnabled = true;
                    cmdAddPassenger.IsEnabled = true;
                    cmdChangeSeat.IsEnabled = true;
                    cmdDeletePassenger.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

          /// <summary>
        /// Button used to add a new passenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addpassMode = true;

                wndAddPassenger = new wndAddPassenger();
                wndAddPassenger.ShowDialog();

                wndAddPassenger.Close();
                if (wndAddPassenger.save == true)
                {

                    cbChoosePassenger.IsEnabled = false;
                    cbChooseFlight.IsEnabled = false;
                    cmdAddPassenger.IsEnabled = false;
                    cmdChangeSeat.IsEnabled = false;
                    cmdDeletePassenger.IsEnabled = false;
                    lblPassengersSeatNumber.Content = "";
                }
                else
                {
                    cbChoosePassenger.IsEnabled = true;
                    cbChooseFlight.IsEnabled = true;
                    cmdAddPassenger.IsEnabled = true;
                    cmdChangeSeat.IsEnabled = true;
                    cmdDeletePassenger.IsEnabled = true;
                }


            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        
        /// <summary>
        /// This method Updates the Passenger names inside the lstData
        /// </summary>
        public void updatePassenger()
        {
            try
            {
                lstData = PassengerManager.GetPassengers(clsMyFlight.sFlightID);
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// This method fills up the combo box of the passenger names
        /// </summary>
        public void fillPass()
        {
            try
            {
                cbChoosePassenger.ItemsSource = lstData;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This method changes the colors of the label clicks
        /// </summary>
        public void UpdateColors()
        {
            try
            {
                clsPassenger Passenger;

                //Extract the selected Passenger object from the combo box
                Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                List<string> seatList = new List<string>();

                Canvas innerGUI = null;

                if (curGUI.Name == "Canvas767")
                {
                    Flight_Title.Content = "767";
                    innerGUI = c767_Seats;
                }
                else if (curGUI.Name == "CanvasA380")
                {
                    Flight_Title2.Content = "A380";
                    innerGUI = cA380_Seats;
                }

                foreach (clsPassenger p in lstData)
                {
                    seatList.Add(p.sSeat);
                }

                foreach (var child in innerGUI.Children.OfType<Label>())
                {
                    if (seatList.Contains(child.Content.ToString()))
                    {
                        child.Background = Brushes.Red;
                    }
                    if (Passenger != null && child.Content.ToString() == Passenger.sSeat)
                    {
                        child.Background = Brushes.Green;
                    }
                    if (child.Background != Brushes.Blue && !seatList.Contains(child.Content.ToString()))
                    {
                        child.Background = Brushes.Blue;
                    }
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// This method updates the label to get which label was pressed
        /// </summary>
        private void UpdatePassSeat()
        {
            try
            {
                clsPassenger Passenger;

                //Extract the selected Passenger object from the combo box
                Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                if (Passenger != null)
                {
                    lblPassengersSeatNumber.Content = Passenger.sSeat.ToString();
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

                /// <summary>
        /// Action for when they select a passenger from the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChoosePassenger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                clsPassenger Passenger;

                //Extract the selected Passenger object from the combo box
                Passenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                lblWarning.Content = "";
                cmdChangeSeat.IsEnabled = true;
                cmdDeletePassenger.IsEnabled = true;

                UpdateColors();
                UpdatePassSeat();
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

         /// <summary>
        /// Allows the user to change their seat after being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSeat_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                changeSeatMode = true;

                if (changeSeatMode == true)
                {
                    lblWarning.Content = "Please click on a empty seat!";
                    cbChoosePassenger.IsEnabled = false;
                    cbChooseFlight.IsEnabled = false;
                    cmdAddPassenger.IsEnabled = false;
                    cmdChangeSeat.IsEnabled = false;
                    cmdDeletePassenger.IsEnabled = false;
                }
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }


        }

           /// <summary>
        /// Button to delete the passengers
        /// Pops up a message box asking if they do want to delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsFlight clsMyFlight;
                clsMyFlight = (clsFlight)cbChooseFlight.SelectedItem;

                clsPassenger clsMyPassenger;
                clsMyPassenger = (clsPassenger)cbChoosePassenger.SelectedItem;

                string messageBoxText = "Are you sure you want to delete this passenger?";
                string caption = "Warning";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                if (result == MessageBoxResult.Yes)
                {
                    PassengerManager.DeletePassenger(lblPassengersSeatNumber.Content.ToString(), clsMyFlight.sFlightID, clsMyPassenger.sPassengerID);
                    cbChoosePassenger.SelectedItem = "";

                    updatePassenger();
                    fillPass();

                    cmdDeletePassenger.IsEnabled = false;
                    cmdChangeSeat.IsEnabled = false;
                }
                else if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

           /// <summary>
        /// Handle the error.
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }


        #endregion methods
    }
}