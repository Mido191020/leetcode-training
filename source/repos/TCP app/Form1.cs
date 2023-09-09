using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net; // IP
using System.Net.Sockets; // TCP
using System.Threading; // Thread
using System.IO; // Stream


namespace TCP_app
{
    public partial class Form1 : Form
    {
        private TcpClient client;
       public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        public String TextToSend;

        public Form1()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); // get my own IP
           foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox5.Text = address.ToString();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void startbutton1_Click(object sender, EventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(textBox6.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;
            backgroundWorker1.RunWorkerAsync(); // start receiving data in background
            backgroundWorker2.WorkerSupportsCancellation = true; // ability to cancel this thread

        }

        private void coectbutton2_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(textBox4.Text), int.Parse(textBox3.Text));
            try
            {
                client.Connect(IP_End);
                if (client.Connected)
                {
                    textBox1.AppendText("Connected to Server" + "\n");
                    STW = new StreamWriter(client.GetStream());
                    STR = new StreamReader(client.GetStream());
                    STW.AutoFlush = true;
                    backgroundWorker1.RunWorkerAsync(); // start receiving data in background
                    backgroundWorker2.WorkerSupportsCancellation = true; // ability to cancel this thread
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString());
            }   
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    recieve = STR.ReadLine();
                    this.textBox1.Invoke(new MethodInvoker(delegate () { textBox1.AppendText("You: " + recieve + "\n"); }));
                    recieve = "";
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(TextToSend);
                this.textBox1.Invoke(new MethodInvoker(delegate () { textBox1.AppendText("Me: " + TextToSend + "\n"); }));

            }
            else
            {
                MessageBox.Show("Sending failed");
            }
            backgroundWorker2.CancelAsync();
        }

        private void sendbutton3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                TextToSend = textBox2.Text;
                backgroundWorker2.RunWorkerAsync();
            }
            textBox2.Text = "";

        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
