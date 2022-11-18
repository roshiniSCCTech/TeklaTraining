using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace HelperLibrary
{
    public class TeklaModelling : GeometricalHelperClass
    {
        protected TSM.Model Model;


        protected TeklaModelling(double originX, double originY, double originZ) : base(originX, originY, originZ)
        {
            Model = new TSM.Model();
        }
        protected TSM.Beam CreateBeam(T3D.Point start, T3D.Point end, string profile, string material, string className, TSM.Position position, string name = "")
        {
            TSM.Beam beam = new TSM.Beam();
            beam.StartPoint = start;
            beam.EndPoint = end;
            beam.Profile.ProfileString = profile;
            beam.Material.MaterialString = material;
            beam.Position = position;
            beam.Name = name;
            beam.Class = className;

            if(beam.Insert())
            {
                Model.CommitChanges();
            }
           
            return beam;
        }
    }
}
