using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace HotKeySample
{
    public partial class Form_Display : Form
    {
        public Form_Display()
        {
            InitializeComponent();
        }

        private string[] sarrayMods = null;             //组合键
        private Keys key=new Keys();                    //热键

        private const int nHotKeyStartID = 0xabdd;           //热键标识
        private const int nHotKeyStopID  = 0xabde;           //热键标识
        private const int nHotKeySpeedID = 0xabdf;           //热键标识
        private const int nHotKeyMoveStartID = 0xabdc;           //热键标识
        private const int nHotKeyMoveStopID = 0xabdb;           //热键标识


        private void textBox_Key_KeyDown(object sender, KeyEventArgs e)
        {
//             string sOutput = "";
//             if (e.Modifiers.ToString().Length > 0 && e.Modifiers.ToString() != "None")
//             {
//                 sOutput = e.Modifiers.ToString().Replace("Control", "Ctrl");
//                 sarrayMods = Regex.Split(sOutput, ",");
//                 sOutput = sOutput.Replace(",", " + ") + " + ";
//             }
//             if (e.KeyValue != 17 && e.KeyValue != 18 && e.KeyValue != 97)
//             {
//                 sOutput = sOutput + e.KeyCode.ToString();
//                 key = e.KeyCode;
//             }
// 
//             this.textBox_Key.Text = sOutput;
        }

        private void button_set_Click(object sender, EventArgs e)
        {
            int nModKey = 0;
            if (sarrayMods.Length > 0)
            {
                for (int i = 0; i < sarrayMods.Length; i++)
                {
                    switch (sarrayMods[i])
                    {
                        case "Alt":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_ALT;
                            break;
                        case "Ctrl":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_CTRL;
                            break;
                        case "Shift":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_SHIFT;
                            break;
                    }
                }
            }

            if (HotKey.RegHotKey(this.Handle, nHotKeyStartID, nModKey, key))
                MessageBox.Show("热键设置成功！");
            else
                MessageBox.Show("热键设置失败！");

        }

        /// <summary>
        /// 重写WndProc响应热键方法
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.WParam.ToInt32())
            {
                case nHotKeyStartID:
                    Method();       //热键调用的方法
                    break;
                case nHotKeyStopID:
                    MethodStop();       //热键调用的方法
                    break;
                case nHotKeySpeedID:
                    MethodSpeed();
                    break;
                case nHotKeyMoveStartID:
                    MethodMoveStart();
                    break;
                case nHotKeyMoveStopID:
                    MethodMoveStop();
                    break;
            }

            base.WndProc(ref m);
        }

        private void MethodSpeed()
        {
            //           nTimes++;
            //             Random random = new Random();
            //             int nRoll = random.Next(1, 100);
            //             string sText = "第 " + nTimes.ToString() + " 次按下热键，本次得到点数:[ " + nRoll.ToString() + " ]";
            //             this.label_text.Text = sText;

            // ShootLoop.startRunSpeedAutoKey();
            MethodStop();
            CostSpeedShoot.StartUpdate();
        }


        /// <summary>
        /// 热键调用的方法
        /// </summary>
        private void Method()
        {

            //ShootLoop.startRunAutoKey();
            MethodStop();
            CostSkill.StartUpdate();


        }

        private void MethodStop()
        {

            //ShootLoop.stopRunAutoKey();
            CostSkill.StopUpdate();
            CostSpeedShoot.StopUpdate();
        }


        private void MethodMoveStop()
        {

            JumpStep.StopUpdate();
        }

        private void MethodMoveStart()
        {
            JumpStep.StartUpdate();
        }

        private void textBox_Key_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            int nModKey = 0;
            if (sarrayMods.Length > 0)
            {
                for (int i = 0; i < sarrayMods.Length; i++)
                {
                    switch (sarrayMods[i])
                    {
                        case "Alt":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_ALT;
                            break;
                        case "Ctrl":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_CTRL;
                            break;
                        case "Shift":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_SHIFT;
                            break;
                    }
                }
            }

            if (HotKey.RegHotKey(this.Handle, nHotKeyStopID, nModKey, key))
                MessageBox.Show("热键设置成功！");
            else
                MessageBox.Show("热键设置失败！");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int nModKey = 0;
            if (sarrayMods.Length > 0)
            {
                for (int i = 0; i < sarrayMods.Length; i++)
                {
                    switch (sarrayMods[i])
                    {
                        case "Alt":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_ALT;
                            break;
                        case "Ctrl":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_CTRL;
                            break;
                        case "Shift":
                            nModKey = nModKey | (int)HotKey.MOD.MOD_SHIFT;
                            break;
                    }
                }
            }

            if (HotKey.RegHotKey(this.Handle, nHotKeySpeedID, nModKey, key))
                MessageBox.Show("热键设置成功！");
            else
                MessageBox.Show("热键设置失败！");
        }

        private void Form_Display_Load(object sender, EventArgs e)
        {

//             HotKey.RegHotKey(this.Handle, nHotKeyStartID, (int)HotKey.MOD.MOD_CTRL, Keys.A);
//             HotKey.RegHotKey(this.Handle, nHotKeyStopID, (int)HotKey.MOD.MOD_CTRL, Keys.S);
//             HotKey.RegHotKey(this.Handle, nHotKeySpeedID, (int)HotKey.MOD.MOD_CTRL, Keys.D);
// // 
//              HotKey.RegHotKey(this.Handle, nHotKeyMoveStartID, (int)HotKey.MOD.MOD_CTRL, Keys.G);
//              HotKey.RegHotKey(this.Handle, nHotKeyMoveStopID, (int)HotKey.MOD.MOD_CTRL, Keys.H);

        }

        private void Form_Display_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShootLoop.stopRunAutoKey();
            CostSkill.StopUpdate();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //  JumpStep.StartUpdate();

            //createMathNoRandom();

            //createMathThreeRandom();

            //createMathAllRandom();

            //createMathFirstRandom(); ///没啥用。 用顺序产生后随机写文件
            //createMathSecondRandom();///没啥用。 用顺序产生后随机写文件

            //createMathThreeRandomMax(6);
            createMathSortRandom();
        }

        private void createMathThreeRandomMax(int mv)
        {

            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random();
            Random random2 = new Random(seed);

            int a = random.Next(1, 10);
            int res, x, b, c = 0;
            List<string> ls = new List<string>();
            int max = 500;

            for (int i = 0; i < 100000; i++)
            {

                a = random.Next(1, mv);
                b = random2.Next(1, mv);
                c = random2.Next(1, mv);
                x = random.Next(1, 5);
                switch (x)
                {
                    case 1:
                        res = a + b + c;
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} + {1} + {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} + {1} + {2} = \t\t", a, b, c);
                        }
                        break;
                    case 2:
                        res = a + b - c;
                        if (a == b && a == c)
                        {
                            break;

                        }
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} + {1} - {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} + {1} - {2} = \t\t", a, b, c);
                        }
                        break;

                    case 3:
                        res = a - b + c;
                        if (a < b)
                            break;
                        if (a == b && a == c)
                            break;
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} - {1} + {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} - {1} + {2} = \t\t", a, b, c);
                        }

                        break;
                    case 4:
                        res = a - b - c;
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} - {1} - {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} - {1} - {2} = \t\t", a, b, c);
                        }
                        break;

                }


                if (ls.Count > max)
                    break;
            }


            WriteLogFilexx(ls);
        }

        private void createMathThreeRandom()
        {

            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random();
            Random random2 = new Random(seed);

            int a = random.Next(1, 10);
            int res,x, b,c = 0;
            List<string> ls = new List<string>();
            int max = 1000;
            
            for (int i = 0; i < 100000; i++)
            {

                a = random.Next(1, 10);
                b = random2.Next(1, 10);
                c = random2.Next(1, 10);
                x = random.Next(1, 5);
                switch (x)
                {
                    case 1:
                        res = a + b + c;
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} + {1} + {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} + {1} + {2} = \t\t", a, b, c);
                        }
                        break;
                    case 2:
                        res = a + b - c;
                        if (a == b && a == c)
                        {
                            break;

                        }
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} + {1} - {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} + {1} - {2} = \t\t", a, b, c);
                        }
                        break;

                    case 3:
                        res = a - b + c;
                        if (a < b)
                            break;
                        if (a == b && a == c)
                            break;
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} - {1} + {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} - {1} + {2} = \t\t", a, b, c);
                        }

                        break;
                    case 4:
                        res = a - b - c;
                        if (res <= 10 && res >= 0)
                        {

                            ls.Add(string.Format("{0} - {1} - {2} = \t\t", a, b, c));
                            Console.WriteLine("{0} - {1} - {2} = \t\t", a, b, c);
                        }
                        break;

                }


                if (ls.Count > max)
                    break;
            }


            WriteLogFilexx(ls);
        }

        private void createMathNoRandom()
        {
            int seed = Guid.NewGuid().GetHashCode();


            int a = 1;

            List<string> ls = new List<string>();
            int max = 20;
            int count = 0;
            int b = 0;
             
            for (int j = 1; j < 9; j++)
            {
                a = j;
                for (int i = 1; i < 10; i++)
                {

                    b = i;
                    if (a + b <= 10)
                    {
                        ls.Add(string.Format("{0} + {1} = \t\t", a, b));
                        Console.WriteLine("{0} + {1} = \t\t", a, b);
                    }

                }


            }

            for (int j = 1; j < 10; j++)
            {
                a = j;
                for (int i = 1; i < 10; i++)
                {

                    b = i;
                    if (a + b <= 10)
                    {
                        ls.Add(string.Format("{0} + {1} = \t\t", b, a));
                        Console.WriteLine("{0} + {1} = \t\t", b, a);
                    }

                }
            }

            for (int j = 1; j < 10; j++)
            {
                a = j;
                for (int i = 1; i < 10; i++)
                {

                    b = i;
                    if (a - b >= 0 )
                    {
                        if(a == 9 && b== 9)
                        {

                        }
                        else
                        {
                            ls.Add(string.Format("{0} - {1} = \t\t\t", a, b));
                        }
                        Console.WriteLine("{0} - {1} = \t\t", a, b);
                    }

                }
            }

            for (int j = 1; j < 10; j++)
            {
                a = j;
                for (int i = 1; i < 10; i++)
                {

                    b = i;
                    if (b - a >= 0)
                    {
                        ls.Add(string.Format("{0} - {1} = \t\t\t", b, a));
                        Console.WriteLine("{0} - {1} = \t\t", b, a);
                    }

                }
            }

            WriteLogFilexx(ls);
        }


        private void createMathSortRandom()
        {
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random(seed);

            int a = 1;
            int b = 0;
            List<string> lsAll = new List<string>();
            List<string> ls = new List<string>();

            for (int j = 1; j < 9; j++)
            {
                a = j;
                for (int i = 1; i < 10; i++)
                {

                    b = i;
                    if (a + b <= 10)
                    {
                        ls.Add(string.Format("{0} + {1} = \t\t", a, b));
                        Console.WriteLine("{0} + {1} = \t\t", a, b);
                    }

                }
                int length = ls.Count();
                for (int i = 0; i < length; i++)
                {
                    int idx = random.Next(0, ls.Count());
                    lsAll.Add(ls[idx]);
                    ls.RemoveAt(idx);
                }


            }
            for (int j = 1; j < 9; j++)
            {
                a = j;
                for (int i = 1; i < 10; i++)
                {

                    b = i;
                    if (a + b <= 10)
                    {
                        ls.Add(string.Format("{0} + {1} = \t\t", b, a));
                        Console.WriteLine("{0} + {1} = \t\t", b, a);
                    }

                }
                int length = ls.Count();
                for (int i = 0; i < length; i++)
                {
                    int idx = random.Next(0, ls.Count());
                    lsAll.Add(ls[idx]);
                    ls.RemoveAt(idx);
                }
            }

            for (int j = 1; j <= 10; j++)
            {
                a = j;
                for (int i = 1; i <= 10; i++)
                {

                    b = i;
                    if (a - b > 0)
                    {
                        if (a == 9 && b == 9)
                        {

                        }
                        else
                        {
                            if (a == 10)
                            {
                                ls.Add(string.Format("{0}- {1} = \t\t", a, b));

                            }
                            else
                            ls.Add(string.Format("{0} - {1} = \t\t\t", a, b));
                        }
                        Console.WriteLine("{0} - {1} = \t\t", a, b);
                    }

                }
                int length = ls.Count();
                for (int i = 0; i < length; i++)
                {
                    int idx = random.Next(0, ls.Count());
                    lsAll.Add(ls[idx]);
                    ls.RemoveAt(idx);
                }
            }

            for (int j = 1; j <= 10; j++)
            {
                a = j;
                for (int i = 1; i <= 10; i++)
                {

                    b = i;
                    if (b - a > 0)
                    {
                        if (b == 10)
                        {
                            ls.Add(string.Format("{0}- {1} = \t\t", b, a));

                        }
                        else
                            ls.Add(string.Format("{0} - {1} = \t\t\t", b, a));
                    }
                    Console.WriteLine("{0} - {1} = \t\t", b, a);

                }
                int length = ls.Count();
                for (int i = 0; i < length; i++)
                {
                    int idx = random.Next(0, ls.Count());
                    lsAll.Add(ls[idx]);
                    ls.RemoveAt(idx);
                }
            }

            WriteLogFilexx(lsAll);
        }

        private void createMathSecondRandom()
        {
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random();
            Random random2 = new Random(seed);
            int a = random.Next(1, 10);

            List<string> ls = new List<string>();
            int max = 10;
            int count = 0;
            for (int j = 1; j < 10; j++)
            {
                count = 0;
                for (int i = 0; i < 1000; i++)
                {

                    a = j;
                    int b = random2.Next(1, 10);
                    if (a + b <= 10)
                    {
                        if (count > (max - (j)))
                        {
                            break;
                        }
                        count++;
                        ls.Add(string.Format("{0} + {1} = \t\t", b, a));
                        Console.WriteLine("{0} + {1} = \t\t", b, a);
                    }

                }
            }


            WriteLogFilexx(ls);
        }


        private void createMathFirstRandom()
        {
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random();
            Random random2 = new Random(seed);
            int a = random.Next(1, 10);

            List<string> ls = new List<string>();
            int max = 8;
            int count = 0;
            for (int j = 1; j <= 10; j++)
            {
                count = 0;
                for (int i = 0; i < 1000; i++)
                {

                    a = j;
                    int b = random2.Next(1, 10);
                    if (a + b <= 10)
                    {
                        if (count > (max - (j-4)))
                        {
                            break;
                        }
                        count++;
                        ls.Add(string.Format("{0} + {1} = \t\t", a, b));
                        Console.WriteLine("{0} + {1} = \t\t", a, b);
                    }


                }
            }


            WriteLogFilexx(ls);
        }


        private void createMathAllRandom()
        {
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random();
            Random random2 = new Random(seed);
            int a = random.Next(1, 10);

            List<string> ls = new List<string>();
            int max = 200;
            for (int i = 0; i < 100000; i++)
            {

                a = random.Next(1, 10);
                int b = random2.Next(1, 10);
                if (a + b <= 10)
                {
                    if (ls.Count > max)
                        break;

                    ls.Add(string.Format("{0} + {1} = \t\t", a, b));
                    Console.WriteLine("{0} + {1} = \t\t", a, b);
                }

            }

            WriteLogFilexx(ls);
        }

        private static int WriteLogFilexx(List<string> slog)
        {
            
            string dt = DateTime.Now.ToString("yyyy_MM_dd");
            string temFile = dt + "math.log";
            FileStream logFile = new FileStream(temFile, FileMode.OpenOrCreate);
            logFile.Seek(0, SeekOrigin.End);
            StreamWriter sw = new StreamWriter(logFile);
            try
            {
                string aline = "";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i+4 <= slog.Count; i=i+4)
                {
                    aline = string.Format("{0}\t{1}\t{2}\t{3}\n", slog[i], slog[i + 1], slog[i + 2], slog[i + 3]);
                    sb.Append(aline);
                }
                sw.WriteLine(sb.ToString());
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
        private void button4_Click(object sender, EventArgs e)
        {

            //JumpStep.StopUpdate();
        }
    }
}
