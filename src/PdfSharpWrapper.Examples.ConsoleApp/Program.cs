using System;
using System.Collections.Generic;
using System.IO;
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
            var fileName = "PdfTestTemplate.pdf";
            var filePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..", @$"PdfSharpWrapper.Tests\{fileName}");
            var modifiedFilePath = filePath.Replace(fileName, $"Modified-{fileName}");
            File.Copy(filePath, modifiedFilePath, true);

            try
            {
                // Read values and output to console
                var dictionary = pdfDocumentReader.Read(filePath);
                WriteValues("Original Values", dictionary);

                // Update values
                dictionary["textField"] = "PdfSharpWrapper";
                dictionary["emptyTextField"] = "Rocks!";
                dictionary["checkBoxField"] = "false";
                dictionary["unCheckedCheckBoxField"] = "True";
                dictionary["radioField"] = "No";
                dictionary["dropDownField"] = null;
                dictionary["unSelectedDropDownField"] = "c";
                dictionary["listBoxField"] = null;
                dictionary["unSelectedListBoxField"] = "a2";

                // Write values and output to console
                pdfDocumentWriter.Write(modifiedFilePath, dictionary);
                WriteValues("Updated Values", dictionary);
            }
            finally
            {
                File.Delete(modifiedFilePath);
            }
        }

        private static void WriteValues(string header, Dictionary<string, string> dictionary)
        {
            Console.WriteLine($"{header} --------------------");
            foreach (var keyValuePair in dictionary)
            {
                Console.WriteLine($"{keyValuePair.Key}, {keyValuePair.Value ?? "null"}");
            }

            Console.WriteLine(Environment.NewLine);
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
