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
        private static FontFamily _labelFontFamily = FontFamily.GenericSansSerif;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            cmbService.SelectedIndex = 0;

            this.cmbService.SelectedIndex = 2;
            this.comboBox1.SelectedIndex = 1;
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
                    MessageBox.Show("Invalid number of boxes");
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

            var tasks = labels.Select(l => new Task( async () => await StartCallerAsync(l, target)));

            Parallel.ForEach(tasks, t => t.Start());

        }

        private static float CalculateFontSize(int height, int width)
        {
            var label = new Label();
            label.Height = height;
            label.Width = width;
            label.Font = new Font(label.Font.FontFamily, 100, label.Font.Style);
            label.Text = @"10000.99
CHF";

            while (label.Width < System.Windows.Forms.TextRenderer.MeasureText(label.Text, new Font(_labelFontFamily, label.Font.Size, label.Font.Style)).Width ||
                   label.Height < System.Windows.Forms.TextRenderer.MeasureText(label.Text, new Font(_labelFontFamily, label.Font.Size, label.Font.Style)).Height)
            {
                label.Font = new Font(label.Font.FontFamily, label.Font.Size - 0.5f, label.Font.Style);
            }

            return label.Font.Size / 1.5f;
        }

        private async System.Threading.Tasks.Task StartCallerAsync(Label label, string target)
        {
            int errorCounter = 0;

            while (!_stop)
            {
                try
                {
                    await Task.Delay(_rnd.Next(1800, 2500));

                    var client = new RestClient(target);
                    client.Timeout = 10000;
                    RestRequest request = new RestRequest();

                    var tid = _rnd.Next(2, 999);
                    request.Resource = $"/api/transaction/{tid}";

                    IRestResponse<Transaction> response = await client.ExecuteGetTaskAsync<Transaction>(request);

                    if (response.IsSuccessful)
                    {
                        SetText(label, $@"{response.Data.amount}
{response.Data.currency}");

                        var version = response.Headers.FirstOrDefault(x => x.Name == "version");

                        if(version != null && version.Value.ToString().Contains("new"))
                            ChangeColor(label, Color.Yellow);
                        else
                            ChangeColor(label, Color.Green);

                        errorCounter = 0;
                    }
                    else
                    {
                        SetText(label, (++errorCounter).ToString());
                        ChangeColor(label, Color.Red);
                        Console.WriteLine($"{request.Resource} => Response status: {response.ResponseStatus} , HTTP status code: {response.StatusCode}");

                    }
                }
                catch (Exception e)
                {
                    SetText(label, (++errorCounter).ToString());
                    Console.WriteLine(e);
                    ChangeColor(label, Color.Orange);
                }
            }
        }

        private static void SetText(Label label, string text)
        {
            label.BeginInvoke((MethodInvoker) delegate { label.Text = text; });
        }

        private static void ChangeColor(Label label, Color labelBackColor)
        {
            label.BeginInvoke((MethodInvoker) delegate { label.BackColor = labelBackColor; });
        }


        private void WaitRand(int ms)
        {
            Thread.Sleep(_rnd.Next(ms));
        }

        private String GetServiceURL(int index)
        {
            if(index == 0) return "http://oldissuing.192.168.64.11.nip.io/";
            else if(index == 1) return "http://issuing.192.168.64.11.nip.io/";
            else return "http://pyissuing.192.168.64.11.nip.io/";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var target = GetServiceURL(cmbService.SelectedIndex);
            Console.WriteLine($"Using target {target}");
            GenerateCallers(int.Parse(comboBox1.SelectedItem.ToString()), target);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _stop = true;
        }
    }
}
