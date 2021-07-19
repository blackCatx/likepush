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
using System.IO;

namespace ControlForm
{
    public partial class AutoKey : Form
    {
        const string USER32DLL = "User32.dll";
        [DllImport(USER32DLL, EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack lpfn, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("gdi32.dll")]
        private static extern int GetPixel(IntPtr hdc, Point p);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        private List<IntPtr> IntWnd = new List<IntPtr>();

        public static int secMax = 100;
        public static int secMin = 50;

        public static int delayTime = 10;

        private static Thread normalThread = null;
        private static Thread normalThreadA = null;

        private Keys key = new Keys();                    //热键
        private byte valKey  = 0x74;//F5 TEXT2 
        private byte valKey2 = 0x76;//F7 text4
        private byte valKeyx = 0x75;//F6 text5
        private byte valKey4 = 0x77;//F8  text16

        private int vTime = 0;
        private int v2Time = 0;



        private int runTime = 0;
        private int onceCdTime = 0;
        private int countDown = 0;
        private int f11Times = 0;
        private int f11CD = 1000;

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
        int selectColor3 = 0;
        int selectColor4 = 0;


        Point selectPoint1;
        Point selectPoint2;
        Point selectPoint3;
        Point selectPoint4;

        bool isClick = false;

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

            res = Int32.TryParse(textBox12.Text, out onceCdTime);
            if (res)
            {
                onceCdTime = onceCdTime* 60 * 1000;
            }
            else
            {
                onceCdTime = 180 * 60 * 1000;
            }


            res = Int32.TryParse(textBox18.Text, out vTime);
            if (res)
            {
                vTime = vTime * 1000;
            }
            else
            {
                vTime = 0;
            }

            res = Int32.TryParse(textBox17.Text, out v2Time);
            if (res)
            {
                v2Time = v2Time * 1000;
            }
            else
            {
                v2Time = 0;
            }
            //LogF(string.Format("CD 时间 {0}, {1}, {2}", vTime.ToString(), v2Time.ToString(), onceCdTime.ToString()));

            StartUpdate();
        }

        public static void ClickKey(byte key)
        {
            //LogF("开始释放按键！");
            //LogF(key.ToString());
            byte scanDown = (byte)GetScanKey(key);
            keybd_event(key, scanDown, 0, 0);
            Thread.Sleep(50);
            byte scanUp = (byte)GetScanUpKey(key);
            keybd_event(key, scanUp, 0x0002, 0);
            //LogF("结束释放按键！");

        }
        public static void ClickKeyF6(byte key)
        {
            keybd_event(0x75, 0x40, 0, 0);
            Thread.Sleep(50);
            keybd_event(0x75, 0xc0, 0x0002, 0);
        }

        public static void ClickKeyF11(byte key)
        {
            keybd_event(0x80, 0x45, 0, 0);
            Thread.Sleep(50);
            keybd_event(0x80, 0xc5, 0x0002, 0);
        }

        public static void ClickKeyF12(byte key)
        {
            keybd_event(0x81, 0x46, 0, 0);//75 76 77 78 79 80 81
            Thread.Sleep(50);
            keybd_event(0x81, 0xc6, 0x0002, 0);
        }

        public static int GetScanKey(byte key)
        {
            return key - 0x74 + 0x3f;
        }
        public static int GetScanUpKey(byte key)
        {
            return key - 0x74 + 0xbf;
        }

