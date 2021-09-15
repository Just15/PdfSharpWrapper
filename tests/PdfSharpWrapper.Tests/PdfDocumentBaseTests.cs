using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PdfSharpCore.Pdf;
using System.IO;

namespace PdfSharpWrapper.Tests
{
    [TestFixture]
    public class PdfDocumentBaseTests
    {
        Mock<ILogger<PdfDocumentBase>> mockLogger;
        private PdfDocumentBase pdfDocumentBase;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLogger = new Mock<ILogger<PdfDocumentBase>>();
            pdfDocumentBase = new PdfDocumentBase(mockLogger.Object);
        }

        [Test]
        public void Open_FileDoesntExist_Exception()
        {
            var fileName = @"C:\Path\That\Doesnt\Exist\FakeFile.pdf";

            // ARRANGE
            // ACT
            // ASSERT
            Assert.Throws<DirectoryNotFoundException>(() => pdfDocumentBase.Open(fileName),
                $"Could not find a part of the path '{fileName}'.");
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
                pdfDocument = pdfDocumentBase.Open(fileName);

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
            var pdfDocument = pdfDocumentBase.TryOpen(fileName);

            // ASSERT
            Assert.IsNull(pdfDocument);
            mockLogger.VerifyLogging($"The file '{fileName}' does not exist.", LogLevel.Error, Times.Once);
        }

        [Test]
        public void TryOpen_X_Exception()
        {
            // ARRANGE
            // ACT
            var pdfDocument = pdfDocumentBase.TryOpen(PdfSharpWrapperSetupFixture.PdfTestTemplateSecuredFilePath);

            // ASSERT
            Assert.IsNull(pdfDocument);
            mockLogger.VerifyLogging($"Exception trying to open '{PdfSharpWrapperSetupFixture.PdfTestTemplateSecuredFilePath}'.", LogLevel.Error, Times.Once);
        }
    }
}
