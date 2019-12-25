using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DictionaryGenerator
{
    internal class WinApi
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern short VkKeyScanEx(char ch, IntPtr dwhkl);
    }
}
