using BrassLoon.RestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RestClient.Test
{
    [TestClass]
    public class RestUtilTest
    {
        [TestMethod]
        public void AppendPathTest()
        {
            string result;
            RestUtil restUtil = new RestUtil();
            result = restUtil.AppendPath("http://example.com/root/", "level1", "level2");
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(string.Equals("http://example.com:80/root/level1/level2", result, StringComparison.OrdinalIgnoreCase));

            result = restUtil.AppendPath("https://example.com/", "/path/target/", "/leaf/");
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(string.Equals("https://example.com:443/path/target/leaf/", result, StringComparison.OrdinalIgnoreCase));
        }
    }
}
