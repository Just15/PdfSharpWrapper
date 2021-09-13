using Microsoft.Extensions.Logging;
using PdfReaderWriter;

namespace PdfSharpWrapper
{
    public class PdfDocumentReader : PdfDocumentBase
    {
        public PdfDocumentReader(ILogger<PdfDocumentReader> logger) : base(logger)
        {
            PdfHelpers.ExtendNet5EncodingSupport();
        }
    }
}
