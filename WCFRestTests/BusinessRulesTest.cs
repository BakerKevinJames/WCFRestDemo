using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFRestDemo2;

namespace WCFRestTests
{
    public class BusinessRulesTest : IBusinessRules
    {
        static IBusinessRules businessRules;

        public BusinessRulesTest(IDatabase database)
        {

        }

        public static IBusinessRules BusinessRules
        {
            get
            {
                return businessRules;
            }

            set
            {
                businessRules = value;
            }
        }

        public Ticket GetNewTicket()
        {
            return businessRules.GetNewTicket();
        }

        public bool IsParkingAvailable()
        {
            return businessRules.IsParkingAvailable();
        }

        public decimal ParkingCharge(TimeSpan timeParked)
        {
            return businessRules.ParkingCharge(timeParked);
        }

        public int ReleaseParkingPlace()
        {
            return businessRules.ReleaseParkingPlace();
        }

        public int ReserveParkingPlace()
        {
            return businessRules.ReserveParkingPlace();
        }

        public Ticket TurnInTicket(string id)
        {
            return businessRules.TurnInTicket(id);
        }
    }
}
