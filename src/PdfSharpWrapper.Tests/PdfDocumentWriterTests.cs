using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

        [TestCase(PdfSharpWrapperSetupFixture.READ_ONLY_TEXT_FIELD)]
        [TestCase(PdfSharpWrapperSetupFixture.READ_ONLY_CHECK_BOX_FIELD)]
        [TestCase(PdfSharpWrapperSetupFixture.READ_ONLY_RADIO_BUTTON_FIELD)]
        [TestCase(PdfSharpWrapperSetupFixture.READ_ONLY_COMBO_BOX_FIELD)]
        [TestCase(PdfSharpWrapperSetupFixture.READ_ONLY_LIST_BOX_FIELD)]
        public void Write_ReadOnly_LogError(string field)
        {
            // ARRANGE
            // ACT
            pdfDocumentWriter.Write(PdfSharpWrapperSetupFixture.PdfTestTemplateReadOnlyFilePath, new Dictionary<string, string>
            {
                { field, "Test Text" }
            });

            // ASSERT
            mockLogger.VerifyLogging($"'{field}' is readonly.", LogLevel.Error, Times.Once);
        }

        [Test]
        public void Write_Null_LogError()
        {
            // ARRANGE
            // ACT
            pdfDocumentWriter.Write(PdfSharpWrapperSetupFixture.PdfTestTemplateReadOnlyFilePath, new Dictionary<string, string>
            {
                { "FieldThatDoesntExist", "Test Text" }
            });

            // ASSERT
            mockLogger.VerifyLogging("Field is null for key: FieldThatDoesntExist.", LogLevel.Error, Times.Once);
        }

        [Test]
        public void Write_PdfTextField_AsExpected()
        {
        }

        [Test]
        public void Write_PdfCheckBoxField_AsExpected()
        {
        }

        [Test]
        public void Write_PdfComboBoxField_AsExpected()
        {
        }

        [Test]
        public void Write_PdfListBoxField_AsExpected()
        {
        }

        [Test]
        public void Write_PdfRadioButtonField_AsExpected()
        {
        }

        [Test]
        public void Write_PdfSignatureField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Write_PdfGenericField_AsExpected()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Write_PdfPushButtonField_AsExpected()
        {
            throw new NotImplementedException();
        }
    }
}
