using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using IronOcr;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Net.PeerToPeer.Collaboration;
using System.IO.Ports;


namespace ConsoleApp3
{
    internal class Program
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);


        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint WM_LBUTTONDBLCLK = 0x0203;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public struct POINT
        {
            public int X;
            public int Y;
        }

        static void Main(string[] args)
        {

            Random rnd = new Random();
            string targetWindowTitle = "Rappelz"; // Замените на заголовок целевого окна
            int AwakX = 1320;
            int AwakY = 427;
            int ScrollX = 1291;
            int ScrollY = 424;
            int EnchantX = 1167;
            int EnchantY = 465;

            ReAwak:
            IntPtr hWnd = FindWindow(null, targetWindowTitle);
            if (hWnd != IntPtr.Zero)
            {
                Console.WriteLine("Окно найдено ! ");
                SetForegroundWindow(hWnd);
                IntPtr lParam = (IntPtr)((ScrollY << 16) | ScrollX);
                SendMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, lParam);
                SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, lParam);
                SendMessage(hWnd, WM_LBUTTONDBLCLK, IntPtr.Zero, lParam);
                Thread.Sleep(1000);
                lParam = (IntPtr)((EnchantY << 16) | EnchantX);
                SendMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, lParam);
                SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, lParam);
                Console.WriteLine("Scroll");
                Thread.Sleep(2500);
                
                lParam = (IntPtr)((AwakY << 16) | AwakX);
                SendMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, lParam);
                SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, lParam);
                SendMessage(hWnd, WM_LBUTTONDBLCLK, IntPtr.Zero, lParam);
                Console.WriteLine("Awakening Stone");
                Thread.Sleep(1000);
                lParam = (IntPtr)((EnchantY << 16) | EnchantX);
                SendMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, lParam);
                SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, lParam);
                Console.WriteLine("Click Enchant");
                Thread.Sleep(4000);

                RECT windowRect;
                GetWindowRect(hWnd, out windowRect);

                int xTop = 930;
                int xBottom = 1235;
                int yTop = 655;
                int yBottom = 833;


                int width = xBottom - xTop;
                int height = yBottom - yTop;

                int xOffset = 910; // Смещение по горизонтали
                int yOffset = 426; // Смещение по вертикали

                int xOffset2 = 920; // Смещение по горизонтали
                int yOffset2 = 445; // Смещение по вертикали

                int x = windowRect.Left + xOffset;
                int y = windowRect.Top + yOffset;

                int x2 = windowRect.Left + xOffset2;
                int y2 = windowRect.Top + yOffset2;
            Eblan:
                Cursor.Position = new Point(x2, y2);
                Thread.Sleep(1000);
                Cursor.Position = new Point(x, y);
                Thread.Sleep(1000);
                GetWindowRect(hWnd, out windowRect);
                // Вычислите новые координаты, учитывая смещение и положение окна на экране
                try
                { 
                    using (Bitmap bitmap = new Bitmap(width, height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            // Сделайте скриншот указанных координат внутри окна
                            graphics.CopyFromScreen(windowRect.Left + xTop, windowRect.Top + yTop, 0, 0, new Size(width, height));
                        }

                        ReplaceColor(bitmap, "#101111", "#FFFFFF");
                        ReplaceColor(bitmap, "#0C1214", "#000000");
                        ReplaceColor(bitmap, "#454D4D", "#FFFFFF");
                        

                        //	#050505

                        bitmap.Save("screenshot.png");
                    }
                }
                catch 
                {
                    goto Eblan;
                }
                Console.WriteLine("DONE");


                var Ocr = new IronTesseract(); // nothing to configure
                Ocr.Language = OcrLanguage.English;
                Ocr.Configuration.WhiteListCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ+ 1234567890";
                
                using (var Input = new OcrInput())
                {
                    Input.AddImage(@"screenshot.png");
                    var Result = Ocr.Read(Input);
                    Console.WriteLine(Result.Lines.Count());
                    if(Result.Lines.Count() < 5)
                    {
                        goto Eblan;
                    }
                    Console.WriteLine(Result.Text);
                    Console.WriteLine("================================");
                    if (CheckStat(Result.Text))
                    {
                        Console.WriteLine("ZBS");
                        
                    }
                    else
                    {
                        
                        Console.WriteLine("GAVNO");
                        goto ReAwak;
                    }
                   

                }
            }
            else
            {
                Console.WriteLine("Окно не найдено");
            }



            Console.ReadLine();
        }

        static void ReplaceColor(Bitmap image, string targetColorHex, string replacementColorHex)
        {
            Color targetColor = ColorTranslator.FromHtml(targetColorHex);
            Color replacementColor = ColorTranslator.FromHtml(replacementColorHex);

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);

                    if (pixelColor == targetColor)
                    {
                        image.SetPixel(x, y, replacementColor);
                    }
                }
            }
        }

        public static bool CheckStat(string res)
        {
            string pattern1 = @"STRENGTH[ ]*\+([0-9]+)";
            string pattern2 = @"AGILITY[ ]*\+([0-9]+)";
            string pattern3 = @"INT[ ]*\+([0-9]+)";
            string pattern4 = @"VITALITY[ ]*\+([0-9]+)";
            string pattern5 = @"WISDOM[ ]*\+([0-9]+)";
            string pattern6 = @"MOV SPEED[ ]*\+([0-9]+)";
            string pattern7 = @"POWER[ ]*\+([0-9]+)";
            string pattern8 = @"DEXTERITY[ ]*\+([0-9]+)";
            int totalInt = 0;
            int totalAgility = 0;
            int totalStrength = 0;
            int totalVitality = 0;
            int totalWisdom = 0;
            int totalmovSpd = 0;
            int totalPower = 0;
            int totalDexterity = 0;
            MatchCollection matches1 = Regex.Matches(res, pattern1);
            foreach (Match match in matches1)
            {
                if (match.Success)
                {
                    int strengthValue;
                    if (int.TryParse(match.Groups[1].Value, out strengthValue))
                    {
                        totalStrength += strengthValue;
                    }
                }
            }
            MatchCollection matches2 = Regex.Matches(res, pattern2);
            foreach (Match match in matches2)
            {
                if (match.Success)
                {
                    int agiValue;
                    if (int.TryParse(match.Groups[1].Value, out agiValue))
                    {
                        totalAgility += agiValue;
                    }
                }
            }
            MatchCollection matchesAgity = Regex.Matches(res, @"Agity[ ]*\+([0-9]+)");
            foreach (Match match1 in matchesAgity)
            {
                int agiValue;
                if (int.TryParse(match1.Groups[1].Value, out agiValue))
                {
                    totalAgility += agiValue;
                }
            }
            MatchCollection matches3 = Regex.Matches(res, pattern3);
            foreach (Match match in matches3)
            {
                if (match.Success)
                {
                    int intValue;
                    if (int.TryParse(match.Groups[1].Value, out intValue))
                    {
                        totalInt += intValue;
                    }
                }
            }
            MatchCollection matches4 = Regex.Matches(res, pattern4);
            foreach (Match match in matches4)
            {
                if (match.Success)
                {
                    int vitValue;
                    if (int.TryParse(match.Groups[1].Value, out vitValue))
                    {
                        totalVitality += vitValue;
                    }
                }
            }
            MatchCollection matches5 = Regex.Matches(res, pattern5);
            foreach (Match match in matches5)
            {
                if (match.Success)
                {
                    int wisdonValue;
                    if (int.TryParse(match.Groups[1].Value, out wisdonValue))
                    {
                        totalWisdom += wisdonValue;
                    }
                }
            }
            MatchCollection matches6 = Regex.Matches(res, pattern6);
            foreach (Match match in matches6)
            {
                if (match.Success)
                {
                    int SpdValue;
                    if (int.TryParse(match.Groups[1].Value, out SpdValue))
                    {
                        totalmovSpd += SpdValue;
                    }
                }
            }
            MatchCollection matches7 = Regex.Matches(res, pattern7);
            foreach (Match match in matches7)
            {
                if (match.Success)
                {
                    int PowerValue;
                    if (int.TryParse(match.Groups[1].Value, out PowerValue))
                    {
                        totalPower += PowerValue;
                    }
                }
            }
            MatchCollection matches8 = Regex.Matches(res, pattern8);
            foreach (Match match in matches8)
            {
                if (match.Success)
                {
                    int DexterityValue;
                    if (int.TryParse(match.Groups[1].Value, out DexterityValue))
                    {
                        totalDexterity += DexterityValue;
                    }
                }
            }
            Console.WriteLine($"Total Strenght = {totalStrength.ToString()}");
            Console.WriteLine($"Total Int = {totalInt.ToString()}");
            Console.WriteLine($"Total Vitality = {totalVitality.ToString()}");
            Console.WriteLine($"Total Wisdom = {totalWisdom.ToString()}");
            Console.WriteLine($"Total Agility = {totalAgility.ToString()}");
            Console.WriteLine($"Total Dexterity = {totalAgility.ToString()}");
            Console.WriteLine($"Total Mov Speed = {totalmovSpd.ToString()}");
            Console.WriteLine($"Total Crit Power = {totalPower.ToString()}");
            
            int needStat = 40;
            if (totalStrength >= needStat || totalInt >= needStat || totalVitality >= needStat || totalWisdom >= needStat || totalmovSpd >= needStat || totalPower >= needStat || totalAgility >= needStat) 
            {
                return true;   
            }
            if(totalPower >= 20 || totalmovSpd >= 20)
            {
                return true;
            }

            return false;
        }

    }
}
