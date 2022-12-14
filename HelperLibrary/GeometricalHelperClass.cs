using System;
using System.Collections.Generic;
using Tekla.Structures.Datatype;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace HelperLibrary
{
    public class GeometricalHelperClass
    {
        readonly T3D.Point _origin;

        protected GeometricalHelperClass(double originX, double originY, double originZ)
        {
            _origin = new T3D.Point(originX, originY, originZ);
        }
        // new point gets shifted along circumference at same elevation of given point
        // it gets shifted anti-clockwise if offset is positive, it gets shifted clockwise if offset is negative
        public TSM.ContourPoint ShiftAlongCircumferenceRad(TSM.ContourPoint point, double offset, short option) // 1. offset = angle in radians / arcLen / chordLen, 2. option = 1(angle), 2(arcLen), 3(chordLen)
        {
            TSM.ContourPoint shiftedPt;
            double ptAngle = Math.Atan((point.Y - _origin.Y) / (point.X - _origin.X)); //angle of point from X - axis
            if (point.X < _origin.X)
            {
                ptAngle += Math.PI;
            }
            double rad = Math.Sqrt(Math.Pow((point.Y - _origin.Y), 2) + Math.Pow((point.X - _origin.X), 2));
            switch (option)
            {
                case 1:  // shift point by offset = angle (in radians)
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    _origin.X + (rad * Math.Cos(ptAngle + offset)),
                    _origin.Y + (rad * Math.Sin(ptAngle + offset)),
                    point.Z), null);
                    break;

                case 2:  // shift point by offset = arc length
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    _origin.X + (rad * Math.Cos(ptAngle + (offset / rad))),
                    _origin.Y + (rad * Math.Sin(ptAngle + (offset / rad))), 
                    point.Z), null);
                    break;

                case 3: // shift point by offset = chord length
                    double theta = Math.Asin(offset / (2 * rad)) * 2;
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    _origin.X + (rad * Math.Cos(ptAngle + theta)),
                    _origin.Y + (rad * Math.Sin(ptAngle + theta)),
                    point.Z), null);
                    break;
                default:
                    shiftedPt = point;
                    break;
            }
            return shiftedPt;
        }


        // new point gets shifted along the 4 axis. 
        // when angle is not given as parametrer, the angle formed by point (first parameter) at the origin from x - axis is taken as angle.
        // when angle is given, the 4 axis gets rotated by that angle.
        public TSM.ContourPoint ShiftHorizontallyRad(TSM.ContourPoint point, double dist, int side, double angle = double.NaN)
        {
            TSM.ContourPoint shiftedPt;
            if (double.IsNaN(angle))
            {
                angle = Math.Atan((point.Y - _origin.Y) / (point.X - _origin.X)); // angle of point from x-axis
                if (point.X < _origin.X)
                {
                    angle += Math.PI;
                }
            }

            switch(side)
            {
                case 1:
                    shiftedPt = new TSM.ContourPoint( new T3D.Point(
                    point.X + (dist * Math.Cos(angle)),
                    point.Y + (dist * Math.Sin(angle)),
                    point.Z), null);
                    break;
                case 2:
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    point.X - (dist * Math.Sin(angle)),
                    point.Y + (dist * Math.Cos(angle)),
                    point.Z), null);
                    break;
                case 3:
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    point.X - (dist * Math.Cos(angle)),
                    point.Y - (dist * Math.Sin(angle)),
                    point.Z), null);
                    break;
                case 4:
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    point.X + (dist * Math.Sin(angle)),
                    point.Y - (dist * Math.Cos(angle)),
                    point.Z), null);
                    break;
                default:
                    shiftedPt = point;
                    break;
            }


            return shiftedPt;
        }

        // new point gets shifted along z-axis by dist
        // it gets shifted above if dist is positive, gets shifted below if dist is negative
        public TSM.ContourPoint ShiftVertically(TSM.ContourPoint point, double dist)
        {
            TSM.ContourPoint shiftedPt = new TSM.ContourPoint(point, point.Chamfer);
            shiftedPt.Z += dist;
            return shiftedPt;
        }

        public double DistanceBetweenPoints( T3D.Point point1, T3D.Point point2)
        {
            double distance = Math.Sqrt(Math.Pow((point1.Y - point2.Y), 2) + Math.Pow((point1.X - point2.X), 2));
            return distance;
        }

        public double AngleBetweenPoints(T3D.Point point1, T3D.Point point2)
        {
            double rad = DistanceBetweenPoints(_origin, point1);
            double chordLength = DistanceBetweenPoints(point2, point1);

            double angle = Math.Asin(chordLength / (2 * rad)) * 2; ;

            return angle;
        }

        public double ArcLengthBetweenPoints(T3D.Point point1, T3D.Point point2)
        {
            double rad = DistanceBetweenPoints(_origin, point1);
            double angle = AngleBetweenPoints(point2, point1);

            double arcLength = rad * angle;
            return arcLength;
        }

        // returns index of segment at elevation FROM STACK BASE
        public int GetSegmentAtElevation(double elevation, List<List<double>> stackSegList)
        {
            int index = 0;

            for (int seg = 0; seg < stackSegList.Count; seg++)
            {
                if (elevation < stackSegList[seg][4] + stackSegList[seg][2])
                {
                    break;
                }
            }

            return index;
        }

        // returns inner radius of segment at elevation FROM STACK BASE
        public double GetRadiusAtElevation(double elevation, List<List<double>> stackSeglist)
        {
            int seg = GetSegmentAtElevation(elevation, stackSeglist);
            
            double height1 = stackSeglist[seg][2];
            double base1 = (stackSeglist[seg][1] - stackSeglist[seg][0]) / 2;
            double height2 = stackSeglist[seg][4] - elevation;
            double base2 = base1 * height2 / height1;

            double radius = (stackSeglist[seg][0] / 2) + base2;

            return radius;
        }
    }
}
