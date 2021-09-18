using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace PdfSharpWrapper.Tests
{
    [TestFixture]
    public class PdfDocumentReaderTests
    {
        Mock<ILogger<PdfDocumentReader>> mockLogger;
        private static PdfDocumentReader pdfDocumentReader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLogger = new Mock<ILogger<PdfDocumentReader>>();
            pdfDocumentReader = new PdfDocumentReader(mockLogger.Object);
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfTextField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfCheckBoxField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfComboBoxField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfListBoxField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfRadioButtonField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfSignatureField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfGenericField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(testCases))]
        public void ReadTryRead_PdfPushButtonField_AsExpected(Func<Dictionary<string, string>> func)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Func<Dictionary<string, string>>> testCases
        {
            get
            {
                yield return () => pdfDocumentReader.Read(PdfSharpWrapperSetupFixture.PdfTestTemplateFilePath);
                yield return () => pdfDocumentReader.TryRead(PdfSharpWrapperSetupFixture.PdfTestTemplateFilePath);
            }
        }
    }
}
