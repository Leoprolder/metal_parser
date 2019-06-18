using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace MetalParser
{
    public partial class Form1 : Form
    {
        //string platinum = "https://ru.investing.com/commodities/platinum";
        //string gold = "https://ru.investing.com/commodities/gold";
        //string silver = "https://ru.investing.com/commodities/silver";
        //string samsung = "https://ru.investing.com/equities/samsung-electronics-co-ltd";
        string samsungUrl = "https://ru.investing.com/equities/samsung-electronics-co-ltd-gdr";
        //string apple = "https://ru.investing.com/equities/apple-computer-inc";
        //Timer tpt = new Timer();
        //Timer tau = new Timer();
        //Timer tag = new Timer();
        System.Windows.Forms.Timer tSamsung = new System.Windows.Forms.Timer();
        //Timer tApple = new Timer();
        //int timeout = 10 * 60 * 1000; //10 минут
        int timeout = 60000;

        string platinum_path = "@/data/platinum-cfd.txt";
        string gold_path = "@/data/gold-cfd.txt";
        string silver_path = "@/data/silver-cfd.txt";
        string samsung_path = "@/data/Samsung-electronincs-Co.txt";
        string samsungJsonData = "@/data/samsung.json";
        string apple_path = "@/data/Apple.txt";

        List<double> platinumValues = new List<double>();
        List<double> goldValues = new List<double>();
        List<double> silverValues = new List<double>();
        List<double> samsungValues = new List<double>();
        List<double> appleValues = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private async Task<string> FindValue(string Url, Object sender, EventArgs e)
        {
            tSamsung.Interval = timeout;
            string parsedValue = null;
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            await Task.Run(async () =>
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = web.Load(Url);
                    if (doc != null)
                    {
                        string openState = doc.DocumentNode.SelectNodes("//*[@id=\"quotes_summary_current_data\"]/div[1]/div[2]/div[2]/text()")[2].InnerText;
                        parsedValue = openState.ToLower().Contains("закрыт") ? "closed" : doc.DocumentNode.SelectNodes("//*[@id='last_last']")[0].InnerText;
                        if (parsedValue == "closed")
                        {
                            cancelTokenSource.Cancel();
                        }
                    }
                }
                catch (Exception ex)
                {
                    parsedValue = "lost connection";
                }
            });
            if(cancelTokenSource.IsCancellationRequested)
            {
                tSamsung.Interval = timeout * 10; //Уменьшение частоты проверки в случае закрытой биржи до 10 минут
                return null;
            }
            return parsedValue;
        }

        /// <summary>
        /// Метод асинхронно получает значение акции и записывает его в файл
        /// </summary>
        /// <param name="url">Ссылка на страницу со значением стоимости акции</param>
        private async void GetValue(string url, Object sender, EventArgs e)
        {
            string value = await FindValue(url, sender, e);
            DateTime date = DateTime.Now;
            if (value == null)
            {
                textBox1.Text += $"{date.ToString("dd.MM.yy hh:mm")} | Торги закрыты.{Environment.NewLine}";
                return;
            }

            value = value.Replace(".", "");
            string line = $"{date.ToString("dd.MM.yy hh:mm")} | {value}";
            
            using (FileStream fs = new FileStream(samsungJsonData, FileMode.Append)) //переделать на json
            {
                Data data = new Data(date, Double.Parse(value));
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Data));
                jsonFormatter.WriteObject(fs, data);
                if (value != "lost connection")
                {
                    samsungValues.Add(Double.Parse(value));
                }
            }
            textBox1.Text += line + Environment.NewLine;
        }

        private void FillValueListsFromFile(string path, List<double> valuesList) //переделать на json
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Data));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                List<Data> dataList = jsonFormatter.ReadObject(fs) as List<Data>;

                foreach (Data data in dataList)
                {
                    valuesList.Add(data.Value);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string value = await GetValue(platinum);
            Console.WriteLine(value);*/
            button1.Enabled = false;
            button2.Enabled = true;

            tSamsung.Interval = timeout;
            tSamsung.Tick += (timer, arguments) => GetValue(samsungUrl, sender, e);
            tSamsung.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            tSamsung.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //FillValueListsFromFile(platinum_path, platinumValues);
                //FillValueListsFromFile(gold_path, goldValues);
                //FillValueListsFromFile(silver_path, silverValues);
                FillValueListsFromFile(samsungJsonData, samsungValues);
                //FillValueListsFromFile(apple_path, appleValues);
            }
            catch(Exception ex)
            {

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Chart bar = new Chart();
            bar.Parent = this;
            bar.Dock = DockStyle.None;
            bar.Location = new System.Drawing.Point(388, 81);
            bar.Margin = new Padding(3, 3, 3, 3);
            bar.Size = new System.Drawing.Size(519, 382);
            bar.ChartAreas.Add(new ChartArea("Time series"));
            Series series = new Series("test");
            series.ChartType = SeriesChartType.Line;
            for (double x = -Math.PI; x <= Math.PI; x += Math.PI / 10.0)
            {
                series.Points.AddXY(x, Math.Sin(x));
            }

            bar.Series.Add(series);
        }
    }
}
