﻿using HelperLibrary;
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

        public FloorSteel(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _slope = 1.0;
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
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, (_global.StackSegList[0][2] / 2) - 15);

            double inclinedDistBetweenBeams = _tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlatePoint3)/5;

            double inclinedDistFromCenter = -_tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlateOrigin);

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
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, (_global.StackSegList[0][2]/2) - (15/ Math.Cos(_slopeAngle))); // plate thickness 30 / 2 = 15

            ContourPoint beamMidPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, xDistFromCenter, 3);
            beamMidPoint.Z = _tModel.PointSlopeForm(new[] { floorPlateOrigin.X, floorPlateOrigin.Z }, _slope, beamMidPoint.X);

            double rad = _tModel.GetRadiusAtElevation(beamMidPoint.Z - _global.Origin.Z, _global.StackSegList);

            double verticalDistanceFromCenter = Math.Sqrt(Math.Pow(rad, 2) - Math.Pow(xDistFromCenter, 2));

            ContourPoint beamPoint1 = _tModel.ShiftHorizontallyRad(beamMidPoint, verticalDistanceFromCenter, 2);
            ContourPoint beamPoint2 = _tModel.ShiftHorizontallyRad(beamMidPoint, verticalDistanceFromCenter, 4);

            _global.ProfileStr = "L100*100*10";
            _global.ClassStr = "3";
            _global.Position.Plane = Position.PlaneEnum.MIDDLE;
            _global.Position.Rotation = Position.RotationEnum.BELOW;
            _global.Position.RotationOffset = _slopeAngle * 180/ Math.PI;
            _global.Position.Depth = Position.DepthEnum.BEHIND;

            if (beamMidPoint.Z < floorPlateOrigin.Z)
            {
                _global.Position.Plane = Position.PlaneEnum.RIGHT;
                _global.Position.Rotation = Position.RotationEnum.TOP;
                _global.Position.RotationOffset = 90 - _slopeAngle * 180 / Math.PI;
            }

            _tModel.CreateBeam(beamPoint1, beamPoint2, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

        }

        public void CreateBrokenBeam(double yDistanceFromCenter, ContourPoint floorPlatePoint1, ContourPoint floorPlatePoint3)
        {
            ContourPoint p1 = _tModel.ShiftHorizontallyRad(floorPlatePoint1, yDistanceFromCenter, 2);
            ContourPoint p2 = _tModel.ShiftHorizontallyRad(floorPlatePoint3, yDistanceFromCenter, 4);

            _tModel.CreateBeam(p1, p2, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, (_global.StackSegList[0][2] / 2) - (15 / Math.Cos(_slopeAngle)));
            double rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);

            double angle = Math.Asin(yDistanceFromCenter/rad);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, (_global.StackSegList[0][2] / 2) + (15 / Math.Cos(_slopeAngle)));
            ContourPoint taperedUpPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0]/2, 1, angle);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            ContourPoint taperedDownPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1]/2, 1, angle);

            _tModel.CreateBeam(taperedUpPoint, taperedDownPoint, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

            angle = Math.Asin(yDistanceFromCenter / rad) + Math.PI;

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, _global.StackSegList[0][2]);
            taperedUpPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2, 1, angle);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            taperedDownPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 1, angle);

            _tModel.CreateBeam(taperedUpPoint, taperedDownPoint, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

        }

    }
}
