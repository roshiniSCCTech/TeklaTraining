using System;
using System.Collections.Generic;
using Tekla.Structures.Datatype;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace HelperClasses
{
    public class GeometricalHelperClass
    {   
        // new point gets shifted along circumference at same elevation of given point
        // it gets shifted anti-clockwise if offset is positive, it gets shifted clockwise if offset is negative
        public static TSM.ContourPoint shiftAlongCircumferenceRad(TSM.ContourPoint point, double offset, short option) // 1. offset = angle in radians / arcLen / chordLen, 2. option = 1(angle), 2(arcLen), 3(chordLen)
        {
            TSM.ContourPoint shiftedPt = null;
            double ptAngle = Math.Atan((point.Y - Global.originY) / (point.X - Global.originX)); //angle of point from X - axis
            double rad = Math.Sqrt(Math.Pow((point.Y - Global.originY), 2) + Math.Pow((point.X - Global.originX), 2));
            switch (option)
            {
                case 1:  // shift point by offset = angle (in radians)
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    rad * Math.Cos(ptAngle + offset),
                    rad * Math.Sin(ptAngle + offset),
                    point.Z), null);
                    break;

                case 2:  // shift point by offset = arc length
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    rad * Math.Cos(ptAngle + (offset / rad)),
                    rad * Math.Sin(ptAngle + (offset / rad)), 
                    point.Z), null);
                    break;

                case 3: // shift point by offset = chord length
                    double theta = Math.Asin(offset / (2 * rad)) * 2;
                    shiftedPt = new TSM.ContourPoint(new T3D.Point(
                    rad * Math.Cos(ptAngle + theta),
                    rad * Math.Sin(ptAngle + theta),
                    point.Z), null);
                    break;
                default:
                    break;
            }
            return shiftedPt;
        }


        // new point gets shifted along the 4 axis. 
        // when angle is not given as parametrer, the angle formed by point (first parameter) at the origin from x - axis is taken as angle.
        // when angle is given, the 4 axis gets rotated by that angle.
        public static TSM.ContourPoint shiftHorizontallyRad(TSM.ContourPoint point, double dist, int side, double angle = double.NaN)
        {
            TSM.ContourPoint shiftedPt = null;
            if (double.IsNaN(angle))
            {
                angle = Math.Atan((point.Y - Global.originY) / (point.X - Global.originX)); // angle of point from x-axis
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
                    break;
            }


            return shiftedPt;
        }

        // new point gets shifted along z-axis by dist
        // it gets shifted above if dist is positive, gets shifted below if dist is negative
        public static TSM.ContourPoint shiftVertically(TSM.ContourPoint point, double dist)
        {
            TSM.ContourPoint shiftedPt = null;
            shiftedPt = point;
            shiftedPt.Z = point.Z + dist;
            return shiftedPt;
        }
    }
}
