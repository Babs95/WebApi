using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Weather_Report
{
	public partial class Form1 : Form
	{
		int id = 0;
		public Form1()
		{
			InitializeComponent();
		}

		private void label8_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{

			//dataGridView1.DataSource 
			
		}

		private async void button1_Click(object sender, EventArgs e)
		{

			PrintData();

		}
		 private async void PrintData()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://localhost:52278/");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response;

				//id == 0 means select all records    


				response = await client.GetAsync("api/Weather");
				if (response.IsSuccessStatusCode)
				{
					WeatherClient[] reports = await response.Content.ReadAsAsync<WeatherClient[]>();
					/*foreach (var report in reports)
					{
						//Console.WriteLine("\n{0}\t{1}\t{2}\t{3}\t{4}", report.City, report.Temperature, report.Humidity, report.Precipitation, report.Wind);
						dataGridView1.DataSource = report.City;
					}*/
					dataGridView1.DataSource = reports.ToList();
				}

			}
		}
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private async void button2_Click(object sender, EventArgs e)
		{

			WeatherClient newReport = new WeatherClient();
			newReport.City = textBox2.Text;
			newReport.Temperature = textBox3.Text;
			newReport.Precipitation = textBox4.Text;
			newReport.Humidity = textBox5.Text;
			newReport.Wind = textBox6.Text;


			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://localhost:52278/");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = await client.PostAsJsonAsync("api/Weather", newReport);
				if (response.IsSuccessStatusCode)
				{
					bool result = await response.Content.ReadAsAsync<bool>();
					if (result) { 
						Console.WriteLine("Report Submitted");
						PrintData();
					}
					else
						Console.WriteLine("An error has occurred");
				}
			}

		}
	}
}
