using Microsoft.Extensions.Logging;
using PdfReaderWriter;

namespace PdfSharpWrapper
{
    public class PdfDocumentWriter : PdfDocumentBase
    {
        public PdfDocumentWriter(ILogger<PdfDocumentWriter> logger) : base(logger)
        {
            PdfHelpers.ExtendNet5EncodingSupport();
        }
    }
}
