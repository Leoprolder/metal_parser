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
        Timer tSamsung = new Timer();
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

        private static async Task<string> FindValue(string Url)
        {
            string parsedValue = null;
            await Task.Run(async () =>
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = web.Load(Url);
                    if (doc != null)
                        parsedValue = doc.DocumentNode.SelectNodes("//*[@id='last_last']")[0].InnerText;
                }
                catch (Exception)
                {
                    parsedValue = "lost connection";
                }
            });
            return parsedValue;
        }

        /// <summary>
        /// Метод асинхронно получает значение для соответствующего металла и записывает его в файл
        /// </summary>
        /// <param name="url">Ссылка на страницу со значением стоимости металла</param>
        /// <param name="option">Вариант металла: platinum, gold, silver</param>
        private async void GetValue(string url)
        {
            string value = await FindValue(url);
            value = value.Replace(".", "");
            string date = DateTime.Now.ToString("dd.MM.yy hh:mm");
            string line = $"{date} | {value}";
            double lineExtr = 0;
            
            using (StreamWriter sw = new StreamWriter(samsung_path, true)) //переделать на json
            {
                sw.WriteLine(line);
                if (value != "lost connection")
                {
                    line += "; expected " + lineExtr;
                    samsungValues.Add(Double.Parse(value));
                }
            }
            textBox1.Text += line + Environment.NewLine;
        }

        private void FillValueListsFromFile(string path, List<double> valuesList) //переделать на json
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                double value;
                string line;

                if (sr.EndOfStream)
                    return;

                while ((line = sr.ReadLine()) != null)
                {
                    value = Double.Parse(line.Substring(line.IndexOf('|') + 2));
                    valuesList.Add(value);
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
            tSamsung.Tick += (timer, arguments) => GetValue(samsungUrl);
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
                FillValueListsFromFile(samsung_path, samsungValues);
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
