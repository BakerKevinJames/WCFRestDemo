using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WCFRestDemo2
{
    /// <summary>
    /// This is not intended for review.  It would have many problems if used in production:
    /// 1. Not thread safe.
    /// 2. If the service is stopped and started again the state would be lost.
    /// However a proper implementation could be configured in the config file and plugged in seamlessly.
    /// </summary>
    public class DatabaseEmulation : IDatabase
    {
        //Demo only: not threadsafe
        private static readonly int totalNumberOfPlaces;
        static DatabaseEmulation()
        {
            totalNumberOfPlaces = availableParking = int.Parse(ConfigurationManager.AppSettings["NUMBER_PARKING_PLACES"]);
        }

        public const int NUMBER_PARKING_PLACES = 20;
        //Demo only: not threadsafe
        private static int availableParking = NUMBER_PARKING_PLACES;
        //Demo only: not threadsafe
        private static IList<Ticket> tickets = new List<Ticket>();

        /// <summary>
        /// Used for resetting unit tests only
        /// </summary>
        public static int AvailableParking
        {
            get
            {
                return availableParking;
            }

            set
            {
                availableParking = value;
            }
        }

        public static IList<Ticket> Tickets
        {
            get
            {
                return tickets;
            }

            set
            {
                tickets = value;
            }
        }

        public IList<Ticket> GetTickets()
        {
            return tickets;
        }

        public Ticket GetTicket(string id)
        {
            var matchingTicket = tickets.Where(t => t.ID == id).FirstOrDefault();
            if (matchingTicket == null) throw new ArgumentException("Ticket ID not found in database", "id");
            return matchingTicket;
        }

        public int GetNumberOfAvailableParkingPlaces()
        {
            return availableParking;
        }

        public void SetNumberOfAvailableParkingPlaces(int parkingPlaces)
        {
            availableParking = parkingPlaces;
        }

        public int GetTotalNumberOfSpaces()
        {
            return totalNumberOfPlaces;
        }

        public void AddTicket(Ticket ticket)
        {
            tickets.Add(ticket);
        }
    }
}