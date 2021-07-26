using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ControlForm
{
    class MouseControl
    {

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);


        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private Thread normalThread = null;

        bool isRunninng = false;

        public void StartUpdate()
        {
            isRunninng = true;

            if (normalThread != null && normalThread.IsAlive) return;
            ThreadStart childRef = new ThreadStart(UpdateClick);
            normalThread = new Thread(childRef);
            normalThread.Start();
        }


        public void StopUpdate()
        {
            isRunninng = false;
            if (normalThread != null && normalThread.IsAlive)
                normalThread.Abort();
        }

        public void UpdateClick()
        {
            Random random = new Random();
            int curTime = 0;

            while (isRunninng)
            {
                curTime = random.Next(300, 1100);
                Thread.Sleep(curTime);
                mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
            }
        }


    }
}
