using System;
using System.Collections.Generic;

namespace WCFRestDemo2
{
    public interface IDatabase
    {
        int GetNumberOfAvailableParkingPlaces();
        void SetNumberOfAvailableParkingPlaces(int parkingPlaces);
        int GetTotalNumberOfSpaces();
        Ticket GetTicket(string id);
        IList<Ticket> GetTickets();
        void AddTicket(Ticket ticket);
    }
}