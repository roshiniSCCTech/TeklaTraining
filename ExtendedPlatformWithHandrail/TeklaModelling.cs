﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using T3D = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using ExtendedPlatformWithHandrail;

namespace HelperClasses
{
    public class TeklaModelling : GeometricalHelperClass
    {
        protected TeklaModelling(double originX, double originY, double originZ) : base(originX, originY, originZ) {}
        protected static TSM.Beam CreateBeam(T3D.Point start, T3D.Point end, string profile, string material, string className, TSM.Position position, string name = "")
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
                CreateExtendedPlatformWithHandrail.Model.CommitChanges();
            }
           
            return beam;
        }
    }
}
