using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
namespace Splains
{
    public partial class Form1 : Form
    {
        private int n;
        private double h;
        private double[] points;
        private double[] xi;
        double[] c;
        double[] b;
        double[] d;

        public Form1()
        {
            InitializeComponent();
        }
        //private struct Coefficients// имеется ввижу коэфициенты сплайна
        //{
        //    double ai;
        //    double bi;
        //    double ci;
        //    double di;
        //    Coefficients(double _ai = 0, double _bi = 0, double _ci = 0, double _di = 0)
        //    {
        //        ai = _ai;
        //        bi = _bi;
        //        ci = _ci;
        //        di = _di;
        //    }
        //}
        double f(int i, int j)
        {
            double result = (points[j] - points[i]) / h;
            return result;
        }
        private void Initialization()//считаем функцию в узлах сетки
        {
            double x = -1;
            for (int i = 0; i < n + 1; i++)
            {
                points[i] = TestFunction(x);
                xi[i] = x;
                x += h;
            }
        }
        private void GetCoef()
        {
            double[] delta = new double[n - 1];
            double[] lambda = new double[n - 1];
            c = new double[n];
            d = new double[n];
            b = new double[n];
            c[n - 1] = 0;
            delta[0] = -h / (2 * h + 2 * h);
            lambda[0] = (3 * f(1, 2) - 3 * f(0, 1)) / (2 * h + 2 * h);
            for (int i = 1; i < n - 1; i++)// прямой ход прогонки
            {
                delta[i] = -h / (2 * h + 2 * h + h * delta[i - 1]);
                lambda[i] = (3 * f(i + 1, i + 2) - 3 * f(i, i + 1) - h * lambda[i - 1]) / (2 * h + 2 * h + h * delta[i - 1]);
            }
            for (int i = n - 1; i > 0; i--)
                c[i - 1] = delta[i - 1] * c[i] + lambda[i - 1];

            for (int i = n - 1; i > 0; i--)
                d[i] = (c[i] - c[i - 1]) / (3 * h);
            d[0] = c[0] / (3 * h);

            for (int i = n - 1; i > 0; i--)
                b[i] = f(i, i + 1) + 2.0 / 3 * h * c[i] + 1.0 / 3 * h * c[i - 1];
            b[0] = f(0, 1) + 2.0 / 3 * h * c[0];

            for (int i = 0; i < n; i++)
            {
                c[i] = c[i] * 2;
                d[i] = d[i] * 6;
            }
        }
        private double TestFunction(double x)
        {
            if (-1 <= x && x <= 0)
                return (x * x * x + 3 * x * x);
            else if (0 < x && x <= 1)
                return (-x * x * x + 3 * x * x);
            else
                return 100000;
        }
        private double Splain(double x,int i)
        {
            return (points[i + 1] + b[i] * (x - xi[i+1]) + c[i] / 2 * (x - xi[i+1]) * (x - xi[i+1]) + d[i] / 6 * (x - xi[i+1]) * (x - xi[i+1]) * (x - xi[i+1]));
        }
        private void Draw()
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            PointPairList point_list1 = new PointPairList();

            double xmin = -1;
            double xmax = 1;
            int i = 0;
            point_list1.Add(-1, points[0]);
            for (double x = xmin + 0.001; x < xmax; x += 0.001)
            {
                if (x >= xi[i + 1])
                    i++;
                point_list1.Add(x, Splain(x, i));

            }
            point_list1.Add(1, points[n]);
            LineItem myCurve1;//графики
            myCurve1 = pane.AddCurve("S(x)", point_list1, Color.Blue, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            n = Convert.ToInt32(textBox1.Text);
            h = 2.0 / n;
            points = new double[n + 1];
            xi = new double[n + 1];
            Initialization();
            GetCoef();
            Draw();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(n, points, b, c, d);
            form.Fill_Table();
            form.Show();
        }
    }
}
