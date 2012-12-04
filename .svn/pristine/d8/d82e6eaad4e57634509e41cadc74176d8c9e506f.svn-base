using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoSuggestControl;
using AutoSuggestControl.Model;
using System.Xml.XPath;
using System.Xml;

namespace AutoSuggestControl
{
    class CInstrumentSuggestion : ISuggestion
    {

        private List<AutoSuggestModel.Instrument> mInstrumentDB;
        List<AutoSuggestModel.Instrument> mInstrumentList;

        public CInstrumentSuggestion()
        {

        }

        public void Initial()
        {
            try
            {
                //this is a string representing where the xml file is located
                string xmlFileName = "Tinkerbell.xml";

                // create an XPathDocument object
                XPathDocument xmlPathDoc = new XPathDocument(xmlFileName);

                // create a navigator for the xpath doc
                XPathNavigator xNav = xmlPathDoc.CreateNavigator();
                XPathNodeIterator instrimentIt = xNav.Select("//Instrument/item");
                if (instrimentIt.Count > 0)
                {
                    mInstrumentDB = new List<AutoSuggestModel.Instrument>();
                    while (instrimentIt.MoveNext())
                    {
                        string l_code = instrimentIt.Current.GetAttribute("Code", "");
                        string l_description = instrimentIt.Current.GetAttribute("Description", "");
                        string l_type = instrimentIt.Current.GetAttribute("Type", "");
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

        public void Filter(string queryText)
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

            //if (mInstrumentList != null)
            //{
            //    mInstrumentList.Clear();
            //}
            mInstrumentList = new List<AutoSuggestModel.Instrument>();
            foreach (KeyValuePair<int, List<AutoSuggestModel.Instrument>> l_items in l_sortedInstrument)
            {
                foreach (AutoSuggestModel.Instrument l_item in l_items.Value)
                {
                    mInstrumentList.Add(l_item);
                }
            }
        }

        public List<AutoSuggestModel.DisplayInstrumentResult> DataToDisplayResult()
        {
            List<AutoSuggestModel.DisplayInstrumentResult> displayList = new List<AutoSuggestModel.DisplayInstrumentResult>();
            AutoSuggestModel.DisplayInstrumentResult headeritem = new AutoSuggestModel.DisplayInstrumentResult();
            headeritem.CategoryName = "Instrument";
            headeritem.HasMore = false;
            headeritem.IsCategory = true;
            displayList.Add(headeritem);

            foreach (AutoSuggestModel.Instrument l_item in mInstrumentList)
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
