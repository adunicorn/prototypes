using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace IssuingClient
{
    public partial class Form1 : Form
    {
        delegate void CallerDelegate(Panel panel);
        readonly Random _rnd = new Random();
        private static bool _stop;

        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateCallers(int count, string target)
        {
            box.Controls.Clear();

            var panels = new List<Panel>();
            int top = 13;
            int colIndex = 0;

            for (int i = 0; i < count; i++)
            {
                int squareSize = 50;

                var p = new Panel();
                p.Top = top;
                p.Left = 5 + (squareSize + 5) * colIndex;
                p.Height = squareSize;
                p.Width = squareSize;

                p.BackColor = Color.White;

                box.Controls.Add(p);

                panels.Add(p);
                colIndex++;
                if (colIndex % 10 == 0)
                {
                    colIndex = 0;
                    top += 55;
                }
            }

            foreach (var p in panels)
            {
                var caller = new CallerDelegate(StartCaller);
                caller.BeginInvoke(p, null, null);
            }
        }

        private void StartCaller(Panel panel)
        {
            while (!_stop)
            {
                WaitRand(500);
                panel.BackColor = Color.AntiqueWhite;

                WaitRand(500);
                panel.BackColor = Color.BlueViolet;
            }
        }

        private void WaitRand(int ms)
        {
                Thread.Sleep(_rnd.Next(ms));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GenerateCallers(32, "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _stop = true;
        }
    }
}
