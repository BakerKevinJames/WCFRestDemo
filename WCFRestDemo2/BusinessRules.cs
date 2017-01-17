using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFRestDemo2
{
    public enum Error
    {
        NoParkingSpacesLeft,
        CantReleaseAnyMoreParkingSpaces
    }
    public class BusinessRules : IBusinessRules
    {
        IDatabase _database;
        //Note:  Do not create a parameterless constructor
        //public BusinessRules()
        //{
        //}

        public BusinessRules(IDatabase database)
        {
            _database = database;
        }

        public Ticket GetNewTicket()
        {
            if(IsParkingAvailable())
            {
                ReserveParkingPlace();
                string id = Guid.NewGuid().ToString();
                Ticket ticket = new Ticket(id);
                ticket.QuantityAvailable = _database.GetNumberOfAvailableParkingPlaces();
                _database.AddTicket(ticket);
                return ticket;
            }
            return null;
        }

        public Ticket TurnInTicket(string id)
        {
            var matchingTicket = _database.GetTickets().Where(t => t.ID == id).FirstOrDefault();
            if (matchingTicket == null) throw new ArgumentException("Ticket ID not found in database", "id");
            matchingTicket.Checkout = DateTime.UtcNow;
            return matchingTicket;
        }

        public int ReleaseParkingPlace()
        {
            if (_database.GetTotalNumberOfSpaces() == _database.GetNumberOfAvailableParkingPlaces())
            {
                throw new InvalidOperationException("Error:" + ((int)Error.CantReleaseAnyMoreParkingSpaces).ToString());
            }
            _database.SetNumberOfAvailableParkingPlaces(_database.GetNumberOfAvailableParkingPlaces() + 1);
            return _database.GetNumberOfAvailableParkingPlaces();
        }

        public int ReserveParkingPlace()
        {
            if (IsParkingAvailable() == false) throw new InvalidOperationException("Error:" + ((int)Error.NoParkingSpacesLeft).ToString());
            _database.SetNumberOfAvailableParkingPlaces(_database.GetNumberOfAvailableParkingPlaces() - 1);
            return _database.GetNumberOfAvailableParkingPlaces();
        }

        public bool IsParkingAvailable()
        {
            return _database.GetNumberOfAvailableParkingPlaces() > 0;
        }

        //2.    When exiting how long the vehicle was parked to compute fees.
        //3.    A method to compute the fees
        //    a.       0-2 hrs. – 5$
        //    b.      2-10 hrs. – 10$
        //    c.       > 10 – 15$

        public decimal ParkingCharge(TimeSpan timeParked)
        {
            decimal charge = 0.0M;
            if(timeParked < TimeSpan.FromHours(2.0))
            {
                charge = 5.0M;
            } else if(timeParked < TimeSpan.FromHours(10.0))
            {
                charge = 10.0M;
            }
            else
            {
                charge = 15.0M;
            }
            return charge;
        }
    }
}