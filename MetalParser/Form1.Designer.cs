namespace MetalParser
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.bar = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MA5 = new System.Windows.Forms.Label();
            this.MA10 = new System.Windows.Forms.Label();
            this.MA20 = new System.Windows.Forms.Label();
            this.MA50 = new System.Windows.Forms.Label();
            this.MA100 = new System.Windows.Forms.Label();
            this.MA200 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bar)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Начать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(94, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Закончить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBox1.Location = new System.Drawing.Point(13, 81);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(258, 335);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Samsung";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(277, 55);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "Построить график";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // bar
            // 
            chartArea2.Name = "ChartArea1";
            this.bar.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.bar.Legends.Add(legend2);
            this.bar.Location = new System.Drawing.Point(277, 81);
            this.bar.Name = "bar";
            this.bar.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Time Series";
            this.bar.Series.Add(series2);
            this.bar.Size = new System.Drawing.Size(630, 335);
            this.bar.TabIndex = 20;
            this.bar.Text = "chart1";
            // 
            // comboBox1
            // 
            this.comboBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "MA",
            "ARMA",
            "SSA"});
            this.comboBox1.Location = new System.Drawing.Point(405, 55);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 21;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(402, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Модель построения";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MA200);
            this.panel1.Controls.Add(this.MA100);
            this.panel1.Controls.Add(this.MA50);
            this.panel1.Controls.Add(this.MA20);
            this.panel1.Controls.Add(this.MA10);
            this.panel1.Controls.Add(this.MA5);
            this.panel1.Location = new System.Drawing.Point(620, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 75);
            this.panel1.TabIndex = 23;
            this.panel1.Visible = false;
            // 
            // MA5
            // 
            this.MA5.AutoSize = true;
            this.MA5.Location = new System.Drawing.Point(4, 4);
            this.MA5.Name = "MA5";
            this.MA5.Size = new System.Drawing.Size(29, 13);
            this.MA5.TabIndex = 0;
            this.MA5.Text = "MA5";
            // 
            // MA10
            // 
            this.MA10.AutoSize = true;
            this.MA10.Location = new System.Drawing.Point(4, 29);
            this.MA10.Name = "MA10";
            this.MA10.Size = new System.Drawing.Size(35, 13);
            this.MA10.TabIndex = 1;
            this.MA10.Text = "MA10";
            // 
            // MA20
            // 
            this.MA20.AutoSize = true;
            this.MA20.Location = new System.Drawing.Point(4, 52);
            this.MA20.Name = "MA20";
            this.MA20.Size = new System.Drawing.Size(35, 13);
            this.MA20.TabIndex = 2;
            this.MA20.Text = "MA20";
            // 
            // MA50
            // 
            this.MA50.AutoSize = true;
            this.MA50.Location = new System.Drawing.Point(137, 4);
            this.MA50.Name = "MA50";
            this.MA50.Size = new System.Drawing.Size(35, 13);
            this.MA50.TabIndex = 3;
            this.MA50.Text = "MA50";
            // 
            // MA100
            // 
            this.MA100.AutoSize = true;
            this.MA100.Location = new System.Drawing.Point(137, 29);
            this.MA100.Name = "MA100";
            this.MA100.Size = new System.Drawing.Size(41, 13);
            this.MA100.TabIndex = 4;
            this.MA100.Text = "MA100";
            // 
            // MA200
            // 
            this.MA200.AutoSize = true;
            this.MA200.Location = new System.Drawing.Point(137, 52);
            this.MA200.Name = "MA200";
            this.MA200.Size = new System.Drawing.Size(41, 13);
            this.MA200.TabIndex = 5;
            this.MA200.Text = "MA200";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(536, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 23);
            this.button4.TabIndex = 24;
            this.button4.Text = "Прогноз";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(919, 428);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataVisualization.Charting.Chart bar;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label MA200;
        private System.Windows.Forms.Label MA100;
        private System.Windows.Forms.Label MA50;
        private System.Windows.Forms.Label MA20;
        private System.Windows.Forms.Label MA10;
        private System.Windows.Forms.Label MA5;
        private System.Windows.Forms.Button button4;
    }
}

