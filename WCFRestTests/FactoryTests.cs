using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFRestDemo2;

namespace WCFRestTests
{
    [TestClass]
    public class FactoryTests
    {
        [TestMethod]
        public void Factory_MakeInstanceTest()
        {
            //depends on App.config containing the instance class name
            IDatabase instance1 = Factory.MakeInstance<IDatabase>();
            Assert.IsNotNull(instance1);
            IBusinessRules instance2 = Factory.MakeInstance<IBusinessRules>("IDatabase");
            Assert.IsNotNull(instance2);
        }
    }
}
