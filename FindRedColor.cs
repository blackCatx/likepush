using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace ControlForm
{
    class FindRedColor
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        private static extern int GetPixel(IntPtr hdc, Point p);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack lpfn, int lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lparam);
        //WM_CHAR消息是俘获某一个字符的消息
        public static int WM_CHAR = 0x102;
        public static int WM_DOWN = 0x100;
        public static int WM_UP = 0x101;

        public static int xSpace = 80;
        public static int ySpace = 40;
        public static int xStart = 0;
        public static int yStart = 0;

        public static int teamCount = 8;

        private static Thread normalThread;
        static IntPtr hdc;
        private static List<IntPtr> IntWnd = new List<IntPtr>();

        public static void initHdc(int count, int x, int y)
        {
            teamCount = count;
            xStart = x + (int)(xSpace* 0.9);
            yStart = y + 20;
            hdc = GetDC(new IntPtr(0));
        }

        public static bool isCostTarget(Point p)
        {

            int c = GetPixel(hdc, p);
            int r = (c & 0xFF);
            int g = (c & 0xFF00)/256;
            int b = (c & 0xFF0000)/65536;
            if (r < 50 && g < 50 && b < 50)
            {
                return true;
            }
            return false;
        }
       

        public static int findCostTarget()
        {
            Point p = new Point(0, 0);
            int iMax = 8;
            int jMax = 5;
            int x = 0;
            int y = 0;
            //int yMax = 1080 / 3;
            for(int i = 0; i < iMax; i++)
            {
                x = xSpace * i + xStart;
                for(int j = 0; j < jMax; j++)
                {
                    y = ySpace * j + yStart;
                    p.X = x;
                    p.Y = y;
                    System.Threading.Thread.Sleep(10);
                    SetCursorPos(x, y);
                    if (isCostTarget(p))
                    {
                        //SetCursorPos(x, y);
                        //System.Threading.Thread.Sleep(10);
                        string d = DateTime.Now.ToLongTimeString();
                        System.Console.WriteLine("{0}, \tPoint>>>>, {1}, {2} ", d, p.X, p.Y);
                        costSkill(0x31);
                    }
                }
            }
            return 0;

        }

        public static void costSkill( int l)
        {
            sendKeyDownMsg(l);
            //Thread.Sleep(2400);

        }

        public static void sendKeyDownMsg(int key)
        {
            foreach (var item in IntWnd)
            {
                SendMessage(item, WM_DOWN, key, 0);
                SendMessage(item, WM_UP, key, 0);
            }
        }

        public static void startHealth()
        {
            EnumWindows((IntPtr hwnd, int lParam) =>
            {
                StringBuilder wName = new StringBuilder(512);
                GetWindowText(hwnd, wName, wName.Capacity);
                if (wName.ToString().Equals("魔兽世界"))
                {
                    IntWnd.Add(hwnd);
                }
                return true;
            }/*)*/, 0);

            ThreadStart childRef = new ThreadStart(update);
            normalThread = new Thread(childRef);
            normalThread.Start();

        }

        public static void update()
        {
            while (true)
            {
                int ret = findCostTarget();
            }
        }

        public static void close()
        {
            if (normalThread != null)
                normalThread.Abort();
        }
    }
}
