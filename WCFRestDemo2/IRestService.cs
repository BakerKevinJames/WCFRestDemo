using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WCFRestDemo2
{
    [ServiceContract]
    public interface IRestService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "GetNewTicket", ResponseFormat = WebMessageFormat.Json, Method ="POST")]
        Checkin GetNewTicket();

        //2.    When exiting how long the vehicle was parked to compute fees.
        [OperationContract]
        [WebGet(UriTemplate = "TurnInTicket?id={id}", ResponseFormat = WebMessageFormat.Json)]
        Checkout TurnInTicket(string id);

        //3.    A method to compute the fees
        //    a.       0-2 hrs. – 5$
        //    b.      2-10 hrs. – 10$
        //    c.       > 10 – 15$
    }
}
