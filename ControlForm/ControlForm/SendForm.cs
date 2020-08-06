using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlForm
{
    public partial class SendForm : Form
    {
        public SendForm()
        {
            InitializeComponent();
        }
        //ReceiveForm
        private IntPtr receiveFormIP = new IntPtr(0);
        //ReceiveForm的sendBtn
        private IntPtr reShowBtnIP = new IntPtr(0);
        //保存子控件
        private List<IntPtr> listWnd = new List<IntPtr>();
        private List<IntPtr> IntWnd = new List<IntPtr>();
        private Thread normalThread;
        private bool isInit = false;
        private Dictionary<Keys, bool> moveKeyMap = new Dictionary<Keys, bool>();
        private bool isRunning = false;
        Cutter cutter = null;
        private void BeginBtn_Click(object sender, EventArgs e)
        {
            //             if (receiveFormIP != new IntPtr(0))
            //             {
            //                 MessageBox.Show("ReceiveForm已打开");
            //                 return;
            //             }
            //不用应用ReceiveForm的方式打开  
            //System.Diagnostics.Process.Start(@"..\..\..\ReceiveForm\bin\Debug\ReceiveForm.exe");
            //防止还没打开ReceiveForm就执行后面语句 
            //获取receiveForm
            // int n = 0;
            //             while (n < 25 && receiveFormIP == new IntPtr(0))
            //             {
            //                 receiveFormIP = FindWindow(null, "ReceiveForm");
            //                 System.Threading.Thread.Sleep(100);
            //                 n++;
            //             }
            // 
            //             label1.Text = "已获取ReceiveForm句柄";
            //             label1.ForeColor = Color.Red;
            // 
            //             //获取receiveForm的所有子控件
            //             //CallBack函数(传入的参数）可以为以下三种形式
            //             //1.new CallBack()
            //             //2.degegate
            //             //3.lambda表达式
            //             // return true 是因为CallBack函数设定的返回值为bool类型
            //             EnumChildWindows(receiveFormIP, /*new CallBack(*//*delegate */(IntPtr hwnd, int lParam) =>
            //             {
            //                 listWnd.Add(hwnd);
            //                 return true;
            //             }/*)*/, 0);
            //             //获取receiveForm的按钮
            //             reShowBtnIP = FindWindowEx(receiveFormIP, new IntPtr(0), null, "Show");
            if (isInit) return;
            EnumWindows((IntPtr hwnd, int lParam) =>
            {
                StringBuilder wName = new StringBuilder(512);
                GetWindowText(hwnd, wName, wName.Capacity);
                if (wName.ToString().Equals("x"))
                {
                    IntWnd.Add(hwnd);
                }
                return true;
            }/*)*/, 0);


            ThreadStart childRef = new ThreadStart(UpdateKey);
            normalThread = new Thread(childRef);
            normalThread.Start();
            isInit = true;

        }
        /// <summary>
        /// 校验是否获取ReveiveFrom句柄 有空把它升级为 AOP 或者特性
        /// </summary>
        public void Verify()
        {
            if (receiveFormIP == new IntPtr(0))
            {
                MessageBox.Show("还未获取ReceiveForm句柄,请点击Begin按钮！");
                return;
            }
        }
        public IntPtr GetTextBoxIP(List<IntPtr> listWnd)
        {
            IntPtr textBoxIP = FindWindowEx(receiveFormIP, new IntPtr(0), null, "");
            return textBoxIP;
        }
        //方法一
        /// <summary>
        /// 获取ReceiveForm的textBox
        /// </summary>
        /// <param name="btn">ReceiveForm中的button</param>
        /// <param name="listWnd">ReceiveForm中的控件列表</param>
        /// <returns></returns>
        public IntPtr GetTextBoxIP(IntPtr btn, List<IntPtr> listWnd)
        {
            IntPtr textBoxIP = new IntPtr(0);
            foreach (IntPtr item in listWnd)
            {
                if (item != btn)
                {
                    textBoxIP = item;
                }

            }
            return textBoxIP;
        }
        /// <summary>
        /// 向textBoxIP中发送字符串
        /// </summary>
        /// <param name="textBoxIP">ReceiveForm的textBox控件</param>
        public void Send(IntPtr textBoxIP)
        {
            //char[] message = textBox1.Text.ToArray();
            string message = textBox1.Text;
            SendMessage(textBoxIP, WM_SETTEXT, IntPtr.Zero, message);
        }
        private void SendBtn_Click(object sender, EventArgs e)
        {
            //             Verify();
            //             //将textbox1中信息发送到ReceiveForm中
            //             IntPtr reTextBoxIp = GetTextBoxIP(listWnd);
            //             Send(reTextBoxIp);//改参数最好为 
            if (isInit) return;
            isInit = true;
            FindRedColor.InitHdc(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text), int.Parse(textBox4.Text), int.Parse(textBox5.Text), double.Parse(textBox6.Text), int.Parse(textBox7.Text), int.Parse(textBox8.Text));
            FindRedColor.StartHealth();
        }

        private void UpdateKey()
        {
            while (true)
            {
                Thread.Sleep(100);

                if (GetKeyDownState(Keys.D1))
                {
                    SendKeyDownMsg(0x31);
                }
                if (GetKeyDownState(Keys.D2))
                {
                    SendKeyDownMsg(0x32);
                }
                if (GetKeyDownState(Keys.D3))
                {
                    SendKeyDownMsg(0x33);
                }
                if (GetKeyDownState(Keys.D4))
                {
                    SendKeyDownMsg(0x34);
                }
                if (GetKeyDownState(Keys.D5))
                {
                    SendKeyDownMsg(0x35);
                }
//                 SyncMoveKeyState(Keys.W);
//                 SyncMoveKeyState(Keys.A);
//                 SyncMoveKeyState(Keys.S);
//                 SyncMoveKeyState(Keys.D);


            }
        }
        private void ShowBtn_Click(object sender, EventArgs e)
        {

            //             ThreadStart childRef = new ThreadStart(update);
            //             normalThread = new Thread(childRef);
            //             normalThread.Start();
            //             Verify();
            //             //点击ReceiveForm的sendBtn  弹出textbox中的信息
            //             SendMessage(reShowBtnIP, WM_CLICK, IntPtr.Zero, "0");
            FindRedColor.StopFindThread();
            isInit = false;


        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            //Verify();
            //IntPtr reTextBoxIp = GetTextBoxIP(listWnd);
            //(IntPtr)8 表示删除操作
            //SendMessage(reTextBoxIp, WM_CHAR, (IntPtr)8, "0");
            //发送个""过去，表示清空
            //SendMessage(reTextBoxIp, WM_SETTEXT, 0x31, 0);
            //             SendMessage(reTextBoxIp, WM_DOWN, 0x31, 0);
            //             SendMessage(reTextBoxIp, WM_UP, 0x31, 0);
//             foreach (var item in IntWnd)
//             {
//                 SendMessage(item, WM_DOWN, 0x33, 0);
//                 SendMessage(item, WM_UP, 0x33, 0);
//             }
            //IntPtr sendTextIP = FindWindowEx(sendFormIP, new IntPtr(0), null, "");
        }
        //处理的消息种类
        //按下按键
        public static int WM_CLICK = 0x00F5;
        //WM_CHAR消息是俘获某一个字符的消息
        public static int WM_CHAR = 0x102;
        public static int WM_DOWN = 0x100;
        public static int WM_UP = 0x101;
        //支持发送中文
        private const int WM_SETTEXT = 0x000C;

        //向指定窗口Send Message（判断接收成功后继续执行后续函数）
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lparam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lparam);
        //向指定窗口Post Message（不用判定是否成功接收）
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        //FindWindowEx是在窗口列表中寻找与指定条件相符的第一个子窗口
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack lpfn, int lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        //获取窗口
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int keyCode);

        public enum KeyStates
        {
            None = 0,
            Down = 1, 
            Toggled = 2,
        }


        public static KeyStates GetKeysTates(Keys key)
        {
            KeyStates state = KeyStates.None;
            short retVal = GetKeyState((int)key);

            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            System.Console.WriteLine("{0}, \tkey>>>>, {1}, {2} ", key, (int)state, retVal);
            return state;

        }

        public static bool GetKeyDownState(Keys key)
        {
            short retVal = GetKeyState((int)key);

            if (retVal < 0)
                return true;
            else
                return false;
        }

        public void SyncMoveKeyState(Keys key)
        {
            bool isDown = GetKeyDownState(key);
            if (isDown)
            {
                if (moveKeyMap.ContainsKey(key))
                {
                    if (moveKeyMap[key])
                        return;
                    moveKeyMap[key] = true;
                }
                else
                {
                    moveKeyMap.Add(key, true);
                }
                foreach (var item in IntWnd)
                {
                    SendMessage(item, WM_DOWN, (int)key, 0);
                }

            }
            else
            {
                if (moveKeyMap.ContainsKey(key))
                {
                    if(moveKeyMap[key])
                    {
                        moveKeyMap[key] = false;
                        foreach (var item in IntWnd)
                        {
                            SendMessage(item, WM_UP, (int)key, 0);
                        }
                    }
                }
            }
        }

        public void SendKeyDownMsg(int key)
        {
            foreach (var item in IntWnd)
            {
                SendMessage(item, WM_DOWN, key, 0);
                SendMessage(item, WM_UP, key, 0);
            }
        }


        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SendForm_Load(object sender, EventArgs e)
        {

        }

        private void SendForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(normalThread != null)
                normalThread.Abort();
            FindRedColor.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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
                int w = cutter.m_width;
                int h = cutter.m_height;
                Point p = cutter.DownPoint;
                textBox2.Text = p.X.ToString();
                textBox3.Text = p.Y.ToString();
                textBox4.Text = w.ToString();
                textBox5.Text = h.ToString();
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {                    }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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
                textBox7.Text = p.X.ToString();
                textBox8.Text = p.Y.ToString();

            }
        }
    }
    public delegate bool CallBack(IntPtr hwnd, int lParam);
}
