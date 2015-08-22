using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aperture.Parser.HTML;
using Aperture.Parser.HTML.DatesAndTimes;

namespace Aperture.Parser.Tests
{
    [TestClass]
    public class LocalDateAndTimeParserTests
    {
        [TestMethod]
        public void TestIsValidLocalDateAndTimeString()
        {
            Assert.IsTrue(
                LocalDateAndTimeParser.IsValidLocalDateAndTimeString("2001-12-31T00:00:00.000"),
                "Incorrectly handles standard time string with T separator.");
            Assert.IsTrue(
                LocalDateAndTimeParser.IsValidLocalDateAndTimeString("2001-12-31 00:00:00"),
                "Incorrectly handles standard timestring with space separator.");
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidLocalDateAndTimeString("0000-12-31T00:00:00"),
                "Incorrectly allows invalid date string.");
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidLocalDateAndTimeString("2001-12-31T24:60:60.9999"),
                "Incorrectly allows invalid time string.");
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidLocalDateAndTimeString("2001-12-31-00:00:00"),
                "Incorrectly accepts invalid date-time separator.");
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidLocalDateAndTimeString("    "),
                "Incorrectly allows only-whitespace string.");
        }

        [TestMethod]
        public void TestIsValidNormalisedLocalDateAndTimeString()
        {
            Assert.IsTrue(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00"));
            Assert.IsTrue(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T23:59:59"));
            Assert.IsTrue(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T23:59:59.3"));
            Assert.IsTrue(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T23:59:59.777"));
            // Spaces are not allowed!
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01 12:00"));
            // Unnecessary and rejected seconds part
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00:00"));
            // Seconds with unnecessary fractional part
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00:01.000"));
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00:01.00"));
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00:01.0"));
            // Seconds with unnecessary trailing 0 in fractional part
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00:01.230"));
            Assert.IsFalse(
                LocalDateAndTimeParser.IsValidNormalisedLocalDateAndTimeString("2001-01-01T12:00:01.20"));
        }

        [TestMethod]
        public void TestParseLocalDateAndTimeString()
        {
            Assert.AreEqual(
                new DateTime(2001, 01, 01, 12, 0, 1),
                LocalDateAndTimeParser.ParseLocalDateAndTimeString("2001-01-01T12:00:01"));
        }
    }
}
