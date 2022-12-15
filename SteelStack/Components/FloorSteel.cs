using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
namespace SteelStack.Components
{
    class FloorSteel
    {
        Globals _global;
        TeklaModelling _tModel;

        double _slope;
        double _slopeAngle;

        List<TSM.ContourPoint> _pointsList;

        List<List<TSM.ContourPoint>> _continuousBeams;

        public FloorSteel(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _slope = 1;
            _slopeAngle = Math.Atan(_slope);

            _pointsList = new List<TSM.ContourPoint>();

            CreateFloorPlate();
        }

        public void CreateFloorPlate()
        {
            TSM.ContourPoint FloorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]);


            TSM.ContourPoint TaperedSegmentPointUp1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][0] / 2, 1);
            TSM.ContourPoint TaperedSegmentPointUp2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][0] / 2, 3);
            FloorPlateOrigin = _tModel.ShiftVertically(FloorPlateOrigin, -_global.StackSegList[0][2]);
            TSM.ContourPoint TaperedSegmentPointDown1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            TSM.ContourPoint TaperedSegmentPointDown2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 3);


            FloorPlateOrigin = _tModel.ShiftVertically(FloorPlateOrigin, _global.StackSegList[0][2] / 2);

            TSM.ContourPoint PlatePoint1 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            double PLatePoint1Z = _tModel.PointSlopeForm(new[] { FloorPlateOrigin.X, FloorPlateOrigin.Z }, _slope, FloorPlateOrigin.X + (_global.StackSegList[0][1] / 2));
            PlatePoint1.Z = PLatePoint1Z;

            TSM.ContourPoint PlatePoint2 = _tModel.ShiftHorizontallyRad(FloorPlateOrigin, _global.StackSegList[0][1] / 2, 3);
            double PLatePoint2Z = _tModel.PointSlopeForm(new[] { FloorPlateOrigin.X, FloorPlateOrigin.Z }, _slope, FloorPlateOrigin.X - (_global.StackSegList[0][1] / 2));
            PlatePoint2.Z = PLatePoint2Z;

            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.FRONT;
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
            CreateFloorSteelCut(floorPlate);

            _pointsList.Clear();

            CreateFloorSteelBeams(FloorPlatePoint1 , FloorPlatePoint2, FloorPlatePoint3, FloorPlatePoint4);
            
        }

        public void CreateFloorSteelCut(TSM.ContourPlate floorPlate)
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

        public void CreateFloorSteelBeams(TSM.ContourPoint floorPlatePoint1, TSM.ContourPoint floorPlatePoint2, TSM.ContourPoint floorPlatePoint3, TSM.ContourPoint floorPlatePoint4)
        {
            // continous beams
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2] / 2);

            double inclinedDistBetweenBeams = _tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlatePoint3)/5;

            double inclinedDistFromCenter = -_tModel.DistanceBetweenPoints(floorPlatePoint3, floorPlateOrigin);

            for ( int i = 0; i < 4; i++ )
            {
                inclinedDistFromCenter += inclinedDistBetweenBeams;
                double xDistanceFromCenter = inclinedDistFromCenter * Math.Cos(_slopeAngle);
                CreateContinuousBeam(xDistanceFromCenter);
            }

            // broken beams

            double distBetweenBeams = _tModel.DistanceBetweenPoints(floorPlatePoint2, floorPlatePoint4)/5;

            double yDistFromCenter = -_tModel.DistanceBetweenPoints(floorPlatePoint2, floorPlateOrigin);

            for (int i = 0; i < 4; i++)
            {
                yDistFromCenter += distBetweenBeams;
                CreateBrokenBeam(yDistFromCenter, floorPlatePoint1, floorPlatePoint3);
            }

        }
        public void CreateContinuousBeam(double xDistFromCenter)
        {
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]/2); // plate thickness 30 / 2 = 15

            ContourPoint beamMidPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, xDistFromCenter, 1);
            beamMidPoint = _tModel.ShiftVertically(beamMidPoint, xDistFromCenter * Math.Tan(_slopeAngle));

            double rad = _tModel.GetRadiusAtElevation(beamMidPoint.Z - _global.Origin.Z, _global.StackSegList);

            double yDistanceFromCenter = Math.Sqrt(Math.Pow(rad, 2) - Math.Pow(xDistFromCenter, 2));

            ContourPoint beamPoint1 = _tModel.ShiftHorizontallyRad(beamMidPoint, yDistanceFromCenter, 2);
            ContourPoint beamPoint2 = _tModel.ShiftHorizontallyRad(beamMidPoint, yDistanceFromCenter, 4);

            _global.ProfileStr = "C100*100*10";
            _global.ClassStr = "3";
            _global.Position.Plane = Position.PlaneEnum.MIDDLE;
            _global.Position.Rotation = Position.RotationEnum.BELOW;
            _global.Position.RotationOffset = _slopeAngle * 180/ Math.PI;
            _global.Position.Depth = Position.DepthEnum.BEHIND;

            if (beamMidPoint.Z < floorPlateOrigin.Z)
            {
                _tModel.CreateBeam(beamPoint2, beamPoint1, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            }
            else
            {
                _tModel.CreateBeam(beamPoint1, beamPoint2, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            }
            

        }

        public void CreateBrokenBeam(double yDistanceFromCenter, ContourPoint floorPlatePoint1, ContourPoint floorPlatePoint3)
        {
            ContourPoint p1 = _tModel.ShiftHorizontallyRad(floorPlatePoint1, yDistanceFromCenter, 2);
            ContourPoint p2 = _tModel.ShiftHorizontallyRad(floorPlatePoint3, yDistanceFromCenter, 4);

            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]);
            double rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            double angle = Math.Asin(yDistanceFromCenter / rad);

            ContourPoint taperedUpPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2, 1, angle);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            angle = Math.Asin(yDistanceFromCenter / rad);

            ContourPoint taperedDownPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 1, angle);

            ContourPoint beamPt1 = _tModel.IntersectionOfLineXZ(p1, p2, taperedUpPoint, taperedDownPoint);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, _global.StackSegList[0][2]);
            rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            angle = Math.PI - Math.Asin(yDistanceFromCenter / rad);

            taperedUpPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2 * Math.Cos(angle), 1);
            taperedUpPoint = _tModel.ShiftHorizontallyRad(taperedUpPoint, yDistanceFromCenter, 2);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            angle = Math.PI - Math.Asin(yDistanceFromCenter / rad);

            taperedDownPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2 * Math.Cos(angle), 1);
            taperedDownPoint = _tModel.ShiftHorizontallyRad(taperedDownPoint, yDistanceFromCenter, 2);

            ContourPoint beamPt2 = _tModel.IntersectionOfLineXZ(p1, p2, taperedUpPoint, taperedDownPoint);

            _global.ProfileStr = "L100*100*10";
            _global.Position.RotationOffset = 0;

            ContourPoint beamSegmentStartPt = new ContourPoint();
            ContourPoint beamSegmentEndPt = new ContourPoint(p2, null);
            beamSegmentEndPt = _tModel.ShiftVertically(beamSegmentEndPt, -50 * Math.Sin(_slopeAngle));
            beamSegmentEndPt = _tModel.ShiftHorizontallyRad(beamSegmentEndPt, -50 * Math.Cos(_slopeAngle), 1, 0);

            double inclinedSegmentLength = _tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlatePoint3) / 5;

            for (int i = 0; i < 5; i++)
            {
                beamSegmentStartPt = _tModel.ShiftVertically(beamSegmentEndPt, 100 * Math.Sin(_slopeAngle));
                beamSegmentStartPt = _tModel.ShiftHorizontallyRad(beamSegmentStartPt, 100 * Math.Cos(_slopeAngle), 1, 0);

                beamSegmentEndPt = _tModel.ShiftVertically(beamSegmentEndPt, inclinedSegmentLength * Math.Sin(_slopeAngle));
                beamSegmentEndPt = _tModel.ShiftHorizontallyRad(beamSegmentEndPt, inclinedSegmentLength * Math.Cos(_slopeAngle), 1, 0);

                if (i == 0)
                {
                    beamSegmentStartPt = beamPt2;
                }

                if (i == 4)
                {
                    beamSegmentEndPt = beamPt1;
                }

                _tModel.CreateBeam(beamSegmentStartPt, beamSegmentEndPt, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            }

        }

    }
}
