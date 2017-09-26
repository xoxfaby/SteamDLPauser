using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamDummy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            System.Diagnostics.Process[] list = System.Diagnostics.Process.GetProcessesByName("SteamDLPauser");
            if (list.Length < 1)
            {
                this.Close();
            }
        }
    }
}
