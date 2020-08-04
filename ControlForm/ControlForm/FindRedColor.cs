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
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int keyCode);
        //WM_CHAR消息是俘获某一个字符的消息
        public static int WM_CHAR = 0x102;
        public static int WM_DOWN = 0x100;
        public static int WM_UP = 0x101;

        public static int xSpace = 100;
        public static int ySpace = 50;
        public static int xStart = 0;
        public static int yStart = 0;
        public static int skillCostTime = 2500;
        public static int teamCount = 2;
        public static int lastTeamPlayerCount = 10;

        private static Thread normalThread;
        private static Thread waterThread;
        static IntPtr hdc;
        private static List<IntPtr> IntWnd = new List<IntPtr>();
        private static bool  isInit = false;
        private static bool isRunning = false;
        private static bool isDrinking = false;

        public static void InitHdc(int count, int x, int y, int len, int height, double costTime = 2.5)
        {
            xSpace = len;
            ySpace = height;
            xStart = x + (int)(xSpace* 0.9);
            yStart = y + ySpace / 2;
            hdc = GetDC(new IntPtr(0));
            skillCostTime = (int)(costTime * 1000);
            CountTeamCount(count);
            if (isInit)
                return;
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
            isInit = true;
        }


        public static void CountTeamCount(double count)
        {
            teamCount = (int) Math.Ceiling( (double)(count / 10) ) ;
            lastTeamPlayerCount =(int) count % 10;
        }

        public static bool IsCostTarget(Point p)
        {

            int c = GetPixel(hdc, p);
            int r = (c & 0xFF);
            int g = (c & 0xFF00)/256;
            int b = (c & 0xFF0000)/65536;
            if (r < 30 && g < 30 && b < 30)
            {
                return true;
            }
            return false;
        }

        public static int FindCostTarget()
        {
            Point p = new Point(0, 0);
            int iMax = teamCount;
            int jMax = 10;
            int x = 0;
            int y = 0;
            for(int i = 0; i < iMax; i++)
            {
                x = xSpace * i + xStart;
                if(i == iMax - 1)
                {
                    jMax = lastTeamPlayerCount == 0 ? 10 : lastTeamPlayerCount; 
                }
                for(int j = 0; j < jMax; j++)
                {
                    y = ySpace * j + yStart;
                    p.X = x;
                    p.Y = y;
                    System.Threading.Thread.Sleep(10);
                    if (IsCostTarget(p))
                    {
                        SetCursorPos(x, y);
                        System.Threading.Thread.Sleep(10);
//                        string d = DateTime.Now.ToLongTimeString();
  //                      System.Console.WriteLine("{0}, \tPoint>>>>, {1}, {2} ", d, p.X, p.Y);
                        if (isDrinking) break;
                        CostSkillStop(p);
                    }

                    //SetCursorPos(x, y);//TestCode
                    //Thread.Sleep(100); //TestCode
                }
                if (GetKeyDownState(0x30))
                {
                    Thread.Sleep(10000);
                }
                if (GetKeyDownState(123))
                {
                    StopFindThread();
                }
            }
            return 0;

        }

        public static int GetSkillLevel(Point p)
        {
            p.X = p.X - (int)(xStart * 0.4);
            if (IsCostTarget(p))
                return 0x32;
            return 0x31;
        }

        public static void CostSkill( int l)
        {
            SendKeyDownMsg(l);
            Thread.Sleep(skillCostTime);
        }


        public static void CostSkillStop( Point p)
        {
            SendKeyDownMsg(GetSkillLevel(p));
            {
                int checkTime = skillCostTime / 3;
                Thread.Sleep(checkTime);
                if (!IsCostTarget(p))
                {
                    SendKeyDownMsg((0x35));
                    return;
                }
                Thread.Sleep(skillCostTime - checkTime);

            }
        }

        public static void SendKeyDownMsg(int key)
        {
            foreach (var item in IntWnd)
            {
                SendMessage(item, WM_DOWN, key, 0);
                SendMessage(item, WM_UP, key, 0);
            }
        }

        public static void StartHealth()
        {

            if (isRunning) return;
            ThreadStart childRef = new ThreadStart(UpdateFind);
            normalThread = new Thread(childRef);
            normalThread.Start();
            isRunning = true;

        }

        public static void UpdateFind()
        {
            while (true)
            {
                int ret = FindCostTarget();
            }
        }

        public static void Close()
        {
            StopFindThread();
        }

        public static void StopFindThread()
        {
            if (normalThread != null)
                normalThread.Abort();
            isRunning = false;

            if (waterThread != null)
                waterThread.Abort();

        }


        public static bool GetKeyDownState(int key)
        {
            short retVal = GetKeyState((int)key);

            if (retVal < 0)
                return true;
            else
                return false;
        }

        public static void CheckSkillState()
        {

            ThreadStart childRef = new ThreadStart(UpdateSkillState);
            waterThread = new Thread(childRef);
            waterThread.Start();


        }

        public static void UpdateSkillState()
        {
            while (true)
            {
                Thread.Sleep(1000);
                CheckWaterState();
            }
        }

        public static void CheckWaterState()
        {
            Point p = new Point(0, 0);
            if (IsSkillReady(p))
            {
                isDrinking = true;
                Thread.Sleep(6000);
                isDrinking = false;
            }
        }


        public static bool IsSkillReady(Point p)
        {

            int c = GetPixel(hdc, p);
            int r = (c & 0xFF);
            int g = (c & 0xFF00) / 256;
            int b = (c & 0xFF0000) / 65536;
            if (r > 200 && g < 30 && b < 30)
            {
                return false;
            }
            return true;
        }
    }
}
