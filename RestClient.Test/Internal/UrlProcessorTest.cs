using BrassLoon.RestClient.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RestClient.Test.Internal
{
    [TestClass]
    public class UrlProcessorTest
    {
        [TestMethod]
        public void QueryStringTest()
        {
            Uri uri = new Uri("http://example.com/query");
            Uri response = UrlProcessor.AppendQueryParameters(uri, new Dictionary<string, string> { { "p1", "v1" }, { "p2", "v2" }, { "p3", "v3" } });
            Assert.AreEqual("http://example.com/query?p1=v1&p2=v2&p3=v3", response.ToString());
        }

        [TestMethod]
        public void AppendPathTest()
        {
            Uri uri = new Uri("http://example.com/path/parent");
            Uri response = UrlProcessor.AppendPaths(uri, new List<string> { "child1", "child2" });
            Assert.AreEqual("http://example.com/path/parent/child1/child2", response.ToString());
        }
    }
}
