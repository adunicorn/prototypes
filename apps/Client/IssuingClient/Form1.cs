using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;

namespace IssuingClient
{
    public partial class Form1 : Form
    {
        delegate System.Threading.Tasks.Task CallerDelegate(Label label, string target);
        readonly Random _rnd = new Random();
        private static bool _stop;
        private static bool _wait;
        private static FontFamily _labelFontFamily = FontFamily.GenericSansSerif;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            cmbService.SelectedIndex = 0;
        }

        private void GenerateCallers(int count, string target)
        {
            box.Controls.Clear();
            int nrCols;
            int nrRows;

            switch (count)
            {
                case 10:
                    nrCols = 5;
                    nrRows = 2;
                    break;
                case 20:
                    nrCols = 5;
                    nrRows = 4;
                    break;
                case 50:
                    nrCols = 10;
                    nrRows = 5;
                    break;
                case 100:
                    nrCols = 10;
                    nrRows = 10;
                    break;
                default:
                    MessageBox.Show("Nr boxes invalid");
                    return;
            }

            var labels = new List<Label>();
            int top = 13;
            int colIndex = 0;
            int height = ((box.Height - 20) / nrRows) - 5;
            int width = ((box.Width - 10) / nrCols) - 5;

            var fontSize = CalculateFontSize(height, width);

            for (int i = 0; i < count; i++)
            {
                var label = new Label();
                label.Name = "lbl_" + Guid.NewGuid().ToString();
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Top = top;
                label.Left = 5 + (width + 5) * colIndex;
                label.Height = height;
                label.Width = width;

                var textHeight = label.Height - label.Top - label.Bottom;

                label.Font = new Font(_labelFontFamily, fontSize, label.Font.Style);
                label.BackColor = Color.White;
                label.Text = ".";

                box.Controls.Add(label);

                labels.Add(label);
                colIndex++;
                if (colIndex % nrCols == 0)
                {
                    colIndex = 0;
                    top += height + 5;
                }
            }

            _stop = false;
            _wait = true;
            foreach (var l in labels)
            {
                var caller = new CallerDelegate(StartCallerAsync);
                caller.BeginInvoke(l, target, null, null);
            }
            _wait = false;
        }

        private static float CalculateFontSize(int height, int width)
        {
            var label = new Label();
            label.Height = height;
            label.Width = width;
            label.Font = new Font(label.Font.FontFamily, 100, label.Font.Style);
            label.Text = @"10'000
CHF";

            while (label.Width < System.Windows.Forms.TextRenderer.MeasureText(label.Text, new Font(_labelFontFamily, label.Font.Size, label.Font.Style)).Width ||
                   label.Height < System.Windows.Forms.TextRenderer.MeasureText(label.Text, new Font(_labelFontFamily, label.Font.Size, label.Font.Style)).Height)
            {
                label.Font = new Font(label.Font.FontFamily, label.Font.Size - 0.5f, label.Font.Style);
            }

            return label.Font.Size / 2;
        }

        private async System.Threading.Tasks.Task StartCallerAsync(Label label, string target)
        {
            while (_wait) { }

            var client = new RestClient(target);
            client.Timeout = 1300;

            int errorCounter = 0;

            while (!_stop)
            {
                try
                {
                    var tid = _rnd.Next(2, 900);

                    await Task.Delay(1000);
                    SetText(label, "...");

                    var response = await client.ExecuteGetTaskAsync<Transaction>(new RestRequest($"/api/transaction/{tid}"));
                    if (!response.IsSuccessful)
                        throw new Exception($"Response code: {response.StatusCode}");

                    SetText(label, $@"{response.Data.amount}
{response.Data.currency}");

                    var header = response.Headers.FirstOrDefault(x => x.Name == "version");

                    if(header == null || header.Value.ToString().Contains("1"))
                        await ChangeColorAsync(label, Color.Green);
                    else
                        await ChangeColorAsync(label, Color.Yellow);

                    errorCounter = 0;
                }
                catch (Exception e)
                {
                    SetText(label, (++errorCounter).ToString());
                    Console.WriteLine(e);
                    await ChangeColorAsync(label, Color.Red);
                }
            }
        }

        private static void SetText(Label label, string text)
        {
            label.BeginInvoke((MethodInvoker) delegate { label.Text = text; });
        }

        private static async Task ChangeColorAsync(Label label, Color labelBackColor)
        {
//            label.BackColor = Color.DimGray;

//            await Task.Delay(50);

            label.BackColor = labelBackColor;
        }


        private void WaitRand(int ms)
        {
            Thread.Sleep(_rnd.Next(ms));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var target = (cmbService.SelectedIndex == 0) ? "http://oldissuing.192.168.64.11.nip.io" : "http://issuing.192.168.64.11.nip.io/";
            Console.WriteLine($"Using target {target}");
            GenerateCallers(int.Parse(comboBox1.SelectedItem.ToString()), target);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _stop = true;
        }
    }
}
