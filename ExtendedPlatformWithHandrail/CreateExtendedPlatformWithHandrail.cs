using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;


namespace ExtendedPlatformWithHandrail
{
    public class CreateExtendedPlatformWithHandrail
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

        public CreateExtendedPlatformWithHandrail(double originX, double originY, double originZ, List<List<Double>> stackSegList)
        {
            _originX = 0;
            _originY = 0;
            _originZ = 0;
            _profileStr = "";
            _classStr = "";
            _nameStr = "";
            _position = new TSM.Position();
            this._stackSegList = new List<List<double>>();
            _numberOfSegments = 3;
        }

        public void build()
        {
            CreateStack();
            CreatePlatform();
            CreateExtendedPlatform();
        }

        void CreateStack()
        {
            
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
