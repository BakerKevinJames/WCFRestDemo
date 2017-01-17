using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFRestDemo2;

namespace WCFRestTests
{
    public class DatabaseTest : IDatabase
    {
        private static IDatabase database;

        public static IDatabase Database
        {
            get
            {
                return database;
            }

            set
            {
                database = value;
            }
        }

        public void AddTicket(Ticket ticket)
        {
            database.AddTicket(ticket);
        }

        public int GetNumberOfAvailableParkingPlaces()
        {
            return database.GetNumberOfAvailableParkingPlaces();
        }

        public Ticket GetTicket(string id)
        {
            return database.GetTicket(id);
        }

        public IList<Ticket> GetTickets()
        {
            return database.GetTickets();
        }

        public int GetTotalNumberOfSpaces()
        {
            return database.GetTotalNumberOfSpaces();
        }

        public void SetNumberOfAvailableParkingPlaces(int parkingPlaces)
        {
            database.SetNumberOfAvailableParkingPlaces(parkingPlaces);
        }
    }
}
