using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoSuggestControl.Model;
using System.Xml.XPath;
using System.Xml;

namespace AutoSuggestControl
{
    class CFieldSuggestion : ISuggestion
    {
        private List<AutoSuggestModel.Field> mFieldDB;
        List<AutoSuggestModel.Field> mFieldList;

        public CFieldSuggestion()
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
                XPathNodeIterator fieldIt = xNav.Select("//Field/item");
                if (fieldIt.Count > 0)
                {
                    mFieldDB = new List<AutoSuggestModel.Field>();
                    while (fieldIt.MoveNext())
                    {
                        string l_code = fieldIt.Current.GetAttribute("Code", "");
                        string l_description = fieldIt.Current.GetAttribute("Description", "");
                        string l_type = fieldIt.Current.GetAttribute("Type", "");
                        mFieldDB.Add(new AutoSuggestModel.Field()
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
            SortedList<int, List<AutoSuggestModel.Field>> l_sortedField = new SortedList<int, List<AutoSuggestModel.Field>>();
            foreach (AutoSuggestModel.Field l_item in mFieldDB)
            {
                string l_target = l_item.CodeName + l_item.Description;
                string l_upperTarget = l_target.ToUpper();
                string l_upperSource = queryText.ToUpper();
                int l_index = l_upperTarget.IndexOf(l_upperSource);
                if (l_index >= 0)
                {
                    int l_sortedIndex = l_sortedField.IndexOfKey(l_index);
                    if (l_sortedIndex == -1)
                    {
                        l_sortedField[l_index] = new List<AutoSuggestModel.Field>();
                    }
                    l_sortedField[l_index].Add(l_item);
                }
            }

            mFieldList = new List<AutoSuggestModel.Field>();
            foreach (KeyValuePair<int, List<AutoSuggestModel.Field>> l_items in l_sortedField)
            {
                foreach (AutoSuggestModel.Field l_item in l_items.Value)
                {
                    mFieldList.Add(l_item);
                }
            }

        }

        public List<AutoSuggestModel.DisplayInstrumentResult> DataToDisplayResult()
        {
            List<AutoSuggestModel.DisplayInstrumentResult> displayList = new List<AutoSuggestModel.DisplayInstrumentResult>();
            AutoSuggestModel.DisplayInstrumentResult headeritem = new AutoSuggestModel.DisplayInstrumentResult();
            headeritem.CategoryName = "Field";
            headeritem.HasMore = false;
            headeritem.IsCategory = true;
            displayList.Add(headeritem);

            foreach (AutoSuggestModel.Field l_item in mFieldList)
            {
                AutoSuggestModel.DisplayInstrumentResult rowItem = new AutoSuggestModel.DisplayInstrumentResult();
                rowItem.CategoryName = "Field";
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