        public void UpdateClick()
        {

            int curTime = secMax;
            Random random = new Random();
            countDown = 0;
            int v2Cd = 0;
            int vCd = 0;

            while (true)
            {
                curTime = random.Next(secMin, secMax);
                //curTime = curTime / 2;
                Thread.Sleep(curTime);
                if(countDown > 0)  countDown -= curTime;
                if (f11CD > 0) f11CD -= curTime;
                if (v2Cd > 0) v2Cd -= curTime;
                if (vCd > 0) vCd -= curTime;

                if (!selectPoint2.IsEmpty)  //第二个按钮按键的判断F7
                {
                    if (IsCostTarget(selectPoint2))
                    {
                        if (v2Cd <= 0)
                        {
                            ClickKey(valKey2);
                            v2Cd = v2Time;
                        }
                    }
                    else
                    {
                        //LogF(string.Format("F7判定点释放失败 x = {0}, y = {1}", selectPoint2.X.ToString(), selectPoint2.Y.ToString()));
                    }

                }
                else
                {
                    //LogF("F7没有选色点！");
                }

                if (!selectPoint1.IsEmpty)//第1个按钮按键的判断F5
                {
                    if (IsCostTarget(selectPoint1))
                    {
                        if (vCd <= 0)
                        {
                            ClickKey(valKey);
                            vCd = vTime;
                        }
                    }
                    else
                    {
                        //LogF(string.Format("F5判定点释放失败 x = {0}, y = {1}", selectPoint1.X.ToString(), selectPoint1.Y.ToString()));
                    }
                }
                else
                {
                    //LogF("F5没有选色点！");
                }

                if (!selectPoint4.IsEmpty) //第3个按钮按键的判断f8
                {
                    if (IsCostTarget(selectPoint4))
                    {
                        if (countDown <= 0 )
                        {
                            ClickKey(valKey4);
                            countDown = onceCdTime;
                            f11Times = 2;
                            f11CD = 1000;

                        }
                    }

                }

                if (f11Times > 0) // F8后会加3次F11
                {
                    if (f11CD <= 0)
                    {
                        ClickKeyF12(0);
                        f11Times--;
                        f11CD = 1000;
                    }
                }

                //SendKeyDownMsg(valKey);
            }
        }

