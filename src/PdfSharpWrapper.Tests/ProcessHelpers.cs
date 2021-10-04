using System.Diagnostics;

namespace PdfSharpWrapper.Tests
{
    public static class ProcessHelpers
    {
        public static void Start(string filePath)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(filePath)
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                };
                process.Start();
            }
        }
    }
}
