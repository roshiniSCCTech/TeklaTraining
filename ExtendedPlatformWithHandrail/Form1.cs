using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tekla.Structures.Filtering.Categories.TaskFilterExpressions;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace ExtendedPlatformWithHandrail
{
    public partial class inputForm : Form
    {
        public inputForm()
        {
            InitializeComponent();
        }

        private void inputForm_Load(object sender, EventArgs e)
        {

        }

        private void txt_originZ_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_createPlatformHandrail_Click(object sender, EventArgs e)
        {
            SendInputs();
        }

        void SendInputs()
        {
            List<List<Double>> stackSegList = new List<List<double>> { };

            stackSegList.Add(new List<double> {
                Convert.ToDouble(txt_topDiameter1.Text),
                Convert.ToDouble(txt_bottomDiameter1.Text),
                Convert.ToDouble(txt_height1.Text),
                Convert.ToDouble(txt_thickness1.Text)});
            stackSegList.Add(new List<double> {
                Convert.ToDouble(txt_topDiameter2.Text),
                Convert.ToDouble(txt_bottomDiameter2.Text),
                Convert.ToDouble(txt_height2.Text),
                Convert.ToDouble(txt_thickness2.Text)});
            stackSegList.Add(new List<double> {
                Convert.ToDouble(txt_topDiameter3.Text),
                Convert.ToDouble(txt_bottomDiameter3.Text),
                Convert.ToDouble(txt_height3.Text),
                Convert.ToDouble(txt_thickness3.Text)});

            CreateExtendedPlatformWithHandrail phObj = new CreateExtendedPlatformWithHandrail(
                Convert.ToDouble(txt_originX.Text),
                Convert.ToDouble(txt_originY.Text),
                Convert.ToDouble(txt_originZ.Text),
                stackSegList);

            phObj.Build();
        }

    }
}
