using CustomControls;
using MsmhTools;
using PSFTools;
using Nikse.SubtitleEdit.Core.Common;
using PersianSubtitleFixes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtfUnknown;

namespace MsmhTools
{
    public static class EncodingTool
    {
        public static readonly string DefaultEncodingDisplayName = "UTF-8"; // 65001 is UTF-8
        private static readonly int UTF8Index = 0;
        private static readonly int UTF8BOMIndex = 1;
        private static readonly string UTF8DisplayName = "UTF-8";
        private static readonly string UTF8BOMDisplayName = "UTF-8 BOM";
        private static readonly string ArabicWindowsDisplayName = "Arabic (Windows)";
        public static readonly SortedSet<Tuple<int, object>> CodePageDisplayName = new();
        public static readonly SortedSet<int> AvailableCodePages = new();
        private static void InitializeCodePages()
        {
            // Register Encodings.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Get Supported Encodings.
            foreach (var ei in Encoding.GetEncodings())
            {
                try
                {
                    if (ei != null)
                    {
                        if (IsEbcdic(ei.CodePage) == false)
                        {
                            AvailableCodePages.Add(ei.CodePage);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("This code page is not supported.");
                }
            }
            // Get Encodings which is only available in .NET Framework.
            for (int i = 37; i <= 65001; i++)
            {
                try
                {
                    var ei = CodePagesEncodingProvider.Instance.GetEncoding(i);
                    if (ei != null)
                    {
                        if (IsEbcdic(ei.CodePage) == false)
                        {
                            AvailableCodePages.Add(ei.CodePage);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("This code page is not supported.");
                }
            }
        }
        public static void InitializeTextEncoding(ToolStripComboBox comboBox)
        {
            InitializeTextEncoding(comboBox.ComboBox);
        }
        public static void InitializeTextEncoding(ComboBox comboBox)
        {
            // Initialize CodePages
            InitializeCodePages();
            comboBox.BeginUpdate();
            comboBox.Items.Clear();
            List<string> encList = new();
            using (var graphics = comboBox.CreateGraphics())
            {
                var maxWidth = 0.0F;
                foreach (int codePage in AvailableCodePages)
                {
                    var displayName = codePage + ": " + Encoding.GetEncoding(codePage).EncodingName;
                    if (codePage.Equals(Encoding.UTF8.CodePage))
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, UTF8DisplayName));
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, UTF8BOMDisplayName));
                        encList.Insert(UTF8Index, UTF8DisplayName);
                        encList.Insert(UTF8BOMIndex, UTF8BOMDisplayName);
                    }
                    else if (codePage.Equals(1256))
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, ArabicWindowsDisplayName));
                        encList.Insert(0, ArabicWindowsDisplayName);
                    }
                    else if (codePage.Equals(1200))
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, displayName));
                        encList.Add(displayName);
                    }
                    else if (codePage.Equals(1201))
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, displayName));
                        encList.Add(displayName);
                    }
                    else if (codePage.Equals(12000))
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, displayName));
                        encList.Add(displayName);
                    }
                    else if (codePage.Equals(12001))
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, displayName));
                        encList.Add(displayName);
                    }
                    else
                    {
                        CodePageDisplayName.Add(new Tuple<int, object>(codePage, displayName));
                        // To Add Other Encodings To Combobox
                        //encList.Add(displayName);
                    }
                    var width = graphics.MeasureString(displayName, comboBox.Font).Width;
                    if (width > maxWidth)
                    {
                        maxWidth = width;
                    }
                }
                comboBox.DropDownWidth = (int)Math.Round(maxWidth + 7.5);
            }
            comboBox.Items.AddRange(encList.ToArray());
            comboBox.EndUpdate();
            // Set Default Encoding
            if (comboBox.SelectedItem == null && FormMain.SubEncoding == null)
            {
                string defaultEncodingDN = GetDefaultEncodingDisplayName();
                if (defaultEncodingDN != null)
                    comboBox.SelectedItem = defaultEncodingDN;
                else
                    comboBox.SelectedItem = DefaultEncodingDisplayName;
            }
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.AutoCompleteMode = AutoCompleteMode.Append;
        }
        private static bool IsEbcdic(int codePage)
        {
            string _EncodingName = Encoding.GetEncoding(codePage).EncodingName;
            string _EncodingWebName = Encoding.GetEncoding(codePage).WebName;
            string _Search = _EncodingName + " " + _EncodingWebName;
            if (_Search.Contains("EBCDIC") || _Search.Contains("IBM") || _Search.Contains("ibm"))
                return true;
            else
                return false;
        }

        private static bool IsCodePageAvailable(int codePage)
        {
            foreach (var item in AvailableCodePages)
            {
                if (codePage == item)
                    return true;
            }
            Console.WriteLine("Code Page Is Not Available In The List.");
            return false;
        }

        private static bool IsDisplayNameAvailable(string encodingDisplayName)
        {
            foreach (var item in CodePageDisplayName)
            {
                if (encodingDisplayName == item.Item2.ToString())
                    return true;
            }
            Console.WriteLine("Display Name Is Not Available In The List.");
            return false;
        }

        public static string GetDefaultEncodingDisplayName()
        {
            DataSet ds;
            ds = FormMain.DataSetSettings;

            if (!ds.Tables.Contains(PSFSettings.SettingsName.General))
                return DefaultEncodingDisplayName;
            if (ds.Tables[PSFSettings.SettingsName.General].Columns.Contains("DefaultEncodingDisplayName"))
            {
                if (ds.Tables[PSFSettings.SettingsName.General].Rows[0] == null)
                    return DefaultEncodingDisplayName;
                else
                {
                    string displayName = (string)ds.Tables[PSFSettings.SettingsName.General].Rows[0]["DefaultEncodingDisplayName"];
                    if (IsDisplayNameAvailable(displayName))
                        return displayName;
                    else
                        return DefaultEncodingDisplayName;
                }
            }
            else
                return DefaultEncodingDisplayName;
        }
        public static bool CompareByEncodingDisplayName(string path1, string path2)
        {
            if (path1 != null && path2 != null)
                if (File.Exists(path1) == true && File.Exists(path2) == true)
                {
                    string path1EDN = GetEncodingDisplayName(path1);
                    string path2EDN = GetEncodingDisplayName(path2);
                    if (path1EDN == path2EDN)
                        return true;
                }
            return false;
        }
        public static string? GetEncodingDisplayName(string filePath)
        {
            string encodingDisplayName = null;
            try
            {
                using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bom = new byte[12]; // Get the byte-order mark, if there is one
                file.Position = 0;
                file.Read(bom, 0, bom.Length);
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                {
                    encodingDisplayName = UTF8BOMDisplayName;
                }
                else if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
                {
                    encodingDisplayName = 12000 + ": " + Encoding.GetEncoding(12000).EncodingName; // UTF-32 (LE)
                }
                else if (bom[0] == 0xff && bom[1] == 0xfe)
                {
                    encodingDisplayName = Encoding.Unicode.CodePage + ": " + Encoding.Unicode.EncodingName;
                }
                else if (bom[0] == 0xfe && bom[1] == 0xff) // utf-16 and ucs-2
                {
                    encodingDisplayName = Encoding.BigEndianUnicode.CodePage + ": " + Encoding.BigEndianUnicode.EncodingName;
                }
                else if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) // ucs-4
                {
                    encodingDisplayName = 12001 + ": " + Encoding.GetEncoding(12001).EncodingName; // UTF-32 (BE)
                }
                else if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76 && (bom[3] == 0x38 || bom[3] == 0x39 || bom[3] == 0x2b || bom[3] == 0x2f)) // utf-7
                {
                    encodingDisplayName = Encoding.UTF7.CodePage + ": " + Encoding.UTF7.EncodingName;
                }
                else if (file.Length > bom.Length)
                {
                    long length = file.Length;
                    if (length > 500000)
                    {
                        length = 500000;
                    }

                    file.Position = 0;
                    var buffer = new byte[length];
                    file.Read(buffer, 0, buffer.Length);

                    if (IsUtf8(buffer, out var couldBeUtf8))
                    {
                        encodingDisplayName = UTF8DisplayName;
                    }
                    else if (couldBeUtf8 && GetDefaultEncodingDisplayName() == UTF8DisplayName)
                    { // keep utf-8 encoding if it's default
                        encodingDisplayName = UTF8DisplayName;
                    }
                    else if (couldBeUtf8 && Path.GetFileName(filePath).EndsWith(".xml", StringComparison.OrdinalIgnoreCase) && Encoding.Default.GetString(buffer).ToLowerInvariant().Replace('\'', '"').Contains("encoding=\"utf-8\""))
                    { // keep utf-8 encoding for xml files with utf-8 in header (without any utf-8 encoded characters, but with only allowed utf-8 characters)
                        encodingDisplayName = UTF8DisplayName;
                    }
                    else
                    {
                        DetectionResult encodingResult = CharsetDetector.DetectFromBytes(buffer);
                        if (encodingResult.Detected != null)
                        {
                            if (encodingResult.Detected.Encoding.CodePage == 1256)
                                encodingDisplayName = ArabicWindowsDisplayName;
                            else
                                encodingDisplayName = encodingResult.Detected.Encoding.CodePage + ": " + Encoding.GetEncoding(encodingResult.Detected.Encoding.CodePage).EncodingName;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
            if (encodingDisplayName == null)
                return null;
            if (!IsDisplayNameAvailable(encodingDisplayName))
                return null;
            return encodingDisplayName;
        }
        public static string? GetEncodingDisplayName(byte[] bytes)
        {
            DetectionResult encodingResult = CharsetDetector.DetectFromBytes(bytes);
            int _CodePage = encodingResult.Detected.Encoding.CodePage;
            string _DisplayName = _CodePage + ": " + encodingResult.Detected.Encoding.EncodingName;
            if (encodingResult.Detected != null)
            {
                if (encodingResult.Detected.Encoding.ToString().Contains("UTF8Encoding"))
                {
                    if (encodingResult.Detected.HasBOM == false)
                        return UTF8DisplayName;
                    else
                        return UTF8BOMDisplayName;
                }
                else if (encodingResult.Detected.Encoding == Encoding.GetEncoding(1256))
                {
                    return ArabicWindowsDisplayName;
                }
                else if (encodingResult.Detected.Encoding == Encoding.GetEncoding(20127))
                {
                    return UTF8DisplayName; // Prefer UTF8 over US-ASCII
                }
                else
                {
                    return _DisplayName;
                }
            }
            else
            {
                Console.WriteLine("Encoding Detection Failed.");
                return null;
            }
        }
        public static string? GetEncodingDisplayName(CustomToolStripComboBox comboBox)
        {
            return GetEncodingDisplayName(comboBox.ComboBox);
        }
        public static string? GetEncodingDisplayName(ToolStripComboBox comboBox)
        {
            return GetEncodingDisplayName(comboBox.ComboBox);
        }
        public static string? GetEncodingDisplayName(ComboBox comboBox)
        {
            if (comboBox.SelectedItem != null)
            {
                string _DisplayNameComboBox = comboBox.SelectedItem.ToString();
                foreach (var enc in CodePageDisplayName)
                {
                    int _CodePage = enc.Item1;
                    Encoding _Encoding = Encoding.GetEncoding(_CodePage);
                    string _DisplayName = enc.Item2.ToString();
                    if (_DisplayName == _DisplayNameComboBox)
                    {
                        if (_DisplayName == UTF8DisplayName)
                            return UTF8DisplayName;
                        else if (_DisplayName == UTF8BOMDisplayName)
                            return UTF8BOMDisplayName;
                        else if (_DisplayName == ArabicWindowsDisplayName)
                            return ArabicWindowsDisplayName;
                        else
                            return _CodePage + ": " + _Encoding.EncodingName;
                    }
                }
                Console.WriteLine("Selected Encoding Is Not Available In The List.");
                return null; // If Encoding not found in the List.
            }
            else
            {
                Console.WriteLine("ComboBox Selected Item Is Null.");
                return null; // comboBox.SelectedItem is null.
            }
        }
        public static bool? GetUTF8EncodingBOM(ToolStripComboBox comboBox)
        {
            return GetUTF8EncodingBOM(comboBox.ComboBox);
        }
        public static bool? GetUTF8EncodingBOM(ComboBox comboBox)
        {
            if (comboBox.SelectedItem != null)
            {
                string _DisplayNameComboBox = comboBox.SelectedItem.ToString();
                if (IsDisplayNameAvailable(_DisplayNameComboBox))
                {
                    if (_DisplayNameComboBox == UTF8DisplayName)
                        return false;
                    else if (_DisplayNameComboBox == UTF8BOMDisplayName)
                        return true;
                }
                return null; // If Encoding not found in the List.
            }
            else
            {
                Console.WriteLine("ComboBox Selected Item Is Null.");
                return null; // comboBox.SelectedItem is null.
            }
        }
        /// <summary>
        /// Will try to determine if buffer is utf-8 encoded or not.
        /// If any non-utf8 sequences are found then false is returned, if no utf8 multi bytes sequences are found then false is returned.
        /// </summary>
        private static bool IsUtf8(byte[] buffer, out bool couldBeUtf8)
        {
            couldBeUtf8 = false;
            int utf8Count = 0;
            int i = 0;
            while (i < buffer.Length - 3)
            {
                byte b = buffer[i];
                if (b > 127)
                {
                    if (b >= 194 && b <= 223 && buffer[i + 1] >= 128 && buffer[i + 1] <= 191)
                    { // 2-byte sequence
                        utf8Count++;
                        i++;
                    }
                    else if (b >= 224 && b <= 239 && buffer[i + 1] >= 128 && buffer[i + 1] <= 191 &&
                                                     buffer[i + 2] >= 128 && buffer[i + 2] <= 191)
                    { // 3-byte sequence
                        utf8Count++;
                        i += 2;
                    }
                    else if (b >= 240 && b <= 244 && buffer[i + 1] >= 128 && buffer[i + 1] <= 191 &&
                                                     buffer[i + 2] >= 128 && buffer[i + 2] <= 191 &&
                                                     buffer[i + 3] >= 128 && buffer[i + 3] <= 191)
                    { // 4-byte sequence
                        utf8Count++;
                        i += 3;
                    }
                    else
                    {
                        return false;
                    }
                }
                i++;
            }
            couldBeUtf8 = true;
            if (utf8Count == 0)
            {
                return false; // not utf-8 (no characters utf-8 encoded...)
            }
            return true;
        }
        public static Encoding? GetEncoding(CustomToolStripComboBox comboBox)
        {
            return GetEncoding(comboBox.ComboBox);
        }
        public static Encoding? GetEncoding(ToolStripComboBox comboBox)
        {
            return GetEncoding(comboBox.ComboBox);
        }
        public static Encoding? GetEncoding(ComboBox comboBox)
        {
            if (comboBox.SelectedItem != null)
            {
                string _DisplayNameComboBox = comboBox.SelectedItem.ToString();
                return GetEncoding(_DisplayNameComboBox);
            }
            else
            {
                Console.WriteLine("ComboBox Selected Item Is Null.");
                return null; // comboBox.SelectedItem is null.
            }
        }
        public static Encoding? GetEncoding(string encodingDisplayName)
        {
            foreach (var enc in CodePageDisplayName)
            {
                int _CodePage = enc.Item1;
                Encoding _Encoding = Encoding.GetEncoding(_CodePage);
                string _DisplayName = enc.Item2.ToString();
                if (_DisplayName == encodingDisplayName)
                {
                    if (_DisplayName == UTF8DisplayName)
                        return new UTF8Encoding(false);
                    else if (_DisplayName == UTF8BOMDisplayName)
                        return new UTF8Encoding(true);
                    else if (_DisplayName == ArabicWindowsDisplayName)
                        return Encoding.GetEncoding(1256);
                    else
                        return _Encoding;
                }
            }
            Console.WriteLine("Selected Encoding Is Not Available In The List.");
            return null; // If Encoding not found in the List.
        }
        public static Encoding? GetEncoding(Encoding encoding, bool hasBom)
        {
            int codePage = encoding.CodePage;
            if (IsCodePageAvailable(codePage))
            {
                if (encoding.ToString().Contains("UTF8Encoding"))
                {
                    if (hasBom == false)
                        return new UTF8Encoding(false);
                    else
                        return new UTF8Encoding(true);
                }
                else
                    return encoding;
            }
            else
                return null; // If Encoding not found in the List.
        }
        public static void UpdateComboBoxEncoding(Encoding subEncoding, bool hasBOM, CustomToolStripComboBox comboBox)
        {
            UpdateComboBoxEncoding(subEncoding, hasBOM, comboBox.ComboBox);
        }
        public static void UpdateComboBoxEncoding(Encoding subEncoding, bool hasBOM, ToolStripComboBox comboBox)
        {
            UpdateComboBoxEncoding(subEncoding, hasBOM, comboBox.ComboBox);
        }
        public static void UpdateComboBoxEncoding(Encoding subEncoding, bool hasBOM, ComboBox comboBox)
        {
            int subEncodingCodePage = subEncoding.CodePage;
            foreach (var enc in CodePageDisplayName)
            {
                int _CodePage = enc.Item1;
                string _DisplayName = enc.Item2.ToString();
                if (_CodePage == subEncodingCodePage)
                {
                    if (subEncoding.ToString().Contains("UTF8Encoding"))
                    {
                        if (hasBOM == false)
                        {
                            comboBox.SelectedItem = UTF8DisplayName;
                            return;
                        }
                        else
                        {
                            comboBox.SelectedItem = UTF8BOMDisplayName;
                            return;
                        }
                    }
                    else if(subEncoding == Encoding.GetEncoding(1256))
                    {
                        comboBox.SelectedItem = ArabicWindowsDisplayName;
                        return;
                    }
                    else
                    {
                        comboBox.Items.Add(_DisplayName);
                        comboBox.SelectedItem = _DisplayName;
                        return;
                    }
                }
            }
        }
        public static void UpdateComboBoxEncoding(string subEncodingDisplayName, CustomToolStripComboBox comboBox)
        {
            UpdateComboBoxEncoding(subEncodingDisplayName, comboBox.ComboBox);
        }
        public static void UpdateComboBoxEncoding(string subEncodingDisplayName, ToolStripComboBox comboBox)
        {
            UpdateComboBoxEncoding(subEncodingDisplayName, comboBox.ComboBox);
        }
        public static void UpdateComboBoxEncoding(string subEncodingDisplayName, ComboBox comboBox)
        {
            if (IsDisplayNameAvailable(subEncodingDisplayName) == false)
                return;
            comboBox.InvokeIt(() =>
            {
                foreach (var _DisplayNameComboBox in comboBox.Items)
                {
                    if (subEncodingDisplayName == _DisplayNameComboBox.ToString())
                    {
                        comboBox.SelectedItem = subEncodingDisplayName;
                        return;
                    }
                }
                comboBox.Items.Add(subEncodingDisplayName);
                comboBox.SelectedItem = subEncodingDisplayName;
                return;
            });
        }
        public static async Task ConvertEncoding(string srcPath, Encoding srcEncoding, string dstPath, ToolStripComboBox comboBox)
        {
            await ConvertEncoding(srcPath, srcEncoding, dstPath, comboBox.ComboBox);
        }
        public static async Task ConvertEncoding(string srcPath, Encoding srcEncoding, string dstPath, ComboBox comboBox)
        {
            if (srcPath != null && srcEncoding != null && comboBox.SelectedItem != null)
            {
                Encoding dstEncoding = GetEncoding(comboBox);
                if (dstEncoding == null)
                    return;
                //-------------------------------------------------------
                string srcDisplayName = GetEncodingDisplayName(srcPath);
                //-------------------------------------------------------
                string dstDisplayName = GetEncodingDisplayName(comboBox);
                //-------------------------------------------------------
                string msg = "File Converted From " + "\"" + srcDisplayName + "\"" + " To " + "\"" + dstDisplayName + "\"";
                string error = "Method 1 Conversion Failed From " + "\"" + srcDisplayName + "\"" + " To " + "\"" + dstDisplayName + "\"";
                string error2 = "Method 2 Conversion Failed From " + "\"" + srcDisplayName + "\"" + " To " + "\"" + dstDisplayName + "\"";
                string info = "No Need To Convert.";
                //-------------------------------------------------------
                if (srcDisplayName == dstDisplayName)
                {
                    Console.WriteLine(info);
                    return;
                }
                //-------------------------------------------------------
                var source = File.ReadAllText(srcPath, srcEncoding);
                byte[] sourceByte = srcEncoding.GetBytes(source);
                // Try Method 1 -------------------------------------------------------
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Start Method 1 Conversion:");
                byte[] outputByte;
                string output;
                if (dstEncoding.ToString().Contains("UTF8Encoding"))
                {
                    if (comboBox.SelectedItem.ToString() == UTF8DisplayName)
                    {
                        // Convert To UTF-8 Without BOM
                        //outputByte = Encoding.Convert(srcEncoding, new UTF8Encoding(false), sourceByte);
                        output = source;
                        dstEncoding = new UTF8Encoding(false);
                    }
                    else
                    {
                        // Convert To UTF-8 With BOM
                        //outputByte = dstEncoding.GetPreamble().Concat(sourceByte).ToArray();
                        output = source;
                        dstEncoding = new UTF8Encoding(true);
                    }
                }
                else
                {
                    outputByte = Encoding.Convert(srcEncoding, dstEncoding, sourceByte);
                    output = dstEncoding.GetString(outputByte);
                }

                using MemoryStream ms1 = new();
                using StreamWriter sw1 = new(ms1, dstEncoding);
                await sw1.WriteAsync(output);
                sw1.Flush(); // Important
                outputByte = ms1.GetBuffer();

                string outDisplayName = GetEncodingDisplayName(outputByte);
                Console.WriteLine("srcDisplayName: " + srcDisplayName);
                Console.WriteLine("dstDisplayName: " + dstDisplayName);
                Console.WriteLine("outDisplayName: " + outDisplayName);

                dstPath = Path.GetFullPath(dstPath);
                if (outDisplayName == dstDisplayName)
                {
                    //await File.WriteAllTextAsync(dstPath, output, dstEncoding);
                    //await File.WriteAllBytesAsync(dstPath, outputByte);
                    ms1.WriteToFile(dstPath);
                    if (File.Exists(dstPath))
                    {
                        Console.WriteLine(msg);
                    }
                    else
                    {
                        Console.WriteLine(error);
                    }
                }
                else
                {
                    Console.WriteLine(error);
                    // Try Method 2 -------------------------------------------------------
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("Start Method 2 Conversion:");

                    using MemoryStream ms2 = new();
                    using StreamWriter sw2 = new(ms2, dstEncoding);
                    await sw2.WriteAsync(source);
                    sw2.Flush(); // Important
                    outputByte = ms2.ToArray();

                    string outDisplayName2 = GetEncodingDisplayName(outputByte);
                    Console.WriteLine("srcDisplayName: " + srcDisplayName);
                    Console.WriteLine("dstDisplayName: " + dstDisplayName);
                    Console.WriteLine("outDisplayName2: " + outDisplayName2);

                    if (outDisplayName2 == dstDisplayName)
                    {
                        //await File.WriteAllTextAsync(dstPath, source, dstEncoding);
                        //await File.WriteAllBytesAsync(dstPath, outputByte);
                        ms2.WriteToFile(dstPath);
                        if (File.Exists(dstPath))
                        {
                            Console.WriteLine(msg);
                        }
                        else
                        {
                            Console.WriteLine(error2);
                        }
                    }
                    else
                    {
                        Console.WriteLine(error2);
                    }
                }
            }
        }
    }
}
