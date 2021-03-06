using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Diagnostics;

namespace AutoSuggestTextBox
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AutoSuggestTextBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AutoSuggestTextBox;assembly=AutoSuggestTextBox"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class AutoSuggestTextBox : TextBox
    {

        #region fields

        #region DependencyPropertyFields

        // Using a DependencyProperty as the backing store for ItemContainerStyle.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemContainerStyleProperty =
            ItemsControl.ItemContainerStyleProperty.AddOwner(
                typeof (AutoSuggestTextBox),
                new UIPropertyMetadata(null, OnItemContainerStyleChanged));

        // Using a DependencyProperty as the backing store for ItemsSource.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof (AutoSuggestTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        // Using a DependencyProperty as the backing store for Binding.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding",
                                        typeof (string),
                                        typeof (AutoSuggestTextBox), new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ItemTemplate.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            ItemsControl.ItemTemplateProperty.AddOwner(
                typeof (AutoSuggestTextBox),
                new UIPropertyMetadata(null, OnItemTemplateChanged));

        // Using a DependencyProperty as the backing store for MaxCompletions.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCompletionsProperty =
            DependencyProperty.Register("MaxCompletions",
                                        typeof (int),
                                        typeof (AutoSuggestTextBox), new UIPropertyMetadata(int.MaxValue));

        // Using a DependencyProperty as the backing store for ItemTemplateSelector.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            ItemsControl.ItemTemplateSelectorProperty.AddOwner(
                typeof (AutoSuggestTextBox),
                new UIPropertyMetadata(null, OnItemTemplateSelectorChanged));


        public static DependencyProperty DefaultTextProperty =
                  DependencyProperty.Register(
                      "DefaultText",
                      typeof(string),
                      typeof(AutoSuggestTextBox));
         
        #endregion DependencyPropertyFields

        private Popup popup;
        private ListBox listBox; 
        private Func<object, string, bool> filter;
        private string textCache = string.Empty;
        private bool suppressEvent = false;

        private FrameworkElement dummy = new FrameworkElement();

        #endregion fields

        /// <summary>
        /// Static constructor of the <seealso cref="AutoSuggestTextBox" class />
        /// </summary>
        static AutoSuggestTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (AutoSuggestTextBox),
                                                     new FrameworkPropertyMetadata(typeof (AutoSuggestTextBox)));
        }

        #region properties

        public Func<object, string, bool> Filter
        {
            get { return this.filter; }

            set
            {
                if (this.filter != value)
                {
                    this.filter = value;
                    if (this.listBox != null)
                    {
                        if (this.filter != null)
                            this.listBox.Items.Filter = this.FilterFunc;
                        else
                            this.listBox.Items.Filter = null;
                    }
                }
            }
        }

        public string Binding
        {
            get { return (string) GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public Style ItemContainerStyle
        {
            get { return (Style) GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        public int MaxCompletions
        {
            get { return (int) GetValue(MaxCompletionsProperty); }
            set { SetValue(MaxCompletionsProperty, value); }
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        // ItemsSource Dependency Property
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        #endregion properties

        #region methods

        public void ShowPopup()
        {
            if (this.listBox == null || this.popup == null)
                this.InternalClosePopup();
            else if (this.listBox.Items.Count == 0)
                this.InternalClosePopup();
            else
                this.InternalOpenPopup();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.popup = Template.FindName("PART_Popup", this) as Popup;
            this.listBox = Template.FindName("PART_ListBox", this) as ListBox;

            if (this.listBox != null)
            {
                this.listBox.PreviewMouseDown += new MouseButtonEventHandler(this.listBox_MouseUp);
                this.listBox.KeyDown += new KeyEventHandler(this.listBox_KeyDown);

                OnItemsSourceChanged(this.ItemsSource);
                OnItemTemplateChanged(this.ItemTemplate);
                OnItemContainerStyleChanged(this.ItemContainerStyle);
                OnItemTemplateSelectorChanged(this.ItemTemplateSelector);

                if (this.filter != null)
                    this.listBox.Items.Filter = this.FilterFunc;
            }
        }

        // ItemsSource Dependency Property
        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (this.listBox == null) 
                return;
            if (itemsSource == null)
                return;

            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                this.listBox.ItemsSource =
                    new LimitedListCollectionView((IList) ((ListCollectionView) itemsSource).SourceCollection)
                        {Limit = this.MaxCompletions};
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                this.listBox.ItemsSource = new LimitedListCollectionView(((CollectionView) itemsSource).SourceCollection)
                                               {Limit = this.MaxCompletions};
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                this.listBox.ItemsSource = new LimitedListCollectionView((IList) itemsSource)
                                               {Limit = this.MaxCompletions};
                Debug.Print("Was IList");
            }
            else
            {
                this.listBox.ItemsSource = new LimitedCollectionView(itemsSource) {Limit = this.MaxCompletions};
                Debug.Print("Was IEnumerable");
            }

            if (this.listBox != null)
                this.listBox.SelectedIndex = -1;

            if (this.listBox.Items.Count == 0)
                this.InternalClosePopup();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (this.suppressEvent) return;

            this.textCache = Text ?? string.Empty;
            Debug.Print("OnTextChanged Text: " + this.textCache);

            if (this.popup != null && this.textCache == string.Empty)
            {
                this.InternalClosePopup();
            }
            else if (this.listBox != null) 
            {
                if (this.filter != null)
                    this.listBox.Items.Filter = this.FilterFunc;

                if (this.popup != null)
                {
                    if (this.listBox.Items.Count == 0)
                        this.InternalClosePopup();
                    else
                        this.InternalOpenPopup();
                }
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (this.Text.Length == 0)
                DefaultText = "";

            if (this.suppressEvent) return;

            if (this.popup != null)
                this.InternalClosePopup();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);        
            DefaultText = "";         
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var fs = FocusManager.GetFocusScope(this);
            var o = FocusManager.GetFocusedElement(fs);

            if (e.Key == Key.Escape)
            {
                this.InternalClosePopup();
                Focus();

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (this.listBox != null && o == this)
                {
                    this.suppressEvent = true;
                    this.listBox.Focus();
                    this.suppressEvent = false;

                    // Fix: Make sure the event is complete and not send to other controls in the window.
                    e.Handled = true;
                }
            }
        }

        // ItemsSource Dependency Property
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoSuggestTextBox;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as IEnumerable);
        }

        // ItemTemplate Dependency Property
        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoSuggestTextBox;
            if (actb == null) return;
            actb.OnItemTemplateChanged(e.NewValue as DataTemplate);
        }

        // ItemContainerStyle Dependency Property
        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoSuggestTextBox;
            if (actb == null) return;
            actb.OnItemContainerStyleChanged(e.NewValue as Style);
        }

        // ItemTemplateSelector Dependency Property
        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoSuggestTextBox;
            if (actb == null) return;
            actb.OnItemTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnItemTemplateChanged(DataTemplate p)
        {
            if (this.listBox == null)
                return;

            this.listBox.ItemTemplate = p;
        }

        private void OnItemContainerStyleChanged(Style p)
        {
            if (this.listBox == null) return;

            this.listBox.ItemContainerStyle = p;
        }

        private void OnItemTemplateSelectorChanged(DataTemplateSelector p)
        {
            if (this.listBox == null) return;

            this.listBox.ItemTemplateSelector = p;
        }


        private void InternalClosePopup()
        {
            if (this.popup != null)
                this.popup.IsOpen = false;
        }

        private void InternalOpenPopup()
        {
            this.popup.IsOpen = true;

            if (this.listBox != null)
            {
                this.listBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// The text in the textbox is changed by a selected item
        /// from the pop-up list of suggested items.
        /// </summary>
        /// <param name="obj">Item to set in Text portion of <see cref="AutoSuggestTextBox"/> control.
        /// <example>this.listBox.SelectedItem</example></param>
        /// <param name="moveFocus">The focus is moved to the next control if this true.
        /// Otherwise, the input focus stays in the <see cref="AutoSuggestTextBox"/> control.</param>
        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (this.popup != null)
            {
                this.InternalClosePopup();
                Dispatcher.Invoke(new Action(() =>
                                                 {
                                                     Focus();
                                                     if (moveFocus)
                                                         MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                                                 }), System.Windows.Threading.DispatcherPriority.Background);
            }

            // Retrieve the Binding object from the control.
            var originalBinding = BindingOperations.GetBinding(this, BindingProperty);
            if (originalBinding == null) return;

            // Set the dummy's DataContext to our selected object.
            this.dummy.DataContext = obj;

            // Apply the binding to the dummy FrameworkElement.
            BindingOperations.SetBinding(this.dummy, TextProperty, originalBinding);


            //Prevent OnTextChange get called again.
            suppressEvent = true;
            if (this.dummy.GetValue(TextProperty) == null)
                this.Text = "";
            else
            {
                this.Text = this.dummy.GetValue(TextProperty).ToString();        
            }

            RaiseExecuteDataEvent(obj);

            this.SelectionStart = this.CaretIndex = this.Text.Length;
            this.listBox.SelectedIndex = -1;
            suppressEvent = false;
        }

        private bool FilterFunc(object obj)
        {
            return this.filter(obj, this.textCache);
        }

        private void listBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject) e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null) return;

            var item = this.listBox.ItemContainerGenerator.ItemFromContainer(dep);

            if (item == null) return;

            this.SetTextValueBySelection(item, false);

            // Fix: Make sure the event is complete and not send to other controls in the window.
            e.Handled = true;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                this.SetTextValueBySelection(this.listBox.SelectedItem, false);               

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
            //else if (e.Key == Key.Tab)
            //{
            //    // The original implementation used a tab key to select an entry and change the focus
            //    // to the next control in the window. I find this confusing and deactivate it to implement
            //    // the standard behaviour of an autocomplete control, which is to
            //    // close list of suggestions and tab away (via escape and tab keys).
            //    this.SetTextValueBySelection(this.listBox.SelectedItem, false);

            //    // Fix: Make sure the event is complete and not send to other controls in the window.
            //    e.Handled = true;
            //}     
        }

        #endregion methods


        #region "Event"


        private static readonly RoutedEvent ListItemSelectedEvent =
                            EventManager.RegisterRoutedEvent("ListItemSelected", RoutingStrategy.Bubble,
                            typeof(RoutedPropertyChangedEventHandler<object>), typeof(AutoSuggestTextBox));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<object> ListItemSelected
        {
            add { AddHandler(ListItemSelectedEvent, value); }
            remove { RemoveHandler(ListItemSelectedEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseExecuteDataEvent(object data)
        {
            var e = new RoutedPropertyChangedEventArgs<object>(null,data, ListItemSelectedEvent);
            RaiseEvent(e);
        }
  
        #endregion
    }
}
