using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenshTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = int.Parse(textBox1.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var s = new Stopwatch();
            s.Start();
           var ss = new WebClient().DownloadString("http://localhost:8080/");
            s.Stop();
            richTextBox1.Text += "resp: " + ss + " - time(mm:ss:ms): " + s.Elapsed.Minutes + ":" + s.Elapsed.Seconds + ":" + s.Elapsed.Milliseconds + Environment.NewLine;

           
        }
    }
}
