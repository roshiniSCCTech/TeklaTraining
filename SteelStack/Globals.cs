using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace SteelStack
{
    class Globals
    {
        public string ProfileStr;
        public const string MaterialStr = "IS2062";
        public string ClassStr;
        public string NameStr;
        public TSM.Position Position;
        
        public readonly TSM.ContourPoint Origin;
         
        // 0 - top inner diameter, 1 - bottom inner diameter, 2 - height, 3 - thickness, 4 - height from base stack to bottom of segment
        public readonly List<List<Double>> StackSegList;
         
        // PLatform 

        public readonly double PlatformStartAngle;
        public readonly double PlatformEndAngle;
        public readonly double PlatformLength;
        
        public readonly double ExtensionStartAngle;
        public readonly double ExtensionEndAngle;
        public readonly double ExtensionLength;

        // Floor Steel
        public readonly double FloorSteelThickness;
        public readonly double FloorSteelSlope;

        public Globals(
            double originX,
            double originY,
            double originZ,
            List<List<Double>> stackSegList,
            double platformStartAngle,
            double platformEndAngle,
            double platformLength,
            double extensionStartAngle,
            double extensionEndAngle,
            double extensionLength,
            double floorSteelThickness,
            double floorSteelSlope
            )
        {
            Origin = new TSM.ContourPoint(new T3D.Point(originX, originY, originZ), null);
            ProfileStr = "";
            ClassStr = "";
            NameStr = "";
            Position = new TSM.Position();
            this.StackSegList = stackSegList;
            PlatformStartAngle = platformStartAngle * Math.PI / 180;
            PlatformEndAngle = platformEndAngle * Math.PI / 180;
            PlatformLength = platformLength;
            ExtensionStartAngle = extensionStartAngle * Math.PI / 180;
            ExtensionEndAngle = extensionEndAngle * Math.PI / 180;
            ExtensionLength = extensionLength;
            FloorSteelThickness = floorSteelThickness;
            FloorSteelSlope = floorSteelSlope;
        }
    }
}
