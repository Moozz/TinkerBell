using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using AutoSuggestControl.Model;
using RestSharp;
using RestSharp.Contrib;
using System.Windows.Data;



namespace AutoSuggestControl
{
   
    public class AutoSuggestViewModel : INotifyPropertyChanged
    {
        #region fields

        private List<AutoSuggestModel.DisplayInstrumentResult> mWaitMessage = null;

        private string mQueryText = string.Empty;

        private IEnumerable mQueryCollection = null;

        private ISuggestion mSuggestion;

        #endregion fields

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region properties       

        public IEnumerable WaitMessage
        {
            get
            {
                if (mWaitMessage == null)
                {
                    mWaitMessage = new List<AutoSuggestModel.DisplayInstrumentResult>();
                    AddDummyResult(mQueryText, true);
                }
                return this.mWaitMessage;
            }
        }

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

        public void InitInstrumentDB()
        {
             mSuggestion = new CInstrumentSuggestion();
             mSuggestion.Initial();
        }

        public void InitFieldDB()
        {
            mSuggestion = new CFieldSuggestion();
            mSuggestion.Initial();
        }

        public void InitOptionDB()
        {
            mSuggestion = new COptionSuggestion();
            mSuggestion.Initial();
        }

        public void InitOptionValueDB(string aOptionName)
        {
            mSuggestion = new COptionValueSuggestion(aOptionName);
            mSuggestion.Initial();
        }

        private void MakeAutosuggestRequest(string queryText)
        {
            mSuggestion.Filter(queryText);
            mQueryCollection = mSuggestion.DataToDisplayResult();
            this.OnPropertyChanged(typeof(AutoSuggestViewModel).GetProperty("QueryCollection").Name);
        }

        private void AddDummyResult(string queryText, bool IsWaiting = true)
        {
            Debug.Print("Show Dummy row");
            List<AutoSuggestModel.Instrument> dummyResult = new List<AutoSuggestModel.Instrument>();
            dummyResult.Add(new AutoSuggestModel.Instrument()
            {
                CodeName = "Searching...",
                Description = "",
                Type = ""
            });

            mWaitMessage = mSuggestion.DataToDisplayResult();
            this.OnPropertyChanged(typeof(AutoSuggestViewModel).GetProperty("WaitMessage").Name);

        }
    }
}
