using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using AutoSuggestControl.Model;
using RestSharp;
using RestSharp.Contrib;




namespace AutoSuggestControl
{
   
    public class AutoSuggestViewModel : INotifyPropertyChanged
    {
        #region fields

        private List<AutoSuggestModel.DisplaySearchResult> mWaitMessage = null;

        private string mQueryText = string.Empty;

        private IEnumerable mQueryCollection = null;
       
   
        #endregion fields

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region properties       

        /// <summary>
        /// Get a text drop-down indicating to the user that the system is busy...
        /// </summary>
        public IEnumerable WaitMessage
        {
            get
            {
                if (mWaitMessage == null)
                {
                    mWaitMessage = new List<AutoSuggestModel.DisplaySearchResult>();
                    AddDummyResult(mQueryText, true);
                }
                return this.mWaitMessage;
            }
        }

        /// <summary>
        /// Get/set the current text that is used in the query part of the drop-down
        /// section to determine the list of suggestions...
        /// </summary>
        public string QueryText
        {
            get
            {
                return this.mQueryText;
            }

            set
            {
                if (this.mQueryText != value)
                {
                    this.mQueryText = value;
                    this.OnPropertyChanged(typeof(AutoSuggestViewModel).GetProperty("QueryText").Name);
                    
                    if (QueryText.Length > 0)
                    {
                        MakeAutosuggestRequest(QueryText);
                    }
                }
            }
        }

        /// <summary>
        /// Get list of suggestions matching the string that a user had typed.
        /// </summary>
        public IEnumerable QueryCollection
        {
            get
            {

                return this.mQueryCollection;
            }
        }
        #endregion properties

        #region methods
        
        /// <summary>
        /// Standard method of the <seealso cref="INotifyPropertyChanged"/> interface
        /// which is used to tell subscribers of the ViewModel when the value of a property
        /// has changed.
        /// </summary>
        /// <param name="prop"></param>
        protected void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion methods



        //additional for web service request.
        private string _apiKey;
        public string ApiKey
        {
            get
            {
                return _apiKey;
            }

            set
            {
                _apiKey = value;
            }
        }

        private RestClient _restClient = null;

        private string _serverUrl;

        public string ServerUrl
        {
            get
            {
                return _serverUrl;
            }

            set
            {
                _serverUrl = value;
                _restClient = new RestClient(_serverUrl);
            }
        }
            
      
        public string UserToken { get; set; }
        private RestRequestAsyncHandle asyncHandle = null;

        private RestRequest CreateRestRequest(string queryText)
        {
            var request = new RestRequest("query={query}&api-key={apikey}", Method.GET);
            request.AddUrlSegment("query", queryText);
            request.AddUrlSegment("apikey", ApiKey);
            //cookie
            request.AddParameter("EIKON_USER_AGENT", "\"NET40,EIKON6.0.75,SR2,HF75.4,HF75.1,HF75.2,HF75.3,HF75.5,DT12.11.2,TA6.0.25,CPO6.0.4\"", ParameterType.Cookie);
            //request.AddParameter("iPlanetDirectoryPro", HttpUtility.UrlEncode(UserToken), ParameterType.Cookie);
            request.AddParameter("iPlanetDirectoryPro", UserToken, ParameterType.Cookie);

            request.AddHeader("Accept-Language", "en");
            return request;
        }

        private void MakeAutosuggestRequest(string queryText)
        {
            if (_restClient == null)
            {   //assign default URL to aint2
                //ServerUrl ="https://amers1.cps.cp.icp2.mpp.extranet.reutest.biz";
                return;
            }

            var request = CreateRestRequest(queryText);
            asyncHandle =  _restClient.ExecuteAsync<AutoSuggestModel.SearchResultList>(request, response =>
            {
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Data != null)
                    {
                        Debug.Print("Got Response from webservice  row count = {0}", response.Data.Result.Count);
                        mQueryCollection = DataToDisplayResult(response.Data.Result);
                        this.OnPropertyChanged(typeof(AutoSuggestViewModel).GetProperty("QueryCollection").Name);
                    }
                    else
                    {
                         AddDummyResult(queryText, false);
                    }
                }
                else
                {
                    if (response != null)
                        Console.WriteLine("Error : {0} ", response.Content);
                }
                //asyncHandle = null;
            });
        }

        private void AddDummyResult(string queryText, bool IsWaiting = true)
        {
            Debug.Print("Show Dummy row");            
            List<AutoSuggestModel.SearchResult> dummyResult = new List<AutoSuggestModel.SearchResult>();
            dummyResult.Add(new AutoSuggestModel.SearchResult()
            {
                Name = "Searching ...",
                HasMore = false,
                Hits = new List<AutoSuggestModel.HitResult>(),
                Size = 1
            });

            if (IsWaiting)
            {
                mWaitMessage = DataToDisplayResult(dummyResult);
                this.OnPropertyChanged(typeof(AutoSuggestViewModel).GetProperty("WaitMessage").Name);
            }
            else
            {
                mQueryCollection = DataToDisplayResult(dummyResult);
                this.OnPropertyChanged(typeof (AutoSuggestViewModel).GetProperty("QueryCollection").Name);                
            }
        }

        private List<AutoSuggestModel.DisplaySearchResult> DataToDisplayResult(List<AutoSuggestModel.SearchResult> resultList)
        {
            List<AutoSuggestModel.DisplaySearchResult> displayList = new List<AutoSuggestModel.DisplaySearchResult>();

            foreach (AutoSuggestModel.SearchResult item in resultList)
            {
                AutoSuggestModel.DisplaySearchResult   headeritem = new AutoSuggestModel.DisplaySearchResult();
                headeritem.CategoryName = item.Name;
                headeritem.HasMore = item.HasMore;
                headeritem.IsCategory = true;
                displayList.Add(headeritem);

                foreach (AutoSuggestModel.HitResult hit in item.Hits)
                {
                    AutoSuggestModel.DisplaySearchResult rowItem = new AutoSuggestModel.DisplaySearchResult();
                    rowItem.CategoryName = item.Name;
                    rowItem.IsCategory = false;
                    rowItem.Score = hit.Score;
                    rowItem.Source = hit.Source;
                    rowItem.Title = hit.Title;
                    rowItem.Subtitle = hit.Subtitle;
                    rowItem.Symbol = hit.Symbol;
                    rowItem.Id = hit.Id;
                    rowItem.S = hit.S;
                    rowItem.St = hit.St;
                    rowItem.ExecuteData = hit.Navigation;
                    displayList.Add(rowItem);
                }
            }

            return displayList;
        }

    }
}
