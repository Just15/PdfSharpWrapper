using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PdfSharpCore.Pdf;

namespace PdfSharpWrapper.Tests
{
    [TestFixture]
    public class PdfDocumentBaseTests
    {
        private Mock<ILogger<TestPdfDocumentBase>> mockLogger;
        private TestPdfDocumentBase testPdfDocumentBase;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLogger = new Mock<ILogger<TestPdfDocumentBase>>();
            testPdfDocumentBase = new TestPdfDocumentBase(mockLogger.Object);
        }

        [Test]
        public void Open_FileDoesntExist_Exception()
        {
            var fileName = @"C:\Path\That\Doesnt\Exist\FakeFile.pdf";

            // ARRANGE
            // ACT
            // ASSERT
            Assert.Throws<DirectoryNotFoundException>(
                () => testPdfDocumentBase.Open(fileName), $"Could not find a part of the path '{fileName}'.");
        }

        [Test]
        public void Open_AcroFormIsNull_ReturnsNull()
        {
            var fileName = "Pdf_AcroForm_Null.pdf";
            PdfDocument pdfDocument;

            try
            {
                // ARRANGE
                using (pdfDocument = new PdfDocument())
                {
                    pdfDocument.Pages.Add(new PdfPage());
                    pdfDocument.Save(fileName);
                }

                // ACT
                pdfDocument = testPdfDocumentBase.Open(fileName);

                // ASSERT
                Assert.IsNull(pdfDocument);
                mockLogger.VerifyLogging($"'AcroForm' is null in '{fileName}'.", LogLevel.Error, Times.Once);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Test]
        public void TryOpen_ExceptionOnOpen_ExceptionSwallowed()
        {
            var fileName = @"C:\Path\That\Doesnt\Exist\FakeFile.pdf";

            // ARRANGE
            // ACT
            var pdfDocument = testPdfDocumentBase.TryOpen(fileName);

            // ASSERT
            Assert.IsNull(pdfDocument);
            mockLogger.VerifyLogging($"The file '{fileName}' does not exist.", LogLevel.Error, Times.Once);
        }

        [Test]
        public void TryOpen_X_Exception()
        {
            // ARRANGE
            // ACT
            var pdfDocument = testPdfDocumentBase.TryOpen(PdfSharpWrapperSetupFixture.PdfTestTemplateSecuredFileName);

            // ASSERT
            Assert.IsNull(pdfDocument);
            mockLogger.VerifyLogging($"Exception trying to open '{PdfSharpWrapperSetupFixture.PdfTestTemplateSecuredFileName}'.", LogLevel.Error, Times.Once);
        }

        public class TestPdfDocumentBase : PdfDocumentBase
        {
            public TestPdfDocumentBase(ILogger logger) : base(logger) { }
        }
    }
}
