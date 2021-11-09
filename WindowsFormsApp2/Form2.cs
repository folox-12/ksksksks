using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp2
{
	public partial class Form2 : Form
	{

        public Socket socket;
        public void fornewthread()
        {
            while (true)
            {
                while (socket.Connected)
                {
                    byte[] buffer = new byte[1024];
                    socket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    Thread.Sleep(100);
                    Invoke(new Action(() => textBox2.Text += message));
                    Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                }
                //socket.Shutdown(SocketShutdown.Receive);
                //socket.Close();
                Application.Exit();
                //byte[] buffer = new byte[1024];
                //socket.Receive(buffer);
                //string message = Encoding.UTF8.GetString(buffer);
                //Invoke(new Action(() => textBox2.Text += message));
                //Invoke(new Action(() => textBox2.Text += "\r\n"));
                //socket.Shutdown(SocketShutdown.Receive);
                ////socket.Close();
            }
        }

        public Form2()
		{
			InitializeComponent();
		}
        string time = DateTime.Now.ToString();
        private void Button1_Click(object sender, EventArgs e)
		{

            string message = time +"/"+textBox1.Text + "/";
            string[] new_message = message.Split('/');
            foreach (var sub in new_message)
            {
                Console.WriteLine($"Substring: {sub}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            Thread.Sleep(100);
            //textBox2.Text += (Encoding.UTF8.GetString(buffer));
            //socket.Send(buffer);
            socket.SendTo(buffer, socket.RemoteEndPoint);
            //socket.Shutdown(SocketShutdown.Send);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.0.0.1", 815);
            Thread mythread1 = new Thread(fornewthread);
            mythread1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reg a = new Reg();
            a.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reg a = new Reg();
            a.Show();
        }
    }
}
