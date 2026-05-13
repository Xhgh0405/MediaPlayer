using System;
using System.Windows.Forms;

namespace HW5_MediaPlayer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMediaPlayer());
        }
    }
}
