using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIVS_LAB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            f = new TaskForm();
            f.Hide();
        }

        TaskForm f;
        Random rgen = new Random((int)DateTime.Now.Second);

        private double AvgDist(double a, double b)
        {
            return a + rgen.NextDouble() * (b - a);
        }

        private double NormDist(float micro, float D, float X)
        {
            double S = 0;
            double w = Math.Sqrt(D);
            for (int i = 0; i < m * m; i++)
            {
                S += X;
            }
            return (S - micro * m * m) / Math.Sqrt(m * m * D);
        }

        private double NormDist(float micro, float D)
        {
            double S = 0;
            double w = Math.Sqrt(D);
            for (int i = 0; i < m*m; i++)
            {
                S += AvgDist(micro -w, micro + w);
            }
            return (S - micro * m*m) / Math.Sqrt(m*m*D);
        }

        private double StudentDistribution(int m)
        {
            float[] x = new float[m];
            for (int i = 0; i < m; i++)
            {
                x[i] = (float)NormDist(0, 1);
            }
            double sq_sum = 0;
            for (int i = 1; i < m; i++)
            {
                sq_sum = x[i] * x[i];
            }
            return x[0] * Math.Sqrt(m) / (Math.Sqrt(sq_sum / m));
        }

        private float[] grid(float from, float to, int count)
        {
            float[] ret = new float[count];
            h = Math.Abs(to - from) / count;
            ret[0] = from;
            for (int i = 1; i < count; i++)
            {
                ret[i] = ret[i - 1] + (float)h;
            }
            return ret;
        }
        double h = 0;
        const int n = 15;
        const float alpha = 0.05f;
        const float y0 = 23.7f;
        int N = 1000;
        int m = 2;

        private void button1_Click(object sender, EventArgs e)
        {
            int.TryParse(maskedTextBox1.Text, out m);
            float[] a = new float[N];
            var cnt = new int[n];
            for (int i = 0; i < N; i++)
            {
                a[i] = (float)StudentDistribution(m);
                //a[i] = (float)NormDist(0, 1);
            }
            float max_A = a[0];
            float min_A = a[0];
            for (int i = 0; i < N; i++)
            {
                if (a[i] > max_A)
                    max_A = a[i];
                if (a[i] < min_A)
                    min_A = a[i];
            }
            var gr = grid(min_A, max_A, n);
            System.Array.Sort(a);
            for (int i = 0; i < N; i++)
            {
                if (a[i] < gr[0])
                    cnt[0]++;
                else if (a[i] >= gr[n - 1])
                    cnt[n - 1]++;
                else
                {
                    int gr_i = 1;
                    while (a[i] >= gr[gr_i] || a[i] < gr[gr_i - 1])
                        gr_i++;
                    cnt[gr_i]++;
                }
            }

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            for (int i = 0; i < n; i++)
            {
                chart1.Series[0].Points.AddXY(gr[i], 1f*cnt[i] / N );
            }
            for (int i = 0; i < N; i++)
            {
                chart2.Series[0].Points.AddXY(a[i], i * 1f / N);
                
            }

            double hi2_sum = 0;
            for (int i = 0; i < n; i++)
            {
                hi2_sum += (cnt[i]* cnt[i]) * n / N;
            }
            double hi2 = n * hi2_sum / N;
            if (hi2 < y0)
                label2.Text = ("Гипотеза принята (alpha = 0.05, [n-1] = 14, hi^2 = " + hi2 + ").");
            else
                label2.Text = ("hi ^ 2 = " + hi2 + ").");


            chart1.Invalidate();
            chart2.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Show();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
