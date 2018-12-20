﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MetalParser
{
    public partial class Form1 : Form
    {
        string platinum = "https://ru.investing.com/commodities/platinum";
        string gold = "https://ru.investing.com/commodities/gold";
        string silver = "https://ru.investing.com/commodities/silver";
        //string samsung = "https://ru.investing.com/equities/samsung-electronics-co-ltd";
        string samsung = "https://ru.investing.com/equities/samsung-electronics-co-ltd-gdr";
        string apple = "https://ru.investing.com/equities/apple-computer-inc";
        Timer tpt = new Timer();
        Timer tau = new Timer();
        Timer tag = new Timer();
        Timer tSamsung = new Timer();
        Timer tApple = new Timer();
        //int timeout = 10 * 60 * 1000; //10 минут
        int timeout = 5000;

        string platinum_path = "@/data/platinum-cfd.txt";
        string gold_path = "@/data/gold-cfd.txt";
        string silver_path = "@/data/silver-cfd.txt";
        string samsung_path = "@/data/Samsung-electronincs-Co.txt";
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
            string parseValue = null;
            await Task.Run(async () =>
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = web.Load(Url);
                    if (doc != null)
                        parseValue = doc.DocumentNode.SelectNodes("//*[@id='last_last']")[0].InnerText;
                }
                catch (Exception)
                {
                    parseValue = "lost connection";
                }
            });
            return parseValue;
        }

        /// <summary>
        /// Метод асинхронно получает значение для соответствующего металла и записывает его в файл
        /// </summary>
        /// <param name="url">Ссылка на страницу со значением стоимости металла</param>
        /// <param name="option">Вариант металла: platinum, gold, silver</param>
        private async void GetValue(string url, string option)
        {
            string value = await FindValue(url);
            value = value.Replace(".", "");
            string line = DateTime.Now.ToString("dd.MM.yy hh:mm | ") + value;
            //string line = DateTime.Now.Year + "." + 
            //    DateTime.Now.Month + "." + 
            //    DateTime.Now.Day + " " + 
            //    DateTime.Now.Hour + ":" + 
            //    DateTime.Now.Minute + " | " + value;
            
            switch (option)
            {
                case "platinum":
                    using (StreamWriter sw = new StreamWriter(platinum_path, true))
                    {
                        sw.WriteLine(line);
                        if (value != "lost connection")
                        {
                            if(Double.Parse(value) > (platinumValues.Count > 1 ? platinumValues[platinumValues.Count-1] : 0))
                            {
                                label4.Text = value;
                                label4.ForeColor = System.Drawing.Color.Green;
                            }
                            else if((Double.Parse(value) == (platinumValues.Count > 1 ? platinumValues[platinumValues.Count - 1] : 0)))
                            {
                                label4.Text = value;
                                label4.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                label4.Text = value;
                                label4.ForeColor = System.Drawing.Color.Red;
                            }

                            label7.Text = DoLineExtr(platinumValues, 5).ToString();

                            platinumValues.Add(Double.Parse(value));
                        }
                    }
                        break;
                case "gold":
                    //textBox2.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(gold_path, true))
                    {
                        sw.WriteLine(line);
                        if (value != "lost connection")
                            goldValues.Add(Double.Parse(value));
                    }
                    break;
                case "silver":
                    //textBox3.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(silver_path, true))
                    {
                        sw.WriteLine(line);
                        if (value != "lost connection")
                            silverValues.Add(Double.Parse(value));
                    }
                    break;
                case "samsung":
                    textBox1.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(samsung_path, true))
                    {
                        sw.WriteLine(line);
                        if (value != "lost connection")
                        {
                            if (Double.Parse(value) > (samsungValues.Count > 1 ? samsungValues[samsungValues.Count - 1] : 0))
                            {
                                label4.Text = value;
                                label4.ForeColor = System.Drawing.Color.Green;
                            }
                            else if ((Double.Parse(value) == (samsungValues.Count > 1 ? samsungValues[samsungValues.Count - 1] : 0)))
                            {
                                label4.Text = value;
                                label4.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                label4.Text = value;
                                label4.ForeColor = System.Drawing.Color.Red;
                            }

                            label7.Text = DoLineExtr(samsungValues, 5).ToString();

                            samsungValues.Add(Double.Parse(value));
                        }
                    }
                    break;
                case "apple":
                    textBox2.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(apple_path, true))
                    {
                        sw.WriteLine(line);
                        if (value != "lost connection")
                        {
                            if (Double.Parse(value) > (appleValues.Count > 1 ? appleValues[appleValues.Count - 1] : 0))
                            {
                                label5.Text = value;
                                label5.ForeColor = System.Drawing.Color.Green;
                            }
                            else if ((Double.Parse(value) == (appleValues.Count > 1 ? appleValues[appleValues.Count - 1] : 0)))
                            {
                                label5.Text = value;
                                label5.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                label5.Text = value;
                                label5.ForeColor = System.Drawing.Color.Red;
                            }

                            label3.Text = DoLineExtr(appleValues, 5).ToString();

                            appleValues.Add(Double.Parse(value));
                        }
                    }
                    break;
                default:
                    break;
            }
            //await Task.Delay(60000);
        }

        private double DoLineExtr(List<double> inputList, int interval)
        {
            List<double> valuesList = new List<double>(inputList);
            List<double> incrementList = new List<double>();
            incrementList.Add(0);
            if (interval > inputList.Count)
                interval = inputList.Count;

            //valuesList.RemoveRange(inputList.Count - interval - 1, interval);
            valuesList.RemoveRange(0, valuesList.Count - interval);
            
            for(int i = 1; i < interval; i++)
                incrementList.Add(valuesList[i] - valuesList[i - 1]);

            double averageIncrement = 0;
            foreach (double value in incrementList)
                averageIncrement += value;

            averageIncrement /= incrementList.Count;

            return inputList[inputList.Count - 1] + averageIncrement;
        }

        private void FillValueListsFromFile(string path, List<double> valuesList)
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
            tSamsung.Tick += (timer, arguments) => GetValue(samsung, "samsung");
            tSamsung.Start();

            tApple.Interval = timeout;
            tApple.Tick += (timer, arguments) => GetValue(apple, "apple");
            tApple.Start();

            //tpt.Interval = timeout;
            //tpt.Tick += (timer, arguments) => GetValue(platinum,"platinum");
            //tpt.Start();

            //tau.Interval = timeout;
            //tau.Tick += (timer, arguments) => GetValue(gold, "gold");
            //tau.Start();

            //tag.Interval = timeout;
            //tag.Tick += (timer, arguments) => GetValue(silver, "silver");
            //tag.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            tSamsung.Stop();
            tApple.Stop();
            //tpt.Stop();
            //tau.Stop();
            //tag.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                FillValueListsFromFile(platinum_path, platinumValues);
                FillValueListsFromFile(gold_path, goldValues);
                FillValueListsFromFile(silver_path, silverValues);
                FillValueListsFromFile(samsung_path, samsungValues);
                FillValueListsFromFile(apple_path, appleValues);
            }
            catch(Exception ex)
            {

            }
            int a = platinumValues.Count;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
