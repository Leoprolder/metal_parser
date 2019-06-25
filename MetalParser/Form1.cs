using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Runtime.Serialization.Json;
using System.Threading;
using MetalParser.Predicting;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace MetalParser
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer tSamsung = new System.Windows.Forms.Timer();
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
        List<Data> samsungValues = new List<Data>();
        List<double> appleValues = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private async Task<string> FindValue(string Url, Object sender, EventArgs e)
        {
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

            if (value != "lost connection")
            {
                Data data = new Data(date, Double.Parse(value));
                samsungValues.Add(data);
                WriteValue(samsungJsonData, data);
            }

            textBox1.Text += line + Environment.NewLine;
        }

        /// <summary>
        /// Пишет полученное значение в json-файл
        /// </summary>
        /// <param name="path">Путь к json-файлу</param>
        /// <param name="data">Дата и цена акции</param>
        private void WriteValue(string path, Data data)
        {
            List<Data> values = ReadValues(path);
            values.Add(data);

            using (FileStream fs = new FileStream(samsungJsonData, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Data>));
                jsonFormatter.WriteObject(fs, values);
            }
        }

        /// <summary>
        /// Читает json-файл со значениями даты и цены и возвращает список
        /// </summary>
        /// <param name="path">Путь к json-файлу</param>
        /// <returns></returns>
        private List<Data> ReadValues(string path)
        {
            List<Data> valuesList = new List<Data>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Data>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                List<Data> dataList = jsonFormatter.ReadObject(fs) as List<Data>;

                foreach (Data data in dataList)
                {
                    valuesList.Add(data);
                }
            }

            return valuesList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                comboBox1.SelectedIndex = 0;
                //При загрузке формы читаем файл и заполняем список значениями из него
                foreach(Data data in ReadValues(samsungJsonData))
                {
                    samsungValues.Add(data);
                }
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
            bar.ChartAreas.Clear();
            bar.ChartAreas.Add(new ChartArea("Time series"));
            Series series = new Series("Original");
            series.ChartType = SeriesChartType.Line;
            for (int i = 0; i < samsungValues.Count; i++)
            {
                series.Points.AddXY(samsungValues[i].Date, samsungValues[i].Value);
            }

            int option = comboBox1.SelectedIndex;
            PredictorFactory predictorFactory = null;
            switch (option)
            {
                case 0:
                    predictorFactory = new PredictorFactory(PredictorTypes.MA);
                    break;

                case 1:
                    predictorFactory = new PredictorFactory(PredictorTypes.ARMA);
                    break;

                case 2:
                    predictorFactory = new PredictorFactory(PredictorTypes.SSA);
                    break;
            }

            Series estimation = new Series("Estimated");
            estimation.ChartType = SeriesChartType.Line;
            List<Double?> predictedValues = new List<Double?>();
            int accuracy = 10;
            if (predictorFactory.Type == PredictorTypes.MA)
            {
                List<Double> predictedValues1 = new List<double>();
                for (int i = 0; i < samsungValues.Count; i++)
                {
                    predictedValues1.Add(samsungValues[i].Value);
                }
                List<Double> copy = MAPredictor.PredictList(predictedValues1, accuracy);

                for (int i = 0; i < samsungValues.Count; i++)
                {
                    estimation.Points.AddXY(samsungValues[i].Date, copy[i]);
                }
            }
            else if (predictorFactory.Type == PredictorTypes.ARMA)
            {
                List<Double> copy = new List<Double>();
                for (int i = 0; i < samsungValues.Count; i++)
                {
                    copy.Add(samsungValues[i].Value);
                }

                List<Double> prediction = ARMAPredictor.PredictList(copy, accuracy);

                for (int i = 0; i < samsungValues.Count; i++)
                {
                    estimation.Points.AddXY(samsungValues[i].Date, prediction[i]);
                }
            }
            else if (predictorFactory.Type == PredictorTypes.SSA)
            {
                List<Double> temp = new List<Double>();
                for (int i = 0; i < samsungValues.Count; i++)
                {
                    temp.Add(samsungValues[i].Value);
                }
                List<double> predictedValues1 = SSAPredictor.PredictList(temp, 10);
                for (int i = 0; i < samsungValues.Count; i++)
                {
                    estimation.Points.AddXY(samsungValues[i].Date, predictedValues1[i]);
                }
            }

            bar.ChartAreas[0].AxisY.Minimum = 970;

            bar.Series.Clear();
            bar.Series.Add(series);
            bar.Series.Add(estimation);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Visible = comboBox1.SelectedIndex == 0;
            button4.Visible = comboBox1.SelectedIndex == 0;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            bar.ChartAreas.Clear();
            bar.ChartAreas.Add(new ChartArea("Time series"));
            Series series = new Series("Original");
            series.ChartType = SeriesChartType.Line;
            Series estimation = new Series("Estimated");
            estimation.ChartType = SeriesChartType.Line;

            List<Double> values = new List<Double>();
            for (int i = 0; i < samsungValues.Count; i++)
            {
                series.Points.AddXY(samsungValues[i].Date, samsungValues[i].Value);
                values.Add(samsungValues[i].Value);
            }

            List<Double> estimated = values.MovingAverage(200).ToList<Double>();
            DateTime date = samsungValues[0].Date;
            for (int i = 0; i < samsungValues.Count + 200; i++)
            {
                if (i < 200)
                    estimation.Points.AddXY(date, values[i]);
                else 
                    estimation.Points.AddXY(date, estimated[i-200]);

                date = date.AddMinutes(1);
            }

            MA5.Text = $"MA5 - {values.MovingAverage(5).ToList<Double>()[values.Count - 1].ToString()}";
            MA10.Text = $"MA10 - {values.MovingAverage(10).ToList<Double>()[values.Count - 1].ToString()}";
            MA20.Text = $"MA20 - {values.MovingAverage(20).ToList<Double>()[values.Count - 1].ToString()}";
            MA50.Text = $"MA50 - {values.MovingAverage(50).ToList<Double>()[values.Count - 1].ToString()}";
            MA100.Text = $"MA100 - {values.MovingAverage(100).ToList<Double>()[values.Count - 1].ToString()}";
            MA200.Text = $"MA200 - {values.MovingAverage(200).ToList<Double>()[values.Count - 1].ToString()}";

            bar.ChartAreas[0].AxisY.Minimum = 970;

            bar.Series.Clear();
            bar.Series.Add(series);
            bar.Series.Add(estimation);
        }
    }
}
