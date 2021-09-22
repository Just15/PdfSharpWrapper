using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace PdfSharpWrapper.Tests
{
    [TestFixture]
    public class PdfDocumentWriterTests
    {
        private Mock<ILogger<PdfDocumentWriter>> mockLogger;
        private PdfDocumentWriter pdfDocumentWriter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLogger = new Mock<ILogger<PdfDocumentWriter>>();
            pdfDocumentWriter = new PdfDocumentWriter(mockLogger.Object);
        }

        [TestCase()]
        public void Read_PdfTextField_AsExpected(string field, string expected)
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfCheckBoxField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfComboBoxField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfListBoxField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfRadioButtonField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfSignatureField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfGenericField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [TestCase()]
        public void Read_PdfPushButtonField_AsExpected()
        {
            throw new NotImplementedException();
        }
    }
}
