using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PdfReaderWriter;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;

namespace PdfSharpWrapper
{
    public class PdfDocumentReader : PdfDocumentBase
    {
        public PdfDocumentReader(ILogger<PdfDocumentReader> logger) : base(logger)
        {
            PdfHelpers.ExtendNet5EncodingSupport();
        }

        public Dictionary<string, string> Read(string filePath)
        {
            return Read(filePath, out _);
        }

        public Dictionary<string, string> Read(string filePath, out List<string> errorMessages)
        {
            Dictionary<string, string> dictionary = null;

            using (var pdfDocument = Open(filePath))
            {
                if (pdfDocument != null)
                {
                    dictionary = DoRead(pdfDocument, out errorMessages);
                }
                else
                {
                    errorMessages = null;
                }
            }

            return dictionary;
        }

        public Dictionary<string, string> TryRead(string filePath)
        {
            return TryRead(filePath, out _);
        }

        public Dictionary<string, string> TryRead(string filePath, out List<string> errorMessages)
        {
            Dictionary<string, string> dictionary = null;

            using (var pdfDocument = TryOpen(filePath))
            {
                if (pdfDocument != null)
                {
                    dictionary = DoRead(pdfDocument, out errorMessages);
                }
                else
                {
                    errorMessages = null;
                }
            }

            return dictionary;
        }

        private Dictionary<string, string> DoRead(PdfDocument pdfDocument, out List<string> errorMessages)
        {
            var dictionary = new Dictionary<string, string>();
            errorMessages = new List<string>();
            var fields = pdfDocument.AcroForm.Fields;

            foreach (var fieldName in fields.Names)
            {
                var field = fields[fieldName];
                switch (field)
                {
                    case PdfTextField text:
                        var textValue = !string.IsNullOrWhiteSpace(text.Text) ? text.Text : null;
                        dictionary.Add(fieldName, textValue);
                        break;
                    case PdfCheckBoxField checkBox:
                        var checkBoxValue = checkBox.Checked.ToString();
                        dictionary.Add(fieldName, checkBoxValue);
                        break;
                    case PdfRadioButtonField radioButton:
                        var radioButtonValue = radioButton.Value?.ToString();
                        dictionary.Add(fieldName, radioButtonValue);
                        break;
                    case PdfComboBoxField comboBox:
                        if (comboBox.Value != null)
                        {
                            var stringValue = comboBox.Value.ToString();
                            var comboBoxValue = stringValue.Substring(1, stringValue.Length - 2);
                            dictionary.Add(fieldName, comboBoxValue);
                        }
                        else
                        {
                            dictionary.Add(fieldName, null);
                        }
                        break;
                    case PdfListBoxField listBox:
                        if (listBox.Value != null)
                        {
                            var stringValue = listBox.Value.ToString();
                            var listBoxValue = stringValue.Substring(1, stringValue.Length - 2);
                            dictionary.Add(fieldName, listBoxValue);
                        }
                        else
                        {
                            dictionary.Add(fieldName, null);
                        }
                        break;
                    default:
                        var errorMessage = $"Unexpected field type of '{field.GetType()}' for '{fieldName}'.";
                        logger.LogError(errorMessage);
                        errorMessages.Add(errorMessage);
                        break;
                }
            }

            return dictionary;
        }
    }
}
