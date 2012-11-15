using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoSuggestControl.Model;

namespace AutoSuggestControl
{
    interface ISuggestion
    {
       void Initial();
       void Filter(string queryText);
       List<AutoSuggestModel.DisplayInstrumentResult> DataToDisplayResult();
    }
}
