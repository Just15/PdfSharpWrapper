using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace PdfSharpWrapper.Tests
{
    [TestFixture]
    public class PdfDocumentReaderTests
    {
        private Mock<ILogger<PdfDocumentReader>> mockLogger;
        private PdfDocumentReader pdfDocumentReader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLogger = new Mock<ILogger<PdfDocumentReader>>();
            pdfDocumentReader = new PdfDocumentReader(mockLogger.Object);
        }

        [TestCase(PdfSharpWrapperSetupFixture.TEXT_FIELD, "PDF Test Text")]
        [TestCase(PdfSharpWrapperSetupFixture.EMPTY_TEXT_FIELD, null)]
        public void Read_PdfTextField_AsExpected(string field, string expected)
        {
            // ARRANGE
            // ACT
            var actual = pdfDocumentReader.Read(PdfSharpWrapperSetupFixture.PdfTestTemplateFileName)[field];

            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(PdfSharpWrapperSetupFixture.CHECK_BOX_FIELD, "True")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_CHECK_BOX_FIELD, "False")]
        public void Read_PdfCheckBoxField_AsExpected(string field, string expected)
        {
            // ARRANGE
            // ACT
            var actual = pdfDocumentReader.Read(PdfSharpWrapperSetupFixture.PdfTestTemplateFileName)[field];

            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(PdfSharpWrapperSetupFixture.RADIO_BUTTON_FIELD, "/Yes")]
        public void Read_PdfRadioButtonField_AsExpected(string field, string expected)
        {
            // ARRANGE
            // ACT
            var actual = pdfDocumentReader.Read(PdfSharpWrapperSetupFixture.PdfTestTemplateRadioSelectedYesFileName)[field];

            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Read_PdfComboBoxField_AsExpected()
        {
            // ARRANGE
            // ACT
            // ASSERT
        }

        [Test]
        public void Read_PdfListBoxField_AsExpected()
        {
            // ARRANGE
            // ACT
            // ASSERT
        }

        [Test]
        public void Read_PdfSignatureField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Read_PdfGenericField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Read_PdfPushButtonField_AsExpected()
        {
            throw new NotImplementedException();
        }
    }
}
