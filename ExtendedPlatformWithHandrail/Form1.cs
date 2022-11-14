using HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            this.getInputs();
            this.createStack();

        }

        private void createStack()
        {
            int counter1 = 0;
            double heightUptoSegmentBase = 0.0D;

            Global.m_position.Depth = TSM.Position.DepthEnum.MIDDLE;
            Global.m_position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            Global.m_position.Rotation = TSM.Position.RotationEnum.FRONT;
            Global.m_materialStr = "IS2062";
            Global.m_class = "1";

            heightUptoSegmentBase = Global.originZ;

            for (counter1 = 0; counter1 < 3; counter1++)
            {
                T3D.Point startPoint = new T3D.Point(Global.originX, Global.originY, heightUptoSegmentBase);
                T3D.Point endPoint = GeometricalHelperClass.shiftVertically(new TSM.ContourPoint(startPoint, null), Global.stackSegList[counter1][2]);
                Global.m_name = "segment" + (counter1 + 1);
                Global.m_profileStr = "CHS" + Global.stackSegList[counter1][1] + "*" + Global.stackSegList[counter1][0] + "*" + Global.stackSegList[counter1][3];
                TeklaModelling.CreateBeam(startPoint, endPoint, Global.m_profileStr, Global.m_materialStr, Global.m_class, Global.m_position, "myBeam");

                heightUptoSegmentBase += Global.stackSegList[counter1][2];
            }
        }

        private void getInputs()
        {

            Global.originX = Convert.ToDouble(txt_originX.Text);
            Global.originY = Convert.ToDouble(txt_originY.Text);
            Global.originZ = Convert.ToDouble(txt_originZ.Text);

            Global.stackSegList.Add(new List<double> { 
                Convert.ToDouble(txt_topDiameter1.Text),
                Convert.ToDouble(txt_bottomDiameter1.Text),
                Convert.ToDouble(txt_height1.Text),
                Convert.ToDouble(txt_thickness1.Text)});
            Global.stackSegList.Add(new List<double> {
                Convert.ToDouble(txt_topDiameter2.Text),
                Convert.ToDouble(txt_bottomDiameter2.Text),
                Convert.ToDouble(txt_height2.Text),
                Convert.ToDouble(txt_thickness2.Text)});
            Global.stackSegList.Add(new List<double> {
                Convert.ToDouble(txt_topDiameter3.Text),
                Convert.ToDouble(txt_bottomDiameter3.Text),
                Convert.ToDouble(txt_height3.Text),
                Convert.ToDouble(txt_thickness3.Text)});

        }

    }
}
