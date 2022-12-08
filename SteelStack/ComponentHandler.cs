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

        public ComponentHandler(Globals global, TeklaModelling teklaModel, Dictionary<string, bool> checkedComponents)
        {
            _global = global;
            _teklaModel = teklaModel;

            new StackShell(global, teklaModel);

            if (checkedComponents["Platform"])
            {
                new Platform(global, teklaModel);
            }
            if (checkedComponents["Handrail"])
            {
                new Handrail(global, teklaModel);
            }
            if (checkedComponents["FloorSteel"])
            {
                new FloorSteel(global, teklaModel);
            }


            // new Test(global, teklaModel);

        }
    }
}
