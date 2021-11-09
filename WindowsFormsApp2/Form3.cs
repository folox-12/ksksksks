using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp2
{
	public partial class Form3 : Form
	{
		public Form3()
		{
			InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			Form1 a = new Form1();
			a.Show();
		}

		private void Button2_Click(object sender, EventArgs e)
		{
			Form2 a = new Form2();
			a.Show();
		}

		private void Form3_Load(object sender, EventArgs e)
		{

		}

        private void Form3_Load_1(object sender, EventArgs e)
        {

        }
    }
}
