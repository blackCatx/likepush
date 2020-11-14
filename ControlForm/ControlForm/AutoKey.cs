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

        public static int secMax = 60000;
        public static int secMin = 20000;

        private static Thread normalThread = null;

        private Keys key = new Keys();                    //热键
        private byte valKey = 0x20;

        public AutoKey()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bool res = Int32.TryParse(txtSecondMin.Text, out secMin);
            if (res)
            {
                secMin = secMin * 1000;
            }
            else
            {
                secMin = 20000;
            }
            res = Int32.TryParse(txtSecondMax.Text, out secMax);
            if (res)
            {
                secMax = secMax * 1000;
            }
            else
            {
                secMax = 60000;
            }
            StartUpdate();
        }

        public static void ClickKey(byte key)
        {
//            keybd_event(0x20, 0, 0, 0);

            keybd_event(key, 0, 0, 0);
            Thread.Sleep(100);
            keybd_event(key, 0, 0x0002, 0);
        }

        public void UpdateClick()
        {

            int curTime = secMax;
            Random random = new Random();
            
            while (true)
            {
                curTime = random.Next(secMin, secMax);    
                Thread.Sleep(curTime);
                ClickKey(valKey);

            }
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
            }

            textBox2.Text = sOutput;
        }
    }
}
