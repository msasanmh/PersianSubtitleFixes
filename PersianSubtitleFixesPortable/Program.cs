using System.Diagnostics;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

const int SW_HIDE = 0;
//const int SW_SHOW = 5;
var handle = GetConsoleWindow();

ShowWindow(handle, SW_HIDE); // To hide
//ShowWindow(handle, SW_SHOW); // To show

Process.Start(Path.Combine(AppContext.BaseDirectory, "PersianSubtitleFixes", "PersianSubtitleFixes.exe"));

//Console.ReadLine();
