using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;



namespace SteelStack.Components
{
    class Platform
    {
        Globals _global;
        TeklaModelling _tModel;

        List<TSM.ContourPoint> _pointsList;

        public Platform(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _pointsList = new List<TSM.ContourPoint>();

            CreatePlatform();

        }

        void CreatePlatform()
        {
            double startAngle;
            double endAngle;

            // first half of platform

            startAngle = _global.PlatformStartAngle;
            endAngle = _global.ExtensionStartAngle;

            if (startAngle != endAngle)
            {
                CreatePlatformPlate(startAngle, endAngle, _global.PlatformLength);
            }

            // extension

            startAngle = _global.ExtensionStartAngle;
            endAngle = _global.ExtensionEndAngle;

            if (startAngle != endAngle)
            {
                CreatePlatformPlate(startAngle, endAngle, _global.PlatformLength + _global.ExtensionLength);
            }
            // second half of platform

            startAngle = _global.ExtensionEndAngle;
            endAngle = _global.PlatformEndAngle;

            if (startAngle != endAngle)
            {
                CreatePlatformPlate(startAngle, endAngle, _global.PlatformLength);
            }
        }

        void CreatePlatformPlate(double startAngle, double endAngle, double length)
        {
            TSM.ContourPoint platformOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[2][4] + (_global.StackSegList[2][2]) / 2);
            double midAngle = (endAngle - startAngle) / 2;
            TSM.ContourPoint startPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3], 1, startAngle);
            TSM.ContourPoint midPoint = new TSM.ContourPoint(_tModel.ShiftAlongCircumferenceRad(startPoint, midAngle, 1), new TSM.Chamfer(0, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint endPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3], 1, endAngle);

            _pointsList.Add(startPoint);
            _pointsList.Add(midPoint);
            _pointsList.Add(endPoint);

            _global.ProfileStr = "PL" + length + "*25";
            _global.ClassStr = "10";
            _global.Position.Plane = TSM.Position.PlaneEnum.RIGHT;
            _global.Position.Rotation = TSM.Position.RotationEnum.FRONT;
            _global.Position.Depth = TSM.Position.DepthEnum.BEHIND;

            _tModel.CreatePolyBeam(_pointsList, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position, "Platform");

            _pointsList.Clear();
        }

    }
}
