using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Http;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Test2
{
    public partial class Form2 : Form
    {
        string connectionString;
        public TextBox resultTextBox;

        public Form2()
        {
            InitializeComponent();

        }

        private void refresh_Click(object sender, EventArgs e)
        {

        }

        private async void Form2_Load(object sender, EventArgs e)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", "f8ce74e00caf759759d311aad1e005a0");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Program.Token); 

                var response = await client.GetAsync("https://api-testing-dogu.freya.cloud/product/findMany?listOnly=true&productCategoryUid=5b693a50ff2c495290a741c5ecdf26d1&locationUid=cc33a3a158d14b34a80171caf35870e2&pageNo=0&top=100");

                string jsonResponse = await response.Content.ReadAsStringAsync();

                ShowResult result = JsonConvert.DeserializeObject<ShowResult>(jsonResponse);
                MessageBox.Show(jsonResponse);
                foreach (Show record in result.payload.records)
                {
                    SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Practica\\Test2\\Produse.mdf;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into Products values (@Name, @Category, @Measure_Unit, @Price_with_Vat, @uid, @vatName, @RepositoryName, @isAvailableOnPos, @isForSale, @isDisabled)", conn);
                    cmd.Parameters.AddWithValue("@Name", record.name);
                    cmd.Parameters.AddWithValue("@Category", record.categoryName);
                    cmd.Parameters.AddWithValue("@Measure_Unit", record.measureUnitName);
                    cmd.Parameters.AddWithValue("@Price_with_Vat", record.unitPriceWithVat);
                    cmd.Parameters.AddWithValue("@uid", record.uid);
                    cmd.Parameters.AddWithValue("@vatName", record.vatName);
                    cmd.Parameters.AddWithValue("@RepositoryName", record.repositoryName);
                    cmd.Parameters.AddWithValue("@isAvailableOnPos", record.isAvailableOnPos);
                    cmd.Parameters.AddWithValue("@isForSale", record.isForSale);
                    cmd.Parameters.AddWithValue("@isDisabled", record.isDisabled);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        private class ShowResult
        {
            public Payload payload { get; set; }
        }

        private class Payload
        {
            public List<Show> records { get; set; }
        }

        private class Show
        {
            public string name { get; set; }
            
            public string categoryName { get; set; }

            public string measureUnitName { get; set; }

            public string vatName { get; set; }

            public string repositoryName { get; set; }

            public Boolean isForSale { get; set; }

            public decimal unitPriceWithVat { get; set; }
            public Boolean isAvailableOnPos { get; set; }
            public string uid { get; set; }
            public Boolean isDisabled { get; set; }

        }
    }
}