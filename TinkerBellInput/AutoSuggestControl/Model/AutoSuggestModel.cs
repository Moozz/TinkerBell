using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoSuggestControl.Model
{
    public class AutoSuggestModel
    {
        public class Instrument
        {
            public string CodeName { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
        }

        // A consolidate data structor for binding data to list
        public class DisplayInstrumentResult
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Symbol { get; set; }

            public string CategoryName { get; set; }            
            public bool HasMore { get; set; }
            public bool IsCategory { get; set; }
        }
    }
}
