using System;

namespace WCFRestDemo2
{
    public interface IBusinessRules
    {
        Ticket GetNewTicket();
        bool IsParkingAvailable();
        int ReleaseParkingPlace();
        int ReserveParkingPlace();
        Ticket TurnInTicket(string id);
        decimal ParkingCharge(TimeSpan timeParked);
    }
}