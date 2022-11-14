using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtendedPlatformWithHandrail;
using Tekla.Structures.Model;
using Tekla.Structures.ModelInternal;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace HelperClasses
{
    public static class Global
    {
        public static double originX; 
        public static double originY;
        public static double originZ;

        public static string m_profileStr;
        public static string m_materialStr;
        public static string m_class;
        public static string m_name;
        public static TSM.Position m_position;
        public static TSM.Model myModel;

        public static List<List<Double>> stackSegList;
        public static int numberOfSegments;
        static Global()
        {
            m_profileStr = string.Empty;
            m_materialStr = string.Empty;
            m_class = string.Empty; 
            m_name = string.Empty; 
            m_position = new TSM.Position();
            myModel = new TSM.Model();
            stackSegList = new List<List<double>>();
        }
    }
}