using BrassLoon.RestClient.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.Test.Internal
{
    [TestClass]
    public class UrlProcessorTest
    {
        [TestMethod]
        public void QueryStringTest()
        {
            Uri uri = new Uri("http://example.com/query");
            UrlProcessor urlProcessor = new UrlProcessor();
            Uri response = urlProcessor.AppendQueryParameters(uri, new Dictionary<string, string> { { "p1", "v1" }, { "p2", "v2" }, { "p3", "v3" } });
            Assert.AreEqual("http://example.com/query?p1=v1&p2=v2&p3=v3", response.ToString());
        }
    }
}
