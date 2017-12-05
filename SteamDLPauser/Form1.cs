using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SteamDLPauser
{
    public partial class Form1 : Form
    {
        List<List<string>> gList = new List<List<string>>();
        public Form1()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {       

            bool dummyActive = false;
            bool processActive = false;

            System.Diagnostics.Process[] listdummy = System.Diagnostics.Process.GetProcessesByName("SteamDummy");
            if (listdummy.Length > 0)
            {
                dummyActive = true;
            }
            foreach (List<string> item in gList)
            {
                   
                System.Diagnostics.Process[] listitem = System.Diagnostics.Process.GetProcessesByName(item[1]);
                if (listitem.Length > 0)
                {
                    bool windowMatch = false;
                    if( item[2] == "")
                    {
                        windowMatch = true;
                    } else
                    {
                        foreach (System.Diagnostics.Process pitem in listitem)
                        {
                            if (pitem.MainWindowTitle == item[2])
                            {
                                windowMatch = true;
                            }
                        }
                    }

                    processActive = true;
                    if (dummyActive == false && windowMatch) 
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(item[0], item[1]);
                        }
                        catch
                        {
                            gList.Remove(item);

                            saveItems();
                            updateItems();
                            MessageBox.Show("oops, that didn't work, removed " + item[0] + " " + item[1] );
                            break;
                        }
                    }
                }
            }
            if (!processActive && dummyActive)
            {
                foreach (System.Diagnostics.Process dummy in listdummy)
                {
                    dummy.Kill();
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateItems()
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (List<string> item in gList)
            {
                var titem = new ListViewItem(item[0]);
                titem.SubItems.Add(item[1]);
                titem.SubItems.Add(item[2]);
                listView1.Items.Add(titem);
            }
            listView1.EndUpdate();
        }

        private void loadItems()
        {
            List<List<string>> tsllist = new List<List<string>>();

            XDocument xmlDoc = new XDocument();
            try
            {
                xmlDoc = XDocument.Load("ls.config");
            }
            catch
            {
                xmlDoc.Add(new XElement("entries"));
                xmlDoc.Save("ls.config");
            }
            IEnumerable<XElement> entries = xmlDoc.Element("entries").Elements() ;
            foreach(var entry in entries)
            {
                List<string> tslist = new List<string>();
                IEnumerable<XElement> vars = entry.Elements();
                    foreach(var var1 in vars)
                {
                    tslist.Add(var1.Value);
                }
                tsllist.Add(tslist);
            }
            gList = tsllist.ToList();
            foreach (List<string> item in gList)
            {
                if (item.Count < 3)
                {
                    item.Add("");
                }
            }
            updateItems();
        }
        private void saveItems()
        {
            XDocument document = new XDocument(new XElement("entries",
                gList.Select(i => new XElement("entry",
                i.Select(j => new XElement("val",j))))));
            document.Save("ls.config");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty && comboBox1.Text != String.Empty)
            {
                List<string> item = new List<string>();
                item.Add(textBox1.Text);
                item.Add(comboBox1.Text);
                item.Add(comboBox2.Text);
                gList.Add(item);
                saveItems();
                updateItems();
            }
            else
            {
                MessageBox.Show("Empty dude");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 dForm2 = new Form2("Really Remove?");
            var dgResult = dForm2.ShowDialog();
            if (dgResult == DialogResult.OK)
            {
                var rlist = listView1.SelectedIndices.Cast<int>();
                    foreach (int i in rlist.OrderByDescending(v => v))
                    {
                        gList.RemoveAt(i);
                       }
                
                saveItems();
                updateItems();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 dForm2 = new Form2("Nope");
            var dgResult = dForm2.ShowDialog();
            if (dgResult == DialogResult.OK)
            {
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadItems();
            notifyIcon1.Icon = new Icon(this.Icon, 40, 40);
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] plist = System.Diagnostics.Process.GetProcesses();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(plist.Select(p => p.ProcessName).Distinct().ToArray());
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void notifyIcon2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                button1_Click(sender, e);
            }
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] plist = System.Diagnostics.Process.GetProcessesByName(comboBox1.Text);
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(plist.Select(p => p.MainWindowTitle).Distinct().ToArray());
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                button1_Click(sender, e);
            }
        }
    }
}
