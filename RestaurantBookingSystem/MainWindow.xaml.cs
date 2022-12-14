using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
using Labb_3;

namespace RestaurantBookingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        BookingSystem bookingSystem = new BookingSystem();

        /// <summary>
        /// Displays all the bookings
        /// </summary>
        public void DisplayContent()
        {
            Bookings.Items.Clear();
            foreach (Booking booking in bookingSystem.listOfBookings)
            {
                Bookings.Items.Add($"{booking.Date} {booking.Time} {booking.Name} {booking.TableNumber}");
            }
        }

        /// <summary>
        /// Makes a new booking
        /// </summary>
        private void Boka(object sender, RoutedEventArgs e)
        {
            string formatted = "";
            DateTime? selectedDate = MyDatePicker.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatted = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            Booking booking = new Booking(formatted, TimeChoice.SelectionBoxItem.ToString(), Name.Text, TableNumberChoice.SelectionBoxItem.ToString());

            try
            {
                if (MyDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please enter date");
                }
                else if (selectedDate < DateTime.Now)
                {
                    MessageBox.Show("Please enter correct date");
                }
                else if (TimeChoice.SelectedIndex == -1)
                {
                    MessageBox.Show("Please enter time");
                }
                else if (Name.Text.Trim().Length < 1)
                {
                    MessageBox.Show("Please enter the name");
                }
                else if (!CheckName(Name.Text))
                {
                    MessageBox.Show("Please enter the correct name: letters only.");
                }
                else if (TableNumberChoice.SelectedIndex == -1)
                {
                    MessageBox.Show("Please enter table number");
                }
                else if (!bookingSystem.isTableFull(booking))
                {
                    bookingSystem.listOfBookings.Add(booking);
                    TableNumberChoice.SelectedIndex = -1;
                    MyDatePicker.SelectedDate = null;
                    TimeChoice.SelectedIndex = -1;
                    Name.Text = "";
                    DisplayContent();
                }
                else if (bookingSystem.listOfBookings
                    .Where(b => b.Date == formatted && b.Time == TimeChoice.SelectionBoxItem.ToString()).ToList().Count() >= 25)
                {
                    MessageBox.Show("All seats at five tables for this timeslot are booked. Please try with another timeslot.");
                }
                else
                {
                    MessageBox.Show("This time and table are not available. Please try with another table.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        /// <summary>
        /// Shows all the bookings
        /// </summary>
        private void VisaBokningar(object sender, RoutedEventArgs e)
        {
            DisplayContent();
        }
        /// <summary>
        /// Checks the name of the person who books the table
        /// </summary>
        /// <param name="name">Name of the person</param>
        /// <returns></returns>
        private bool CheckName(string name)
        {
            Regex r = new Regex(@"^[a-zA-ZäöüßÄÖÜ]+$|^[a-zA-ZäöüßÄÖÜ]+[\s[a-zA-ZäöüßÄÖÜ]+]*$");
            if (r.IsMatch(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Cancels the selected booking
        /// </summary>
        private void Avboka(object sender, RoutedEventArgs e)
        {
            if (Bookings.SelectedItem == null)
                return;

            bookingSystem.listOfBookings.RemoveAt(Bookings.SelectedIndex);
            Bookings.Items.Remove(Bookings.SelectedItem);
        }

        /// <summary>
        /// Loads the list of bookings
        /// </summary>
        private async void OpenFile(object sender, RoutedEventArgs e)
        {
            bookingSystem.OpenFile();
            DisplayContent();
        }

        /// <summary>
        /// Saves the current list of bookings
        /// </summary>
        private async void SaveFile(object sender, RoutedEventArgs e)
        {
            try
            {
                bookingSystem.SaveFile();
                Bookings.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
