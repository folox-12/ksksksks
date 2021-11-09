using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;


namespace WindowsFormsApp2
{
	public partial class Form1 : Form
	{
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientsList = new List<Socket>();
        public Socket client;


        public void fornewthread()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ipAddress, 815);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptSocket();
                clientsList.Add(client);
                Thread messageAccept = new Thread(() =>
                {

                Invoke(new Action(() => textBox2.Text += "Подключение с " + client.RemoteEndPoint));
                Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                byte[] buffer = new byte[1024];
                while (client.Connected)
                {
                    Thread.Sleep(100);
                    client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                        string[] splitMessage = message.Split(new char[] { '/' });
                        string[] splitMessage2 = splitMessage[1].Split(' ');
                        Console.WriteLine(splitMessage2);
                        if (splitMessage2[0]=="reg:")
                        {
                            using (var cn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = bd.accdb "))
                            {
                                cn.Open();
                                using (OleDbCommand Number = cn.CreateCommand())
                                {
                                    Number.CommandText = "INSERT INTO persons (Person_name,Person_login,Person_password) values" +
                                        " ('" + splitMessage2[1] + "','" + splitMessage2[2] + "','" + splitMessage2[3] + "');";
                                    int numberOfUpdatedItems = Number.ExecuteNonQuery(); 
                                }
                                cn.Close();
                            }
                        }

                        using (var cn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = bd.accdb "))
                        {
                            cn.Open();
                            using (OleDbCommand Number = cn.CreateCommand())
                            {
                                Number.CommandText = "INSERT INTO history (Отправитель,Дата,Содержимое) values" +
                                    " ('" + ipAddress + "','" + splitMessage[0] + "','" + splitMessage[1] + "');";
                                int numberOfUpdatedItems = Number.ExecuteNonQuery();
                            }
                            cn.Close();
                        }


                        Invoke(new Action(() => textBox2.Text += message));
                        Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                        //client.Shutdown(SocketShutdown.Receive);
                        Thread.Sleep(100);

                        foreach (Socket clint in clientsList)
                        {
                            clint.Send(buffer);
                        }
                        buffer = new byte[1024];
                        message = Encoding.UTF8.GetString(buffer);
                    }

                    clientsList.Remove(client);
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                });
                messageAccept.Start();
            }
        }
        public Form1()
		{
			InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
            //string clients = null;
            //for (int i = 0; i < clientsList.Count; i++)
            //{
            //    clients += clientsList[i].RemoteEndPoint.ToString();
            //}
            //MessageBox.Show(clients);
            //IPEndPoint ep = new IPEndPoint(clientIp, 815);
            //string message = textBox2.Text;
            //byte[] buffer = Encoding.UTF8.GetBytes(message);
            //socket.Send(buffer);
            //socket.Shutdown(SocketShutdown.Send);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, 815));
            Thread mythread = new Thread(fornewthread);
			mythread.Start();
        }

    }

}

//socket.Listen(5);
//client = socket.Accept();
//byte[] buffer = new byte[1024];
//client.Receive(buffer);
//Invoke(new Action(() => textBox2.Text += (Encoding.UTF8.GetString(buffer))));
//Invoke(new Action(() => textBox2.Text += "\r\n"));
//client.Shutdown(SocketShutdown.Receive);
//client.Close();
