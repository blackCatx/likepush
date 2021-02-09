using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace ControlForm
{
    public partial class AutoKey : Form
    {
        const string USER32DLL = "User32.dll";
        [DllImport(USER32DLL, EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        [DllImport(USER32DLL, EntryPoint = "MapVirtualKey")]
        public static extern int MapVirtualKey(int bVk, int uMapType);


        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack lpfn, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("gdi32.dll")]
        private static extern int GetPixel(IntPtr hdc, Point p);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        private List<IntPtr> IntWnd = new List<IntPtr>();

        public static int secMax = 60000;
        public static int secMin = 20000;

        private static Thread normalThread = null;
        private static Thread normalThreadA = null;

        private Keys key = new Keys();                    //热键
        private byte valKey = 0x20;

        //WM_CHAR消息是俘获某一个字符的消息
        public static int WM_CHAR = 0x102;
        public static int WM_DOWN = 0x100;
        public static int WM_UP = 0x101;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lparam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd,
        int hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        int uFlags
        );




        Cutter cutter = null;
        static IntPtr hdc;
        int selectColor = 0;
        int selectColor2 = 0;

        Point selectPoint;
        Point selectPoint2;

        bool bCostSecondSkill = false;
        bool bCoolDown = true;
        bool bSafe = true;

        int secondCdtime = 30000;


        public AutoKey()
        {
            InitializeComponent();
            hdc = GetDC(new IntPtr(0));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bool res = Int32.TryParse(txtSecondMin.Text, out secMin);
            if (res)
            {
                secMin = secMin * 100;
            }
            else
            {
                secMin = 1000;
            }
            res = Int32.TryParse(txtSecondMax.Text, out secMax);
            if (res)
            {
                secMax = secMax * 100;
            }
            else
            {
                secMax = 1000;
            }

            bCoolDown = true;
            bCostSecondSkill = false;
            bSafe = false;
            int nTxtTime = 30;
            int.TryParse(textBox6.Text, out nTxtTime);
            secondCdtime = nTxtTime * 1000;
            StartUpdate();
        }

        public static void ClickKey(byte key)
        {
            //            keybd_event(0x20, 0, 0, 0);
            
            keybd_event(0x74, 0x3f, 0, 0);
            Thread.Sleep(100);
            keybd_event(0x74, 0xbf, 0x0002, 0);
            //keybd_event(key, 0, 0, 0);
            //Thread.Sleep(100);
            //keybd_event(key, 0, 0x0002, 0);
        }
        public static void ClickKeyA(byte key)
        {

            keybd_event(0x75, 0x40, 0, 0);
            Thread.Sleep(100);
            keybd_event(0x75, 0xc0, 0x0002, 0);
        }


        public static void ClickKeyF7(byte key)
        {
            keybd_event(0x76, 0x41, 0, 0);
            Thread.Sleep(100);
            keybd_event(0x76, 0xc1, 0x0002, 0);
        }

        public void UpdateClick()
        {

            int curTime = secMax;
            Random random = new Random();
            int tick = 0;
            
            while (true)
            {
                curTime = random.Next(secMin, secMax);    
                Thread.Sleep(curTime);
                tick = tick + curTime;



                if (IsCostTarget(selectPoint2))
                {

//                     if (IsCostTarget(selectPoint))
//                     {
//                         ClickKey(valKey);
//                         bSafe = false;
//                     }
//                     else
//                     {
//                         bSafe = true;
//                     }


                    if (bSafe)
                    {
                        if(tick > secondCdtime)
                        {
                            tick = 0;
                            ClickKeyF7(valKey);
                            bCostSecondSkill = false;

                        }
                    }
                    else
                    {
                        bCostSecondSkill = true;
                    }
                }
                else
                {
                    if (bCostSecondSkill)
                    {
                        if (true)
                        {
                            if (tick > secondCdtime)
                            {
                                tick = 0;
                                ClickKeyF7(valKey);
                                bCostSecondSkill = false;
                            }
                        }
                    }
                }


                if (selectColor != 0)
                {
                    if (IsCostTarget(selectPoint))
                    {
                        bSafe = false;
                        ClickKey(valKey);
                    }
                    else
                    {
                        bSafe = true;
                    }

                }

                //SendKeyDownMsg(valKey);
            }
        }

        public bool IsCostTarget(Point p)
        {

            int c = GetPixel(hdc, p);
            int r = (c & 0xFF);
            int g = (c & 0xFF00) / 256;
            int b = (c & 0xFF0000) / 65536;
            if (r < 80 && g < 80 && b < 80)
            {
                return true;
            }
            return false;
        }

        public void FindProgram()
        {
            string allname = "";

            EnumWindows((IntPtr hwnd, int lParam) =>
            {

                string strName = textProName.Text;
                StringBuilder wName = new StringBuilder(512);
                GetWindowText(hwnd, wName, wName.Capacity);
                if (wName.ToString().Equals(strName))
                {
                    IntWnd.Add(hwnd);
                }
                allname = allname + "\n" + wName.ToString();
                return true;
            }/*)*/, 0);
            
        }

        public void StartUpdate()
        {

            if (normalThread != null && normalThread.IsAlive) return;
            ThreadStart childRef = new ThreadStart(UpdateClick);
            normalThread = new Thread(childRef);
            normalThread.Start();
        }

        public void StopUpdate()
        {
            if (normalThread != null && normalThread.IsAlive)
                normalThread.Abort();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopUpdate();
        }

        private void AutoKey_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopUpdate();
            StopTimer();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            string sOutput = "";
//             if (e.Modifiers.ToString().Length > 0 && e.Modifiers.ToString() != "None")
//             {
//                 sOutput = e.Modifiers.ToString().Replace("Control", "Ctrl");
//                 sarrayMods = Regex.Split(sOutput, ",");
//                 sOutput = sOutput.Replace(",", " + ") + " + ";
//             }
            if (e.KeyValue != 17 && e.KeyValue != 18 && e.KeyValue != 97)
            {
                sOutput = e.KeyCode.ToString();
                key = e.KeyCode;
                valKey = byte.Parse( e.KeyValue.ToString() );
                int scanValup = get_Lparam(valKey, 1);
                int scanValDow = get_Lparam(valKey, 0);
            }

            textBox2.Text = sOutput;
        }
        //按下鼠标左键
        public static int WM_LBUTTONDOWN = 0x201;
        //释放鼠标左键
        public static int WM_LBUTTONUP = 0x202;
        public void SendKeyDownMsg(int key)
        {
            foreach (var item in IntWnd)
            {

                //SendMessage(item, WM_LBUTTONDOWN, 100, 100);
                int lp = 0x01;
                lp = lp | 0x3f << 16;
                int lparam = lp;
                int upLparam = get_Lparam(valKey, 1);
                int downLparam = get_Lparam(valKey, 0);
                SendMessage(item, WM_DOWN, key, lparam);
                Thread.Sleep(100);
                int x = 0x3f + 0x80;//0xbf
                int scUp =0x1 << 31 | 0x1 ;
                lp = scUp | 0xbf << 16;
                lparam = lp;
                SendMessage(item, WM_UP, key, lparam);
                //SendMessage(item, WM_LBUTTONUP, 100, 100);

            }
        }

        public static void SetWindowPos(IntPtr hWnd)
        {
            SetWindowPos(hWnd, -1, 0, 0, 0, 0, 0x4000 | 0x0001 | 0x0002);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Point p = SelectPoint();
            textBox7.Text = p.X.ToString();
            textBox8.Text = p.Y.ToString();
            selectColor = GetPixel(hdc, p);
            selectPoint = p;
        }

        public void StartTimer()
        {

            if (normalThreadA != null && normalThreadA.IsAlive) return;
            ThreadStart childRef = new ThreadStart(TimerClick);
            normalThreadA = new Thread(childRef);
            normalThreadA.Start();
        }
        public void TimerClick()
        {
            if (false)
            {
                int curTime = 2000;
                while (true)
                {

                    SendKeyDownMsg(0x74);
                    Thread.Sleep(curTime);
                }
            }
            else {

                int curTime = 10 * 60 * 1000;
                while (true)
                {
                    ClickKeyA(valKey);
                    Thread.Sleep(curTime);
                }
            }

        }

        public void StopTimer()
        {
            if (normalThreadA != null && normalThreadA.IsAlive)
                normalThreadA.Abort();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            FindProgram();
            StartTimer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Point p = SelectPoint();
            textBox1.Text = p.X.ToString();
            textBox3.Text = p.Y.ToString();
            selectColor2 = GetPixel(hdc, p);
            selectPoint2 = p;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private Point SelectPoint()
        {
            // 新建一个和屏幕大小相同的图片
            Bitmap CatchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);

            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g = Graphics.FromImage(CatchBmp);

            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));

            // 创建截图窗体
            cutter = new Cutter();

            // 指示窗体的背景图片为屏幕图片
            cutter.BackgroundImage = CatchBmp;
            // 显示窗体
            //cutter.Show();
            // 如果Cutter窗体结束，则从剪切板获得截取的图片，并显示在聊天窗体的发送框中
            if (cutter.ShowDialog() == DialogResult.OK)
            {
                Point p = cutter.DownPoint;

                return p;
            }
            return Point.Empty;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Point p = SelectPoint();
            selectColor = GetPixel(hdc, p);
            textBox7.Text = p.X.ToString();
            textBox8.Text = p.Y.ToString();
            selectPoint = p;
            textBox1.Text = p.X.ToString();
            textBox3.Text = p.Y.ToString();
            selectColor2 = selectColor;
            selectPoint2 = p;
        }


        private int get_Lparam(int vk, int flag)
        {
            int scanCode = MapVirtualKey(vk, 0);
            return flag | (scanCode << 16) | (flag << 30) | (flag << 31);
        }
    }
}
