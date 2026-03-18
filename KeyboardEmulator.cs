using System;
using System.Runtime.InteropServices;
using System.Threading;


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
        public KEYBDINPUT ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(
            uint nInputs,
            INPUT[] pInputs,
            int cbSize
        );

        public const int INPUT_KEYBOARD = 1;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_SCANCODE = 0x0008;
    }


    internal class KeyboardEmulator
    {


        private static void SendScanCode(ushort scanCode, bool keyUp = false)
        {

            var input = new INPUT
            {
                type = NativeMethods.INPUT_KEYBOARD,
                ki = new KEYBDINPUT
                {
                    wVk = 0,
                    wScan = scanCode,
                    dwFlags = NativeMethods.KEYEVENTF_SCANCODE |
                              (keyUp ? NativeMethods.KEYEVENTF_KEYUP : 0),
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            };


            NativeMethods.SendInput(1, new[] { input }, Marshal.SizeOf<INPUT>());
        }


        public static void TypeChar(char c)
        {

            bool shift = false;
            ushort scan = ScanCodeMap.Resolve(c, out shift);


            //switch (c)
            //{
            //    // Number row (Shift + number)
            //    case '!': shift = true; scan = 0x02; break; // 1
            //    case '@': shift = true; scan = 0x03; break; // 2
            //    case '#': shift = true; scan = 0x04; break; // 3
            //    case '$': shift = true; scan = 0x05; break; // 4
            //    case '%': shift = true; scan = 0x06; break; // 5
            //    case '^': shift = true; scan = 0x07; break; // 6
            //    case '&': shift = true; scan = 0x08; break; // 7
            //    case '*': shift = true; scan = 0x09; break; // 8
            //    case '(': shift = true; scan = 0x0A; break; // 9
            //    case ')': shift = true; scan = 0x0B; break; // 0

            //    // Punctuation (Shift variants)
            //    case '_': shift = true; scan = 0x0C; break; // -
            //    case '+': shift = true; scan = 0x0D; break; // =
            //    case '{': shift = true; scan = 0x1A; break; // [
            //    case '}': shift = true; scan = 0x1B; break; // ]
            //    case '|': shift = true; scan = 0x2B; break; // \
            //    case ':': shift = true; scan = 0x27; break; // ;
            //    case '"': shift = true; scan = 0x28; break; // '
            //    case '<': shift = true; scan = 0x33; break; // ,
            //    case '>': shift = true; scan = 0x34; break; // .
            //    case '?': shift = true; scan = 0x35; break; // /

            //    default:
            //        scan = ScanCodeMap.Resolve(c, out shift);
            //        break;
            //}


            if (shift) SendScanCode(0x2A);      // Shift down
            SendScanCode(scan);                 // Key down
            SendScanCode(scan, keyUp: true);    // Key up
            if (shift) SendScanCode(0x2A, true); // Shift up

            Thread.Sleep(5); // human‑like pacing
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
