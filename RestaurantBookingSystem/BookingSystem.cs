using RestaurantBookingSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Labb_3
{
    internal class BookingSystem : IBookingSystem
    {

        public List<Booking> listOfBookings = new List<Booking>() {
            new Booking("25.11.2022", "16:00", "Anna", "1"),
            new Booking("26.11.2022", "21:00", "Peter", "5"),
            new Booking("28.11.2022", "19:00", "Karl", "3"),
            new Booking("29.11.2022", "20:00", "Martin", "2"),};

        public bool isTableFull(Booking booking)
        {
            List<Booking> Matches = listOfBookings
        .Where(b => b.Date == booking.Date && b.Time == booking.Time)
        .Where(b => b.TableNumber == booking.TableNumber).ToList();
            return Matches.Count >= 5;
        }

        public async void OpenFile()
        {
            try
            {
                if (File.Exists("bookingsData.json"))
                {
                    using FileStream fs = File.OpenRead("bookingsData.json");
                    listOfBookings = await JsonSerializer.DeserializeAsync<List<Booking>>(fs);
                }
                else
                {
                    MessageBox.Show("Sorry. We couldn't find the data");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public async void SaveFile()
        {
            using FileStream stream = File.Create("bookingsData.json");
            await JsonSerializer.SerializeAsync(stream, listOfBookings);
            await stream.DisposeAsync();
        }
    }
}
