using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoSuggestControl;
using System.Xml.XPath;
using AutoSuggestControl.Model;
using System.Xml;

namespace AutoSuggestControl
{
    class COptionValueSuggestion : ISuggestion
    {
        private List<AutoSuggestModel.OptionValue> mOptionValueDB;
        List<AutoSuggestModel.OptionValue> mOptionValueList;
        private string mOptionName;

        public COptionValueSuggestion(string aParameterName)
        {
            mOptionName = aParameterName;
        }

        public void Initial()
        {
            try
            {
                //this is a string representing where the xml file is located
                string xmlFileName = "C:\\Dev\\ExcelInova\\TinkerBell\\Database\\Tinkerbell.xml";

                // create an XPathDocument object
                XPathDocument xmlPathDoc = new XPathDocument(xmlFileName);

                // create a navigator for the xpath doc
                XPathNavigator xNav = xmlPathDoc.CreateNavigator();
                string xPathStr = "//Parameter/item[@Code=\'" + mOptionName + "\']/value";

                XPathNodeIterator fieldIt = xNav.Select(xPathStr);
                if (fieldIt.Count > 0)
                {
                    mOptionValueDB = new List<AutoSuggestModel.OptionValue>();
                    while (fieldIt.MoveNext())
                    {
                        string l_value = fieldIt.Current.GetAttribute("Value", "");
                        string l_description = fieldIt.Current.GetAttribute("Description", "");
                        mOptionValueDB.Add(new AutoSuggestModel.OptionValue()
                        {
                            CodeName = l_value,
                            Description = l_description
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
            SortedList<int, List<AutoSuggestModel.OptionValue>> l_sortedOption = new SortedList<int, List<AutoSuggestModel.OptionValue>>();
            foreach (AutoSuggestModel.OptionValue l_item in mOptionValueDB)
            {
                string l_target = l_item.CodeName + l_item.Description;
                string l_upperTarget = l_target.ToUpper();
                string l_upperSource = queryText.ToUpper();
                int l_index = l_upperTarget.IndexOf(l_upperSource);
                if (l_index >= 0)
                {
                    int l_sortedIndex = l_sortedOption.IndexOfKey(l_index);
                    if (l_sortedIndex == -1)
                    {
                        l_sortedOption[l_index] = new List<AutoSuggestModel.OptionValue>();
                    }
                    l_sortedOption[l_index].Add(l_item);
                }
            }

            mOptionValueList = new List<AutoSuggestModel.OptionValue>();
            foreach (KeyValuePair<int, List<AutoSuggestModel.OptionValue>> l_items in l_sortedOption)
            {
                foreach (AutoSuggestModel.OptionValue l_item in l_items.Value)
                {
                    mOptionValueList.Add(l_item);
                }
            }
        }

        public List<AutoSuggestModel.DisplayInstrumentResult> DataToDisplayResult()
        {
            List<AutoSuggestModel.DisplayInstrumentResult> displayList = new List<AutoSuggestModel.DisplayInstrumentResult>();
            AutoSuggestModel.DisplayInstrumentResult headeritem = new AutoSuggestModel.DisplayInstrumentResult();
            headeritem.CategoryName = "Parameter Value";
            headeritem.HasMore = false;
            headeritem.IsCategory = true;
            displayList.Add(headeritem);

            foreach (AutoSuggestModel.OptionValue l_item in mOptionValueList)
            {
                AutoSuggestModel.DisplayInstrumentResult rowItem = new AutoSuggestModel.DisplayInstrumentResult();
                rowItem.CategoryName = "Parameter Value";
                rowItem.IsCategory = false;
                rowItem.Title = l_item.Description;
                rowItem.Subtitle = "";
                rowItem.Symbol = l_item.CodeName;
                displayList.Add(rowItem);
            }
            return displayList;
        }
    }
}
