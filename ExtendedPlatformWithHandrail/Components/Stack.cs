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
    class StackShell
    {
        Globals _global;
        TeklaModelling _tModel;

        public StackShell(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            CalculateElevation();
            CreateStack();
        }

        void CalculateElevation()
        {
            double elevation = 0;

            foreach (List<double> segment in _global.StackSegList)
            {
                segment.Add(elevation);
                elevation += segment[2];
            }
        }
        void CreateStack()
        {
            int counter = 0;

            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.FRONT;
            _global.ClassStr = "1";

            for (counter = 0; counter < 3; counter++)
            {
                T3D.Point startPoint = new T3D.Point(_global.Origin.X, _global.Origin.Y, _global.Origin.Z + _global.StackSegList[counter][4]);
                T3D.Point endPoint = _tModel.ShiftVertically(new TSM.ContourPoint(startPoint, null), _global.StackSegList[counter][2]);
                _global.NameStr = "segment" + (counter + 1);

                // CHS profile requires outer diameter, we get inner diameter fom user input. Hence outerDiameter = innerDiameter + (2 * segmentThickness)
                _global.ProfileStr = "CHS" + (_global.StackSegList[counter][1] + (2 *  _global.StackSegList[counter][3])) + "*" + (_global.StackSegList[counter][0] + (2 * _global.StackSegList[counter][3])) + "*" + _global.StackSegList[counter][3];
                _tModel.CreateBeam(startPoint, endPoint, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position, "myBeam");
            }
        }
    }
}
