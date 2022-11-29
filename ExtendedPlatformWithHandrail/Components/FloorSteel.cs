using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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

            _pointsList= new List<TSM.ContourPoint>();

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


            FloorPlateOrigin = _tModel.ShiftVertically(FloorPlateOrigin, _global.StackSegList[0][2]/2);

            TSM.ContourPoint PlatePoint1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            double PLatePoint1Z = _tModel.PointSlopeForm(new[] { FloorPlateOrigin.X, FloorPlateOrigin.Z }, 1.0/30.0, FloorPlateOrigin.X + (_global.StackSegList[0][1] / 2));
            PlatePoint1.Z = PLatePoint1Z;

            TSM.ContourPoint PlatePoint2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 3);
            double PLatePoint2Z = _tModel.PointSlopeForm(new[] { FloorPlateOrigin.X, FloorPlateOrigin.Z }, 1.0/30.0, FloorPlateOrigin.X - (_global.StackSegList[0][1] / 2));
            PlatePoint2.Z = PLatePoint2Z;

            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.FRONT;

            
            TSM.ContourPoint FloorPlatePoint1 = new ContourPoint(_tModel.IntersectionOfLineXZ(PlatePoint1, PlatePoint2, TaperedSegmentPointUp1, TaperedSegmentPointDown1), new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint FloorPlatePoint2 = new ContourPoint(_tModel.ShiftHorizontallyRad(FloorPlateOrigin, _tModel.GetRadiusAtElevation(FloorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList), 1, Math.PI/2), new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint FloorPlatePoint3 = new ContourPoint(_tModel.IntersectionOfLineXZ(PlatePoint1, PlatePoint2, TaperedSegmentPointUp2, TaperedSegmentPointDown2), new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint FloorPlatePoint4 = new ContourPoint(_tModel.ShiftHorizontallyRad(FloorPlateOrigin, _tModel.GetRadiusAtElevation(FloorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList), 1, 3 * Math.PI/2), new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));

            //_tModel.CreateBeam( FloorPlatePoint1, FloorPlatePoint3, "ROD30", Globals.MaterialStr, "3", _global.Position, "Beam1");
            //_tModel.CreateBeam( FloorPlatePoint2, FloorPlatePoint4, "ROD30", Globals.MaterialStr, "3", _global.Position, "Beam1");

            _pointsList.Add(FloorPlatePoint1);
            _pointsList.Add(FloorPlatePoint2);
            _pointsList.Add(FloorPlatePoint3);
            _pointsList.Add(FloorPlatePoint4);
            _pointsList.Add(FloorPlatePoint1);

            _tModel.CreateContourPlate(_pointsList, "PL30", Globals.MaterialStr, "3", _global.Position, "FloorPlate") ;

            _pointsList.Clear();
        }
    }
}