        public Color GetScreenBmpPixel(Point p)
        {
            try
            {

            // 新建一个和屏幕大小相同的图片
                Bitmap CatchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);

            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
                Graphics g = Graphics.FromImage(CatchBmp);

            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));
                Color r = CatchBmp.GetPixel(p.X, p.Y);
                CatchBmp.Dispose();
                g.Dispose();
                return r;
            }
            catch(Exception ex)
            {
                return Color.White;

            }
        }

        public bool IsCostTarget(Point p)
        {
            Color cl = GetScreenBmpPixel(p);
            int rr = cl.R;
            //             int c = GetPixel(hdc, p);
            //             int r = (c & 0xFF);
            //             int g = (c & 0xFF00) / 256;
            //             int b = (c & 0xFF0000) / 65536;
            int r = cl.R;
            int g = cl.G;
            int b = cl.B;
            if (r < 80 && g < 80 && b < 80)
            {
                return true;
            }
            //LogF(string.Format("判定释放点 x = {0}, y = {1},  r={2}, g={3}, b={4}", p.X.ToString(), p.Y.ToString(), r, g, b));

            return false;
        }

        public bool IsDebuffSide(Point p)
        {

            int c = GetPixel(hdc, p);
            int r = (c & 0xFF);
            int g = (c & 0xFF00) / 256;
            int b = (c & 0xFF0000) / 65536;
            if (r > 140 && g < 100 && b < 100)
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
                if(e.KeyValue > 0x73 && e.KeyValue < 0x82)
                {
                    sOutput = e.KeyCode.ToString();
                    key = e.KeyCode;
                    valKey = byte.Parse(e.KeyValue.ToString());
                }

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
                SendMessage(item, WM_DOWN, key, lparam);
                Thread.Sleep(50);
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
            // 新建一个和屏幕大小相同的图片

            Point p = SelectPoint();
            textBox7.Text = p.X.ToString();
            textBox8.Text = p.Y.ToString();
            selectColor = GetPixel(hdc, p);
            selectPoint1 = p;
            //LogF(string.Format("F5选色点！{0} x = {1}, y = {2}", selectColor.ToString(), textBox7.Text, textBox8.Text));

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

                int curTime = (delayTime * 60 * 1000) + 5000;
                while (true)
                {
                    ClickKey(valKeyx);
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
            Int32.TryParse(textBox13.Text, out delayTime);

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
            //LogF(string.Format("F7选色点！{0}", selectColor2.ToString()));

        }

        private void button6_Click(object sender, EventArgs e)
        {

            Point p = SelectPoint();
            textBox14.Text = p.X.ToString();
            textBox15.Text = p.Y.ToString();
            selectColor4 = GetPixel(hdc, p);
            selectPoint4 = p;

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

                Color c = CatchBmp.GetPixel(p.X, p.Y);
                //LogF(string.Format("选择判定释放点 x = {0}, y = {1},  r={2}, g={3}, b={4}", p.X.ToString(), p.Y.ToString(), c.R, c.G, c.B));

                return p;

            }
            return Point.Empty;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Point p = SelectPoint();

            for (int i = 0; i < 100; i++)
            {
                p.X = p.X - i;
                p.Y = p.Y - i;
                if (IsDebuffSide(p))
                {
                    textBox10.Text = p.X.ToString();
                    textBox9.Text = p.Y.ToString();
                    selectColor3 = GetPixel(hdc, p);
                    selectPoint3 = p;
                    MessageBox.Show("找到红色边框");
                    return;
                }
            }
            for (int i = 0; i < 50; i++)
            {
                p.X = p.X + i;
                p.Y = p.Y + i;
                if (IsDebuffSide(p))
                {
                    textBox10.Text = p.X.ToString();
                    textBox9.Text = p.Y.ToString();
                    selectColor3 = GetPixel(hdc, p);
                    selectPoint3 = p;
                    MessageBox.Show("找到红色边框");
                    return;
                }
            }
            MessageBox.Show("没找到红色边框, 重新选点");

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
//             string sOutput = "";
//             if (e.KeyValue != 17 && e.KeyValue != 18 && e.KeyValue != 97)
//             {
//                 sOutput = e.KeyCode.ToString();
//                 valKeyx = byte.Parse(e.KeyValue.ToString());
//             }
// 
//             textBox2.Text = sOutput;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            string sOutput = "";

            if (e.KeyValue != 17 && e.KeyValue != 18 && e.KeyValue != 97)
            {
                if (e.KeyValue > 0x73 && e.KeyValue < 0x82)
                {
                    sOutput = e.KeyCode.ToString();
                    key = e.KeyCode;
                    valKey2 = byte.Parse(e.KeyValue.ToString());
                }

            }

            textBox4.Text = sOutput;
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            string sOutput = "";

            if (e.KeyValue != 17 && e.KeyValue != 18 && e.KeyValue != 97)
            {
                if (e.KeyValue > 0x73 && e.KeyValue < 0x82)
                {
                    sOutput = e.KeyCode.ToString();
                    key = e.KeyCode;
                    valKeyx = byte.Parse(e.KeyValue.ToString());
                }

            }

            textBox5.Text = sOutput;
        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            string sOutput = "";

            if (e.KeyValue != 17 && e.KeyValue != 18 && e.KeyValue != 97)
            {
                if (e.KeyValue > 0x73 && e.KeyValue < 0x82)
                {
                    sOutput = e.KeyCode.ToString();
                    key = e.KeyCode;
                    valKey4 = byte.Parse(e.KeyValue.ToString());
                }

            }

            textBox16.Text = sOutput;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private static int LogF(string slog)
        {
            return 0;
            string _logDir = "c:\\ConvexEditer\\";
            string _logFile = "log_";
            string dt = DateTime.Now.ToString("yyyy_MM_dd");
            string temFile = _logDir + _logFile + dt + ".log";
            FileStream logFile = new FileStream(temFile, FileMode.OpenOrCreate);
            logFile.Seek(0, SeekOrigin.End);
            StreamWriter sw = new StreamWriter(logFile);

            try
            {
                sw.WriteLine(slog);
            }
            catch (IOException e)
            {
               
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }

            return 0;
        }

    }
}
