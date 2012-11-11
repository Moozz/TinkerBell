using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoSuggestControl.Model;
using System.Xml.XPath;
using System.Xml;

namespace AutoSuggestControl
{
    class COptionSuggestion : ISuggestion
    {
        private List<AutoSuggestModel.Option> mOptionDB;
        List<AutoSuggestModel.Option> mOptionList;

        public COptionSuggestion()
        {

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
                XPathNodeIterator fieldIt = xNav.Select("//Parameter/item");
                if (fieldIt.Count > 0)
                {
                    mOptionDB = new List<AutoSuggestModel.Option>();
                    while (fieldIt.MoveNext())
                    {
                        string l_code = fieldIt.Current.GetAttribute("Code", "");
                        string l_description = fieldIt.Current.GetAttribute("Description1", "");
                        string l_type = fieldIt.Current.GetAttribute("Type", "");
                        mOptionDB.Add(new AutoSuggestModel.Option()
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
            SortedList<int, List<AutoSuggestModel.Option>> l_sortedOption = new SortedList<int, List<AutoSuggestModel.Option>>();
            foreach (AutoSuggestModel.Option l_item in mOptionDB)
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
                        l_sortedOption[l_index] = new List<AutoSuggestModel.Option>();
                    }
                    l_sortedOption[l_index].Add(l_item);
                }
            }

            mOptionList = new List<AutoSuggestModel.Option>();
            foreach (KeyValuePair<int, List<AutoSuggestModel.Option>> l_items in l_sortedOption)
            {
                foreach (AutoSuggestModel.Option l_item in l_items.Value)
                {
                    mOptionList.Add(l_item);
                }
            }
        }

        public List<AutoSuggestModel.DisplayInstrumentResult> DataToDisplayResult()
        {
            List<AutoSuggestModel.DisplayInstrumentResult> displayList = new List<AutoSuggestModel.DisplayInstrumentResult>();
            AutoSuggestModel.DisplayInstrumentResult headeritem = new AutoSuggestModel.DisplayInstrumentResult();
            headeritem.CategoryName = "Parameter";
            headeritem.HasMore = false;
            headeritem.IsCategory = true;
            displayList.Add(headeritem);

            foreach (AutoSuggestModel.Option l_item in mOptionList)
            {
                AutoSuggestModel.DisplayInstrumentResult rowItem = new AutoSuggestModel.DisplayInstrumentResult();
                rowItem.CategoryName = "Parameter";
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
