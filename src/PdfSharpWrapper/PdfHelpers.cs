using System.Text;
using PdfSharpCore.Pdf;

namespace PdfSharpWrapper
{
    public static class PdfHelpers
    {
        public static void ExtendNet5EncodingSupport()
        {
            // .NET 5 supports less encodings than .NET Framework, this extends .NET 5 support for additional encodings.
#if !NETSTANDARD && !NETCOREAPP2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);
#endif
        }

        public static void SetNeedAppearances(PdfDocument pdfDocument)
        {
            // The "NeedsAppearances" element needs to be set to true. Otherwise the PDF will "hide" the values on the form.
            if (pdfDocument.AcroForm.Elements.ContainsKey("/NeedAppearances"))
            {
                pdfDocument.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                pdfDocument.AcroForm.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }
        }
    }
}
