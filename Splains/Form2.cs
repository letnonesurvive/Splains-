using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Splains
{
    public partial class Form2 : Form
    {
        int n;
        double[] a;
        double[] b;
        double[] c;
        double[] d;
        public Form2(int _n, double[] _a,double [] _b,double[] _c,double[] _d)
        {
            n = _n;
            a = _a;
            b = _b;
            c = _c;
            d = _d;
            InitializeComponent();
        }
        public void Fill_Table()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowCount = n;
            dataGridView1.ColumnCount = 5;
            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                dataGridView1.Rows[i].Cells[1].Value = a[i+1];
                dataGridView1.Rows[i].Cells[2].Value = b[i];
                dataGridView1.Rows[i].Cells[3].Value = c[i];
                dataGridView1.Rows[i].Cells[4].Value = d[i];
            }
        }
    }
}
