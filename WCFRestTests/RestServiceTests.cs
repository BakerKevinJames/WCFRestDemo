using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFRestDemo2;

namespace WCFRestTests
{
    [TestClass]
    public class RestServiceTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseEmulation.AvailableParking = 20;
            DatabaseEmulation.Tickets.Clear();
        }


        [TestMethod]
        public void RestService_GetAvailability()
        {
            Guid id1;
            Guid id2;
            RestService restService = new RestService();
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            Checkin checkin = restService.GetNewTicket();
            Assert.IsNotNull(checkin);
            Assert.IsTrue(DatabaseEmulation.NUMBER_PARKING_PLACES - 1 == checkin.AvailablePlaces);
            Assert.IsTrue(Guid.TryParse(checkin.ID, out id1));

            restService = new RestService();
            checkin = restService.GetNewTicket();
            Assert.IsNotNull(checkin);
            Assert.IsTrue(DatabaseEmulation.NUMBER_PARKING_PLACES - 2 == checkin.AvailablePlaces);
            Assert.IsTrue(Guid.TryParse(checkin.ID, out id2));
            Assert.IsFalse(id1.ToString() == id2.ToString());
        }

        [TestMethod]
        public void RestService_GetDurationParked()
        {
            RestService restService = new RestService();
            BusinessRulesTest.BusinessRules = new BusinessRules(new DatabaseEmulation());
            Checkin checkin = restService.GetNewTicket();
            Checkout checkout = restService.TurnInTicket(checkin.ID);
            //Assert.IsTrue(checkin.ID == checkout.)
        }

        [TestMethod]
        void BusinessRules_GetNewTicketTest()
        {
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            Ticket ticket = instance2.GetNewTicket();
        }
        [TestMethod]
        void BusinessRules_IsParkingAvailableTest()
        {
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            bool result = instance2.IsParkingAvailable();
        }
        [TestMethod]
        void BusinessRules_ReleaseParkingPlaceTest()
        {
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            int result = instance2.ReleaseParkingPlace();
        }
        [TestMethod]
        void BusinessRules_ReserveParkingPlaceTest()
        {
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            int result = instance2.ReserveParkingPlace();
        }
        [TestMethod]
        void BusinessRules_TurnInTicketTest()
        {
            string id = null;
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            Ticket result = instance2.TurnInTicket(id);
        }
        [TestMethod]
        void BusinessRules_ParkingChargeTest()
        {
            TimeSpan timeParked = TimeSpan.FromMilliseconds(1);
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            decimal result = instance2.ParkingCharge(timeParked);
        }

    }
}
