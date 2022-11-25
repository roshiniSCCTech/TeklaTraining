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
    class Handrail
    {
        Globals _global;
        TeklaModelling _tModel;

        const double _minHandrailModuleArcLength = 1725;
        const double _maxHandrailModuleArcLength = 3500;

        List<TSM.ContourPoint> _pointsList;
        public Handrail(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            _pointsList = new List<TSM.ContourPoint> ();

            CreateHandrail();
        }

        void CreateHandrail()
        {
            double remainingPlatformAngle = _global.PlatformEndAngle - _global.PlatformStartAngle;

            TSM.ContourPoint platformOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[2][4] + (_global.StackSegList[2][2]) / 2);
            TSM.ContourPoint startPoint;
            TSM.ContourPoint endPoint;

            // first half of platform

            startPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + _global.PlatformLength, 1, _global.PlatformStartAngle);
            endPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + _global.PlatformLength, 1, _global.ExtensionStartAngle);

            if (startPoint != endPoint)
            {
                CreateHandrailBetweenPoints(startPoint, endPoint);
            }

            // extension

            startPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + _global.PlatformLength + _global.ExtensionLength, 1, _global.ExtensionStartAngle);
            endPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + _global.PlatformLength + _global.ExtensionLength, 1, _global.ExtensionEndAngle);

            CreateHandrailBetweenPoints(startPoint, endPoint);

            // second half of platform

            startPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + _global.PlatformLength, 1, _global.ExtensionEndAngle);
            endPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + _global.PlatformLength, 1, _global.PlatformEndAngle);

            if (startPoint != endPoint)
            {
                CreateHandrailBetweenPoints(startPoint, endPoint);
            }
        }
        void CreateHandrailBetweenPoints(TSM.ContourPoint platformStartPoint, TSM.ContourPoint platformEndPoint)
        {
            double remainingPlatformArcDistance = _tModel.ArcLengthBetweenPoints(platformStartPoint, platformEndPoint);

            TSM.ContourPoint handrailModuleStartPoint = platformStartPoint;
            TSM.ContourPoint handrailModuleEndPoint;
            double handrailModuleArcDistance;

            while (remainingPlatformArcDistance >= 0)
            {

                if (remainingPlatformArcDistance <= _maxHandrailModuleArcLength)
                {
                    handrailModuleArcDistance = remainingPlatformArcDistance;
                }
                else
                {
                    if (remainingPlatformArcDistance - _maxHandrailModuleArcLength >= _minHandrailModuleArcLength)
                    {
                        handrailModuleArcDistance = _maxHandrailModuleArcLength;
                    }
                    else
                    {
                        handrailModuleArcDistance = remainingPlatformArcDistance / 2 - 12.5;
                    }
                }

                handrailModuleEndPoint = _tModel.ShiftAlongCircumferenceRad(handrailModuleStartPoint, handrailModuleArcDistance, 2);

                CreateHandrailModule(handrailModuleStartPoint, handrailModuleEndPoint);

                handrailModuleStartPoint = _tModel.ShiftAlongCircumferenceRad(handrailModuleEndPoint, 25, 2);
                remainingPlatformArcDistance -= handrailModuleArcDistance + 25;
            }

        }
        void CreateHandrailModule(TSM.ContourPoint startPoint, TSM.ContourPoint endPoint)
        {
            startPoint = _tModel.ShiftVertically(startPoint, (1000 - 21.2));
            endPoint = _tModel.ShiftVertically(endPoint, (1000 - 21.2));

            startPoint = _tModel.ShiftHorizontallyRad(startPoint, 21.2, 3);
            endPoint = _tModel.ShiftHorizontallyRad(endPoint, 21.2, 3);

            startPoint = _tModel.ShiftAlongCircumferenceRad(startPoint, 250, 2);
            endPoint = _tModel.ShiftAlongCircumferenceRad(endPoint, -250, 2);

            double midArc = _tModel.ArcLengthBetweenPoints(startPoint, endPoint) / 2;

            TSM.ContourPoint midPoint = _tModel.ShiftAlongCircumferenceRad(startPoint, midArc, 2);

            _global.ProfileStr = "32NB(M) PIPE";
            _global.ClassStr = "2";
            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;

            // vertical rods

            _tModel.CreateBeam(startPoint, _tModel.ShiftVertically(startPoint, -(1000 - 21.2)), _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            _tModel.CreateBeam(midPoint, _tModel.ShiftVertically(midPoint, -(1000 - 21.2)), _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            _tModel.CreateBeam(endPoint, _tModel.ShiftVertically(endPoint, -(1000 - 21.2)), _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

            // horizontal rods

            TSM.ContourPoint horizontalStartPoint = new TSM.ContourPoint(startPoint, null);
            TSM.ContourPoint horizontalMidPoint = new TSM.ContourPoint(midPoint, new TSM.Chamfer(0, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint horizontalEndPoint = new TSM.ContourPoint(endPoint, null);

            for (int rod = 0; rod < 2; rod++)
            {
                _pointsList.Add(horizontalStartPoint);
                _pointsList.Add(horizontalMidPoint);
                _pointsList.Add(horizontalEndPoint);

                _tModel.CreatePolyBeam(_pointsList, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

                _pointsList.Clear();

                horizontalStartPoint = _tModel.ShiftVertically(horizontalStartPoint, -(500 - 21.2));
                horizontalMidPoint = _tModel.ShiftVertically(horizontalMidPoint, -(500 - 21.2));
                horizontalEndPoint = _tModel.ShiftVertically(horizontalEndPoint, -(500 - 21.2));
            }

            // bent pipes

            CreateBentPipe(startPoint, -(250 - 21.2), -(500 - 21.2));
            CreateBentPipe(endPoint, (250 - 21.2), -(500 - 21.2));

        }
        void CreateBentPipe(TSM.ContourPoint point, double horizontalDistance, double verticalDistance)
        {
            TSM.ContourPoint point1 = new TSM.ContourPoint(point, null);
            TSM.ContourPoint point2 = new TSM.ContourPoint(_tModel.ShiftAlongCircumferenceRad(point1, horizontalDistance, 2), new TSM.Chamfer(100, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            TSM.ContourPoint point3 = new TSM.ContourPoint(_tModel.ShiftVertically(point2, verticalDistance), new TSM.Chamfer(100, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT)); ;
            TSM.ContourPoint point4 = _tModel.ShiftVertically(point1, verticalDistance);

            _pointsList.Add(point1);
            _pointsList.Add(point2);
            _pointsList.Add(point3);
            _pointsList.Add(point4);

            _tModel.CreatePolyBeam(_pointsList, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

            _pointsList.Clear();
        }
    }
}
