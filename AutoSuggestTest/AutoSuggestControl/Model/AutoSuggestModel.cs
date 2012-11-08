using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoSuggestControl.Model
{
    public class AutoSuggestModel
    {
        public class HitResultData
        {
            public string Type { get; set; }
            public string Trbc { get; set; }
            public string Ticker { get; set; }
            public string AssetCategory { get; set; }
            public string ExchangeName { get; set; }
        }

        public class HitResult
        {
            public double Score { get; set; }
            public string Source { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Symbol { get; set; }
            public HitResultData Data { get; set; }
            public long Id { get; set; }
            public string S { get; set; }
            public string St { get; set; }
            public NavigationData Navigation { get; set; }
        }

        public  class NavigationData
        {
            public string Name { get; set; }
            public string Target { get; set; }
            public string Url { get; set; }
        }

        public class SearchResult
        {
            public string Name { get; set; }
            public List<HitResult> Hits { get; set; }
            public bool HasMore { get; set; }
            public int Size { get; set; }
        }

        public class SearchResultList
        {
            public List<SearchResult> Result { get; set; }
        }

        // A consolidate data structor for binding data to list
        public class DisplaySearchResult
        {
            public double Score { get; set; }
            public string Source { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Symbol { get; set; }
            public HitResultData Data { get; set; }
            public long Id { get; set; }
            public string S { get; set; }
            public string St { get; set; }

            public string CategoryName { get; set; }            
            public bool HasMore { get; set; }
            public bool IsCategory { get; set; }
            public NavigationData ExecuteData { get; set; }
        }
    }
}
