using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using HelperLibrary;
using static Tekla.Structures.Filtering.Categories.TaskFilterExpressions;
using Tekla.Structures.Model;
using System.Transactions;

namespace ExtendedPlatformWithHandrail
{
    public class CreateExtendedPlatformWithHandrail : TeklaModelling
    {
        readonly ContourPoint _origin;

        string _profileStr;
        const string _materialStr = "IS2062";
        string _classStr;
        string _nameStr;
        TSM.Position _position;

        // 0 - top diameter, 1 - bottom diameter, 2 - height, 3 - thickness, 4 - height from base stack to bottom of segment
        readonly List<List<Double>> _stackSegList;
        readonly int _numberOfSegments;

        readonly double _platformStartAngle;
        readonly double _platformEndAngle;
        readonly double _platformLength;

        readonly double _extensionStartAngle;
        readonly double _extensionEndAngle;
        readonly double _extensionLength;


        const double minHandrailModuleArcLength = 1725;
        const double maxHandrailModuleArcLength = 3500;

        List<ContourPoint> pointsList;

        double segRadius;
        double segRadiusWithThickness;

        public CreateExtendedPlatformWithHandrail(
            double originX,
            double originY,
            double originZ,
            List<List<Double>> stackSegList,
            double platformStartAngle,
            double platformEndAngle,
            double platformLength,
            double extensionStartAngle,
            double extensionEndAngle,
            double extensionLength) : base(originX, originY, originZ)
        {
            _origin = new ContourPoint(new T3D.Point(originX, originY, originZ), null);
            _profileStr = "";
            _classStr = "";
            _nameStr = "";
            _position = new TSM.Position();
            this._stackSegList = stackSegList;
            _numberOfSegments = 3;
            _platformStartAngle = platformStartAngle * Math.PI / 180;
            _platformEndAngle = platformEndAngle * Math.PI / 180;
            _platformLength = platformLength;
            _extensionStartAngle = extensionStartAngle * Math.PI / 180;
            _extensionEndAngle = extensionEndAngle * Math.PI / 180;
            _extensionLength = extensionLength;

            pointsList = new List<ContourPoint>();
        }

        public void Build()
        {
            CalculateElevation();
            CreateStack();
            CreatePlatform();
            CreateExtendedPlatform();
            CreateHandrail();
        }

        // calculates height till bottom of the ith segment from base of the stack and adds it at 4th index in the List<double> of ith segment
        void CalculateElevation()
        {
            double elevation = 0;

            foreach(List<double> segment in _stackSegList)
            {
                segment.Add(elevation);
                elevation += segment[2];
            }
        }
        void CreateStack()
        {
            int counter = 0;

            _position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _position.Rotation = TSM.Position.RotationEnum.FRONT;
            _classStr = "1";

            for (counter = 0; counter < 3; counter++)
            {
                T3D.Point startPoint = new T3D.Point(_origin.X, _origin.Y, _origin.Z + _stackSegList[counter][4]);
                T3D.Point endPoint = ShiftVertically(new TSM.ContourPoint(startPoint, null), _stackSegList[counter][2]);
                _nameStr = "segment" + (counter + 1);
                _profileStr = "CHS" + _stackSegList[counter][1] + "*" + _stackSegList[counter][0] + "*" + _stackSegList[counter][3];
                CreateBeam(startPoint, endPoint, _profileStr, _materialStr, _classStr, _position, "myBeam");
            }
        }

