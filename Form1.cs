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

        private double NormDist(float micro, float D)
        {
            return Math.Exp(Math.Pow(rgen.NextDouble(), 2) / (-2)) / Math.Sqrt(Math.PI * 2);
        }

        private double StudentDistribution(int m)
        {
            float[] x = new float[m];
            for (int i = 0; i < m; i++)
            {
                x[i] = (float)NormDist(0,1);
            }
            double sq_sum = 0;
            for (int i = 1; i < m; i++)
            {
                sq_sum = x[i] * x[i];
            }
            return x[0] / (Math.Sqrt(sq_sum / m));
        }

        private float[] grid(float from, float to, int count)
        {
            float[] ret = new float[count];
            float h = Math.Abs(to - from) / count;
            ret[0] = from;
            for (int i = 1; i < count; i++)
            {
                ret[i] = ret[i - 1] + h;
            }
            return ret;
        }

        int interval_cnt = 15;

        private void button1_Click(object sender, EventArgs e)
        {
            float[] a = new float[1000];
            var cnt = new int[interval_cnt];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (float)StudentDistribution(3);
            }
            float max_A = a[0];
            float min_A = a[0];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > max_A)
                    max_A = a[i];
                if (a[i] < min_A)
                    min_A = a[i];
            }
            var gr = grid(min_A, max_A, interval_cnt);
            for (int i = 0; i < a.Length; i++)
            {
                int j = 0;
                while (j < interval_cnt && a[i] >= gr[j])
                    j++;
                if (j == interval_cnt)
                    j = interval_cnt - 1;
                cnt[j]++;
            }
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            for (int i = 0; i < gr.Length; i++)
            {
                chart1.Series[0].Points.AddXY(gr[i], cnt[i]);
            }
            float sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
                chart2.Series[0].Points.AddXY(i,sum);

            }
            chart1.Series[1].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending);
            chart1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Show();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
