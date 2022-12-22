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
    class Handrail
    {
        Globals _global;
        TeklaModelling _tModel;

        const double _minHandrailModuleArcLength = 1725;
        const double _maxHandrailModuleArcLength = 3500;

        bool _extensionStartsAtMidPlatform = false;
        bool _extensionEndsAtMidPlatform = false;

        List<TSM.ContourPoint> _pointsList;
        public Handrail(Globals global, TeklaModelling tModel)
        {
            _global = global;
            _tModel = tModel;

            if (_global.PlatformStartAngle != _global.ExtensionStartAngle)
            {
                _extensionStartsAtMidPlatform = true;
            }

            if (_global.PlatformEndAngle != _global.ExtensionEndAngle)
            {
                _extensionEndsAtMidPlatform = true;
            }

            _pointsList = new List<TSM.ContourPoint>();

            CreateHandrail();
        }

        void CreateHandrail()
        {
            double startAngle;
            double endAngle;
            TSM.ContourPoint platformOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[2][4] + (_global.StackSegList[2][2]) / 2);

            // first half of platform

            startAngle = _global.PlatformStartAngle;
            endAngle = _global.ExtensionStartAngle;

            if (startAngle != endAngle)
            {
                CreateArcHandrailBetweenPoints(startAngle, endAngle, _global.PlatformLength);
            }

            // straight handrail at platformStartAngle

            TSM.ContourPoint straightHandrailStartPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3], 1, startAngle);
            TSM.ContourPoint straightHandrailEndPoint = _tModel.ShiftHorizontallyRad(straightHandrailStartPoint, _global.PlatformLength, 1);
            if(_global.PlatformStartAngle == _global.ExtensionStartAngle)
            {
                straightHandrailEndPoint = _tModel.ShiftHorizontallyRad(straightHandrailEndPoint, _global.ExtensionLength, 1);
            }

            CreateStraightHandrailModule(straightHandrailStartPoint, straightHandrailEndPoint, 1);

            // extension

            startAngle = _global.ExtensionStartAngle;
            endAngle = _global.ExtensionEndAngle;

            if (startAngle != endAngle)
            {
                CreateArcHandrailBetweenPoints(startAngle, endAngle, _global.PlatformLength + _global.ExtensionLength, true);
            }

            // second half of platform

            startAngle = _global.ExtensionEndAngle;
            endAngle = _global.PlatformEndAngle;

            if (startAngle != endAngle)
            {
                CreateArcHandrailBetweenPoints(startAngle, endAngle, _global.PlatformLength);
            }

            // straight handrail at platformEndAngle

            straightHandrailStartPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3], 1, endAngle);
            straightHandrailEndPoint = _tModel.ShiftHorizontallyRad(straightHandrailStartPoint, _global.PlatformLength, 1);
            if (_global.PlatformEndAngle == _global.ExtensionEndAngle)
            {
                straightHandrailEndPoint = _tModel.ShiftHorizontallyRad(straightHandrailEndPoint, _global.ExtensionLength, 1);
            }

            CreateStraightHandrailModule(straightHandrailStartPoint, straightHandrailEndPoint, -1);
        }

        void CreateArcHandrailBetweenPoints(double platformStartAngle, double platformEndAngle, double length, bool extension = false)
        {
            TSM.ContourPoint platformOrigin = _tModel.ShiftVertically(_global.Origin, _global.StackSegList[2][4] + (_global.StackSegList[2][2]) / 2);

            TSM.ContourPoint platformStartPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + length, 1, platformStartAngle);
            TSM.ContourPoint platformEndPoint = _tModel.ShiftHorizontallyRad(platformOrigin, _global.StackSegList[2][0] / 2 + _global.StackSegList[2][3] + length, 1, platformEndAngle);

            double remainingPlatformArcDistance = _tModel.ArcLengthBetweenPointsXY(platformStartPoint, platformEndPoint);

            if (extension)
            {
                if (_extensionStartsAtMidPlatform)
                {
                    remainingPlatformArcDistance += _global.ExtensionLength;
                    platformStartPoint = _tModel.ShiftAlongCircumferenceRad(platformStartPoint, -_global.ExtensionLength, 2);
                }

                if (_extensionEndsAtMidPlatform)
                {
                    remainingPlatformArcDistance += _global.ExtensionLength;
                    platformEndPoint = _tModel.ShiftAlongCircumferenceRad(platformEndPoint, _global.ExtensionLength, 2);
                }
            }

            TSM.ContourPoint handrailModuleStartPoint = platformStartPoint;
            TSM.ContourPoint handrailModuleEndPoint;
            double handrailModuleArcDistance;

            while (remainingPlatformArcDistance >= 0)
            {
                bool bentAtStart = false;
                bool bentAtEnd = false;

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

                if (extension)
                {
                    if (remainingPlatformArcDistance >= _tModel.ArcLengthBetweenPointsXY(platformStartPoint, platformEndPoint) - 1)
                    {
                        if (_extensionStartsAtMidPlatform)
                        {
                            bentAtStart = true;
                        }
                    }

                    if (remainingPlatformArcDistance == handrailModuleArcDistance)
                    {
                        if (_extensionEndsAtMidPlatform)
                        {
                            bentAtEnd = true;
                        }
                    } 
                }

                handrailModuleEndPoint = _tModel.ShiftAlongCircumferenceRad(handrailModuleStartPoint, handrailModuleArcDistance, 2);
             
                CreateArcHandrailModule2(handrailModuleStartPoint, handrailModuleEndPoint, bentAtStart, bentAtEnd);

                handrailModuleStartPoint = _tModel.ShiftAlongCircumferenceRad(handrailModuleEndPoint, 25, 2);
                remainingPlatformArcDistance -= handrailModuleArcDistance + 25;
            }

        }

        void CreateArcHandrailModule(TSM.ContourPoint startPoint, TSM.ContourPoint endPoint, bool bentAtStart = false, bool bentAtEnd = false)
        {
            startPoint = _tModel.ShiftVertically(startPoint, (1000 - 21.2));
            endPoint = _tModel.ShiftVertically(endPoint, (1000 - 21.2));

            startPoint = _tModel.ShiftHorizontallyRad(startPoint, 21.2, 3);
            endPoint = _tModel.ShiftHorizontallyRad(endPoint, 21.2, 3);

            startPoint = _tModel.ShiftAlongCircumferenceRad(startPoint, 250, 2);
            endPoint = _tModel.ShiftAlongCircumferenceRad(endPoint, -250, 2);

            double midArc = _tModel.ArcLengthBetweenPointsXY(startPoint, endPoint) / 2;

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

            CreateArcBentPipe(startPoint, -(250 - 21.2), -(500 - 21.2));
            CreateArcBentPipe(endPoint, (250 - 21.2), -(500 - 21.2));

        }

        void CreateArcHandrailModule2(TSM.ContourPoint startPoint, TSM.ContourPoint endPoint, bool bentAtStart = false, bool bentAtEnd = false)
        {
            startPoint = _tModel.ShiftVertically(startPoint, (1000 - 21.2));
            endPoint = _tModel.ShiftVertically(endPoint, (1000 - 21.2));

            startPoint = _tModel.ShiftHorizontallyRad(startPoint, 21.2, 3);
            endPoint = _tModel.ShiftHorizontallyRad(endPoint, 21.2, 3);

            startPoint = _tModel.ShiftAlongCircumferenceRad(startPoint, 250, 2);
            endPoint = _tModel.ShiftAlongCircumferenceRad(endPoint, -250, 2);

            double midArc = _tModel.ArcLengthBetweenPointsXY(startPoint, endPoint) / 2;
            TSM.ContourPoint midPoint = _tModel.ShiftAlongCircumferenceRad(startPoint, midArc, 2);

            TSM.ContourPoint bendStartPoint = new TSM.ContourPoint();
            TSM.ContourPoint bendEndPoint = new TSM.ContourPoint();

            if (bentAtStart)
            {
                bendStartPoint = new ContourPoint(_tModel.ShiftAlongCircumferenceRad(startPoint, _global.ExtensionLength - 250 + 21.2, 2), null);
                startPoint = _tModel.ShiftHorizontallyRad(bendStartPoint, _global.ExtensionLength - 250, 3);
            }

            if (bentAtEnd)
            {
                bendEndPoint = new ContourPoint(_tModel.ShiftAlongCircumferenceRad(endPoint, -_global.ExtensionLength + 250 - 21.2, 2), null);
                endPoint = _tModel.ShiftHorizontallyRad(bendEndPoint, _global.ExtensionLength - 250, 3);
            }

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
                if (bentAtStart)
                {
                    _pointsList.Add(bendStartPoint);
                }
                _pointsList.Add(horizontalMidPoint);
                if (bentAtEnd)
                {
                    _pointsList.Add(bendEndPoint);
                }
                _pointsList.Add(horizontalEndPoint);

                _tModel.CreatePolyBeam(_pointsList, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

                _pointsList.Clear();

                horizontalStartPoint = _tModel.ShiftVertically(horizontalStartPoint, -(500 - 21.2));
                if (bentAtStart)
                {
                    bendStartPoint = _tModel.ShiftVertically(bendStartPoint, -(500 - 21.2));
                }
                horizontalMidPoint = _tModel.ShiftVertically(horizontalMidPoint, -(500 - 21.2));
                if (bentAtEnd)
                {
                    bendEndPoint = _tModel.ShiftVertically(bendEndPoint, -(500 - 21.2));
                }
                horizontalEndPoint = _tModel.ShiftVertically(horizontalEndPoint, -(500 - 21.2));
            }

            // bent pipes

            if (bentAtStart)
            {
                CreateStraightBentPipe(startPoint, -(250 - 21.2), -(500 - 21.2));
            }
            else
            {
                CreateArcBentPipe(startPoint, -(250 - 21.2), -(500 - 21.2));
            }

            if (bentAtEnd)
            {
                CreateStraightBentPipe(endPoint, -(250 - 21.2), -(500 - 21.2));
            }
            else
            {
                CreateArcBentPipe(endPoint, 250 - 21.2, -(500 - 21.2));
            }
        }

        void CreateArcBentPipe(TSM.ContourPoint point, double horizontalDistance, double verticalDistance)
        {
            TSM.ContourPoint point1 = new TSM.ContourPoint(point, null);
            TSM.ContourPoint point2 = new TSM.ContourPoint(_tModel.ShiftAlongCircumferenceRad(point1, horizontalDistance, 2), new TSM.Chamfer(100, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
            TSM.ContourPoint point3 = new TSM.ContourPoint(_tModel.ShiftVertically(point2, verticalDistance), new TSM.Chamfer(100, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)); ;
            TSM.ContourPoint point4 = _tModel.ShiftVertically(point1, verticalDistance);

            _pointsList.Add(point1);
            _pointsList.Add(point2);
            _pointsList.Add(point3);
            _pointsList.Add(point4);

            _tModel.CreatePolyBeam(_pointsList, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

            _pointsList.Clear();
        }

        // handrailShiftSide shifts the rod by the radius of the pipe either clockwise (1) or anti-clockwise(-1). It can either be 1 or -1. 
        void CreateStraightHandrailModule(TSM.ContourPoint startPoint, TSM.ContourPoint endPoint, int handrailShiftSide)
        {
            startPoint = _tModel.ShiftVertically(startPoint, (1000 - 21.2));
            endPoint = _tModel.ShiftVertically(endPoint, (1000 - 21.2));

            startPoint = _tModel.ShiftHorizontallyRad(startPoint, 250, 1);
            endPoint = _tModel.ShiftHorizontallyRad(endPoint, 42.2 + 250, 3);

            startPoint = _tModel.ShiftAlongCircumferenceRad(startPoint, handrailShiftSide * 21.2, 2);
            endPoint = _tModel.ShiftAlongCircumferenceRad(endPoint, handrailShiftSide * 21.2, 2);

            _pointsList.Add(startPoint);
            _pointsList.Add(endPoint);

            if (_tModel.DistanceBetweenPoints(startPoint, endPoint) > 600)
            {
                TSM.ContourPoint midPoint = _tModel.MidPoint(startPoint, endPoint);
                _pointsList.Add(midPoint);
            }
            
            _global.ProfileStr = "32NB(M) PIPE";
            _global.ClassStr = "2";
            _global.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _global.Position.Depth = TSM.Position.DepthEnum.MIDDLE;

            // vertical rods

            foreach (TSM.ContourPoint point in _pointsList)
            {
                _tModel.CreateBeam(point, _tModel.ShiftVertically(point, -(1000 - 21.2)), _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);
            }

            _pointsList.Clear();

            // horizontal rods

            TSM.ContourPoint horizontalStartPoint = new TSM.ContourPoint(startPoint, null);
            TSM.ContourPoint horizontalEndPoint = new TSM.ContourPoint(endPoint, null);

            for (int rod = 0; rod < 2; rod++)
            {

                _tModel.CreateBeam(horizontalStartPoint, horizontalEndPoint, _global.ProfileStr, Globals.MaterialStr, _global.ClassStr, _global.Position);

                _pointsList.Clear();

                horizontalStartPoint = _tModel.ShiftVertically(horizontalStartPoint, -(500 - 21.2));
                horizontalEndPoint = _tModel.ShiftVertically(horizontalEndPoint, -(500 - 21.2));
            }

            // bent pipes

            CreateStraightBentPipe(startPoint, -(250 - 21.2), -(500 - 21.2));
            CreateStraightBentPipe(endPoint, (250 - 21.2), -(500 - 21.2));
        }

        void CreateStraightBentPipe(TSM.ContourPoint point, double horizontalDistance, double verticalDistance)
        {
            TSM.ContourPoint point1 = new TSM.ContourPoint(point, null);
            TSM.ContourPoint point2 = new TSM.ContourPoint(_tModel.ShiftHorizontallyRad(point1, horizontalDistance, 1), new TSM.Chamfer(100, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
            TSM.ContourPoint point3 = new TSM.ContourPoint(_tModel.ShiftVertically(point2, verticalDistance), new TSM.Chamfer(100, 0, TSM.Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)); ;
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