        void CreatePlatform()
        {
            ContourPoint platformOrigin = ShiftVertically(_origin, _stackSegList[2][4] + (_stackSegList[2][2])/2);
            double midAngle = (_platformEndAngle - _platformStartAngle) / 2;
            ContourPoint startPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0]/2 + _stackSegList[2][3], 1, _platformStartAngle);
            ContourPoint midPoint = new ContourPoint(ShiftAlongCircumferenceRad(startPoint, midAngle, 1), new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            ContourPoint endPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3], 1, _platformEndAngle);

            pointsList.Add(startPoint);
            pointsList.Add(midPoint);
            pointsList.Add(endPoint);

            _profileStr = "PL" + _platformLength + "*25";
            _classStr = "10";
            _position.Plane = Position.PlaneEnum.RIGHT;
            _position.Rotation = Position.RotationEnum.FRONT;
            _position.Depth = Position.DepthEnum.BEHIND; 

            CreatePolyBeam(pointsList, _profileStr, _materialStr, _classStr, _position, "Platform");

            pointsList.Clear();
        }
        void CreateExtendedPlatform()
        {
            ContourPoint platformOrigin = ShiftVertically(_origin, _stackSegList[2][4] + (_stackSegList[2][2]) / 2);
            double midAngle = (_extensionEndAngle - _extensionStartAngle) / 2;
            ContourPoint startPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength, 1, _extensionStartAngle);
            ContourPoint midPoint = new ContourPoint(ShiftAlongCircumferenceRad(startPoint, midAngle, 1), new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            ContourPoint endPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength, 1, _extensionEndAngle);

            List<ContourPoint> platformPointList = new List<ContourPoint>
            {
                startPoint,
                midPoint,
                endPoint
            };

            _profileStr = "PL" + _extensionLength + "*25";
            _classStr = "10";
            _position.Plane = Position.PlaneEnum.RIGHT;
            _position.Rotation = Position.RotationEnum.FRONT;
            _position.Depth = Position.DepthEnum.BEHIND;

            CreatePolyBeam(platformPointList, _profileStr, _materialStr, _classStr, _position, "Platform");
        }

        void CreateHandrail()
        {
            double remainingPlatformAngle = _platformEndAngle - _platformStartAngle;

            ContourPoint platformOrigin = ShiftVertically(_origin, _stackSegList[2][4] + (_stackSegList[2][2]) / 2);
            ContourPoint startPoint;
            ContourPoint endPoint;

            // first half of platform

            startPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength, 1, _platformStartAngle);
            endPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength, 1, _extensionStartAngle);

            if(startPoint != endPoint)
            {
                CreateHandrailBetweenPoints(startPoint, endPoint);
            }
            
            // extension

            startPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength + _extensionLength, 1, _extensionStartAngle);
            endPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength + _extensionLength, 1, _extensionEndAngle);

            CreateHandrailBetweenPoints(startPoint, endPoint);

            // second half of platform

            startPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength, 1, _extensionEndAngle);
            endPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength, 1, _platformEndAngle);

            if (startPoint != endPoint)
            {
                CreateHandrailBetweenPoints(startPoint, endPoint);
            }
        }
        void CreateHandrailBetweenPoints(ContourPoint platformStartPoint, ContourPoint platformEndPoint)
        {
            double remainingPlatformArcDistance =  ArcLengthBetweenPoints(platformStartPoint, platformEndPoint);

            ContourPoint handrailModuleStartPoint = platformStartPoint;
            ContourPoint handrailModuleEndPoint;
            double handrailModuleArcDistance;

            while ( remainingPlatformArcDistance >= 0)
            {
                
                if(remainingPlatformArcDistance <= maxHandrailModuleArcLength)
                {
                    handrailModuleArcDistance = remainingPlatformArcDistance;
                }
                else
                {
                    if (remainingPlatformArcDistance - maxHandrailModuleArcLength >= minHandrailModuleArcLength)
                    {
                        handrailModuleArcDistance = maxHandrailModuleArcLength;
                    }
                    else
                    {
                        handrailModuleArcDistance = remainingPlatformArcDistance / 2 - 12.5;
                    }
                }

                handrailModuleEndPoint = ShiftAlongCircumferenceRad(handrailModuleStartPoint, handrailModuleArcDistance, 2);

                CreateHandrailModule(handrailModuleStartPoint, handrailModuleEndPoint);

                handrailModuleStartPoint = ShiftAlongCircumferenceRad(handrailModuleEndPoint, 25, 2);
                remainingPlatformArcDistance -= handrailModuleArcDistance + 25;
            }

        }
        void CreateHandrailModule(ContourPoint startPoint, ContourPoint endPoint)
        {
            startPoint = ShiftVertically(startPoint, (1000 - 21.2));
            endPoint = ShiftVertically(endPoint, (1000 - 21.2));

            startPoint = ShiftHorizontallyRad(startPoint, 21.2, 3);
            endPoint = ShiftHorizontallyRad(endPoint, 21.2, 3);

            startPoint = ShiftAlongCircumferenceRad(startPoint, 250, 2);
            endPoint = ShiftAlongCircumferenceRad(endPoint, -250, 2);

            double midArc = ArcLengthBetweenPoints(startPoint, endPoint) / 2;

            ContourPoint midPoint = ShiftAlongCircumferenceRad(startPoint, midArc, 2);

            _profileStr = "32NB(M) PIPE";
            _classStr = "2";
            _position.Plane = Position.PlaneEnum.MIDDLE;
            _position.Depth = Position.DepthEnum.MIDDLE;

            // vertical rods

            CreateBeam(startPoint, ShiftVertically(startPoint, -(1000 - 21.2)), _profileStr, _materialStr, _classStr, _position);
            CreateBeam(midPoint, ShiftVertically(midPoint, -(1000 - 21.2)), _profileStr, _materialStr, _classStr, _position);
            CreateBeam(endPoint, ShiftVertically(endPoint, -(1000 - 21.2)), _profileStr, _materialStr, _classStr, _position);

            // horizontal rods

            ContourPoint horizontalStartPoint = new ContourPoint(startPoint, null);
            ContourPoint horizontalMidPoint = new ContourPoint(midPoint, new Chamfer(0, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
            ContourPoint horizontalEndPoint = new ContourPoint(endPoint, null);
            
            for (int rod = 0; rod < 2; rod++)
            {
                pointsList.Add(horizontalStartPoint);
                pointsList.Add(horizontalMidPoint);
                pointsList.Add(horizontalEndPoint);

                CreatePolyBeam(pointsList, _profileStr, _materialStr, _classStr, _position);

                pointsList.Clear();

                horizontalStartPoint = ShiftVertically(horizontalStartPoint, -(500 - 21.2));
                horizontalMidPoint = ShiftVertically(horizontalMidPoint, -(500 - 21.2));
                horizontalEndPoint = ShiftVertically(horizontalEndPoint, -(500 - 21.2));
            }

            // bent pipes

            CreateBentPipe(startPoint, -(250 - 21.2), -(500 - 21.2));
            CreateBentPipe(endPoint, (250 - 21.2), -(500 - 21.2));

        }
        void CreateBentPipe(ContourPoint point, double horizontalDistance, double verticalDistance)
        {
            ContourPoint point1 = new ContourPoint(point, null);
            ContourPoint point2 = new ContourPoint(ShiftAlongCircumferenceRad(point1, horizontalDistance, 2), new Chamfer(42.4, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
            ContourPoint point3 = new ContourPoint(ShiftVertically(point2, verticalDistance), new Chamfer(100, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)); ;
            ContourPoint point4 = ShiftVertically(point1, verticalDistance);

            pointsList.Add(point1);
            pointsList.Add(point2);
            pointsList.Add(point3);
            pointsList.Add(point4);

            CreatePolyBeam(pointsList, _profileStr, _materialStr, _classStr, _position);

            pointsList.Clear();
        }
    }
}
