using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PdfReaderWriter;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.Advanced;

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
                    var errorMessage = $"Field is null for key: {keyValuePair.Key}.";
                    logger.LogInformation(errorMessage);
                    errorMessages.Add(errorMessage);
                }
                else if (field.ReadOnly)
                {
                    var errorMessage = $"'{field.Name}' is readonly.";
                    logger.LogInformation(errorMessage);
                    errorMessages.Add(errorMessage);
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
                        case PdfRadioButtonField radioButton:
                            var fieldPdfValue = dictionary[field.Name];
                            var fieldValue = !string.IsNullOrEmpty(fieldPdfValue) ? new PdfName($"/{fieldPdfValue}") : new PdfName("/null");
                            radioButton.Value = fieldValue;
                            break;
                        case PdfComboBoxField:
                            var pdfValue = dictionary[field.Name];

                            // Get the index that should be selected
                            var opt = field.Elements["/Opt"];
                            var optPdfArray = (opt is PdfReference pdfReference) ? (PdfArray)pdfReference.Value : (PdfArray)opt;

                            // If 'optPdfArray.Elements.Items[0]' is a PdfString then the first item is a prompt like "Select X from the list"
                            int optIndex;
                            if (optPdfArray.Elements.Items[0] is PdfString pdfString)
                            {
                                var optValues = optPdfArray.Skip(1).Select(i => ((PdfString)((PdfArray)i).Elements.Items[0]).Value).ToList();
                                optIndex = optValues.IndexOf(pdfValue);
                                optIndex++;
                            }
                            else
                            {
                                var optValues = optPdfArray.Elements.Items.Select(i => ((PdfString)((PdfArray)i).Elements.Items[0]).Value).ToList();
                                optIndex = optValues.IndexOf(pdfValue);
                            }

                            if (optIndex >= 0)
                            {
                                field.Value = new PdfString(pdfValue);

                                var i = field.Elements["/I"];
                                // 'i' is null if no item is selected
                                if (i != null)
                                {
                                    // Select the specified index
                                    var iPdfArray = (i is PdfReference reference2) ? (PdfArray)reference2.Value : (PdfArray)i;
                                    iPdfArray.Elements[0] = new PdfInteger(optIndex);
                                }
                            }
                            break;
                        default:
                            var errorMessage = $"Unexpected field type of '{field.GetType()}' for '{keyValuePair.Key}'.";
                            logger.LogError(errorMessage);
                            errorMessages.Add(errorMessage);
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
