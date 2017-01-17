using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;

namespace WCFRestDemo2
{
//    Good news from Nike! 

//They would like to move forward in the interview process with you for the.Net Software Engineer.This manager likes to start the interview with a coding exercise, in which the instructions are below in blue font.
//Please complete this as soon as possible.When completed, send me the zip file or host it on GitHub or Google Drive, and send me the link.

//Avoid using DropBox as both Randstad and Nike servers tend to have too many security protocols in place to access that.
//Let me know if you have any questions. 
//Complete the below.NET coding challenge.Please have them return this no later than Monday of next week.
//Write a REST Service, define the contracts and an implementation using which one can get
//1.    Parking space availability
//2.    When exiting how long the vehicle was parked to compute fees.
//3.    A method to compute the fees
//    a.       0-2 hrs. – 5$
//    b.      2-10 hrs. – 10$
//    c.       > 10 – 15$
//Include error handling and unit testing.

//Please plan to complete in 3 hours.

    [AspNetCompatibilityRequirements
   (RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RestService : IRestService
    {
        public Checkin GetNewTicket()
        {
            var ticket = Factory.MakeInstance<IBusinessRules>("IDatabase").GetNewTicket();
            Checkin checkin = new Checkin();
            checkin.ID = ticket.ID;
            checkin.AvailablePlaces = ticket.QuantityAvailable;
            return checkin;
        }

        public Checkout TurnInTicket(string id)
        {
            Checkout checkout = new Checkout();
            var businessRules = Factory.MakeInstance<IBusinessRules>("IDatabase");
            Ticket ticket = businessRules.TurnInTicket(id);
            checkout.Duration = ticket.Checkout - ticket.Checkin;
            checkout.Charge = businessRules.ParkingCharge(checkout.Duration);
            return checkout;           
        }
    }

    [DataContract]
    public class Checkin
    {
        [DataMember]
        public string ID;
        [DataMember]
        public int AvailablePlaces;
    }

    [DataContract]
    public class Checkout
    {
        [DataMember]
        public TimeSpan Duration;
        [DataMember]
        public decimal Charge;
    }

    [DataContract]
    public class Ticket
    {
        public Ticket(string id)
        {
            ID = id;
            Checkin = DateTime.UtcNow;
        }
        [DataMember]
        public string ID;
        [DataMember]
        public DateTime Checkin;
        [DataMember]
        public DateTime Checkout;
        [DataMember]
        public int QuantityAvailable;
    }
}