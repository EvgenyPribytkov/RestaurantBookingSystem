using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingSystem
{
    internal interface IBooking
    {
        string Name { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string TableNumber { get; set; }
    }
}
