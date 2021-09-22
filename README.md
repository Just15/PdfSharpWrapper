# PdfSharpWrapper

https://www.pdfescape.com/open/

Open a PDF document using TryOpen() or Open().
```
PdfDocumentReader pdfDocumentReader = new PdfDocumentReader(loggerFactory.CreateLogger<PdfDocumentReader>());
PdfDocument pdfDocumentTryOpen = pdfDocumentReader.TryOpen(@"C:\Path\To\PdfFile.pdf");
PdfDocument pdfDocumentOpen = pdfDocumentReader.Open(@"C:\Path\To\PdfFile.pdf");
```