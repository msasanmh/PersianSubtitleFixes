using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PersianSubtitleFixes;
using System.Security.Cryptography;
using Force.Crc32;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Data;
using System.Xml.Serialization;
using CustomControls;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MsmhTools
{
    public static class Extensions
    {
        //-----------------------------------------------------------------------------------
        public static string? AssemblyDescription(this Assembly assembly)
        {
            if (assembly != null && Attribute.IsDefined(assembly, typeof(AssemblyDescriptionAttribute)))
            {
                var descriptionAttribute = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute));
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Description;
                }
            }
            return null;
        }
        //-----------------------------------------------------------------------------------
        public static T IsNotNull<T>([NotNull] this T? value, [CallerArgumentExpression(parameterName: "value")] string? paramName = null)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
            else
                return value;
        } // Usage: someVariable.IsNotNull();
        //-----------------------------------------------------------------------------------
        public static void EnableDoubleBuffer(this Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
        }
        //-----------------------------------------------------------------------------------
        public static GraphicsPath Shrink(this GraphicsPath path, float width)
        {
            using GraphicsPath gp = new();
            gp.AddPath(path, false);
            gp.CloseAllFigures();
            gp.Widen(new Pen(Color.Black, width * 2));
            int position = 0;
            GraphicsPath result = new();
            while (position < gp.PointCount)
            {
                // skip outer edge
                position += CountNextFigure(gp.PathData, position);
                // count inner edge
                int figureCount = CountNextFigure(gp.PathData, position);
                var points = new PointF[figureCount];
                var types = new byte[figureCount];

                Array.Copy(gp.PathPoints, position, points, 0, figureCount);
                Array.Copy(gp.PathTypes, position, types, 0, figureCount);
                position += figureCount;
                result.AddPath(new GraphicsPath(points, types), false);
            }
            path.Reset();
            path.AddPath(result, false);
            return path;
        }

        private static int CountNextFigure(PathData data, int position)
        {
            int count = 0;
            for (int i = position; i < data.Types.Length; i++)
            {
                count++;
                if (0 != (data.Types[i] & (int)PathPointType.CloseSubpath))
                    return count;
            }
            return count;
        }
        //-----------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int radiusTopLeft, int radiusTopRight, int radiusBottomRight, int radiusBottomLeft)
        {
            GraphicsPath path;
            path = Tools.Drawing.RoundedRectangle(bounds, radiusTopLeft, radiusTopRight, radiusBottomRight, radiusBottomLeft);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = SmoothingMode.Default;
        }
        //-----------------------------------------------------------------------------------
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int radiusTopLeft, int radiusTopRight, int radiusBottomRight, int radiusBottomLeft)
        {
            GraphicsPath path;
            path = Tools.Drawing.RoundedRectangle(bounds, radiusTopLeft, radiusTopRight, radiusBottomRight, radiusBottomLeft);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = SmoothingMode.Default;
        }
        //-----------------------------------------------------------------------------------
        public static string ToXml(this DataSet ds)
        {
            using var memoryStream = new MemoryStream();
            using TextWriter streamWriter = new StreamWriter(memoryStream);
            var xmlSerializer = new XmlSerializer(typeof(DataSet));
            xmlSerializer.Serialize(streamWriter, ds);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        //-----------------------------------------------------------------------------------
        public static string ToXmlWithWriteMode(this DataSet ds, XmlWriteMode xmlWriteMode)
        {
            using var ms = new MemoryStream();
            using TextWriter sw = new StreamWriter(ms);
            ds.WriteXml(sw, xmlWriteMode);
            return new UTF8Encoding(false).GetString(ms.ToArray());
        }
        //-----------------------------------------------------------------------------------
        public static DataSet ToDataSet(this DataSet ds, string xmlFile, XmlReadMode xmlReadMode)
        {
            ds.ReadXml(xmlFile, xmlReadMode);
            return ds;
        }
        //-----------------------------------------------------------------------------------
        public static void AddVScrollBar(this DataGridView dataGridView, CustomVScrollBar customVScrollBar)
        {
            customVScrollBar.Dock = DockStyle.Right;
            customVScrollBar.Visible = true;
            customVScrollBar.BringToFront();
            dataGridView.Controls.Add(customVScrollBar);
            dataGridView.ScrollBars = ScrollBars.None;
            dataGridView.SizeChanged += (object? sender, EventArgs e) =>
            {
                // To update LargeChange on form resize
                customVScrollBar.LargeChange = dataGridView.DisplayedRowCount(false);
            };
            dataGridView.RowsAdded += (object? sender, DataGridViewRowsAddedEventArgs e) =>
            {
                customVScrollBar.Maximum = dataGridView.RowCount;
                customVScrollBar.LargeChange = dataGridView.DisplayedRowCount(false);
                customVScrollBar.SmallChange = 1;
            };
            dataGridView.Scroll += (object? sender, ScrollEventArgs e) =>
            {
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    if (dataGridView.Rows.Count > 0)
                    {
                        customVScrollBar.Value = e.NewValue;
                    }
                }
            };
            customVScrollBar.Scroll += (object? sender, EventArgs e) =>
            {
                if (dataGridView.Rows.Count > 0)
                    if (customVScrollBar.Value < dataGridView.Rows.Count)
                        dataGridView.FirstDisplayedScrollingRowIndex = customVScrollBar.Value;
            };
        }
        //-----------------------------------------------------------------------------------
        public static Icon? GetApplicationIcon(this Form form)
        {
            return Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }
        //-----------------------------------------------------------------------------------
        public static Icon? GetDefaultIcon(this Form form)
        {
            return (Icon)typeof(Form).GetProperty("DefaultIcon", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
        }
        //-----------------------------------------------------------------------------------
        public static void SetDefaultIcon(this Form form, Icon icon)
        {
            if (icon != null)
                typeof(Form).GetField("defaultIcon", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, icon);
        }
        //-----------------------------------------------------------------------------------
        /// <summary>
        /// Invalidate Controls. Use on Form_SizeChanged event.
        /// </summary>
        public static void Invalidate(this Control.ControlCollection controls)
        {
            foreach (Control c in controls)
                c.Invalidate();
        }
        //-----------------------------------------------------------------------------------
        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeBrightness(this Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }
            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
        //-----------------------------------------------------------------------------------
        /// <summary>
        /// Check Color is Light or Dark.
        /// </summary>
        /// <returns>
        /// Returns "Dark" or "Light" as string.
        /// </returns>
        public static string DarkOrLight(this Color color)
        {
            if (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722 < 255 / 2)
            {
                return "Dark";
            }
            else
            {
                return "Light";
            }
        }
        //-----------------------------------------------------------------------------------
        public static void AutoSizeLastColumn(this ListView listView)
        {
            if (listView.Columns.Count > 1)
            {
                //ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                //ListView1.Columns[ListView1.Columns.Count - 1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                //ListView1.Columns[ListView1.Columns.Count - 1].Width = -2; // -2 = Fill remaining space
                int cs = 0;
                for (int n = 0; n < listView.Columns.Count - 1; n++)
                {
                    var column = listView.Columns[n];
                    cs += column.Width;
                }
                listView.BeginUpdate();
                listView.Columns[listView.Columns.Count - 1].Width = Math.Max(400, listView.ClientRectangle.Width - cs);
                listView.EndUpdate();
            }
        }
        //-----------------------------------------------------------------------------------
        public static void AutoSizeLastColumn(this DataGridView dataGridView)
        {
            if (dataGridView.Columns.Count > 0)
            {
                int cs = 0;
                for (int n = 0; n < dataGridView.Columns.Count - 1; n++)
                {
                    var columnWidth = dataGridView.Columns[n].Width;
                    var columnDivider = dataGridView.Columns[n].DividerWidth;
                    cs += columnWidth + columnDivider;
                }
                cs += (dataGridView.Margin.Left + dataGridView.Margin.Right) * 2;
                foreach (var scroll in dataGridView.Controls.OfType<VScrollBar>())
                {
                    if (scroll.Visible == true)
                        cs += SystemInformation.VerticalScrollBarWidth;
                }
                dataGridView.Columns[dataGridView.Columns.Count - 1].Width = Math.Max(400, dataGridView.ClientRectangle.Width - cs);
            }
        }
        //-----------------------------------------------------------------------------------
        public static List<string> RemoveDuplicates(this List<string> list)
        {
            List<string> NoDuplicates = list.Distinct().ToList();
            return NoDuplicates;
        }
        //-----------------------------------------------------------------------------------
        public static void WriteToFile(this MemoryStream memoryStream, string dstPath)
        {
            using FileStream fs = new(dstPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Position = 0;
            memoryStream.WriteTo(fs);
            fs.Flush();
        }
        //-----------------------------------------------------------------------------------
        public static void SetToolTip(this Control control, string titleMessage, string bodyMessage)
        {
            ToolTip tt = new();
            tt.ToolTipIcon = ToolTipIcon.Info;
            tt.IsBalloon = false;
            tt.ShowAlways = true;
            tt.UseAnimation = true;
            tt.UseFading = true;
            tt.InitialDelay = 1000;
            tt.AutoPopDelay = 300;
            tt.AutomaticDelay = 300;
            tt.ToolTipTitle = titleMessage;
            tt.SetToolTip(control, bodyMessage);
        }
        //-----------------------------------------------------------------------------------
        public static void InvokeIt(this ISynchronizeInvoke sync, Action action)
        {
            // If the invoke is not required, then invoke here and get out.
            if (!sync.InvokeRequired)
            {
                action();
                return;
            }
            sync.Invoke(action, Array.Empty<object>());
            // Usage:
            // textBox1.InvokeIt(() => textBox1.Text = text);
        }
        //-----------------------------------------------------------------------------------
        public static bool Compare(this List<string> list1, List<string> list2)
        {
            return Enumerable.SequenceEqual(list1, list2);
        }
        public static bool Compare(this string string1, string string2)
        {
            return string1.Equals(string2, StringComparison.Ordinal);
        }
        //-----------------------------------------------------------------------------------
        public static bool IsInteger(this string s)
        {
            if (int.TryParse(s, out _))
                return true;
            return false;
        }
        //-----------------------------------------------------------------------------------
        public static bool IsBool(this string s)
        {
            if (bool.TryParse(s, out _))
                return true;
            return false;
        }
        //-----------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static class Tools
    {
        public static class Controllers
        {
            //-----------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------
            public static IEnumerable<Control> GetAllControls(Control control)
            {
                if (control == null)
                    throw new ArgumentNullException(nameof(control));
                return implementation();
                IEnumerable<Control> implementation()
                {
                    foreach (Control control in control.Controls)
                    {
                        foreach (Control child in GetAllControls(control))
                        {
                            yield return child;
                        }
                        yield return control;
                    }
                }
            }
            //-----------------------------------------------------------------------------------
            public static IEnumerable<Control> GetAllControlByType(Control control, Type type)
            {
                var controls = control.Controls.Cast<Control>().ToList();
                return controls.SelectMany(ctrl => GetAllControlByType(ctrl, type))
                                          .Concat(controls)
                                          .Where(c => c.GetType() == type);
            } // Usage: var c = GetAllControlByType(this, typeof(CustomButton))
            //-----------------------------------------------------------------------------------
            public static List<T> GetSubControls<T>(Control control)
            {
                var type = control.GetType();
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                var contextMenus = fields.Where(c => c.GetValue(control) != null &&
                (c.GetValue(control).GetType().IsSubclassOf(typeof(T)) || c.GetValue(control).GetType() == typeof(T)));
                var menus = contextMenus.Select(c => c.GetValue(control));
                return menus.Cast<T>().ToList();
            } // Usage: var toolStripButtons = GetSubControls<ToolStripDropButton>(form);
            //-----------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------
        }
        //=======================================================================================
        public static class Drawing
        {
            //-----------------------------------------------------------------------------------
            public static GraphicsPath RoundedRectangle(Rectangle bounds, int radiusTopLeft, int radiusTopRight, int radiusBottomRight, int radiusBottomLeft)
            {
                int diameterTopLeft = radiusTopLeft * 2;
                int diameterTopRight = radiusTopRight * 2;
                int diameterBottomRight = radiusBottomRight * 2;
                int diameterBottomLeft = radiusBottomLeft * 2;

                Rectangle arc1 = new(bounds.Location, new Size(diameterTopLeft, diameterTopLeft));
                Rectangle arc2 = new(bounds.Location, new Size(diameterTopRight, diameterTopRight));
                Rectangle arc3 = new(bounds.Location, new Size(diameterBottomRight, diameterBottomRight));
                Rectangle arc4 = new(bounds.Location, new Size(diameterBottomLeft, diameterBottomLeft));
                GraphicsPath path = new();

                // Top Left Arc  
                if (radiusTopLeft == 0)
                {
                    path.AddLine(arc1.Location, arc1.Location);
                }
                else
                {
                    path.AddArc(arc1, 180, 90);
                }
                // Top Right Arc  
                arc2.X = bounds.Right - diameterTopRight;
                if (radiusTopRight == 0)
                {
                    path.AddLine(arc2.Location, arc2.Location);
                }
                else
                {
                    path.AddArc(arc2, 270, 90);
                }
                // Bottom Right Arc
                arc3.X = bounds.Right - diameterBottomRight;
                arc3.Y = bounds.Bottom - diameterBottomRight;
                if (radiusBottomRight == 0)
                {
                    path.AddLine(arc3.Location, arc3.Location);
                }
                else
                {
                    path.AddArc(arc3, 0, 90);
                }
                // Bottom Left Arc 
                arc4.X = bounds.Right - diameterBottomLeft;
                arc4.Y = bounds.Bottom - diameterBottomLeft;
                arc4.X = bounds.Left;
                if (radiusBottomLeft == 0)
                {
                    path.AddLine(arc4.Location, arc4.Location);
                }
                else
                {
                    path.AddArc(arc4, 90, 90);
                }
                path.CloseFigure();
                return path;
            }
            //-----------------------------------------------------------------------------------
            public static Bitmap Invert(Bitmap source)
            {
                //create a blank bitmap the same size as original
                Bitmap newBitmap = new(source.Width, source.Height);
                //get a graphics object from the new image
                Graphics g = Graphics.FromImage(newBitmap);
                // create the negative color matrix
                ColorMatrix colorMatrix = new(new float[][]
                {
                    new float[] {-1, 0, 0, 0, 0},
                    new float[] {0, -1, 0, 0, 0},
                    new float[] {0, 0, -1, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {1, 1, 1, 0, 1}
                });
                // create some image attributes
                ImageAttributes attributes = new();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                            0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                //dispose the Graphics object
                g.Dispose();
                return newBitmap;
            }
            //-----------------------------------------------------------------------------------
            [DllImport("user32.dll")]
            private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
            private const int WM_SETREDRAW = 11;
            public static void SuspendDrawing(Control parent)
            {
                _ = SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            }
            public static void ResumeDrawing(Control parent)
            {
                _ = SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                parent.Refresh();
            }
            //-----------------------------------------------------------------------------------
        }
        //=======================================================================================
        public static class ProcessManager
        {
            //-----------------------------------------------------------------------------------
            public static void SetProcessPriority(ProcessPriorityClass processPriorityClass)
            {
                Process.GetCurrentProcess().PriorityClass = processPriorityClass;
            }
            //-----------------------------------------------------------------------------------
            [Flags]
            private enum ThreadAccess : int
            {
                TERMINATE = (0x0001),
                SUSPEND_RESUME = (0x0002),
                GET_CONTEXT = (0x0008),
                SET_CONTEXT = (0x0010),
                SET_INFORMATION = (0x0020),
                QUERY_INFORMATION = (0x0040),
                SET_THREAD_TOKEN = (0x0080),
                IMPERSONATE = (0x0100),
                DIRECT_IMPERSONATION = (0x0200)
            }

            [DllImport("kernel32.dll")]
            private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

            [DllImport("kernel32.dll")]
            private static extern uint SuspendThread(IntPtr hThread);

            [DllImport("kernel32.dll")]
            private static extern int ResumeThread(IntPtr hThread);

            [DllImport("kernel32.dll")]
            private static extern int CloseHandle(IntPtr hThread);

            public static void ThrottleProcess(int processId, double limitPercent)
            {
                var process = Process.GetProcessById(processId);
                var processName = process.ProcessName;
                var p = new PerformanceCounter("Process", "% Processor Time", processName);
                Task.Run(() =>
                {
                    while (true)
                    {
                        var interval = 100;
                        Thread.Sleep(interval);
                        var currentUsage = p.NextValue() / Environment.ProcessorCount;
                        Console.WriteLine(currentUsage);
                        if (currentUsage < limitPercent) continue;
                        var suspensionTime = (currentUsage - limitPercent) / currentUsage * interval;
                        SuspendProcess(processId);
                        Thread.Sleep((int)suspensionTime);
                        ResumeProcess(processId);
                    }
                });
            }
            public static void SuspendProcess(int pId)
            {
                var process = Process.GetProcessById(pId);
                SuspendProcess(process);
            }
            public static void SuspendProcess(Process process)
            {
                foreach (ProcessThread thread in process.Threads)
                {
                    var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                    if (pOpenThread == IntPtr.Zero)
                    {
                        break;
                    }
                    _ = SuspendThread(pOpenThread);
                }
            }
            public static void ResumeProcess(int pId)
            {
                var process = Process.GetProcessById(pId);
                ResumeProcess(process);
            }
            public static void ResumeProcess(Process process)
            {
                foreach (ProcessThread thread in process.Threads)
                {
                    var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                    if (pOpenThread == IntPtr.Zero)
                    {
                        break;
                    }
                    _ = ResumeThread(pOpenThread);
                }
            }
            //-----------------------------------------------------------------------------------
        }
        //=======================================================================================
        public static class Info
        {
            public static readonly string CurrentPath = AppContext.BaseDirectory;
            public static readonly string CurrentPath2 = Path.GetDirectoryName(Application.ExecutablePath);
            public static readonly string ApplicationName = Path.GetFileName(Application.ExecutablePath);
            public static readonly string ApplicationNameWithoutExtension = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            public static readonly string ApplicationFullPath = Application.ExecutablePath;
            public static readonly string ApplicationFullPathWithoutExtension = Path.Combine(CurrentPath, ApplicationNameWithoutExtension);
            public static AssemblyName CallingAssemblyName => Assembly.GetCallingAssembly().GetName();
            public static AssemblyName EntryAssemblyName => Assembly.GetEntryAssembly().GetName();
            public static AssemblyName ExecutingAssemblyName => Assembly.GetExecutingAssembly().GetName();
            public static FileVersionInfo InfoCallingAssembly => FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location);
            public static FileVersionInfo InfoEntryAssembly => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            public static FileVersionInfo InfoExecutingAssembly => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            public static bool IsRunningOnWindows
            {
                get
                {
                    var platform = GetPlatform();
                    if (platform == OSPlatform.Windows)
                        return true;
                    else
                        return false;
                }
            }

            public static bool IsRunningOnLinux
            {
                get
                {
                    var platform = GetPlatform();
                    if (platform == OSPlatform.Linux)
                        return true;
                    else
                        return false;
                }
            }

            public static bool IsRunningOnMac
            {
                get
                {
                    var platform = GetPlatform();
                    if (platform == OSPlatform.OSX)
                        return true;
                    else
                        return false;
                }
            }

            private static OSPlatform GetPlatform()
            {
                // Current versions of Mono report MacOSX platform as Unix
                return Environment.OSVersion.Platform == PlatformID.MacOSX || (Environment.OSVersion.Platform == PlatformID.Unix && Directory.Exists("/Applications") && Directory.Exists("/System") && Directory.Exists("/Users"))
                     ? OSPlatform.OSX
                     : Environment.OSVersion.Platform == PlatformID.Unix
                     ? OSPlatform.Linux
                     : OSPlatform.Windows;
            }
        }
        //=======================================================================================
        public static class Openlinks
        {
            public static void OpenFolderFromFileName(string fileName)
            {
                string folderName = Path.GetDirectoryName(fileName);
                if (Info.IsRunningOnWindows)
                {
                    var argument = @"/select, " + fileName;
                    Process.Start("explorer.exe", argument);
                }
                else
                {
                    OpenFolder(folderName);
                }
            }

            public static void OpenFolder(string folder)
            {
                OpenItem(folder, "folder");
            }

            public static void OpenUrl(string url)
            {
                OpenItem(url, "url");
            }

            public static void OpenFile(string file)
            {
                OpenItem(file, "file");
            }

            public static void OpenItem(string item, string type)
            {
                try
                {
                    if (Info.IsRunningOnWindows || Info.IsRunningOnMac)
                    {
                        var startInfo = new ProcessStartInfo(item)
                        {
                            UseShellExecute = true
                        };

                        Process.Start(startInfo);
                    }
                    else if (Info.IsRunningOnLinux)
                    {
                        var process = new Process
                        {
                            EnableRaisingEvents = false,
                            StartInfo = { FileName = "xdg-open", Arguments = item }
                        };
                        process.Start();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Cannot open {type}: {item}{Environment.NewLine}{Environment.NewLine}{exception.Source}: {exception.Message}", "Error opening URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //=======================================================================================
        public static class Xml
        {
            //-----------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------
            public static void RemoveNodesWithoutChild(string xmlFile)
            {
                if (File.Exists(xmlFile))
                {
                    bool isXmlValid = IsValidXML(File.ReadAllText(xmlFile));
                    if (isXmlValid == true)
                    {
                        XmlDocument doc = new();
                        doc.Load(xmlFile);
                        var nodes = doc.DocumentElement;
                        foreach (XmlNode node in nodes)
                            if (node.HasChildNodes == false)
                                nodes.RemoveChild(node);
                        doc.Save(xmlFile);
                    }
                }
                else
                    Console.WriteLine("XML File Not Exist.");
            }
            //-----------------------------------------------------------------------------------
            public static bool IsValidXML(string content)
            {
                try
                {
                    if (string.IsNullOrEmpty(content) == false)
                    {
                        XmlDocument xmlDoc = new();
                        xmlDoc.LoadXml(content);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (XmlException ex)
                {
                    Console.WriteLine("XML Error: " + ex.Message);
                    return false;
                }
            }
            //-----------------------------------------------------------------------------------
        }
        //=======================================================================================
        public class ScrollBar
        {
            //-----------------------------------------------------------------------------------
            // Is Vertical Scrollbar Visible
            private const int WS_VSCROLL = 0x200000;
            private const int GWL_STYLE = -16;
            public static bool IsVScrollbarVisible(IntPtr hWnd)
            {
                int nMessage = WS_VSCROLL;
                int nStyle = NativeMethods.GetWindowLong(hWnd, GWL_STYLE);
                bool bVisible = (nStyle & nMessage) != 0;
                return bVisible;
            } // Usage: IsVScrollbarVisible(ListView1.Handle);
            //-----------------------------------------------------------------------------------
            // Is Horizontal Scrollbar Visible
            private const int WS_HSCROLL = 0x100000;
            public static bool IsHScrollbarVisible(IntPtr hWnd)
            {
                int nMessage = WS_HSCROLL;
                int nStyle = NativeMethods.GetWindowLong(hWnd, GWL_STYLE);
                bool bVisible = (nStyle & nMessage) != 0;
                return bVisible;
            } // Usage: IsHScrollbarVisible(ListView1.Handle);
            //-----------------------------------------------------------------------------------
        }
        //=======================================================================================
        public class HTML
        {
            public static string RemoveHtmlFontTag(string s)
            {
                s = s.Replace("</font>", string.Empty);
                s = s.Replace("</FONT>", string.Empty);
                s = s.Replace("</Font>", string.Empty);
                s = s.Replace("<font>", string.Empty);
                s = s.Replace("<FONT>", string.Empty);
                s = s.Replace("<Font>", string.Empty);

                while (s.ToLower().Contains("<font"))
                {
                    int startIndex = s.ToLower().IndexOf("<font");
                    int endIndex = Math.Max(s.IndexOf(">"), startIndex + 4);
                    s = s.Remove(startIndex, (endIndex - startIndex) + 1);
                }
                return s;
            }
            //-----------------------------------------------------------------------------------
            public static string? RemoveHtmlTags(string s)
            {
                if (string.IsNullOrEmpty(s))
                    return null;
                //if (s == null)
                //    return null;
                if (!s.Contains('<'))
                    return s;
                s = s.Replace("<i>", string.Empty);
                s = s.Replace("</i>", string.Empty);
                s = s.Replace("<b>", string.Empty);
                s = s.Replace("</b>", string.Empty);
                s = s.Replace("<u>", string.Empty);
                s = s.Replace("</u>", string.Empty);
                s = s.Replace("<I>", string.Empty);
                s = s.Replace("</I>", string.Empty);
                s = s.Replace("<B>", string.Empty);
                s = s.Replace("</B>", string.Empty);
                s = s.Replace("<U>", string.Empty);
                s = s.Replace("</U>", string.Empty);
                s = RemoveParagraphTag(s);
                return RemoveHtmlFontTag(s).Trim();
            }
            //-----------------------------------------------------------------------------------
            internal static string GetHtmlColorCode(Color color)
            {
                return string.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
            }
            //-----------------------------------------------------------------------------------
            internal static string RemoveBrackets(string inputString)
            {
                string pattern = @"^[\[\{\(]|[\]\}\)]$";
                return Regex.Replace(inputString, pattern, string.Empty).Trim();
            }
            //-----------------------------------------------------------------------------------
            internal static string RemoveParagraphTag(string s)
            {
                s = s.Replace("</p>", string.Empty);
                s = s.Replace("</P>", string.Empty);
                s = s.Replace("<P>", string.Empty);
                s = s.Replace("<P>", string.Empty);

                while (s.ToLower().Contains("<p "))
                {
                    int startIndex = s.ToLower().IndexOf("<p ");
                    int endIndex = Math.Max(s.IndexOf(">"), startIndex + 4);
                    s = s.Remove(startIndex, endIndex - startIndex + 1);
                }
                return s;
            }
        }
        //=======================================================================================
        public class Files
        {
            //-----------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------
            public static bool CompareByLength(string path1, string path2)
            {
                int path1Length = File.ReadAllText(path1).Length;
                int path2Length = File.ReadAllText(path2).Length;
                if (path1Length == path2Length)
                    return true;
                else
                    return false;
            }
            //-----------------------------------------------------------------------------------
            public static bool CompareByReadBytes(string path1, string path2)
            {
                byte[] path1Bytes = File.ReadAllBytes(path1);
                byte[] path2Bytes = File.ReadAllBytes(path2);
                if (path1Bytes == path2Bytes)
                    return true;
                else
                    return false;
            }
            //-----------------------------------------------------------------------------------
            public static bool CompareByUTF8Bytes(string path1, string path2)
            {
                byte[] path1Bytes = Encoding.UTF8.GetBytes(path1);
                byte[] path2Bytes = Encoding.UTF8.GetBytes(path2);
                if (path1Bytes == path2Bytes)
                    return true;
                else
                    return false;
            }
            //-----------------------------------------------------------------------------------
            public static bool CompareByCRC(string path1, string path2)
            {
                string path1CRC = GetCRC32(path1);
                string path2CRC = GetCRC32(path2);
                if (path1CRC == path2CRC)
                    return true;
                else
                    return false;
            }
            //-----------------------------------------------------------------------------------
            public static bool CompareBySHA512(string path1, string path2)
            {
                string path1CRC = GetSHA512(path1);
                string path2CRC = GetSHA512(path2);
                if (path1CRC == path2CRC)
                    return true;
                else
                    return false;
            }
            //-----------------------------------------------------------------------------------
            public static bool CompareByReadLines(string path1, string path2)
            {
                return File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));
            }
            //-----------------------------------------------------------------------------------
            public static async Task WriteAllTextAppendAsync(string filePath, string fileContent, Encoding encoding)
            {
                if (File.Exists(filePath) == true)
                {
                    using FileStream fileStream = new(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    using StreamWriter writer = new(fileStream, encoding);
                    await writer.WriteAsync(fileContent);
                }
                else
                    Console.WriteLine("File Not Exist: " + Path.GetFileName(filePath));
            }
            //-----------------------------------------------------------------------------------
            public static void WriteAllText(string filePath, string fileContent, Encoding encoding)
            {
                using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using StreamWriter writer = new(fileStream, encoding);
                //fileStream.SetLength(0); // Overwrite File When FileMode is FileMode.OpenOrCreate
                writer.AutoFlush = true;
                writer.Write(fileContent);
            }
            //-----------------------------------------------------------------------------------
            public static async Task WriteAllTextAsync(string filePath, string fileContent, Encoding encoding)
            {
                using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using StreamWriter writer = new(fileStream, encoding);
                //fileStream.SetLength(0); // Overwrite File When FileMode is FileMode.OpenOrCreate
                writer.AutoFlush = true;
                await writer.WriteAsync(fileContent);
            }
            //-----------------------------------------------------------------------------------
            public static bool IsFileLocked(string fileNameOrPath)
            {
                string filePath = Path.GetFullPath(fileNameOrPath);
                if (File.Exists(filePath))
                {
                    FileStream stream = null;
                    FileInfo fileInfo = new(filePath);
                    try
                    {
                        stream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
                    {
                        Console.WriteLine("File is in use by another process.");
                        return true;
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                    //file is not locked
                    return false;
                }
                else
                {
                    Console.WriteLine("File not exist: " + filePath);
                    return false;
                }
            }
            //-----------------------------------------------------------------------------------
            public static List<string>? FindFilesByPartialName(string partialName, string dirPath)
            {
                if (Directory.Exists(dirPath))
                {
                    DirectoryInfo hdDirectoryInWhichToSearch = new(dirPath);
                    FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");
                    List<string> list = new();
                    foreach (FileInfo foundFile in filesInDir)
                    {
                        string fullName = foundFile.FullName;
                        list.Add(fullName);
                    }
                    return list;
                }
                Console.WriteLine("Directory Not Exist: " + dirPath);
                return null;
            }
            //-----------------------------------------------------------------------------------
            public static string GetCRC32(string filePath)
            {
                if (File.Exists(filePath))
                {
                    var bytes = ReadAllBytes(filePath);
                    uint crc32 = Crc32Algorithm.Compute(bytes);
                    return crc32.ToString();
                }
                else
                {
                    Console.WriteLine("File Not Exist: " + Path.GetFileName(filePath));
                    return string.Empty;
                }
            }
            //-----------------------------------------------------------------------------------
            public static string GetSHA512(string filePath)
            {
                if (File.Exists(filePath))
                {
                    var bytes = ReadAllBytes(filePath);
                    using var hash = SHA512.Create();
                    var hashedInputBytes = hash.ComputeHash(bytes);
                    // Convert to text
                    // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                    var hashedInputStringBuilder = new StringBuilder(128);
                    foreach (var b in hashedInputBytes)
                        hashedInputStringBuilder.Append(b.ToString("X2"));
                    return hashedInputStringBuilder.ToString();
                }
                return string.Empty;
            }
            //-----------------------------------------------------------------------------------
            public static byte[] ReadAllBytes(MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }
            //-----------------------------------------------------------------------------------
            public static byte[]? ReadAllBytes(string filePath)
            {
                try
                {
                    using FileStream fsSource = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    // Read the source file into a byte array.
                    byte[] bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = bytes.Length;
                    return bytes;
                }
                catch (FileNotFoundException ioEx)
                {
                    Console.WriteLine(ioEx.Message);
                    return null;
                }
            }
            //-----------------------------------------------------------------------------------
            public static async Task<byte[]?> ReadAllBytesAsync(string filePath)
            {
                try
                {
                    using FileStream fsSource = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    // Read the source file into a byte array.
                    byte[] bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = await fsSource.ReadAsync(bytes.AsMemory(numBytesRead, numBytesToRead));
                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = bytes.Length;
                    return bytes;
                }
                catch (FileNotFoundException ioEx)
                {
                    Console.WriteLine(ioEx.Message);
                    return null;
                }
            }
            //-----------------------------------------------------------------------------------
            public static void WriteAllBytes(string filePath, byte[] bytes)
            {
                try
                {
                    int numBytesToRead = bytes.Length;
                    // Write the byte array to the other FileStream.
                    using FileStream fsNew = new(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    fsNew.Write(bytes, 0, numBytesToRead);
                }
                catch (FileNotFoundException ioEx)
                {
                    Console.WriteLine(ioEx.Message);
                }
            }
            //-----------------------------------------------------------------------------------
            public static async Task WriteAllBytesAsync(string filePath, byte[] bytes)
            {
                try
                {
                    int numBytesToRead = bytes.Length;
                    // Write the byte array to the other FileStream.
                    using FileStream fsNew = new(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    await fsNew.WriteAsync(bytes.AsMemory(0, numBytesToRead));
                }
                catch (FileNotFoundException ioEx)
                {
                    Console.WriteLine(ioEx.Message);
                }
            }
            //-----------------------------------------------------------------------------------
            
            //-----------------------------------------------------------------------------------
        }
        //=======================================================================================
        public class Texts
        {
            public static string? GetTextByLineNumber(string text, int lineNo)
            {
                string[] lines = text.Replace("\r", "").Split('\n');
                return lines.Length >= lineNo ? lines[lineNo - 1] : null;
            }
        }
        //=======================================================================================
        public class IsolatedStorage
        {
            //-----------------------------------------------------------------------------------

            //-----------------------------------------------------------------------------------
            public static IDictionary<string, string> DicLineByLine(string fileName)
            {
                string read = ReadIsolatedTextFile(fileName);
                var split1 = read.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                IDictionary<string, string> Dic = new Dictionary<string, string>();
                int a = 0;
                int b = 1;
                for (; b < split1.Length; a += 2, b += 2)
                {
                    if (!Dic.ContainsKey(split1[a]))
                        Dic.Add(split1[a], split1[b]);
                }
                return Dic;
            }
            //-----------------------------------------------------------------------------------
            public static List<string> ListLineByLine(string fileName)
            {
                string read = ReadIsolatedTextFile(fileName);
                var split1 = read.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                List<string> list = new();
                foreach (var line in split1)
                    list.Add(line);
                return list;
                // Usage: List<string> items = ListLineByLine();
            }
            //-----------------------------------------------------------------------------------
            public static int? CountLines(string fileName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                if (isoStore.FileExists(fileName))
                {
                    using IsolatedStorageFileStream isoStream = new(fileName, FileMode.Open, isoStore);
                    using StreamReader reader = new(isoStream);
                    int count = 0;
                    while (reader.ReadLine() != null)
                    {
                        count++;
                    }
                    isoStore.Close();
                    return count;
                }
                return null;
            }
            //-----------------------------------------------------------------------------------
            public static int FilesTotalNumber
            {
                get
                {
                    IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                    int count = 0;
                    // Retrieve all the files in the directory by calling the GetAllFiles
                    foreach (string file in GetAllFiles("*", isoStore))
                    {
                        if (isoStore.FileExists(file))
                            count++;
                    }
                    return count;
                }
            }
            //-----------------------------------------------------------------------------------
            public static bool IsFileExist(string fileName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                if (isoStore.FileExists(fileName))
                {
                    isoStore.Close();
                    //Console.WriteLine("File Exist: " + fileName);
                    return true;
                }
                isoStore.Close();
                Console.WriteLine("File Not Exist: " + fileName);
                return false;
            }
            public static bool IsDirectoryExist(string directoryName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                if (isoStore.DirectoryExists(directoryName))
                {
                    isoStore.Close();
                    Console.WriteLine("Directory Exist: " + directoryName);
                    return true;
                }
                isoStore.Close();
                Console.WriteLine("Directory Not Exist: " + directoryName);
                return false;
            }
            //-----------------------------------------------------------------------------------
            public static string GetCRC32(string fileName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                if (isoStore.FileExists(fileName))
                {
                    IsolatedStorageFileStream isoStream = new(fileName, FileMode.Open, isoStore);
                    StreamReader reader = new(isoStream);
                    var r = reader.ReadToEnd();
                    var bytes = Encoding.UTF8.GetBytes(r);
                    uint crc32 = Crc32Algorithm.Compute(bytes);
                    reader.Close();
                    isoStream.Close();
                    isoStore.Close();
                    return crc32.ToString();
                }
                Console.WriteLine("File Not Exist: " + fileName);
                return string.Empty;
            }
            //-----------------------------------------------------------------------------------
            public static string GetSHA512(string fileName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                if (isoStore.FileExists(fileName))
                {
                    IsolatedStorageFileStream isoStream = new(fileName, FileMode.Open, isoStore);
                    StreamReader reader = new(isoStream);
                    var r = reader.ReadToEnd();
                    var bytes = Encoding.UTF8.GetBytes(r);
                    using var hash = SHA512.Create();
                    var hashedInputBytes = hash.ComputeHash(bytes);
                    // Convert to text
                    // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                    var hashedInputStringBuilder = new StringBuilder(128);
                    foreach (var b in hashedInputBytes)
                        hashedInputStringBuilder.Append(b.ToString("X2"));
                    reader.Close();
                    isoStream.Close();
                    isoStore.Close();
                    return hashedInputStringBuilder.ToString();
                }
                return string.Empty;
            }
            //-----------------------------------------------------------------------------------
            public static async void SaveIsolatedTextFile(string fileName, string content, Encoding encoding)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                //if (isoStore.FileExists("multiple_replace.xml")) { isoStore.DeleteFile("multiple_replace.xml"); }
                using IsolatedStorageFileStream isoStream = new(fileName, FileMode.Create, isoStore);
                using StreamWriter writer = new(isoStream, encoding);
                await writer.WriteAsync(content);
                isoStore.Close();
            }
            //-----------------------------------------------------------------------------------
            public static void SaveIsolatedTextFileAppend(string fileName, string content)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                //if (isoStore.FileExists("multiple_replace.xml")) { isoStore.DeleteFile("multiple_replace.xml"); }
                if (isoStore.FileExists(fileName))
                {
                    using IsolatedStorageFileStream isoStream = new(fileName, FileMode.Append, isoStore);
                    using StreamWriter writer = new(isoStream);
                    writer.Write(content);
                }
                else
                {
                    using IsolatedStorageFileStream isoStream = new(fileName, FileMode.CreateNew, isoStore);
                    using StreamWriter writer = new(isoStream);
                    writer.Write(content);
                }
                isoStore.Close();
            }
            //-----------------------------------------------------------------------------------
            public static string? ReadIsolatedTextFile(string fileName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                //if (isoStore.FileExists("multiple_replace.xml")) { isoStore.DeleteFile("multiple_replace.xml"); }

                if (isoStore.FileExists(fileName))
                {
                    using IsolatedStorageFileStream isoStream = new(fileName, FileMode.Open, FileAccess.Read, isoStore);
                    using StreamReader reader = new(isoStream);
                    var r = reader.ReadToEnd();
                    isoStore.Close();
                    return r;
                }
                else
                {
                    Console.WriteLine("Isolated Storage File Does Not Exist.");
                    isoStore.Close();
                    return null;
                }
            }
            //-----------------------------------------------------------------------------------
            public static void RemoveIsolatedStorage()
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                try
                {
                    isoStore.Remove();
                    Console.WriteLine("Isolated Storage removed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                isoStore.Close();
                isoStore.Dispose();
            }
            //-----------------------------------------------------------------------------------
            public static void DeleteFile(string fileName)
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                // Retrieve all the files in the directory by calling the GetAllFiles
                foreach (string file in GetAllFiles(fileName, isoStore))
                {
                    try
                    {
                        if (isoStore.FileExists(file))
                            isoStore.DeleteFile(file);
                        if (!isoStore.FileExists(file))
                            Console.WriteLine("File Deleted: " + file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        MessageBox.Show("Error: " + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            //-----------------------------------------------------------------------------------
            public static void DeleteIsolatedFilesAndDirectories()
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                // Retrieve all the files in the directory by calling the GetAllFiles
                foreach (string file in GetAllFiles("*", isoStore))
                {
                    try
                    {
                        if (isoStore.FileExists(file))
                            isoStore.DeleteFile(file);
                        if (!isoStore.FileExists(file))
                            Console.WriteLine("File Deleted: " + file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        MessageBox.Show("Error: " + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Retrieve all the directories in Isolated Storage by calling the GetAllDirectories
                foreach (string directory in GetAllDirectories("*", isoStore))
                { // Exception will thrown when directory in not empty or exist, so delete directories after deleting files.
                    try
                    {
                        if (isoStore.DirectoryExists(directory))
                            isoStore.DeleteDirectory(directory);
                        if (!isoStore.DirectoryExists(directory))
                            Console.WriteLine("Directory Deleted: " + directory);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        MessageBox.Show("Error: " + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            //-----------------------------------------------------------------------------------
            // Method to retrieve all directories, recursively, within a store.
            private static List<string> GetAllDirectories(string pattern, IsolatedStorageFile storeFile)
            {
                // Get the root of the search string.
                string root = Path.GetDirectoryName(pattern);
                if (root != "")
                {
                    root += "/";
                }
                // Retrieve directories.
                List<string> directoryList = new(storeFile.GetDirectoryNames(pattern));
                // Retrieve subdirectories of matches.
                for (int i = 0, max = directoryList.Count; i < max; i++)
                {
                    string directory = directoryList[i] + "/";
                    List<string> more = GetAllDirectories(root + directory + "*", storeFile);
                    // For each subdirectory found, add in the base path.
                    for (int j = 0; j < more.Count; j++)
                    {
                        more[j] = directory + more[j];
                    }
                    // Insert the subdirectories into the list and
                    // update the counter and upper bound.
                    directoryList.InsertRange(i + 1, more);
                    i += more.Count;
                    max += more.Count;
                }
                return directoryList;
            } // End of GetAllDirectories.
            //-----------------------------------------------------------------------------------
            private static List<string> GetAllFiles(string pattern, IsolatedStorageFile storeFile)
            {
                // Get the root and file portions of the search string.
                string fileString = Path.GetFileName(pattern);
                List<string> fileList = new(storeFile.GetFileNames(pattern));
                // Loop through the subdirectories, collect matches,
                // and make separators consistent.
                foreach (string directory in GetAllDirectories("*", storeFile))
                {
                    foreach (string file in storeFile.GetFileNames(directory + "/" + fileString))
                    {
                        fileList.Add((directory + "/" + file));
                    }
                }
                return fileList;
            } // End of GetAllFiles.
        }
        //=======================================================================================
        public class Resource
        {
            public static string? GetResourceTextFile(string path)
            {
                if (ResourceExists(path) == true)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
                    path = assembly.GetManifestResourceNames().Single(str => str.EndsWith(path));
                    Stream stream = assembly.GetManifestResourceStream(path);
                    if (stream != null)
                    {
                        using StreamReader reader = new(stream);
                        return reader.ReadToEnd();
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            //-----------------------------------------------------------------------------------
            public static bool ResourceExists(string resourceName)
            {
                string[] resourceNames = null;
                if (resourceNames == null)
                    resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                Console.WriteLine("Resource Exist: " + resourceNames.Contains(resourceName));
                return resourceNames.Contains(resourceName);
            }
        }
        //=======================================================================================
        
        //=======================================================================================
        public class CenterWinDialog : IDisposable
        {
            private int mTries = 0;
            private readonly Form mOwner;
            public CenterWinDialog(Form owner)
            {
                mOwner = owner;
                if (owner.WindowState != FormWindowState.Minimized)
                    owner.BeginInvoke(new MethodInvoker(FindDialog));
            }
            private void FindDialog()
            {
                // Enumerate windows to find the message box
                if (mTries < 0) return;
                EnumThreadWndProc callback = new(CheckWindow);
                if (EnumThreadWindows(GetCurrentThreadId(), callback, IntPtr.Zero))
                {
                    if (++mTries < 10) mOwner.BeginInvoke(new MethodInvoker(FindDialog));
                }
            }
            private bool CheckWindow(IntPtr hWnd, IntPtr lp)
            {
                // Checks if <hWnd> is a dialog
                StringBuilder sb = new(260);
                _ = GetClassName(hWnd, sb, sb.Capacity);
                if (sb.ToString() != "#32770") return true;
                // Got it
                Rectangle frmRect = new(mOwner.Location, mOwner.Size);
                GetWindowRect(hWnd, out RECT dlgRect);
                MoveWindow(hWnd,
                    frmRect.Left + (frmRect.Width - dlgRect.Right + dlgRect.Left) / 2,
                    frmRect.Top + (frmRect.Height - dlgRect.Bottom + dlgRect.Top) / 2,
                    dlgRect.Right - dlgRect.Left,
                    dlgRect.Bottom - dlgRect.Top, true);
                return false;
            }
            public void Dispose()
            {
                mTries = -1;
            }
            // P/Invoke declarations
            private delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lp);
            [DllImport("user32.dll")]
            private static extern bool EnumThreadWindows(int tid, EnumThreadWndProc callback, IntPtr lp);
            [DllImport("kernel32.dll")]
            private static extern int GetCurrentThreadId();
            [DllImport("user32.dll")]
            private static extern int GetClassName(IntPtr hWnd, StringBuilder buffer, int buflen);
            [DllImport("user32.dll")]
            private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
            [DllImport("user32.dll")]
            private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
            private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
        }
        // Usage:
        // using (new CenterWinDialog(this))
        // {
        //      MessageBox.Show("MessageBox is Parent center.");
        // }
        //=======================================================================================
        public class TransparentLabel : Label
        {
            public TransparentLabel()
            {
                SetStyle(ControlStyles.Opaque, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
            }
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams parms = base.CreateParams;
                    parms.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
                    return parms;
                }
            }
        } // Usage: Tools.TransparentLabel MyLabel = new();
        //=======================================================================================
    }
}
