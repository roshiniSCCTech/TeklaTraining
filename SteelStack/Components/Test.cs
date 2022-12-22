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
    class Test
    {
        Globals _global;
        TeklaModelling _tModel;


        List<TSM.ContourPoint> _pointsList;


        public Test(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _pointsList = new List<TSM.ContourPoint>();

            Build();
        }

        public void Build()
        {
            //TestIntersectionOfLineXY();

            // TestEllipseBeams();

            /*TSM.ContourPoint origin = new TSM.ContourPoint(_global.Origin, null);
            TSM.ContourPoint p1 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(origin, 5000, 1), new TSM.Chamfer(0, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint p2 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(origin, 2500, 1, Math.PI/2), null);
            TSM.ContourPoint p3 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(origin, 5000, 3), new TSM.Chamfer(0, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint p4 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(origin, 2500, 1, 3 * Math.PI/2), null);

            _pointsList.Add(p1);
            _pointsList.Add(p2);
            _pointsList.Add(p3);
            _pointsList.Add(p4);

            ContourPlate plate = _tModel.CreateContourPlate(_pointsList, "PL30", Globals.MaterialStr, "3", _global.Position, "Plate");*/
/*
            p1 = _tModel.ShiftVertically(p1, 200);
            p2 = _tModel.ShiftVertically(p1, -400);
            Beam cut = _tModel.CreateBeam(p1, p2, "SPD3000*3000*2000*2000*300", Globals.MaterialStr, BooleanPart.BooleanOperativeClassName, _global.Position, "cut");

            _tModel.cutPart(cut, plate);*/
        }

        public void TestIntersectionOfLineXY()
        {
            TSM.ContourPoint StartPoint1 = new TSM.ContourPoint(_global.Origin, null);
            TSM.ContourPoint EndPoint1 = _tModel.ShiftHorizontallyRad(StartPoint1, 4000, 1, 1);

            TSM.ContourPoint StartPoint2 = _tModel.ShiftHorizontallyRad(_global.Origin, 2500, 1);
            TSM.ContourPoint EndPoint2 = _tModel.ShiftHorizontallyRad(StartPoint2, 4000, 1, 2);

            TSM.ContourPoint StartPoint3 = _tModel.IntersectionOfLineXY(StartPoint1, EndPoint1, StartPoint2, EndPoint2);
            TSM.ContourPoint EndPoint3 = _tModel.ShiftVertically(StartPoint3, 1000);

            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _global.Position.Rotation = TSM.Position.RotationEnum.FRONT;

            _tModel.CreateBeam(StartPoint1, EndPoint1, "Rod50", Globals.MaterialStr, "3", _global.Position, "Beam1");
            _tModel.CreateBeam(StartPoint2, EndPoint2, "Rod50", Globals.MaterialStr, "3", _global.Position, "Beam2");
            _tModel.CreateBeam(StartPoint3, EndPoint3, "Rod50", Globals.MaterialStr, "3", _global.Position, "Beam3");
        }

        public void TestEllipseBeams()
        {
            TSM.ContourPoint origin = new TSM.ContourPoint(_global.Origin, null);
            /*TSM.ContourPoint p1 = _tModel.ShiftHorizontallyRad(origin, 5000, 1);
            TSM.ContourPoint p2 = _tModel.ShiftHorizontallyRad(origin, 2500, 2);
            TSM.ContourPoint p3 = _tModel.ShiftHorizontallyRad(origin, 5000, 3);
            TSM.ContourPoint p4 = _tModel.ShiftHorizontallyRad(origin, 2500, 4);

            _pointsList.Add(p1);
            _pointsList.Add(p2);
            _pointsList.Add(p3);
            _pointsList.Add(p4);


            ContourPlate plate = _tModel.CreateContourPlate(_pointsList, "PL30", Globals.MaterialStr, "3", _global.Position, "Plate");*/

            TSM.ContourPoint p1 = _tModel.ShiftVertically(origin, 15);
            TSM.ContourPoint p2 = _tModel.ShiftVertically(origin, -15);

            _global.ProfileStr = "ELD10000*5000*10000*5000";
            _global.ClassStr = "3";
            _global.Position.Plane = Position.PlaneEnum.MIDDLE;
            _global.Position.Rotation = Position.RotationEnum.BELOW;
            _global.Position.Depth = Position.DepthEnum.MIDDLE;

            _tModel.CreateBeam(p1, p2, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

            origin = _tModel.ShiftVertically(origin, -30);

            double angle = 0;

            while (angle < 360)
            {
                double radAngle = angle * Math.PI / 180;
                double minorAxis = 2500;
                double majorAxis = 5000;

                double x = _global.Origin.X + (majorAxis * Math.Cos(radAngle));
                double y = _global.Origin.Y + (minorAxis * Math.Sin(radAngle));

                ContourPoint pt = new ContourPoint(new T3D.Point(x, y, origin.Z), null);


                _global.ProfileStr = "L100*100*10";
                _global.ClassStr = "3";
                _global.Position.Plane = Position.PlaneEnum.MIDDLE;
                _global.Position.Rotation = Position.RotationEnum.BELOW;
                _global.Position.Depth = Position.DepthEnum.BEHIND;

                _tModel.CreateBeam(origin, pt, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

                angle += 30;
            }
        }

        public void TestShiftHorizontally()
        {

        }

    }
}
