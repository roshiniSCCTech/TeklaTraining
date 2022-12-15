using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        double _numOfStiffenerBeams;

        List<TSM.ContourPoint> _pointsList;

        List<List<TSM.ContourPoint>> _continuousBeams;

        public FloorSteel(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _slope = _global.FloorSteelSlope;
            _slopeAngle = Math.Atan(_slope);
            _numOfStiffenerBeams = 4;

            _pointsList = new List<TSM.ContourPoint>();

            CreateFloorPlate();
        }

        public void CreateFloorPlate()
        {
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]);


            TSM.ContourPoint taperedSegmentPointUp1 = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2, 1);
            TSM.ContourPoint taperedSegmentPointUp2 = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2, 3);
            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            TSM.ContourPoint taperedSegmentPointDown1 = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            TSM.ContourPoint taperedSegmentPointDown2 = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 3);


            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, _global.StackSegList[0][2] / 2);

            TSM.ContourPoint platePoint1 = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 1);
            platePoint1 = _tModel.ShiftVertically(platePoint1, _global.StackSegList[0][1] / 2 * Math.Tan(_slopeAngle));

            TSM.ContourPoint platePoint2 = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 3);
            platePoint2 = _tModel.ShiftVertically(platePoint2, -_global.StackSegList[0][1] / 2 * Math.Tan(_slopeAngle));

            TSM.ContourPoint floorPlatePoint1 = new TSM.ContourPoint(_tModel.IntersectionOfLineXZ(platePoint1, platePoint2, taperedSegmentPointUp1, taperedSegmentPointDown1), null);
            TSM.ContourPoint floorPlatePoint2 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(floorPlateOrigin, _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList), 1, Math.PI / 2), null);
            TSM.ContourPoint floorPlatePoint3 = new TSM.ContourPoint(_tModel.IntersectionOfLineXZ(platePoint1, platePoint2, taperedSegmentPointUp2, taperedSegmentPointDown2), null);
            TSM.ContourPoint floorPlatePoint4 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(floorPlateOrigin, _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList), 1, 3 * Math.PI / 2), null);

            // the center of the floor plate doesn't coincide with the origin as the tapered segment cuts the plate more on the top (smaller radius) and less on the bottom (larger radius),
            // shifting the center to the left of the origin for a positive slope

            TSM.ContourPoint floorPlateCenter = _tModel.MidPoint(floorPlatePoint1, floorPlatePoint3);
            TSM.ContourPoint floorPlateUpPoint = _tModel.ShiftHorizontallyRad(floorPlateCenter, _global.FloorSteelThickness * Math.Sin(_slopeAngle), 3, 0);
            floorPlateUpPoint = _tModel.ShiftVertically(floorPlateUpPoint, _global.FloorSteelThickness * Math.Cos(_slopeAngle));
            TSM.ContourPoint floorPlateDownPoint = floorPlateCenter;

            double majorAxis = _tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlatePoint3);
            double minorAxis = _tModel.DistanceBetweenPoints(floorPlatePoint2, floorPlatePoint4);

            _global.ProfileStr = "ELD" + minorAxis + "*" + majorAxis + "*" + minorAxis + "*" + majorAxis;
            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.FRONT;

            TSM.Beam floorPlate = _tModel.CreateBeam(floorPlateUpPoint, floorPlateDownPoint, _global.ProfileStr, Globals.MaterialStr, "12", _global.Position, "FloorPlate");
         
            CreateFloorSteelBeams(floorPlatePoint1, floorPlatePoint2, floorPlatePoint3, floorPlatePoint4);

        }

        public void CreateFloorSteelBeams(TSM.ContourPoint floorPlatePoint1, TSM.ContourPoint floorPlatePoint2, TSM.ContourPoint floorPlatePoint3, TSM.ContourPoint floorPlatePoint4)
        {
            // continous beams
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2] / 2);

            double inclinedDistBetweenBeams = _tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlatePoint3) / (_numOfStiffenerBeams + 1);

            double inclinedDistFromCenter = -_tModel.DistanceBetweenPoints(floorPlatePoint3, floorPlateOrigin);

            for ( int i = 0; i < _numOfStiffenerBeams; i++ )
            {
                inclinedDistFromCenter += inclinedDistBetweenBeams;
                double xDistanceFromCenter = inclinedDistFromCenter * Math.Cos(_slopeAngle);
                CreateContinuousBeam(xDistanceFromCenter);
            }

            // broken beams

            double distBetweenBeams = _tModel.DistanceBetweenPoints(floorPlatePoint2, floorPlatePoint4) / (_numOfStiffenerBeams + 1);

            double yDistFromCenter = -_tModel.DistanceBetweenPoints(floorPlatePoint4, floorPlateOrigin);

            for (int i = 0; i < _numOfStiffenerBeams; i++)
            {
                yDistFromCenter += distBetweenBeams;
                CreateBrokenBeam(yDistFromCenter, floorPlatePoint1, floorPlatePoint3);
            }

        }
        public void CreateContinuousBeam(double xDistFromCenter)
        {
            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]/2);

            TSM.ContourPoint beamMidPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, xDistFromCenter, 1);
            beamMidPoint = _tModel.ShiftVertically(beamMidPoint, xDistFromCenter * Math.Tan(_slopeAngle));

            double rad = _tModel.GetRadiusAtElevation(beamMidPoint.Z - _global.Origin.Z, _global.StackSegList);

            double yDistanceFromCenter = Math.Sqrt(Math.Pow(rad, 2) - Math.Pow(xDistFromCenter, 2));

            TSM.ContourPoint beamPoint1 = _tModel.ShiftHorizontallyRad(beamMidPoint, yDistanceFromCenter, 2);
            TSM.ContourPoint beamPoint2 = _tModel.ShiftHorizontallyRad(beamMidPoint, yDistanceFromCenter, 4);

            _global.ProfileStr = "C100*100*10";
            _global.ClassStr = "3";
            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.BELOW;
            _global.Position.RotationOffset = _slopeAngle * 180/ Math.PI;
            _global.Position.Depth = TSM.Position.DepthEnum.BEHIND;

            if (beamMidPoint.Z < floorPlateOrigin.Z)
            {
                _tModel.CreateBeam(beamPoint2, beamPoint1, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            }
            else
            {
                _tModel.CreateBeam(beamPoint1, beamPoint2, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            }
            

        }

        public void CreateBrokenBeam(double yDistanceFromCenter, TSM.ContourPoint floorPlatePoint1, TSM.ContourPoint floorPlatePoint3)
        {
            TSM.ContourPoint p1 = _tModel.ShiftHorizontallyRad(floorPlatePoint1, yDistanceFromCenter, 2);
            TSM.ContourPoint p2 = _tModel.ShiftHorizontallyRad(floorPlatePoint3, yDistanceFromCenter, 4);

            TSM.ContourPoint floorPlateOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[0][2]);
            double rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            double angle = Math.PI - Math.Asin(yDistanceFromCenter / rad); // to get mirror image of tapered points along y - axis

            TSM.ContourPoint taperedUpPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2 * Math.Cos(angle), 1);
            taperedUpPoint = _tModel.ShiftHorizontallyRad(taperedUpPoint, yDistanceFromCenter, 2);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            angle = Math.PI - Math.Asin(yDistanceFromCenter / rad); // to get mirror image of tapered points along y-axis

            TSM.ContourPoint taperedDownPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2 * Math.Cos(angle), 1);
            taperedDownPoint = _tModel.ShiftHorizontallyRad(taperedDownPoint, yDistanceFromCenter, 2);

            TSM.ContourPoint beamPt1 = _tModel.IntersectionOfLineXZ(p1, p2, taperedUpPoint, taperedDownPoint);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, _global.StackSegList[0][2]);
            rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            angle = Math.Asin(yDistanceFromCenter / rad);

            taperedUpPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][0] / 2, 1, angle);

            floorPlateOrigin = _tModel.ShiftVertically(floorPlateOrigin, -_global.StackSegList[0][2]);
            rad = _tModel.GetRadiusAtElevation(floorPlateOrigin.Z - _global.Origin.Z, _global.StackSegList);
            angle = Math.Asin(yDistanceFromCenter / rad);

            taperedDownPoint = _tModel.ShiftHorizontallyRad(floorPlateOrigin, _global.StackSegList[0][1] / 2, 1, angle);

            TSM.ContourPoint beamPt2 = _tModel.IntersectionOfLineXZ(p1, p2, taperedUpPoint, taperedDownPoint);

            _global.ProfileStr = "L100*100*10";
            _global.Position.RotationOffset = 0;

            TSM.ContourPoint beamSegmentStartPt = new TSM.ContourPoint();
            TSM.ContourPoint beamSegmentEndPt = new TSM.ContourPoint(p2, null);
            beamSegmentEndPt = _tModel.ShiftVertically(beamSegmentEndPt, -50 * Math.Sin(_slopeAngle)); // 50 = 100/2 where 100 is width of C profile continuous beam
            beamSegmentEndPt = _tModel.ShiftHorizontallyRad(beamSegmentEndPt, -50 * Math.Cos(_slopeAngle), 1, 0); // 50 = 100/2 where 100 is width of C profile continuous beam

            double inclinedSegmentLength = _tModel.DistanceBetweenPoints(floorPlatePoint1, floorPlatePoint3) / (_numOfStiffenerBeams + 1);

            for (int i = 0; i <= _numOfStiffenerBeams; i++)
            {
                beamSegmentStartPt = _tModel.ShiftVertically(beamSegmentEndPt, 100 * Math.Sin(_slopeAngle)); // 100 is width of C profile continuous beam
                beamSegmentStartPt = _tModel.ShiftHorizontallyRad(beamSegmentStartPt, 100 * Math.Cos(_slopeAngle), 1, 0); // 100 is width of C profile continuous beam

                beamSegmentEndPt = _tModel.ShiftVertically(beamSegmentEndPt, inclinedSegmentLength * Math.Sin(_slopeAngle));
                beamSegmentEndPt = _tModel.ShiftHorizontallyRad(beamSegmentEndPt, inclinedSegmentLength * Math.Cos(_slopeAngle), 1, 0);

                if (i == 0)
                {
                    beamSegmentStartPt = beamPt1;
                }

                if (i == _numOfStiffenerBeams)
                {
                    beamSegmentEndPt = beamPt2;
                }

                if (beamSegmentStartPt.Z < beamSegmentEndPt.Z)
                {
                    _tModel.CreateBeam(beamSegmentStartPt, beamSegmentEndPt, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
                }
            }

        }

    }
}
