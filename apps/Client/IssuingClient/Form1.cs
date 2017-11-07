using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using RestSharp;

namespace IssuingClient
{
    public partial class Form1 : Form
    {
        delegate void CallerDelegate(Label label, string target);
        readonly Random _rnd = new Random();
        private static bool _stop;
        private static bool _wait;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            cmbService.SelectedIndex = 0;
        }

        private void GenerateCallers(int count, string target)
        {
            box.Controls.Clear();

            var labels = new List<Label>();
            int top = 13;
            int colIndex = 0;

            for (int i = 0; i < count; i++)
            {
                int squareSize = 50;

                var label = new Label();
                label.Top = top;
                label.Left = 5 + (squareSize + 5) * colIndex;
                label.Height = squareSize;
                label.Width = squareSize;

                var textHeight = label.Height - label.Top - label.Bottom;

                if (textHeight > 0)
                {
                    label.Font = new Font(label.Font.FontFamily, textHeight, GraphicsUnit.Pixel);
                }
                label.BackColor = Color.White;
                label.Text = "asd";

                box.Controls.Add(label);

                labels.Add(label);
                colIndex++;
                if (colIndex % 10 == 0)
                {
                    colIndex = 0;
                    top += 55;
                }
            }

            _stop = false;
            _wait = true;
            foreach (var l in labels)
            {
                var caller = new CallerDelegate(StartCaller);
                caller.BeginInvoke(l, target, null, null);
            }
            _wait = false;
        }

        private void StartCaller(Label label, string target)
        {
            while (_wait) { }

            var client = new RestClient(target);
            client.Timeout = 100;
            var rnd = new Random();

            while (!_stop)
            {
                WaitRand(3000);
                label.BackColor = Color.White;
                Thread.Sleep(100);

                try
                {
                    var tid = rnd.Next(1, 10);

                    var response = client.Execute(new RestRequest($"/api/transaction/{tid}", Method.GET));
                    if (!response.IsSuccessful)
                        throw new Exception($"Response code: {response.StatusCode}");

                    label.BackColor = Color.Green;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    label.BackColor = Color.Red;
                }
            }
        }

        private void WaitRand(int ms)
        {
            Thread.Sleep(_rnd.Next(ms));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var target = (cmbService.SelectedIndex == 0) ? "http://localhost:9100/" : "http://localhost:5000/";
            GenerateCallers(int.Parse(comboBox1.SelectedItem.ToString()), target);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _stop = true;
        }
    }
}
