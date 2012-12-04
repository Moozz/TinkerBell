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
using ExcelManager;
using System.Timers;

using TinkerBell;

namespace TinkerBell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        enum Types { Insturments, Fields, Parameters };
        int m_noOfInstruments = 0;
        int m_noOfFields = 0;
        int m_noOfParameters = 0;

        const double inputWidth = 380;
        const double inputHeight = 30;
        const double textboxHeight = 25;
        const double textboxWidth = 100;
        //const double descriptionWidth = 180;
        const double removeButtonWidth = 25;
        const double removeButtonHeight = 25;

        // Instrument & Field
        List<Label> instrumentLabel;
        List<Label> fieldLabel;
        List<AutoSuggestControl.AutoSuggestBox> instrumentTextBox;
        List<AutoSuggestControl.AutoSuggestBox> fieldTextBox;
        List<Label> instrumentDescriptionLabel;
        List<Label> fieldDescriptionLabel;

        List<WrapPanel> instrumentWrapPanel;
        List<WrapPanel> fieldWrapPanel;

        List<Button> instrumentRemoveButton;
        List<Button> fieldRemoveButton;

        // Parameters
        List<Label> parameterLabel;
        List<WrapPanel> parameterWrapPanel;
        List<AutoSuggestControl.AutoSuggestBox> parameterKeyTextBox;
        List<Label> parameterSeparatorLabel;
        List<AutoSuggestControl.AutoSuggestBox> parameterValueTextBox;
        List<Label> parameterDescriptionLabel;
        List<Button> parameterRemoveButton;
        List<String> parameterDescription;

        const double textboxParameterKeyWidth = 80;
        const double labelParameterSaparatorWidth = 12;
        const double textboxParameterValueWidth = 30;
        //const double labelParameterDescriptionWidth = 200;

        static Timer _timer;

        static CExcelManager m_excelMgr;
        public MainWindow()
        {
            InitializeComponent();

            // Instrument & Fields
            instrumentLabel = new List<Label>();
            fieldLabel = new List<Label>();

            instrumentWrapPanel = new List<WrapPanel>();
            fieldWrapPanel = new List<WrapPanel>();

            instrumentTextBox = new List<AutoSuggestControl.AutoSuggestBox>();
            fieldTextBox = new List<AutoSuggestControl.AutoSuggestBox>();

            instrumentDescriptionLabel = new List<Label>();
            fieldDescriptionLabel = new List<Label>();

            instrumentRemoveButton = new List<Button>();
            fieldRemoveButton = new List<Button>();

            // Paramters
            parameterLabel = new List<Label>();
            parameterWrapPanel = new List<WrapPanel>();
            parameterKeyTextBox = new List<AutoSuggestControl.AutoSuggestBox>();
            parameterSeparatorLabel = new List<Label>();
            parameterValueTextBox = new List<AutoSuggestControl.AutoSuggestBox>();
            parameterDescriptionLabel = new List<Label>();
            parameterRemoveButton = new List<Button>();
            parameterDescription = new List<string>();

            this.Instrumentslb.ItemsReordered += new RoutedEventHandler(this.OnItemsInstrumentsReordered);
            this.Fieldslb.ItemsReordered += new RoutedEventHandler(this.OnItemsFieldReordered);
            this.Parameterslb.ItemsReordered += new RoutedEventHandler(this.OnItemsParametersReordered);

            _timer = new Timer(1000); // Set up the timer for 3 seconds
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
        }
        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_excelMgr = new CExcelManager();
            if (m_excelMgr.IsExcelReady())
            {
                Timer timer = (Timer)sender; // Get the timer that fired the event
                timer.Stop(); // Stop the timer that fired the event
            }
            
        }

        private void OnItemsParametersReordered(object sender, RoutedEventArgs e)
        {
           
            ReorderableListBox l_listBox = (ReorderableListBox)sender;
            int index = l_listBox.SelectedIndex;
            int from = l_listBox.OriginalItemIndex;

            if (index == from)
                return;

            Label originalObject = (Label)l_listBox.Items.GetItemAt(from);

            Parameterslb.Items.RemoveAt(from);
            Parameterslb.Items.Insert(index, originalObject);
            Parameterslb.UpdateLayout();

            parameterLabel.Insert(index, parameterLabel[from]);
            parameterWrapPanel.Insert(index, parameterWrapPanel[from]);
            parameterKeyTextBox.Insert(index, parameterKeyTextBox[from]);
            parameterSeparatorLabel.Insert(index, parameterSeparatorLabel[from]);
            parameterValueTextBox.Insert(index, parameterValueTextBox[from]);
            parameterDescriptionLabel.Insert(index, parameterDescriptionLabel[from]);
            parameterRemoveButton.Insert(index, parameterRemoveButton[from]);
            parameterDescription.Insert(index, parameterDescription[from]);


            parameterLabel.RemoveAt(from);
            parameterWrapPanel.RemoveAt(from);
            parameterKeyTextBox.RemoveAt(from);
            parameterSeparatorLabel.RemoveAt(from);
            parameterValueTextBox.RemoveAt(from);
            parameterDescriptionLabel.RemoveAt(from);
            parameterRemoveButton.RemoveAt(from);
            parameterDescription.RemoveAt(from);

      
            if (from > index)
            {
                m_excelMgr.m_parameterList.Insert(index, m_excelMgr.m_parameterList[from]);
                m_excelMgr.m_parameterList.RemoveAt(from + 1);
            }
            else
            {
                m_excelMgr.m_parameterList.Insert(index + 1, m_excelMgr.m_parameterList[from]);
                m_excelMgr.m_parameterList.RemoveAt(from);
            }

            m_excelMgr.RefreshParameter();

            //Update parameter tag
            for (int l_index = from; l_index < m_noOfFields; ++l_index)
            {
                parameterLabel[l_index].Tag = l_index;
                parameterWrapPanel[l_index].Tag = l_index;
                parameterKeyTextBox[l_index].Tag = l_index;
                parameterSeparatorLabel[l_index].Tag = l_index;
                parameterValueTextBox[l_index].Tag = l_index;
                parameterDescriptionLabel[l_index].Tag = l_index;
                parameterRemoveButton[l_index].Tag = l_index;
            }

            RefreshInterpreter();

        }

        private void OnItemsFieldReordered(object sender, RoutedEventArgs e)
        {
            ReorderableListBox l_listBox = (ReorderableListBox)sender;
            int index = l_listBox.SelectedIndex;
            int from = l_listBox.OriginalItemIndex;

            if (index == from)
                return;

            Label originalObject = (Label)l_listBox.Items.GetItemAt(from);

            Fieldslb.Items.RemoveAt(from);
            Fieldslb.Items.Insert(index, originalObject);
            Fieldslb.UpdateLayout();
            fieldLabel.Insert(index, fieldLabel[from]);
            fieldWrapPanel.Insert(index, fieldWrapPanel[from]);
            fieldTextBox.Insert(index, fieldTextBox[from]);
            fieldDescriptionLabel.Insert(index, fieldDescriptionLabel[from]);
            fieldRemoveButton.Insert(index, fieldRemoveButton[from]);

            fieldLabel.RemoveAt(from);
            fieldWrapPanel.RemoveAt(from);
            fieldTextBox.RemoveAt(from);
            fieldDescriptionLabel.RemoveAt(from);
            fieldRemoveButton.RemoveAt(from);

            if (from > index)
            {
                m_excelMgr.m_fieldList.Insert(index, m_excelMgr.m_fieldList[from]);
                m_excelMgr.m_fieldList.RemoveAt(from + 1);
            }
            else
            {
                m_excelMgr.m_fieldList.Insert(index + 1, m_excelMgr.m_fieldList[from]);
                m_excelMgr.m_fieldList.RemoveAt(from);
            }

            m_excelMgr.RefreshField();
            
            //Update instrument tag
            for (int l_index = from; l_index < m_noOfFields; ++l_index)
            {
                fieldLabel[l_index].Tag = l_index;
                fieldWrapPanel[l_index].Tag = l_index;
                fieldTextBox[l_index].Tag = l_index;
                fieldDescriptionLabel[l_index].Tag = l_index;
                fieldRemoveButton[l_index].Tag = l_index;
            }

            RefreshInterpreter();
        }

        private void OnItemsInstrumentsReordered(object sender, RoutedEventArgs e)
        {
            ReorderableListBox l_listBox = (ReorderableListBox)sender;
            int index = l_listBox.SelectedIndex;
            int from = l_listBox.OriginalItemIndex;

            if (index == from)
                return;

            Label originalObject = (Label)l_listBox.Items.GetItemAt(from);

            Instrumentslb.Items.RemoveAt(from);
            Instrumentslb.Items.Insert(index, originalObject);
            Instrumentslb.UpdateLayout();
            instrumentLabel.Insert(index, instrumentLabel[from]);
            instrumentWrapPanel.Insert(index, instrumentWrapPanel[from]);
            instrumentTextBox.Insert(index, instrumentTextBox[from]);
            instrumentDescriptionLabel.Insert(index, instrumentDescriptionLabel[from]);
            instrumentRemoveButton.Insert(index, instrumentRemoveButton[from]);

            instrumentLabel.RemoveAt(from);
            instrumentWrapPanel.RemoveAt(from);
            instrumentTextBox.RemoveAt(from);
            instrumentDescriptionLabel.RemoveAt(from);
            instrumentRemoveButton.RemoveAt(from);

            if (from > index)
            {
                m_excelMgr.m_instrumentList.Insert(index, m_excelMgr.m_instrumentList[from]);
                m_excelMgr.m_instrumentList.RemoveAt(from + 1);
            }
            else
            {
                m_excelMgr.m_instrumentList.Insert(index+1, m_excelMgr.m_instrumentList[from]);
                m_excelMgr.m_instrumentList.RemoveAt(from);
            }
            
            m_excelMgr.RefreshInstrument();
           

            //Update instrument tag
            for (int l_index = from; l_index < m_noOfInstruments; ++l_index)
            {
                instrumentLabel[l_index].Tag = l_index;
                instrumentWrapPanel[l_index].Tag = l_index;
                instrumentTextBox[l_index].Tag = l_index;
                instrumentDescriptionLabel[l_index].Tag = l_index;
                instrumentRemoveButton[l_index].Tag = l_index;
            }

            RefreshInterpreter();
        }

        public void init()
        {
            m_excelMgr.FinalizeExcelManager();
            m_excelMgr.InitActiveCell();
            m_excelMgr.CellSelected += new CExcelManager.CellSelectedEventHandler(m_excelMgr_CellSelected);
            ClearInputWindow();
        }

        //reset 
        public void ClearInputWindow()
        {
            instrumentLabel.Clear();
            instrumentTextBox.Clear();
            instrumentDescriptionLabel.Clear();
            instrumentWrapPanel.Clear();
            instrumentRemoveButton.Clear();

            Instrumentslb.Items.Clear();
            m_noOfInstruments = 0;

            fieldLabel.Clear();
            fieldTextBox.Clear();
            fieldDescriptionLabel.Clear();
            fieldWrapPanel.Clear();
            fieldRemoveButton.Clear();
            Fieldslb.Items.Clear();
            m_noOfFields = 0;

            parameterLabel.Clear();
            parameterWrapPanel.Clear();
            parameterKeyTextBox.Clear();
            parameterSeparatorLabel.Clear();
            parameterValueTextBox.Clear();
            parameterDescriptionLabel.Clear();
            parameterRemoveButton.Clear();
            Parameterslb.Items.Clear();
            m_noOfParameters = 0;

            RefreshInterpreter();
        }

        private void m_excelMgr_CellSelected(CExcelManager a_excelMgr)
        {
            ClearInputWindow();
            if (a_excelMgr != null)
            {
                ReEntranceInput(a_excelMgr.RetreiveInstrument, a_excelMgr.RetreiveField, a_excelMgr.RetreiveParameter);
            }
            
        }

        private void ReEntranceInput(List<String> a_instrumentList, List<String> a_fieldList, List<ExcelManager.CExcelManager.Parameter> a_parametersList)
        {
            // Add instrument
            for (int index = 0; index < a_instrumentList.Count; ++index)
            {
                AddNewInputInstrument(a_instrumentList[index]);
            }
            //m_excelMgr.RefreshInstrument();

            // Add field
            for (int index = 0; index < a_fieldList.Count; ++index)
            {
                AddNewInputField(a_fieldList[index]);
            }
            //m_excelMgr.RefreshField();

            // Add parameters
            for (int index = 0; index < a_parametersList.Count; ++index)
            {
                AddNewInputParametersLine(a_parametersList[index]);
            }
           // m_excelMgr.RefreshParameter();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            //ParseRDataParameter("=Rdata(\"EUR=;JPY=\",\"ASK;BID\",\"FRQ:5S\",,\"CH:In RH:Fd TRANSPOSE:Y\")");
            init();
        }

        public void RefreshInterpreter()
        {
            // Update Interpreter
            // This functions returns TYPE1 of FIELD_DES1 and FIELD_DES2 and TYPE2 of FIELD_DES1, FIELD_DES2 
            // for INS_DES1, INS_DES2 INS_TYPE_OF_1_2 and INS_DES3, INS_DES4 INS_TYPE_OF_3_4

            InterpreterText.Document.Blocks.Clear();
            if (m_excelMgr.RetreiveInstrument.Count != 0 && m_excelMgr.RetreiveField.Count != 0)
            {
                Paragraph l_paragraphInstrumentAndField = new Paragraph();
                l_paragraphInstrumentAndField.TextIndent = 8;
                l_paragraphInstrumentAndField.Inlines.Add("This function returns ");
                Dictionary<string, List<string>> l_groupOfField = m_excelMgr.RetreiveField
                                                                  .GroupBy(x => DatabaseReader.FindFieldType(x))
                                                                  .ToDictionary(x => x.Key, x => x.Select(y => DatabaseReader.FindFieldDescription(y)).ToList());
                
                foreach (KeyValuePair<string, List<string>> l_elementsOfkey in l_groupOfField)
                {
                    l_paragraphInstrumentAndField.Inlines.Add(new Bold(new Run(l_elementsOfkey.Key)));
                    l_paragraphInstrumentAndField.Inlines.Add(" of ");
                    for (int i = 0; i < l_elementsOfkey.Value.Count; ++i)
                    {
                        l_paragraphInstrumentAndField.Inlines.Add(new Bold(new Run(l_elementsOfkey.Value[i])));
                        if (i != l_elementsOfkey.Value.Count - 1)
                            l_paragraphInstrumentAndField.Inlines.Add(new Bold(new Run(", ")));
                    }
                    if (!object.Equals(l_elementsOfkey, l_groupOfField.Last()))
                        l_paragraphInstrumentAndField.Inlines.Add(" and ");
                }

                l_paragraphInstrumentAndField.Inlines.Add(" for ");
                Dictionary<string, List<string>> l_groupOfInstrument = m_excelMgr.RetreiveInstrument
                                                              .GroupBy(x => DatabaseReader.FindInstrumentType(x))
                                                              .ToDictionary(x => x.Key, x => x.Select(y => DatabaseReader.FindInstrumentDescription(y)).ToList());

                foreach (KeyValuePair<string, List<string>> l_elementsOfkey in l_groupOfInstrument)
                {
                    for (int i = 0; i < l_elementsOfkey.Value.Count; ++i)
                    {
                        l_paragraphInstrumentAndField.Inlines.Add(new Bold(new Run(l_elementsOfkey.Value[i])));
                        if (i != l_elementsOfkey.Value.Count - 1)
                            l_paragraphInstrumentAndField.Inlines.Add(new Bold(new Run(", ")));
                    }
                    l_paragraphInstrumentAndField.Inlines.Add(new Bold(new Run(" " + l_elementsOfkey.Key)));

                    if (!object.Equals(l_elementsOfkey, l_groupOfInstrument.Last()))
                        l_paragraphInstrumentAndField.Inlines.Add(" and ");
                }

                InterpreterText.Document.Blocks.Add(l_paragraphInstrumentAndField);

                Paragraph l_paragraphParams = new Paragraph();
                if (m_excelMgr.RetreiveParameter.Count != 0)
                {
                    foreach (CExcelManager.Parameter l_each in m_excelMgr.RetreiveParameter)
                    {
                        l_paragraphParams.Inlines.Add("- ");
                        if (l_each.Name == "START" || l_each.Name == "END")
                        {
                            l_paragraphParams.Inlines.Add(DatabaseReader.FindParameterDescription(l_each.Name).Replace("{0}", l_each.Value));
                        }
                        else
                        {
                            string l_valueDescription = DatabaseReader.FindValueDescription(l_each.Name, l_each.Value);
                            l_paragraphParams.Inlines.Add(DatabaseReader.FindParameterDescription(l_each.Name).Replace("{0}", l_valueDescription));
                        }

                        l_paragraphParams.Inlines.Add("\n");
                    }
                }
                InterpreterText.Document.Blocks.Add(l_paragraphParams);
            }
            else
            {
                InterpreterText.AppendText("Please add at least one instrument and one field :)");
            }
        }

        private void AddNewInputInstrument(String a_instrument)
        {
            Label label = new Label();
            label.Width = inputWidth;
            label.Height = inputHeight;
            label.Padding = new Thickness(2);

            WrapPanel labelPanel = new WrapPanel();
            labelPanel.Orientation = Orientation.Horizontal;
            
            AutoSuggestControl.AutoSuggestBox instrumentBox = new AutoSuggestControl.AutoSuggestBox();
            instrumentBox.ListItemSelected += new RoutedPropertyChangedEventHandler<object>(instrumentBox_ListItemSelected);
            instrumentBox.InitInstrumentAutoSuggest();

            instrumentBox.Width = textboxWidth;
            instrumentBox.Height = textboxHeight;

            Label instrumentDescLabel = new Label();
            //instrumentDescLabel.Width = descriptionWidth;
            instrumentDescLabel.Height = textboxHeight;
            instrumentDescLabel.FontSize = 13;
            instrumentDescLabel.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            instrumentDescLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x40, 0x3e, 0x3c));

            Label space = new Label();
            space.Width = 10;
            space.Height = textboxHeight;

            Button removeButton = new Button();
            removeButton.Content = "-";
            removeButton.Width = removeButtonWidth;
            removeButton.Height = removeButtonHeight;
            removeButton.Background = Brushes.White;
            removeButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xcb, 0x8b));
            removeButton.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x52, 0x00));

            labelPanel.Children.Add(removeButton);
            labelPanel.Children.Add(space);
            labelPanel.Children.Add(instrumentBox);
            label.Content = labelPanel;

            label.Tag = m_noOfInstruments;
            labelPanel.Tag = m_noOfInstruments;
            instrumentBox.Tag = m_noOfInstruments;
            instrumentDescLabel.Tag = m_noOfInstruments;
            removeButton.Tag = m_noOfInstruments;
            removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Instruments_MouseDownEvent);

            instrumentLabel.Add(label);
            instrumentWrapPanel.Add(labelPanel);
            instrumentTextBox.Add(instrumentBox);
            instrumentDescriptionLabel.Add(instrumentDescLabel);
            instrumentRemoveButton.Add(removeButton);

            if (a_instrument.Length > 0)
            {
                instrumentBox.Text = a_instrument;
                instrumentBox.IsEnabled = false;
                String description = DatabaseReader.FindInstrumentDescription(a_instrument);
                instrumentDescLabel.Content = description;
                labelPanel.Children.Add(instrumentDescLabel);

                //m_excelMgr.InstrumentList = a_instrument;
            }

            Instrumentslb.Items.Add(label);
            
            ++m_noOfInstruments;
        }

        private void AddNewInputField(string a_field)
        {
            Label label = new Label();
            label.Width = inputWidth;
            label.Height = inputHeight;
            label.Padding = new Thickness(2);

            WrapPanel labelPanel = new WrapPanel();
            labelPanel.Orientation = Orientation.Horizontal;

            AutoSuggestControl.AutoSuggestBox fieldBox = new AutoSuggestControl.AutoSuggestBox();
            fieldBox.ListItemSelected += new RoutedPropertyChangedEventHandler<object>(fieldBox_ListItemSelected);
            fieldBox.InitFieldAutoSuggest();


            Label fieldDescLabel = new Label();
            //fieldDescLabel.Width = descriptionWidth;
            fieldDescLabel.Height = textboxHeight;
            fieldDescLabel.FontSize = 13;
            fieldDescLabel.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            fieldDescLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x40, 0x3e, 0x3c));

            fieldBox.Width = textboxWidth;
            fieldBox.Height = textboxHeight;

            Label space = new Label();
            space.Width = 10;
            space.Height = textboxHeight;

            Button removeButton = new Button();
            removeButton.Content = "-";
            removeButton.Width = removeButtonWidth;
            removeButton.Height = removeButtonHeight;
            removeButton.Background = Brushes.White;
            removeButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xcb, 0x8b));
            removeButton.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x52, 0x00));

            labelPanel.Children.Add(removeButton);
            labelPanel.Children.Add(space);
            labelPanel.Children.Add(fieldBox);
            label.Content = labelPanel;

            label.Tag = m_noOfFields;
            labelPanel.Tag = m_noOfFields;
            fieldBox.Tag = m_noOfFields;
            fieldDescLabel.Tag = m_noOfFields;
            removeButton.Tag = m_noOfFields;
            removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Fields_MouseDownEvent);

            fieldLabel.Add(label);
            fieldWrapPanel.Add(labelPanel);
            fieldTextBox.Add(fieldBox);
            fieldDescriptionLabel.Add(fieldDescLabel);
            fieldRemoveButton.Add(removeButton);

            Grid.SetColumn(label, 0);
            Grid.SetRow(label, m_noOfFields);

            if (a_field.Length > 0)
            {
                fieldBox.Text = a_field;
                fieldBox.IsEnabled = false;
                String description = DatabaseReader.FindFieldDescription(a_field);
                fieldDescLabel.Content = description;
                labelPanel.Children.Add(fieldDescLabel);

                //m_excelMgr.FieldList = a_field;
            }

            Fieldslb.Items.Add(label);
            ++m_noOfFields;
        }

        private void AddNewInputParametersLine(ExcelManager.CExcelManager.Parameter a_parameters)
        {
            Label label = new Label();
            label.Width = inputWidth;
            label.Height = inputHeight;
            label.Padding = new Thickness(2);

            WrapPanel labelPanel = new WrapPanel();
            labelPanel.Orientation = Orientation.Horizontal;
            labelPanel.Width = inputWidth;
            labelPanel.Height = inputHeight;

            AutoSuggestControl.AutoSuggestBox parameterKeyBox = new AutoSuggestControl.AutoSuggestBox();
            parameterKeyBox.ListItemSelected += new RoutedPropertyChangedEventHandler<object>(parameterKeyBox_ListItemSelected);
            parameterKeyBox.InitOptionAutoSuggest();
            parameterKeyBox.Width = textboxParameterKeyWidth;
            parameterKeyBox.Height = textboxHeight;

            Label labelSeparator = new Label();
            labelSeparator.Width = labelParameterSaparatorWidth;
            labelSeparator.Height = textboxHeight;
            labelSeparator.Content = ":";

            AutoSuggestControl.AutoSuggestBox parameterValueBox = new AutoSuggestControl.AutoSuggestBox();
            parameterValueBox.ListItemSelected += new RoutedPropertyChangedEventHandler<object>(parameterValueBox_ListItemSelected);
            parameterValueBox.Width = textboxParameterValueWidth;
            parameterValueBox.Height = textboxHeight;

            Label labelDescription = new Label();
            //labelDescription.Width = labelParameterDescriptionWidth;
            labelDescription.Height = textboxHeight;
            labelDescription.FontSize = 13;
            labelDescription.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            labelDescription.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x40, 0x3e, 0x3c));

            Label space = new Label();
            space.Width = 10;
            space.Height = textboxHeight;

            Button removeButton = new Button();
            removeButton.Content = "-";
            removeButton.Width = removeButtonWidth;
            removeButton.Height = removeButtonHeight;
            removeButton.Background = Brushes.White;
            removeButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xcb, 0x8b));
            removeButton.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x52, 0x00));



            labelPanel.Children.Add(removeButton);
            labelPanel.Children.Add(space);
            labelPanel.Children.Add(parameterKeyBox);
            label.Content = labelPanel;

            label.Tag = m_noOfParameters;
            labelPanel.Tag = m_noOfInstruments;
            parameterKeyBox.Tag = m_noOfParameters;
            labelSeparator.Tag = m_noOfParameters;
            parameterValueBox.Tag = m_noOfParameters;
            labelDescription.Tag = m_noOfParameters;
            removeButton.Tag = m_noOfParameters;

            removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Parameters_MouseDownEvent);
            //parameterKeyBox.PreviewKeyUp += new KeyEventHandler(keyTextbox_Parameters_KeyInput);
            
            parameterLabel.Add(label);
            parameterWrapPanel.Add(labelPanel);
            parameterKeyTextBox.Add(parameterKeyBox);
            parameterSeparatorLabel.Add(labelSeparator);
            parameterValueTextBox.Add(parameterValueBox);
            parameterDescriptionLabel.Add(labelDescription);
            parameterRemoveButton.Add(removeButton);

            Grid.SetColumn(label, 0);
            Grid.SetRow(label, m_noOfParameters);

            if (a_parameters != null)
            {
                parameterKeyBox.Text = a_parameters.Name;
                labelPanel.Children.Add(labelSeparator);

                parameterValueBox.Text = a_parameters.Value;
                labelPanel.Children.Add(parameterValueBox);

                //string description = FindParameterDescription()
            }

            Parameterslb.Items.Add(label);
            ++m_noOfParameters;
        }

        private void InstrumentsAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewInputInstrument("");
            //InstrumentsAddButton.Focus();
            //InstrumentsAddButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
            //Keyboard.Focus(instrumentTextBox[instrumentTextBox.Count - 1]);       
        }

        private void FieldsAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewInputField("");
        }

        private void ParametersAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewInputParametersLine(null);
        }

        private void removeButton_Instruments_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String l_buttonName = ((Button)sender).Tag.ToString();
            int l_instrumentIndex;
            Int32.TryParse(l_buttonName, out l_instrumentIndex); //InstrumentRemoveButton{index}

            instrumentLabel.RemoveAt(l_instrumentIndex);
            instrumentWrapPanel.RemoveAt(l_instrumentIndex);
            instrumentTextBox.RemoveAt(l_instrumentIndex);
            instrumentDescriptionLabel.RemoveAt(l_instrumentIndex);
            instrumentRemoveButton.RemoveAt(l_instrumentIndex);

            Instrumentslb.Items.RemoveAt(l_instrumentIndex);


            m_excelMgr.ResetInstrument(l_instrumentIndex);
            m_excelMgr.RefreshInstrument();
            --m_noOfInstruments;

            //Update instrument tag
            for (int l_index = l_instrumentIndex; l_index < m_noOfInstruments; ++l_index)
            {
                instrumentLabel[l_index].Tag = l_index;
                instrumentWrapPanel[l_index].Tag = l_index;
                instrumentTextBox[l_index].Tag = l_index;
                instrumentDescriptionLabel[l_index].Tag = l_index;
                instrumentRemoveButton[l_index].Tag = l_index;
            }

            RefreshInterpreter();
        }

        private void removeButton_Fields_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String l_buttonName = ((Button)sender).Tag.ToString();
            int l_fieldIndex;
            Int32.TryParse(l_buttonName, out l_fieldIndex); //FieldRemoveButton{index}

            fieldLabel.RemoveAt(l_fieldIndex);
            fieldWrapPanel.RemoveAt(l_fieldIndex);
            fieldTextBox.RemoveAt(l_fieldIndex);
            fieldDescriptionLabel.RemoveAt(l_fieldIndex);
            fieldRemoveButton.RemoveAt(l_fieldIndex);

            Fieldslb.Items.RemoveAt(l_fieldIndex);

            m_excelMgr.ResetField(l_fieldIndex);
            m_excelMgr.RefreshField();
            --m_noOfFields;

            //Update field tag
            for (int l_index = l_fieldIndex; l_index < m_noOfFields; ++l_index)
            {
                fieldLabel[l_index].Tag = l_index;
                fieldWrapPanel[l_index].Tag = l_index;
                fieldTextBox[l_index].Tag = l_index;
                fieldDescriptionLabel[l_index].Tag = l_index;
                fieldRemoveButton[l_index].Tag = l_index;
            }

            RefreshInterpreter();
        }

        private void removeButton_Parameters_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String l_buttonName = ((Button)sender).Tag.ToString();
            int l_parameterIndex;
            Int32.TryParse(l_buttonName, out l_parameterIndex); //parameterRemoveButton{index}

            parameterLabel.RemoveAt(l_parameterIndex);
            parameterWrapPanel.RemoveAt(l_parameterIndex);
            parameterKeyTextBox.RemoveAt(l_parameterIndex);
            parameterSeparatorLabel.RemoveAt(l_parameterIndex);
            parameterValueTextBox.RemoveAt(l_parameterIndex);
            parameterDescriptionLabel.RemoveAt(l_parameterIndex);
            parameterRemoveButton.RemoveAt(l_parameterIndex);
            parameterDescription.RemoveAt(l_parameterIndex);


            Parameterslb.Items.RemoveAt(l_parameterIndex);

            m_excelMgr.ResetParameter(l_parameterIndex);
            m_excelMgr.RefreshParameter();
            --m_noOfParameters;

            for (int l_index = l_parameterIndex; l_index < m_noOfParameters; ++l_index)
            {
                parameterLabel[l_index].Tag = l_index;
                parameterWrapPanel[l_index].Tag = l_index;
                parameterKeyTextBox[l_index].Tag = l_index;
                parameterSeparatorLabel[l_index].Tag = l_index;
                parameterValueTextBox[l_index].Tag = l_index;
                parameterDescriptionLabel[l_index].Tag = l_index;
                parameterRemoveButton[l_index].Tag = l_index;
            }

            RefreshInterpreter();
        }

        void instrumentBox_ListItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            String l_textboxName = ((AutoSuggestControl.AutoSuggestBox)sender).Tag.ToString();
            int l_instrumentIndex;
            Int32.TryParse(l_textboxName, out l_instrumentIndex); //instrument{index}
            //m_excelMgr.ResetInstrument(l_instrumentIndex);

            m_excelMgr.InstrumentList = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Symbol;
            m_excelMgr.RefreshInstrument();

            // Add Description
            instrumentDescriptionLabel[l_instrumentIndex].Content = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Title;
            instrumentWrapPanel[l_instrumentIndex].Children.Add(instrumentDescriptionLabel[l_instrumentIndex]);
            RefreshInterpreter();

            ((AutoSuggestControl.AutoSuggestBox)sender).IsEnabled = false;
        }

        void fieldBox_ListItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            String l_textboxName = ((AutoSuggestControl.AutoSuggestBox)sender).Tag.ToString();
            int l_fieldIndex;
            Int32.TryParse(l_textboxName, out l_fieldIndex); //field{index}
            //m_excelMgr.ResetField(l_fieldIndex);

            m_excelMgr.FieldList = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Symbol;
            m_excelMgr.RefreshField();
      
            // Add Description
            fieldDescriptionLabel[l_fieldIndex].Content = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Title;
            fieldWrapPanel[l_fieldIndex].Children.Add(fieldDescriptionLabel[l_fieldIndex]);
            RefreshInterpreter();

            ((AutoSuggestControl.AutoSuggestBox)sender).IsEnabled = false;
        }

        void parameterKeyBox_ListItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            String parameterKey = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Symbol;
            // TODO Add to parameter list;
            
            String l_textboxName = ((AutoSuggestControl.AutoSuggestBox)sender).Tag.ToString();
            int l_parameterIndex;
            Int32.TryParse(l_textboxName, out l_parameterIndex); //paramter{index}

            m_excelMgr.AddParameter(parameterKey, ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Subtitle);
            
            if (l_parameterIndex >= m_noOfParameters - 1)
            {
                parameterDescription.Add(((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Title);
            }
            else
            {
                parameterDescription[l_parameterIndex] = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Title;
            }

            parameterWrapPanel[l_parameterIndex].Children.Add(parameterSeparatorLabel[l_parameterIndex]);
            parameterWrapPanel[l_parameterIndex].Children.Add(parameterValueTextBox[l_parameterIndex]);

            parameterValueTextBox[l_parameterIndex].InitOptionValueAutoSuggest(parameterKey);

            ((AutoSuggestControl.AutoSuggestBox)sender).IsEnabled = false;
        }

        void parameterValueBox_ListItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // TODO Add to parameter list;
            String l_parameterValue = ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Symbol;
            
            String l_textboxName = ((AutoSuggestControl.AutoSuggestBox)sender).Tag.ToString();
            int l_parameterIndex;
            Int32.TryParse(l_textboxName, out l_parameterIndex); //paramter{index}

            m_excelMgr.AddParameterValue(l_parameterValue);
            m_excelMgr.RefreshParameter();

            String description = parameterDescription[l_parameterIndex];
            description = description.Replace("{0}", ((AutoSuggestControl.Model.AutoSuggestModel.DisplayInstrumentResult)e.NewValue).Title);
            // Add Tooltips

            parameterDescriptionLabel[l_parameterIndex].Content = description;
            parameterWrapPanel[l_parameterIndex].Children.Add(parameterDescriptionLabel[l_parameterIndex]);
            RefreshInterpreter();

            ((AutoSuggestControl.AutoSuggestBox)sender).IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            m_excelMgr.RemoveExcelApp();
        }
    }
}
