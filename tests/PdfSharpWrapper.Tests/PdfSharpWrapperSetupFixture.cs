using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace PdfSharpWrapper.Tests
{
    public class PdfSharpWrapperSetupFixture
    {
        internal static readonly string BaseProjectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
        internal static readonly string PdfTestTemplateSecuredFilePath = Path.Combine(BaseProjectDirectory, "PdfTestTemplate_Secured.pdf");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());
        }
    }
}
