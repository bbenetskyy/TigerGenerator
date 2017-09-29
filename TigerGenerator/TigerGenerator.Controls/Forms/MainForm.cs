using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace TigerGenerator.Controls.Forms
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
            Finished = false;
            SplashScreenManager.ShowForm(typeof(SplashForm));
            while (!Finished)
            {
                Thread.Sleep(500);
            }
            SplashScreenManager.CloseForm();
        }

        public static bool Finished;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Close();
        }
    }
}