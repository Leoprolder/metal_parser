using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        Timer tpt = new Timer();
        Timer tau = new Timer();
        Timer tag = new Timer();
        int timeout = 10 * 60 * 1000; //12 минут
        //int timeout = 5000;
        string platinum_path = "@/data/platinum-cfd.txt";
        string gold_path = "@/data/gold-cfd.txt";
        string silver_path = "@/data/silver-cfd.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private static async Task<string> FindValue(string Url)
        {
            string metal = null;
            await Task.Run(async () =>
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = web.Load(Url);
                    if (doc != null)
                        metal = doc.DocumentNode.SelectNodes("//*[@id='last_last']")[0].InnerText;
                }
                catch (Exception)
                {
                    metal = "lost connection";
                }
            });
            return metal;
        }

        private async void GetValue(string url, int opt)
        {
            string value = await FindValue(url);
            string line = DateTime.Now.Year + "." + 
                DateTime.Now.Month + "." + 
                DateTime.Now.Day + " " + 
                DateTime.Now.Hour + ":" + 
                DateTime.Now.Minute + " - " + value;
            //opt:
            // 1 - platinum
            // 2 - gold
            // 3 - silver
            switch (opt)
            {
                case 1:
                    textBox1.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(platinum_path, true))
                        sw.WriteLine(line);
                        break;
                case 2:
                    textBox2.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(gold_path, true))
                        sw.WriteLine(line);
                    break;
                case 3:
                    textBox3.Text += line + Environment.NewLine;
                    using (StreamWriter sw = new StreamWriter(silver_path, true))
                        sw.WriteLine(line);
                    break;
                default:
                    break;
            }
            //await Task.Delay(60000);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string value = await GetValue(platinum);
            Console.WriteLine(value);*/
            button1.Enabled = false;
            button2.Enabled = true;
            tpt.Interval = timeout;
            tpt.Tick += (timer, arguments) => GetValue(platinum,1);
            tpt.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            tpt.Stop();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button3.Enabled = true;
            tau.Interval = timeout;
            tau.Tick += (timer, arguments) => GetValue(gold,2);
            tau.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            button3.Enabled = false;
            tau.Stop();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            button5.Enabled = true;
            tag.Interval = timeout;
            tag.Tick += (timer, arguments) => GetValue(silver,3);
            tag.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            button5.Enabled = false;
            tag.Stop();
        }
    }
}
