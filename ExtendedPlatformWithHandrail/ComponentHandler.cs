using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteelStack.Components;

namespace SteelStack
{
    class ComponentHandler
    {
        Globals _global;
        TeklaModelling _teklaModel;

        public ComponentHandler(Globals global, TeklaModelling teklaModel)
        {
            _global = global;
            _teklaModel = teklaModel;

            new StackShell(global, teklaModel);
            new Platform(global, teklaModel);
            new Handrail(global, teklaModel);

        }
    }
}
