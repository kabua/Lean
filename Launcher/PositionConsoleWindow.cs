using System;
using System.Drawing;
using System.Runtime.InteropServices;
using QuantConnect.Lean.Launcher.Properties;

namespace QuantConnect.Lean.Launcher
{
    public class PositionConsoleWindow
    {
        public PositionConsoleWindow(string settingsKeyName)
        {
            _settings = new Settings { SettingsKey = settingsKeyName };
        }

        public void SetPosition()
        {
            if (_settings.Size.Width > 0.0 && _settings.Size.Height > 0.0)
            {
                var location = _settings.Location;
                var size = _settings.Size;

                var thisConsole = GetConsoleWindow();
                SetWindowPos(thisConsole, 0, location.X, location.Y, size.Width, size.Height, 0);
            }
        }

        public void SavePosition()
        {
            var thisConsole = GetConsoleWindow();
            RECT rect;
            if (GetWindowRect(thisConsole, out rect))
            {
                _settings.Location = new Point(rect.Left, rect.Top);
                _settings.Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);

                _settings.Save();
            }
        }

        private readonly Settings _settings;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}