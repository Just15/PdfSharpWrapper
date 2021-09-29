using Microsoft.Extensions.Logging;
using PdfReaderWriter;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using System;
using System.Collections.Generic;

namespace PdfSharpWrapper
{
    public class PdfDocumentWriter : PdfDocumentBase
    {
        public PdfDocumentWriter(ILogger<PdfDocumentWriter> logger) : base(logger)
        {
            PdfHelpers.ExtendNet5EncodingSupport();
        }

        public bool Write(string filePath, Dictionary<string, string> dictionary)
        {
            return Write(filePath, dictionary, out _);
        }

        public bool Write(string filePath, Dictionary<string, string> dictionary, out List<string> errorMessages)
        {
            using (var pdfDocument = Open(filePath))
            {
                if (pdfDocument != null)
                {
                    return DoWrite(pdfDocument, dictionary, out errorMessages);
                }
                else
                {
                    errorMessages = null;
                    return false;
                }
            }
        }

        public bool TryWrite(string filePath, Dictionary<string, string> dictionary)
        {
            return TryWrite(filePath, dictionary, out _);
        }

        public bool TryWrite(string filePath, Dictionary<string, string> dictionary, out List<string> errorMessages)
        {
            using (var pdfDocument = TryOpen(filePath))
            {
                if (pdfDocument != null)
                {
                    return DoWrite(pdfDocument, dictionary, out errorMessages);
                }
                else
                {
                    errorMessages = null;
                    return false;
                }
            }
        }

        private bool DoWrite(PdfDocument pdfDocument, Dictionary<string, string> dictionary, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            var fields = pdfDocument.AcroForm.Fields;

            foreach (var keyValuePair in dictionary)
            {
                var field = fields[keyValuePair.Key];
                if (field == null)
                {
                    logger.LogInformation($"Field is null for key: {keyValuePair.Key}.");
                    errorMessages.Add($"The field is null for key: {keyValuePair.Key}.");
                }
                else if (field.ReadOnly)
                {
                    logger.LogInformation($"'{field.Name}' is readonly.");
                    errorMessages.Add($"'{field.Name}' is readonly.");
                }
                else
                {
                    switch (field)
                    {
                        case PdfTextField text:
                            var textPdfValue = dictionary[field.Name];
                            text.Value = new PdfString(textPdfValue);
                            break;
                        case PdfCheckBoxField checkBox:
                            var checkBoxPdfValue = dictionary[field.Name];
                            var checkBoxValue = Convert.ToBoolean(checkBoxPdfValue);
                            checkBox.Checked = checkBoxValue;
                            break;
                        default:
                            logger.LogError($"Unexpected field type of '{field.GetType()}' for '{keyValuePair.Key}'.");
                            errorMessages.Add($"The field '{keyValuePair.Key}' is of unexpected type '{field.GetType()}'.");
                            break;
                    }
                }
            }

            PdfHelpers.SetNeedAppearances(pdfDocument);
            pdfDocument.Save(pdfDocument.FullPath);

            return true;
        }
    }
}
