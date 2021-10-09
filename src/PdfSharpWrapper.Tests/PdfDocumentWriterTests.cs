using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace PdfSharpWrapper.Tests
{
    [TestFixture]
    public class PdfDocumentWriterTests
    {
        private Mock<ILogger<PdfDocumentWriter>> mockLogger;
        private PdfDocumentWriter pdfDocumentWriter;
        private PdfDocumentReader pdfDocumentReader;

        private static readonly string testFileName = $"Test-{PdfSharpWrapperSetupFixture.PdfTestTemplateFileName}";
        private static readonly string testFilePath = Path.Combine(AppContext.BaseDirectory, testFileName);

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLogger = new Mock<ILogger<PdfDocumentWriter>>();
            pdfDocumentWriter = new PdfDocumentWriter(mockLogger.Object);
            pdfDocumentReader = new PdfDocumentReader(new Mock<ILogger<PdfDocumentReader>>().Object);
        }

        [SetUp]
        public void SetUp()
        {
            File.Copy(PdfSharpWrapperSetupFixture.PdfTestTemplateFileName, testFileName, true);
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
            pdfDocumentWriter.Write(PdfSharpWrapperSetupFixture.PdfTestTemplateReadOnlyFileName, new Dictionary<string, string>
            {
                { field, "Test Text" }
            });

            // ASSERT
            mockLogger.VerifyLogging($"'{field}' is readonly.", LogLevel.Information, Times.Once);
        }

        [Test]
        public void Write_Null_LogError()
        {
            // ARRANGE
            // ACT
            pdfDocumentWriter.Write(PdfSharpWrapperSetupFixture.PdfTestTemplateReadOnlyFileName, new Dictionary<string, string>
            {
                { "FieldThatDoesntExist", "Test Text" }
            });

            // ASSERT
            mockLogger.VerifyLogging("Field is null for key: FieldThatDoesntExist.", LogLevel.Information, Times.Once);
        }

        [TestCase(PdfSharpWrapperSetupFixture.TEXT_FIELD, "", null)]
        [TestCase(PdfSharpWrapperSetupFixture.TEXT_FIELD, null, null)]
        [TestCase(PdfSharpWrapperSetupFixture.TEXT_FIELD, "a1!", "a1!")]
        [TestCase(PdfSharpWrapperSetupFixture.EMPTY_TEXT_FIELD, "", null)]
        [TestCase(PdfSharpWrapperSetupFixture.EMPTY_TEXT_FIELD, null, null)]
        [TestCase(PdfSharpWrapperSetupFixture.EMPTY_TEXT_FIELD, "a1!", "a1!")]
        public void Write_PdfTextField_AsExpected(string field, string value, string expected)
        {
            // ARRANGE
            // ACT
            pdfDocumentWriter.Write(testFileName, new Dictionary<string, string>
            {
                { field, value }
            });

            var actual = pdfDocumentReader.Read(testFileName)[field];

            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(PdfSharpWrapperSetupFixture.CHECK_BOX_FIELD, "true", "True")]
        [TestCase(PdfSharpWrapperSetupFixture.CHECK_BOX_FIELD, "false", "False")]
        [TestCase(PdfSharpWrapperSetupFixture.CHECK_BOX_FIELD, "True", "True")]
        [TestCase(PdfSharpWrapperSetupFixture.CHECK_BOX_FIELD, "False", "False")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_CHECK_BOX_FIELD, "true", "True")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_CHECK_BOX_FIELD, "false", "False")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_CHECK_BOX_FIELD, "True", "True")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_CHECK_BOX_FIELD, "False", "False")]
        public void Write_PdfCheckBoxField_AsExpected(string field, string value, string expected)
        {
            // ARRANGE
            // ACT
            pdfDocumentWriter.Write(testFileName, new Dictionary<string, string>
            {
                { field, value }
            });

            var actual = pdfDocumentReader.Read(testFileName)[field];

            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(PdfSharpWrapperSetupFixture.RADIO_BUTTON_FIELD, "Yes", "/Yes")]
        [TestCase(PdfSharpWrapperSetupFixture.RADIO_BUTTON_FIELD, "No", "/No")]
        [TestCase(PdfSharpWrapperSetupFixture.RADIO_BUTTON_FIELD, "NA", "/NA")]
        public void Write_PdfRadioButtonField_AsExpected(string field, string value, string expected)
        {
            // ARRANGE
            // ACT
            pdfDocumentWriter.Write(testFileName, new Dictionary<string, string>
            {
                { field, value }
            });

            var actual = pdfDocumentReader.Read(testFileName)[field];

            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(PdfSharpWrapperSetupFixture.COMBO_BOX_FIELD, "a", "a")]
        [TestCase(PdfSharpWrapperSetupFixture.COMBO_BOX_FIELD, "b", "b")]
        [TestCase(PdfSharpWrapperSetupFixture.COMBO_BOX_FIELD, "c", "c")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_COMBO_BOX_FIELD, "a", "a")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_COMBO_BOX_FIELD, "b", "b")]
        [TestCase(PdfSharpWrapperSetupFixture.UNCHECKED_COMBO_BOX_FIELD, "c", "c")]
        public void Write_PdfComboBoxField_AsExpected(string field, string value, string expected)
        {
            // ARRANGE
            pdfDocumentWriter.Write(testFileName, new Dictionary<string, string>
            {
                { field, value }
            });

            var actual = pdfDocumentReader.Read(testFileName)[field];

            // ACT
            // ASSERT
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Write_PdfListBoxField_AsExpected()
        {
            // ARRANGE
            // ACT
            // ASSERT
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
