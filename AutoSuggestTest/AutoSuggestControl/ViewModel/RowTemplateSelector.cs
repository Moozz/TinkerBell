using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AutoSuggestControl.Model;

namespace AutoSuggestControl.ViewModel
{
    public class RowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HeaderTemplate { get; set; }
        public DataTemplate RowTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,  DependencyObject container)
        {
            if (item is AutoSuggestModel.DisplaySearchResult)
            {
                var suggestedItem = item as AutoSuggestModel.DisplaySearchResult;
                if (suggestedItem.IsCategory)
                    return HeaderTemplate;
                else
                {
                    return RowTemplate;
                }
            }

            return RowTemplate;
        }
    }
}
