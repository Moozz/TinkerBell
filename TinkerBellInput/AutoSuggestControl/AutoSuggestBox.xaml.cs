using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoSuggestControl
{
    /// <summary>
    /// Interaction logic for AutoSuggestBox.xaml
    /// </summary>
    public partial class AutoSuggestBox : UserControl
    {
        public AutoSuggestBox()
        {
            InitializeComponent();
            var vm = this.FindResource("AutoSuggestVM") as AutoSuggestViewModel;
            if (vm != null)
            {
                vm.InitInstrumentDB();
            }
        }
      

        #region Text Dependency Property

        public string Text
        {
            get
            {
                return commandBox.Text;                
            }
            set
            {
                SetValue(TextTokenProperty, value);
                commandBox.Focus();
                commandBox.Text = value;
                commandBox.SelectionStart = commandBox.Text.Length;
            }
        }

        public static readonly DependencyProperty TextTokenProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoSuggestBox), new UIPropertyMetadata(""));

        #endregion



        private void commandBox_ListItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RaiseListItemSelectedEvent(e.NewValue);
        }

        #region "Event"
        private static readonly RoutedEvent ListItemSelectedEvent =
                            EventManager.RegisterRoutedEvent("ListItemSelected", RoutingStrategy.Bubble,
                            typeof(RoutedPropertyChangedEventHandler<object>), typeof(AutoSuggestBox));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<object> ListItemSelected
        {
            add { AddHandler(ListItemSelectedEvent, value); }
            remove { RemoveHandler(ListItemSelectedEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseListItemSelectedEvent(object data)
        {
            var e = new RoutedPropertyChangedEventArgs<object>(null,data, ListItemSelectedEvent);
            RaiseEvent(e);
        }
        #endregion
    }
}
