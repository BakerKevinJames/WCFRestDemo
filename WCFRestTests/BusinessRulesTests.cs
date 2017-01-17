using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFRestDemo2;

namespace WCFRestTests
{
    [TestClass]
    public class BusinessRulesTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseEmulation.AvailableParking = 20;
            DatabaseEmulation.Tickets.Clear();
        }

        [TestMethod]
        public void BusinessRules_GetNewTicketTest()
        {
            IBusinessRules businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            Ticket ticket = businessRules.GetNewTicket();
            Assert.IsTrue(ticket != null);
        }
        [TestMethod]
        public void BusinessRules_IsParkingAvailableTest()
        {
            IBusinessRules businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            bool result = businessRules.IsParkingAvailable();
            Assert.IsTrue(result);
            for(int i = 0; i < DatabaseEmulation.NUMBER_PARKING_PLACES; i++)
            {
                businessRules.GetNewTicket();
            }
            result = businessRules.IsParkingAvailable();
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void BusinessRules_ReleaseParkingPlaceTest()
        {
            IBusinessRules businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            bool causedException = false;
            try
            {
                businessRules.ReleaseParkingPlace();
            }
            catch (InvalidOperationException)
            {
                causedException = true;
            }
            Assert.IsTrue(causedException);

            businessRules.GetNewTicket();
            Assert.IsTrue(DatabaseEmulation.AvailableParking == DatabaseEmulation.NUMBER_PARKING_PLACES - 1);
            int result = businessRules.ReleaseParkingPlace();
            Assert.IsTrue(result == DatabaseEmulation.NUMBER_PARKING_PLACES);
        }
        [TestMethod]
        public void BusinessRules_ReserveParkingPlaceTest()
        {
            IBusinessRules businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            int result = businessRules.ReserveParkingPlace();
            Assert.IsTrue(DatabaseEmulation.AvailableParking == DatabaseEmulation.NUMBER_PARKING_PLACES - 1);
            Assert.IsTrue(DatabaseEmulation.AvailableParking == result);
        }
        [TestMethod]
        public void BusinessRules_TurnInTicketTest()
        {
            IBusinessRules businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            Ticket newTicket = businessRules.GetNewTicket();
            System.Threading.Thread.Sleep(1);
            Ticket result = businessRules.TurnInTicket(newTicket.ID);
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Checkout > result.Checkin);
        }
        [TestMethod]
        public void BusinessRules_ParkingChargeTest()
        {
            TimeSpan timeParked = TimeSpan.FromMilliseconds(1);
            IBusinessRules businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            decimal result = businessRules.ParkingCharge(timeParked);
            Assert.IsTrue(result == 5.0M);
            timeParked = TimeSpan.FromMinutes(119);
            result = businessRules.ParkingCharge(timeParked);
            Assert.IsTrue(result == 5.0M);
            timeParked = TimeSpan.FromHours(2.0);
            result = businessRules.ParkingCharge(timeParked);
            Assert.IsTrue(result == 10.0M);
            timeParked = TimeSpan.FromHours(9.9);
            result = businessRules.ParkingCharge(timeParked);
            Assert.IsTrue(result == 10.0M);
            timeParked = TimeSpan.FromHours(10);
            result = businessRules.ParkingCharge(timeParked);
            Assert.IsTrue(result == 15.0M);
            timeParked = TimeSpan.FromHours(15);
            result = businessRules.ParkingCharge(timeParked);
            Assert.IsTrue(result == 15.0M);
        }
    }
}
