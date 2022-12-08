using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
namespace SteelStack.Components
{
    class FloorSteel
    {
        Globals _global;
        TeklaModelling _tModel;

        List<TSM.ContourPoint> _pointsList;

        public FloorSteel(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _pointsList = new List<TSM.ContourPoint>();

            FloorPlate();
        }

        public void FloorPlate()
        {
            TSM.ContourPoint FloorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]);


            TSM.ContourPoint TaperedSegmentPointUp1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][0] / 2, 1);
            TSM.ContourPoint TaperedSegmentPointUp2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][0] / 2, 3);
            FloorPlateOrigin = _tModel.ShiftVertically(FloorPlateOrigin, -_global.StackSegList[0][2]);
            TSM.ContourPoint TaperedSegmentPointDown1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            TSM.ContourPoint TaperedSegmentPointDown2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 3);


            FloorPlateOrigin = _tModel.ShiftVertically(FloorPlateOrigin, _global.StackSegList[0][2] / 2);

            TSM.ContourPoint PlatePoint1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            double PLatePoint1Z = _tModel.PointSlopeForm(new[] { FloorPlateOrigin.X, FloorPlateOrigin.Z }, 1.0, FloorPlateOrigin.X + (_global.StackSegList[0][1] / 2));
            PlatePoint1.Z = PLatePoint1Z;

            TSM.ContourPoint PlatePoint2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 3);
            double PLatePoint2Z = _tModel.PointSlopeForm(new[] { FloorPlateOrigin.X, FloorPlateOrigin.Z }, 1.0, FloorPlateOrigin.X - (_global.StackSegList[0][1] / 2));
            PlatePoint2.Z = PLatePoint2Z;

            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.FRONT;


            TSM.ContourPoint FloorPlatePoint1 = new TSM.ContourPoint(_tModel.IntersectionOfLineXZ(PlatePoint1, PlatePoint2, TaperedSegmentPointUp1, TaperedSegmentPointDown1), new TSM.Chamfer(0, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint FloorPlatePoint2 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(FloorPlateOrigin, _tModel.GetRadiusAtElevation(FloorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList), 1, Math.PI / 2), null);
            TSM.ContourPoint FloorPlatePoint3 = new TSM.ContourPoint(_tModel.IntersectionOfLineXZ(PlatePoint1, PlatePoint2, TaperedSegmentPointUp2, TaperedSegmentPointDown2), new TSM.Chamfer(0, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint FloorPlatePoint4 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(FloorPlateOrigin, _tModel.GetRadiusAtElevation(FloorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList), 1, 3 * Math.PI / 2), null);

            _pointsList.Add(FloorPlatePoint1);
            _pointsList.Add(FloorPlatePoint2);
            _pointsList.Add(FloorPlatePoint3);
            _pointsList.Add(FloorPlatePoint4);

            TSM.ContourPlate floorPlate = _tModel.CreateContourPlate(_pointsList, "PL30", Globals.MaterialStr, "12", _global.Position, "FloorPlate");
            FloorSteelCut(floorPlate);

            _pointsList.Clear();
        }

        public void FloorSteelCut(TSM.ContourPlate floorPlate)
        {
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.BELOW;
            _global.ClassStr = BooleanPart.BooleanOperativeClassName;

            T3D.Point startPoint = new T3D.Point(_global.Origin.X, _global.Origin.Y, _global.Origin.Z + _global.StackSegList[0][4]);
            T3D.Point endPoint = _tModel.ShiftVertically(new TSM.ContourPoint(startPoint, null), _global.StackSegList[0][2]);
            _global.NameStr = "floorSteelCut";


            double cutThickness = (_global.StackSegList[0][1] - _global.StackSegList[0][0]) * 2;

            // SPD profile is needed as CHS will cut inner part of the plate too and no floorsteel would be there

            _global.ProfileStr = "SPD" + (_global.StackSegList[0][1] + (2 * cutThickness)) + "*" + (_global.StackSegList[0][0] + (2 * cutThickness)) + "*" + cutThickness;
            TSM.Beam cut = _tModel.CreateBeam(startPoint, endPoint, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position, "myBeam");

            _tModel.cutPart(cut, floorPlate);

        }
    }
}
