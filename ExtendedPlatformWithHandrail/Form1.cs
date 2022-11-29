using System;
using System.Configuration;
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
using HelperLibrary;

namespace SteelStack
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

        private void btn_createModel_Click(object sender, EventArgs e)
        {
            handleInputs();
            SendInputs();
        }

        private void handleInputs()
        {
            if (txt_originX.Text == "" || txt_originX.Text == null)
            {
                txt_originX.Text = ConfigurationManager.AppSettings["originX"];
            }

            if (txt_originY.Text == "" || txt_originY.Text == null)
            {             
                txt_originY.Text = ConfigurationManager.AppSettings["originY"];
            }

            if (txt_originZ.Text == "" || txt_originZ.Text == null)
            {
                txt_originZ.Text = ConfigurationManager.AppSettings["originZ"];
            }

            if (txt_bottomDiameter1.Text == "" || txt_bottomDiameter1.Text == null)
            {
                txt_bottomDiameter1.Text = ConfigurationManager.AppSettings["bottomDiameter1"];
            }

            if (txt_topDiameter1.Text == "" || txt_topDiameter1.Text == null)
            {
                txt_topDiameter1.Text = ConfigurationManager.AppSettings["topDiameter1"];
            }

            if (txt_height1.Text == "" || txt_height1.Text == null)
            {
                txt_height1.Text = ConfigurationManager.AppSettings["height1"];
            }

            if (txt_thickness1.Text == "" || txt_thickness1.Text == null)
            {
                txt_thickness1.Text = ConfigurationManager.AppSettings["thickness1"];
            }

            if (txt_bottomDiameter2.Text == "" || txt_bottomDiameter2.Text == null)
            {
                txt_bottomDiameter2.Text = ConfigurationManager.AppSettings["bottomDiameter2"];
            }

            if (txt_topDiameter2.Text == "" || txt_topDiameter2.Text == null)
            {
                txt_topDiameter2.Text = ConfigurationManager.AppSettings["topDiameter2"];
            }

            if (txt_height2.Text == "" || txt_height2.Text == null)
            {
                txt_height2.Text = ConfigurationManager.AppSettings["height2"];
            }

            if (txt_thickness2.Text == "" || txt_thickness2.Text == null)
            {
                txt_thickness2.Text = ConfigurationManager.AppSettings["thickness2"];
            }

            if (txt_bottomDiameter3.Text == "" || txt_bottomDiameter3.Text == null)
            {
                txt_bottomDiameter3.Text = ConfigurationManager.AppSettings["bottomDiameter3"];
            }

            if (txt_topDiameter3.Text == "" || txt_topDiameter3.Text == null)
            {
                txt_topDiameter3.Text = ConfigurationManager.AppSettings["topDiameter3"];
            }

            if (txt_height3.Text == "" || txt_height3.Text == null)
            {
                txt_height3.Text = ConfigurationManager.AppSettings["height3"];
            }

            if (txt_thickness3.Text == "" || txt_thickness3.Text == null)
            {
                txt_thickness3.Text = ConfigurationManager.AppSettings["thickness3"];
            }

            if (txt_platformStartAngle.Text == "" || txt_platformStartAngle.Text == null)
            {
                txt_platformStartAngle.Text = ConfigurationManager.AppSettings["platformStartAngle"];
            }

            if (txt_platformEndAngle.Text == "" || txt_platformEndAngle.Text == null)
            {
                txt_platformEndAngle.Text = ConfigurationManager.AppSettings["platformEndAngle"];
            }

            if (txt_platformLength.Text == "" || txt_platformLength.Text == null)
            {
                txt_platformLength.Text = ConfigurationManager.AppSettings["platformLength"];
            }

            if (txt_extensionStartAngle.Text == "" || txt_extensionStartAngle.Text == null)
            {
                txt_extensionStartAngle.Text = ConfigurationManager.AppSettings["extensionStartAngle"];
            }

            if (txt_extensionEndAngle.Text == "" || txt_extensionEndAngle.Text == null)
            {
                txt_extensionEndAngle.Text = ConfigurationManager.AppSettings["extensionEndAngle"];
            }

            if (txt_extensionLength.Text == "" || txt_extensionLength.Text == null)
            {
                txt_extensionLength.Text = ConfigurationManager.AppSettings["extensionLength"];
            }
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
                Convert.ToDouble(txt_thickness3.Text)
            });

            Globals global = new Globals(
                Convert.ToDouble(txt_originX.Text),
                Convert.ToDouble(txt_originY.Text),
                Convert.ToDouble(txt_originZ.Text),
                stackSegList,
                Convert.ToDouble(txt_platformStartAngle.Text),
                Convert.ToDouble(txt_platformEndAngle.Text),
                Convert.ToDouble(txt_platformLength.Text),
                Convert.ToDouble(txt_extensionStartAngle.Text),
                Convert.ToDouble(txt_extensionEndAngle.Text),
                Convert.ToDouble(txt_extensionLength.Text));

            TeklaModelling teklaModel = new TeklaModelling(
                Convert.ToDouble(txt_originX.Text),
                Convert.ToDouble(txt_originY.Text),
                Convert.ToDouble(txt_originZ.Text));

            ComponentHandler componentHandle = new ComponentHandler(global, teklaModel);

            
        }

        private void lbl_startAngle_Click(object sender, EventArgs e)
        {

        }
    }
}
