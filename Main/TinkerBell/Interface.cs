using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinkerBell
{
    interface IWillHearMyParent
    {
        void OnMyParentToldsMeThatHeChangesParameters();
    }

    interface IWillHearMyChilds
    {
        void OnMyChildToldsMeThatHeChangesParameters();
    }
}
