using NUnit.Framework;
using System.Diagnostics;

namespace PdfSharpWrapper.Tests
{
    public class PdfSharpWrapperSetupFixture
    {
        // AppContext.BaseDirectory

        // Text
        internal const string TEXT_FIELD = "textField";
        internal const string EMPTY_TEXT_FIELD = "emptyTextField";
        internal const string READ_ONLY_TEXT_FIELD = "readOnlyTextField";

        // CheckBox
        internal const string CHECK_BOX_FIELD = "checkBoxField";
        internal const string UNCHECKED_CHECK_BOX_FIELD = "unCheckedCheckBoxField";
        internal const string READ_ONLY_CHECK_BOX_FIELD = "readOnlyCheckBoxField";

        // RadioButton
        internal const string RADIO_BUTTON_FIELD = "radioField";
        internal const string READ_ONLY_RADIO_BUTTON_FIELD = "readOnlyRadioField";

        // ComboBox
        internal const string COMBO_BOX_FIELD = "dropDownField";
        internal const string UNCHECKED_COMBO_BOX_FIELD = "unSelectedDropDownField";
        internal const string READ_ONLY_COMBO_BOX_FIELD = "readOnlyDropDownField";

        // ListBox
        internal const string LIST_BOX_FIELD = "listBoxField";
        internal const string UNSELECTED_LIST_BOX_FIELD = "unSelectedListBoxField";
        internal const string READ_ONLY_LIST_BOX_FIELD = "readOnlyListBoxField";

        // File Paths
        internal static readonly string PdfTestTemplateFileName = "PdfTestTemplate.pdf";
        internal static readonly string PdfTestTemplateReadOnlyFileName = "PdfTestTemplate_ReadOnly.pdf";
        internal static readonly string PdfTestTemplateSecuredFileName = "PdfTestTemplate_Secured.pdf";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public static void StartPdf(string filePath)
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
