using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Test
{
    public class HashHelperTest
    {
        [Test]
        public void Md5Test()
        {
            string input = "password123";
            string expectedHash = "482c811da5d5b4bc6d497ffa98491e38"; // MD5 hash of "password123"

            string actualHash = Digital.Base.Extensions.Extension.CreateMD5(input);

            Assert.AreEqual(expectedHash, actualHash);
        }
    }
}
