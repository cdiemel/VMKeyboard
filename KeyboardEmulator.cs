using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace VMKeyboard
{

    internal static class ScanCodeMap
    {
        // US-ANSI keyboard scan codes (Set 1)
        // Returns scan code; outputs whether Shift must be held.
        public static ushort Resolve(char c, out bool shift)
        {
            shift = false;

            // Letters
            if (c >= 'a' && c <= 'z')
                return LetterScanCode(c);

            if (c >= 'A' && c <= 'Z')
            {
                shift = true;
                return LetterScanCode(char.ToLowerInvariant(c));
            }

            // Digits
            if (c >= '0' && c <= '9')
                return DigitScanCode(c);

            // Whitespace / controls (optional but useful)
            switch (c)
            {
                case ' ': return 0x39; // Space
                case '\t': return 0x0F; // Tab
                case '\r': // Enter (CR)
                case '\n': return 0x1C; // Enter (LF)
                case '\b': return 0x0E; // Backspace
            }

            // Punctuation (non-shift and shift)
            switch (c)
            {
                // Number row shift variants
                case '!': shift = true; return 0x02; // 1
                case '@': shift = true; return 0x03; // 2
                case '#': shift = true; return 0x04; // 3
                case '$': shift = true; return 0x05; // 4
                case '%': shift = true; return 0x06; // 5
                case '^': shift = true; return 0x07; // 6
                case '&': shift = true; return 0x08; // 7
                case '*': shift = true; return 0x09; // 8
                case '(': shift = true; return 0x0A; // 9
                case ')': shift = true; return 0x0B; // 0

                // ` key
                case '`': return 0x29;
                case '~': shift = true; return 0x29;

                // - and =
                case '-': return 0x0C;
                case '_': shift = true; return 0x0C;
                case '=': return 0x0D;
                case '+': shift = true; return 0x0D;

                // [ ] and \ (and their shift variants)
                case '[': return 0x1A;
                case '{': shift = true; return 0x1A;
                case ']': return 0x1B;
                case '}': shift = true; return 0x1B;
                case '\\': return 0x2B;
                case '|': shift = true; return 0x2B;

                // ; and '
                case ';': return 0x27;
                case ':': shift = true; return 0x27;
                case '\'': return 0x28;
                case '"': shift = true; return 0x28;

                // , . /
                case ',': return 0x33;
                case '<': shift = true; return 0x33;
                case '.': return 0x34;
                case '>': shift = true; return 0x34;
                case '/': return 0x35;
                case '?': shift = true; return 0x35;
            }

            throw new NotSupportedException(
                $"Character '{c}' (U+{(int)c:X4}) not supported by US ScanCodeMap.");
        }

        private static ushort DigitScanCode(char digit)
        {
            switch (digit)
            {
                case '1': return 0x02;
                case '2': return 0x03;
                case '3': return 0x04;
                case '4': return 0x05;
                case '5': return 0x06;
                case '6': return 0x07;
                case '7': return 0x08;
                case '8': return 0x09;
                case '9': return 0x0A;
                case '0': return 0x0B;
                default: throw new ArgumentOutOfRangeException(nameof(digit));
            }
        }

        private static ushort LetterScanCode(char lower)
        {
            switch(lower)
            {
                case 'a': return 0x1E;
                case 'b': return 0x30;
                case 'c': return 0x2E;
                case 'd': return 0x20;
                case 'e': return 0x12;
                case 'f': return 0x21;
                case 'g': return 0x22;
                case 'h': return 0x23;
                case 'i': return 0x17;
                case 'j': return 0x24;
                case 'k': return 0x25;
                case 'l': return 0x26;
                case 'm': return 0x32;
                case 'n': return 0x31;
                case 'o': return 0x18;
                case 'p': return 0x19;
                case 'q': return 0x10;
                case 'r': return 0x13;
                case 's': return 0x1F;
                case 't': return 0x14;
                case 'u': return 0x16;
                case 'v': return 0x2F;
                case 'w': return 0x11;
                case 'x': return 0x2D;
                case 'y': return 0x15;
                case 'z': return 0x2C;
                default: throw new ArgumentOutOfRangeException(nameof(lower));
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        public uint type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
        // KEYBDINPUT fields and behavior are documented by Microsoft. [2](https://github.com/AbishekPonmudi/PlanqX_EDR-Endpoint-Detection-and-Response)
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }


    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(
            uint nInputs,
            INPUT[] pInputs,
            int cbSize
        );


        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        public static void SendChecked(INPUT[] input)
        {
            uint sent = SendInput(1, input, Marshal.SizeOf<INPUT>());
            if (sent == 0)
            {
                uint err = GetLastError();
                throw new InvalidOperationException($"SendInput failed. GetLastError={err}");
            }
        }

        internal const uint INPUT_MOUSE = 0;
        internal const uint INPUT_KEYBOARD = 1;
        internal const uint INPUT_HARDWARE = 2;

        internal const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        internal const uint KEYEVENTF_KEYUP = 0x0002;
        internal const uint KEYEVENTF_UNICODE = 0x0004;
        internal const uint KEYEVENTF_SCANCODE = 0x0008;

    }


    internal class KeyboardEmulator
    {
        static readonly Random _rng = new Random();

        private static void KeyDelay(int minMs=5, int maxMs=25, bool specialKey=false) => Thread.Sleep(_rng.Next(minMs, maxMs + (specialKey ? 5 : 0)));

        private static void SendScanCode(ushort scanCode, bool keyUp = false, bool extendedKey = false)
        {
            KeyDelay(5,10);

            uint flags = NativeMethods.KEYEVENTF_SCANCODE |
                             (extendedKey ? NativeMethods.KEYEVENTF_EXTENDEDKEY : 0) |
                             (keyUp ? NativeMethods.KEYEVENTF_KEYUP : 0);


            INPUT[] inputs = new INPUT[2];

            inputs[0] = new INPUT

            {
                type = NativeMethods.INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = scanCode,
                        dwFlags = flags,
                        time = 0,
                        dwExtraInfo = UIntPtr.Zero
                    }
                }
            };


            inputs[1] = inputs[0];
            inputs[1].U.ki.dwFlags = flags | NativeMethods.KEYEVENTF_KEYUP;



            try
            {
                NativeMethods.SendChecked(inputs);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }


        public static void TypeChar(char c)
        {

            //bool shift = false;
            ushort scan = ScanCodeMap.Resolve(c, out bool shift);

            if (shift)
            {
                SendScanCode(0x2A);
                Thread.Sleep(5);
            }
            SendScanCode(scan);                 // Key down
            SendScanCode(scan, keyUp: true);    // Key up

            if (shift)
            {
                SendScanCode(0x2A, keyUp: true); 
                Thread.Sleep(5);
            }

        }

        public static void TypeText(string text)
        {
            foreach (char c in text)
            {
                TypeChar(c);
            }
        }

    }
}
