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
                    mWaitMessage = new List<AutoSuggestModel.DisplayInstrumentResult>();
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

        private List<AutoSuggestModel.Instrument> mInstrumentDB;

        public void InitInstrumentDB()
        {
            try
            {
                //this is a string representing where the xml file is located
                string xmlFileName = "D:\\TinkerBell\\Database\\Tinkerbell.xml";

                // create an XPathDocument object
                XPathDocument xmlPathDoc = new XPathDocument(xmlFileName);

                // create a navigator for the xpath doc
                XPathNavigator xNav = xmlPathDoc.CreateNavigator();

                //navigate and print the document
                XPathNodeIterator xPathIt = xNav.Select("//Instrument/item");
                if (xPathIt.Count > 0)
                {
                    mInstrumentDB = new List<AutoSuggestModel.Instrument>();
                    while (xPathIt.MoveNext())
                    {
                        string l_code = xPathIt.Current.GetAttribute("Code", "");
                        string l_description = xPathIt.Current.GetAttribute("Description", "");
                        string l_type = xPathIt.Current.GetAttribute("Type", "");
                        mInstrumentDB.Add(new AutoSuggestModel.Instrument()
                            {
                                CodeName = l_code,
                                Description = l_description,
                                Type = l_type
                            });

                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }

        private List<AutoSuggestModel.Instrument> InstrumentFilter(string queryText)
        {
            SortedList<int, List<AutoSuggestModel.Instrument>> l_sortedInstrument = new SortedList<int, List<AutoSuggestModel.Instrument>>();   
            foreach (AutoSuggestModel.Instrument l_item in mInstrumentDB)
            {
                string l_target = l_item.CodeName + l_item.Description;
                string l_upperTarget = l_target.ToUpper();
                string l_upperSource = queryText.ToUpper();
                int l_index = l_upperTarget.IndexOf(l_upperSource);
                if (l_index >= 0)
                {
                    int l_sortedIndex = l_sortedInstrument.IndexOfKey(l_index);
                    if (l_sortedIndex == -1)
                    {
                        l_sortedInstrument[l_index] = new List<AutoSuggestModel.Instrument>();
                    }
                    l_sortedInstrument[l_index].Add(l_item);
                }
            }

            List<AutoSuggestModel.Instrument> l_instrumentList = new List<AutoSuggestModel.Instrument>();
            foreach (KeyValuePair<int, List<AutoSuggestModel.Instrument>> l_items in l_sortedInstrument)
            {
                foreach (AutoSuggestModel.Instrument l_item in l_items.Value)
                {
                    l_instrumentList.Add(l_item);
                }
            }
                
            return l_instrumentList;
        }

        private void MakeAutosuggestRequest(string queryText)
        {
            List<AutoSuggestModel.Instrument> instrumentList = InstrumentFilter(queryText);
            if (instrumentList.Count > 0)
            {
                mQueryCollection = DataToDisplayResult(instrumentList);
            }
            else
            {
                mQueryCollection = new List<AutoSuggestModel.Instrument>();
            }
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

            mWaitMessage = DataToDisplayResult(dummyResult);
            this.OnPropertyChanged(typeof(AutoSuggestViewModel).GetProperty("WaitMessage").Name);

        }

        private List<AutoSuggestModel.DisplayInstrumentResult> DataToDisplayResult( List<AutoSuggestModel.Instrument> a_instrumentList)
        {         
            List<AutoSuggestModel.DisplayInstrumentResult> displayList = new List<AutoSuggestModel.DisplayInstrumentResult>();
            AutoSuggestModel.DisplayInstrumentResult headeritem = new AutoSuggestModel.DisplayInstrumentResult();
            headeritem.CategoryName = "Instrument";
            headeritem.HasMore = false;
            headeritem.IsCategory = true;
            displayList.Add(headeritem);

            foreach (AutoSuggestModel.Instrument l_item in a_instrumentList)
            {
                AutoSuggestModel.DisplayInstrumentResult rowItem = new AutoSuggestModel.DisplayInstrumentResult();
                rowItem.CategoryName = "Instrument";
                rowItem.IsCategory = false;
                rowItem.Title = l_item.Description;
                rowItem.Subtitle = l_item.Type;
                rowItem.Symbol = l_item.CodeName;
                displayList.Add(rowItem);
            }
            return displayList;
        }

    }
}
