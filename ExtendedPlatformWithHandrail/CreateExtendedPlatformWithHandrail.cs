using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using HelperLibrary;
using static Tekla.Structures.Filtering.Categories.TaskFilterExpressions;

namespace ExtendedPlatformWithHandrail
{
    public class CreateExtendedPlatformWithHandrail : TeklaModelling
    {
        readonly double _originX;
        readonly double _originY;
        readonly double _originZ;

        string _profileStr;
        const string _materialStr = "IS2062";
        string _classStr;
        string _nameStr;
        TSM.Position _position;

        readonly List<List<Double>> _stackSegList;
        readonly int _numberOfSegments;

        public CreateExtendedPlatformWithHandrail(double originX, double originY, double originZ, List<List<Double>> stackSegList) : base(originX, originY, originZ)
        {
            _originX = originX;
            _originY = originY;
            _originZ = originZ;
            _profileStr = "";
            _classStr = "";
            _nameStr = "";
            _position = new TSM.Position();
            this._stackSegList = stackSegList;
            _numberOfSegments = 3;
        }

        public void Build()
        {
            CreateStack();
            CreatePlatform();
            CreateExtendedPlatform();
        }

        void CreateStack()
        {
            int counter1 = 0;
            double heightUptoSegmentBase = 0.0D;

            _position.Depth = TSM.Position.DepthEnum.MIDDLE;
            _position.Plane = TSM.Position.PlaneEnum.MIDDLE;
            _position.Rotation = TSM.Position.RotationEnum.FRONT;
            _classStr = "1";

            heightUptoSegmentBase = _originZ;

            for (counter1 = 0; counter1 < 3; counter1++)
            {
                T3D.Point startPoint = new T3D.Point(_originX, _originY, heightUptoSegmentBase);
                T3D.Point endPoint = ShiftVertically(new TSM.ContourPoint(startPoint, null), _stackSegList[counter1][2]);
                _nameStr = "segment" + (counter1 + 1);
                _profileStr = "CHS" + _stackSegList[counter1][1] + "*" + _stackSegList[counter1][0] + "*" + _stackSegList[counter1][3];
                CreateBeam(startPoint, endPoint, _profileStr, _materialStr, _classStr, _position, "myBeam");
                heightUptoSegmentBase += _stackSegList[counter1][2];
            }
        }
        void CreatePlatform()
        {
            CreateHandrail();
        }
        void CreateExtendedPlatform()
        {
            CreateHandrail();
        }
        void CreateHandrail()
        {
        }

    }
}
