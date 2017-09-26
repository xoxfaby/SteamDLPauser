using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamDLPauser
{
    public partial class Form2 : Form
    {

        public String[] tempInfo;

        string bMessage;
        public Form2(string cMessage)
        {
            InitializeComponent();

            bMessage = cMessage;
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Text = bMessage;
            label1.Left = (label1.Parent.Width - label1.Width) / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
