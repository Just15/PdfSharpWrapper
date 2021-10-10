using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PdfSharpWrapper.Examples.ConsoleApp
{
    internal class Program
    {
        private readonly PdfDocumentReader pdfDocumentReader;
        private readonly PdfDocumentWriter pdfDocumentWriter;

        private static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        public Program(PdfDocumentReader pdfDocumentReader, PdfDocumentWriter pdfDocumentWriter)
        {
            this.pdfDocumentReader = pdfDocumentReader;
            this.pdfDocumentWriter = pdfDocumentWriter;
        }

        private void Run()
        {
            Console.WriteLine("Hello World!");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<Program>();
                    services.AddTransient<PdfDocumentReader>();
                    services.AddTransient<PdfDocumentWriter>();
                });
        }
    }
}
