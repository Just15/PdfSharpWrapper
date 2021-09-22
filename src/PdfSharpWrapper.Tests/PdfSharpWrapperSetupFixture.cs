using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace PdfSharpWrapper.Tests
{
    public class PdfSharpWrapperSetupFixture
    {
        // Text
        internal const string TEXT_FIELD = "textField";
        internal const string EMPTY_TEXT_FIELD = "emptyTextField";
        internal const string READ_ONLY_TEXT_FIELD = "readOnlyTextField";

        // CheckBox
        private const string CHECK_BOX_FIELD = "checkBoxField";
        private const string UNCHECKED_CHECK_BOX_FIELD = "unCheckedCheckBoxField";
        internal const string READ_ONLY_CHECK_BOX_FIELD = "readOnlyCheckBoxField";

        // RadioButton
        private const string RADIO_BUTTON_FIELD = "radioField";
        internal const string READ_ONLY_RADIO_BUTTON_FIELD = "readOnlyRadioField";

        // ComboBox
        private const string COMBO_BOX_FIELD = "dropDownField";
        private const string UNCHECKED_COMBO_BOX_FIELD = "unSelectedDropDownField";
        internal const string READ_ONLY_COMBO_BOX_FIELD = "readOnlyDropDownField";

        // ListBox
        private const string LIST_BOX_FIELD = "listBoxField";
        private const string UNSELECTED_LIST_BOX_FIELD = "unSelectedListBoxField";
        internal const string READ_ONLY_LIST_BOX_FIELD = "readOnlyListBoxField";

        // File Paths
        internal static readonly string PdfTestTemplateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfTestTemplate.pdf");
        internal static readonly string PdfTestTemplateReadOnlyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfTestTemplate_ReadOnly.pdf");
        internal static readonly string PdfTestTemplateSecuredFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfTestTemplate_Secured.pdf");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());
        }
    }
}
