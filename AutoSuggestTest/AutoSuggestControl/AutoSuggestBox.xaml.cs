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
    //A delegate type for hooking up change notifications.
    public delegate void ListItemSelectedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Interaction logic for AutoSuggestBox.xaml
    /// </summary>
    public partial class AutoSuggestBox : UserControl
    {
        public AutoSuggestBox()
        {
            InitializeComponent();
        }
      
        #region UserToken Dependency Property

        public string UserToken
        {
            get { return (string)GetValue(UserTokenProperty); }
            set
            {
                SetValue(UserTokenProperty, value);

                AutoSuggestViewModel vm = this.FindResource("AutoSuggestVM") as AutoSuggestViewModel;
                if (vm != null)
                    vm.UserToken = value;
            }
        }

        // Using a DependencyProperty as the backing store for MaxCompletions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserTokenProperty =
            DependencyProperty.Register("UserToken", typeof(string), typeof(AutoSuggestBox), new UIPropertyMetadata(""));

        #endregion

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

        #region AutoSuggestServerUrl Dependency Property
        public string AutoSuggestServerUrl
        {
            get { return (string)GetValue(AutoSuggestServerUrlProperty); }
            set
            {
                SetValue(AutoSuggestServerUrlProperty, value);

                var vm = this.FindResource("AutoSuggestVM") as AutoSuggestViewModel;
                if (vm != null)
                    vm.ServerUrl = value;
            }
        }
        
        public static readonly DependencyProperty AutoSuggestServerUrlProperty =
            DependencyProperty.Register("AutoSuggestServerUrl", typeof(string), typeof(AutoSuggestBox), new UIPropertyMetadata(""));
        #endregion

        #region AutoSuggestApiKey Dependency Property
        public string AutoSuggestApiKey
        {
            get { return (string)GetValue(AutoSuggestApiKeyProperty); }
            set
            {
                SetValue(AutoSuggestApiKeyProperty, value);

                var vm = this.FindResource("AutoSuggestVM") as AutoSuggestViewModel;
                if (vm != null)
                    vm.ApiKey = value;
            }
        }

        public static readonly DependencyProperty AutoSuggestApiKeyProperty =
            DependencyProperty.Register("AutoSuggestApiKey", typeof(string), typeof(AutoSuggestBox), new UIPropertyMetadata(""));

        #endregion

        private void commandBox_ListItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RaiseListItemSelectedEvent(e.NewValue);
        }

        #region "Event"
        //[PLink] An event that clients can use to be notified whenever the
        // elements of the list change.
        public event ListItemSelectedEventHandler ListSelected;

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
            //Fire event to client
            if (ListSelected != null)
            {
                commandBox.Text = "";
                ListSelected(data, e);
            }
        }
        #endregion
    }
}
