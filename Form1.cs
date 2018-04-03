using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace FYPSERVER
{
    public partial class Registration_Server : Form 
    {
        public TcpClient client;
        public StreamReader STR;
        private String receive;
        private String[] userInfo;
        private int serial_number = 20;
        private String private_Key = "123123123";
        public byte[] eccprvkey = new byte[20];
        public byte[] eccpubkey_x,eccpubkey_y;



        public Registration_Server()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtIP.Text = address.ToString();
                }
            }
        }

        public void setPrivateKey()
        {
            Random rnd = new Random();
            rnd.NextBytes(this.eccprvkey);
            
               
        }

        public void setPublicKey()
        {
           
               
        }
        public byte[] getPrivateKey()
        {
            return this.eccprvkey;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            setPrivateKey();
  
            lblStatus.Text = "Server Started!!";
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(txtPort.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            lblStatus.Text = "Client Connected !!";
            STR = new StreamReader(client.GetStream());
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true; // ability to cancel this thread 

        }

        private void insert_Into_DB()
        {
            using (SqlConnection sqlcon = new SqlConnection(@"Data Source=INTEL\TEW_SQLEXPRESS;Initial Catalog=LoginDB;Integrated Security=True;Pooling=False"))
            {
                string select_query = "SELECT MAX(ID) FROM [DBO].[USERS]";
                Console.WriteLine("connection opened !!");
                using (SqlCommand command = new SqlCommand(select_query, sqlcon))
                {
                    sqlcon.Open();
                    serial_number = (int)command.ExecuteScalar();
                    serial_number++;
                    sqlcon.Close();
                }
                string insert_query = "INSERT INTO [dbo].[UserDB] ([ID],[EID]) VALUES (@id, @Username ,@Password)";
                using (SqlCommand command = new SqlCommand(insert_query, sqlcon))
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        userInfo[1] = GetMd5Hash(md5Hash, userInfo[1]);
                    }
                    command.Parameters.AddWithValue("@id", serial_number);
                    command.Parameters.AddWithValue("Username", userInfo[0].ToString());
                    command.Parameters.AddWithValue("Password", userInfo[1]);
                    sqlcon.Open();
                    int result = command.ExecuteNonQuery();
                    sqlcon.Close();
                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into database !!");
                    }
                    receive = "";
                }
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    userInfo = null;
                    userInfo = receive.Split(':');
                    userInfo = userInfo[1].Split('!');
                    MessageBox.Show("ID : " + userInfo[0] + "\n" + "HID : " + userInfo[1]);
                    insert_Into_DB();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker2.CancelAsync();
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Registration_Server_Load(object sender, EventArgs e)
        {

        }
    }
}

