using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumberToWordsApplication.Tests
{
    [TestClass]
    public class NumberToWordsTests
    {
        // Dollar Tests
        [TestMethod]
        public void Test_Dollar_OnesDigits()
        {
            Assert.AreEqual("ZERO DOLLARS", NumberToWords.Wordify("0"));

            Assert.AreEqual("ONE DOLLAR", NumberToWords.Wordify("1"));

            Assert.AreEqual("TWO DOLLARS", NumberToWords.Wordify("2"));

            Assert.AreEqual("NINE DOLLARS", NumberToWords.Wordify("9"));
        }

        [TestMethod]
        public void Test_Dollar_TeensDigits()
        {
            Assert.AreEqual("TEN DOLLARS", NumberToWords.Wordify("10"));

            Assert.AreEqual("ELEVEN DOLLARS", NumberToWords.Wordify("11"));

            Assert.AreEqual("TWELVE DOLLARS", NumberToWords.Wordify("12"));

            Assert.AreEqual("NINETEEN DOLLARS", NumberToWords.Wordify("19"));
        }

        [TestMethod]
        public void Test_Dollar_TensDigits()
        {
            Assert.AreEqual("TWENTY DOLLARS", NumberToWords.Wordify("20"));

            Assert.AreEqual("THIRTY DOLLARS", NumberToWords.Wordify("30"));

            Assert.AreEqual("FORTY DOLLARS", NumberToWords.Wordify("40"));

            Assert.AreEqual("NINETY DOLLARS", NumberToWords.Wordify("90"));
        }

        [TestMethod]
        public void Test_Dollar_Hyphens()
        {
            Assert.AreEqual("TWENTY-ONE DOLLARS", NumberToWords.Wordify("21"));

            Assert.AreEqual("TWENTY-TWO DOLLARS", NumberToWords.Wordify("22"));

            Assert.AreEqual("TWENTY-THREE DOLLARS", NumberToWords.Wordify("23"));

            Assert.AreEqual("TWENTY-NINE DOLLARS", NumberToWords.Wordify("29"));

        }

        [TestMethod]
        public void Test_Dollar_Suffixes()
        {
            Assert.AreEqual("ONE HUNDRED DOLLARS", NumberToWords.Wordify("100"));

            Assert.AreEqual("ONE THOUSAND DOLLARS", NumberToWords.Wordify("1000"));

            Assert.AreEqual("ONE MILLION DOLLARS", NumberToWords.Wordify("1000000"));

            Assert.AreEqual("ONE BILLION DOLLARS", NumberToWords.Wordify("1000000000"));

            Assert.AreEqual("ONE TRILLION DOLLARS", NumberToWords.Wordify("1000000000000"));
        }

        [TestMethod]
        public void Test_Dollar_MultipleSegmentSuffixes()
        {
            Assert.AreEqual("ONE HUNDRED AND TWENTY-THREE THOUSAND, " +
                "THREE HUNDRED AND FORTY-FIVE DOLLARS",
                NumberToWords.Wordify("123345"));

            Assert.AreEqual("ONE HUNDRED AND TWENTY-THREE BILLION, " +
                "THREE HUNDRED AND FORTY-FIVE THOUSAND DOLLARS",
                NumberToWords.Wordify("123000345000"));

            Assert.AreEqual("ONE HUNDRED AND TWENTY-THREE BILLION, " +
                "FOUR HUNDRED AND FIFTY-SIX MILLION, " +
                "SEVEN HUNDRED AND EIGHTY-NINE THOUSAND, " +
                "TWELVE DOLLARS",
                NumberToWords.Wordify("123456789012"));
        }

        [TestMethod]
        public void Test_Dollar_LeadingZeroes()
        {
            Assert.AreEqual("ONE HUNDRED AND TWENTY-THREE DOLLARS",
                NumberToWords.Wordify("000123"));
        }

        [TestMethod]
        public void Test_Dollar_Separators_Valid()
        {
            Assert.AreEqual("ONE THOUSAND DOLLARS", NumberToWords.Wordify("1,000"));

            Assert.AreEqual("ONE MILLION DOLLARS", NumberToWords.Wordify("1 000 000"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Dollar_Characters_Invalid()
        {
            NumberToWords.Wordify("5c");
        }

        [TestMethod]
        public void Test_Dollar_Length_Valid()
        {
            Assert.AreEqual("ONE HUNDRED AND TWENTY-THREE TRILLION DOLLARS",
                NumberToWords.Wordify("123000000000000"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Dollar_Length_Invalid()
        {
            NumberToWords.Wordify("1234000000000000");
        }

        [TestMethod]
        public void Test_Dollar_Bounds()
        {
            Assert.AreEqual("ZERO DOLLARS", NumberToWords.Wordify(""));

            Assert.AreEqual("NINE HUNDRED AND NINETY-NINE TRILLION, " +
                "NINE HUNDRED AND NINETY-NINE BILLION, " +
                "NINE HUNDRED AND NINETY-NINE MILLION, " +
                "NINE HUNDRED AND NINETY-NINE THOUSAND, " +
                "NINE HUNDRED AND NINETY-NINE DOLLARS",
               NumberToWords.Wordify("999999999999999"));
        }


        // Cent Tests
        [TestMethod]
        public void Test_Cent_Length_Valid()
        {
            Assert.AreEqual("ZERO DOLLARS AND ZERO CENTS",
                NumberToWords.Wordify("."));

            Assert.AreEqual("ZERO DOLLARS AND ZERO CENTS",
                NumberToWords.Wordify(".0"));

            Assert.AreEqual("ZERO DOLLARS AND ONE CENT",
                NumberToWords.Wordify(".01"));

            Assert.AreEqual("ZERO DOLLARS AND TWO CENTS",
                NumberToWords.Wordify(".02"));

            Assert.AreEqual("ZERO DOLLARS AND NINETY-NINE CENTS",
                NumberToWords.Wordify(".99"));
        }

        [TestMethod]
        public void Test_Cent_IncompleteTens()
        {
            Assert.AreEqual("ZERO DOLLARS AND TEN CENTS",
                NumberToWords.Wordify(".1"));

            Assert.AreEqual("ZERO DOLLARS AND TWENTY CENTS",
                NumberToWords.Wordify(".2"));

            Assert.AreEqual("ZERO DOLLARS AND THIRTY CENTS",
                NumberToWords.Wordify(".3"));

            Assert.AreEqual("ZERO DOLLARS AND NINETY CENTS",
                NumberToWords.Wordify(".9"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Cent_Length_Invalid()
        {
            NumberToWords.Wordify(".123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Cent_Character_Invalid()
        {
            NumberToWords.Wordify(".g");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Cent_Separator_Invalid_1()
        {
            NumberToWords.Wordify(". ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Cent_Separator_Invalid_2()
        {
            NumberToWords.Wordify(".,");
        }


        // Dollar and Cent Tests
        [TestMethod]
        public void Test_Both_Basic()
        {
            Assert.AreEqual("ONE DOLLAR AND FIFTY CENTS",
                NumberToWords.Wordify("1.50"));

            Assert.AreEqual("TEN DOLLARS AND FIFTY CENTS",
                NumberToWords.Wordify("10.50"));

            Assert.AreEqual("ONE HUNDRED DOLLARS AND FIFTY CENTS",
                NumberToWords.Wordify("100.50"));

            Assert.AreEqual("ONE THOUSAND DOLLARS AND FIFTY CENTS",
                NumberToWords.Wordify("1000.50"));

            Assert.AreEqual("ONE TRILLION DOLLARS AND FIFTY CENTS",
                NumberToWords.Wordify("1000000000000.50"));
        }

        [TestMethod]
        public void Test_Both_Upper_Bound()
        {
            Assert.AreEqual("NINE HUNDRED AND NINETY-NINE TRILLION, " +
                "NINE HUNDRED AND NINETY-NINE BILLION, " +
                "NINE HUNDRED AND NINETY-NINE MILLION, " +
                "NINE HUNDRED AND NINETY-NINE THOUSAND, " +
                "NINE HUNDRED AND NINETY-NINE DOLLARS" +
                " AND NINETY-NINE CENTS",
               NumberToWords.Wordify("999999999999999.99"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Null()
        {
            NumberToWords.Wordify(null);
        }
    }
}
