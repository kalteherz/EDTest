using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileReverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileReverse.Tests
{
    [TestClass()]
    public class ReverserTests
    {
        private void ReverseTest(string testName, string source, string expected)
        {
            // arrange
            File.WriteAllText(testName, source);

            // act
            new Reverser(testName).ReverseFile().Wait();
            var actual = File.ReadAllText(testName);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReverseEmptyFile()
        {
            var testName = "EmptyFile";

            // arrange
            var source = "";
            var expected = "";

            // act and assert
            ReverseTest(testName, source, expected);
        }


        [TestMethod()]
        public void Reverse1()
        {
            var testName = "Reverse1";

            // arrange
            var source = "1";
            var expected = "1";

            // act and assert
            ReverseTest(testName, source, expected);
        }


        [TestMethod()]
        public void Reverse12()
        {
            var testName = "Reverse12";

            // arrange
            var source = "12";
            var expected = "21";

            // act and assert
            ReverseTest(testName, source, expected);
        }


        [TestMethod()]
        public void Reverse12345678()
        {
            var testName = "Reverse12345678";

            // arrange
            var source = "12345678";
            var expected = "87654321";

            // act and assert
            ReverseTest(testName, source, expected);
        }


        [TestMethod()]
        public void Reverse123456789()
        {
            var testName = "Reverse123456789";

            // arrange
            var source = "123456789";
            var expected = "987654321";

            // act and assert
            ReverseTest(testName, source, expected);
        }


    }
}