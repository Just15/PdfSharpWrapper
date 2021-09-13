using Microsoft.Extensions.Logging;

namespace PdfSharpWrapper
{
    public abstract class PdfDocumentBase
    {
        protected readonly ILogger logger;

        public PdfDocumentBase(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
