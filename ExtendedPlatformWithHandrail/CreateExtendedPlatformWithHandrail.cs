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

        readonly List<List<Double>> _stackSegList;
        readonly int _numberOfSegments;

        readonly double _platformStartAngle;
        readonly double _platformEndAngle;
        readonly double _platformLength;

        readonly double _extensionStartAngle;
        readonly double _extensionEndAngle;
        readonly double _extensionLength;

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
        }

        public void Build()
        {
            CalculateElevation();
            CreateStack();
            CreatePlatform();
            CreateExtendedPlatform();
            CreateHandrail();
        }

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

            List<ContourPoint> platformPointList = new List<ContourPoint>
            {
                startPoint,
                midPoint,
                endPoint
            };

            _profileStr = "PL" + _platformLength + "*25";
            _classStr = "10";
            _position.Plane = Position.PlaneEnum.RIGHT;
            _position.Rotation = Position.RotationEnum.FRONT;
            _position.Depth = Position.DepthEnum.BEHIND; 

            CreatePolyBeam(platformPointList, _profileStr, _materialStr, _classStr, _position, "Platform");
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
            ContourPoint platformOrigin = ShiftVertically(_origin, _stackSegList[2][4] + (_stackSegList[2][2]) / 2);
            ContourPoint startPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength + _extensionLength, 1, _extensionStartAngle);
            ContourPoint endPoint = ShiftHorizontallyRad(platformOrigin, _stackSegList[2][0] / 2 + _stackSegList[2][3] + _platformLength + _extensionLength, 1, _extensionEndAngle);

            CreateHandrailBetweenPoints(startPoint, endPoint);
        }
        void CreateHandrailBetweenPoints(ContourPoint startPoint, ContourPoint endPoint)
        {
            double remainingPlatformArcDistance =  ArcLengthBetweenPoints(startPoint, endPoint);
            double minHandrailArcLength = 1700;
            double maxHandrailArcLength = 3500;

            ContourPoint startSingleHandrail = startPoint;
            ContourPoint endSingleHandrail;

            while( remainingPlatformArcDistance != 0)
            {
                double singleHandrailArcDistance;

                if(remainingPlatformArcDistance <= maxHandrailArcLength)
                {
                    singleHandrailArcDistance = remainingPlatformArcDistance;
                }
                else
                {
                    if (remainingPlatformArcDistance - maxHandrailArcLength >= minHandrailArcLength)
                    {
                        singleHandrailArcDistance = maxHandrailArcLength;
                    }
                    else
                    {
                        singleHandrailArcDistance = remainingPlatformArcDistance / 2;
                    }
                }

                endSingleHandrail = ShiftAlongCircumferenceRad(startSingleHandrail, singleHandrailArcDistance, 2);

                CreateBeam(startSingleHandrail, endSingleHandrail, "ROD10", _materialStr, "2", _position);

                startSingleHandrail = endSingleHandrail;
                remainingPlatformArcDistance -= singleHandrailArcDistance;
            }

        }

    }
}
