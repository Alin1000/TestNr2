using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Test2
{
    public partial class Form1 : Form
    {

        private TextBox resultTextBox;

        public Form1()
        {
            InitializeComponent();
        }

        private class ShowResult
        {
            public Payload payload { get; set; }
        }

        private class Payload
        {
            public string token { get; set; }
        }

        private async Task Login_Click(object sender, EventArgs e)
        {
            /* MessageBox.Show("Incercare");
             using (var client = new HttpClient())
             {
                 var response = await client.PostAsync("https://api-testing-dogu.freya.cloud/login", new FormUrlEncodedContent(new Dictionary<string, string>()
             {
                 { "username", usernametextbox.Text },
                 { "password", passwordtextbox.Text }
             }));
                 MessageBox.Show(response.StatusCode.ToString());

                 if (response.StatusCode == HttpStatusCode.OK)
                 {
                     string jsonResponse = await response.Content.ReadAsStringAsync();
                     ShowResult result = JsonConvert.DeserializeObject<ShowResult>(jsonResponse);
                     MessageBox.Show(result.token);

                     resultTextBox.Text = "";
                 }
             }*/
        }

        private async void login_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", "f8ce74e00caf759759d311aad1e005a0");

                var response = await client.PostAsync("https://api-testing-dogu.freya.cloud/login", new StringContent("{\"username\": \"" + usernametextbox.Text + "\", \"password\": \"" + passwordtextbox.Text + "\"}", Encoding.UTF8, "application/json"));
                

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    ShowResult result = JsonConvert.DeserializeObject<ShowResult>(jsonResponse);
                    
                    Program.Token = result.payload.token;

                    /*SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Practica\\Test2\\Produse.mdf;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into UserTab values (1, 'Token', @Value)", conn);
                    cmd.Parameters.AddWithValue("@Value", Program.Token);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Successfully Saved");*/

                    this.Hide();
                    Form2 fm2 = new Form2();
                    fm2.ShowDialog();
                } 
                else
                {
                    MessageBox.Show("Bad credentials!");
                }
            }
        }
    }
}