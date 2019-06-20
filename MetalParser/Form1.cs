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
using MetalParser.Predicting;
using System.Linq;

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
        List<Data> samsungValues = new List<Data>();
        List<double> appleValues = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private async Task<string> FindValue(string Url, Object sender, EventArgs e)
        {
            //tSamsung.Interval = timeout;
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

            using (FileStream fs = new FileStream(samsungJsonData, FileMode.Append))
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
            //Chart bar = new Chart();
            //bar.Palette = ChartColorPalette.Pastel;
            //bar.Parent = this;
            //bar.Dock = DockStyle.None;
            //bar.Location = new System.Drawing.Point(388, 81);
            //bar.Margin = new Padding(3, 3, 3, 3);
            //bar.Size = new System.Drawing.Size(519, 382);
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
                    //if (i < accuracy)
                    //    predictedValues.Add(samsungValues[i].Value);
                    //else
                    //{
                    //    Double sum = 0.0;
                    //    for (int j = i; j < i + accuracy; j++)
                    //    {
                    //        sum += samsungValues[j - accuracy].Value;
                    //    }
                    //    predictedValues.Add(sum/accuracy);
                    //}
                    predictedValues1.Add(samsungValues[i].Value);
                }

                //predictedValues1 = new List<Double>(MAPredictor.PredictList(predictedValues1, accuracy));
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
                    //if (i < accuracy)
                    //    predictedValues.Add(samsungValues[i].Value);
                    //else
                    //{
                    //    List<Double> tempList = new List<Double>();
                    //    for (int j = i; j < i + accuracy; j++)
                    //    {
                    //        tempList.Add(samsungValues[j - accuracy].Value);
                    //    }
                    //    predictedValues.Add(predictorFactory.PredictValue(tempList, accuracy));
                    //}
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
                //?
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

            bar.ChartAreas[0].AxisY.Minimum = 900;

            bar.Series.Clear();
            bar.Series.Add(series);
            bar.Series.Add(estimation);
        }
    }
}
