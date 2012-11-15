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

using TinkerBell;
using System.Timers;

namespace TinkerBell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWillHearMyChilds
    {
        private List<IWillHearMyParent> m_childsWhoAreListeningToMe;
        private CParameters m_parameter;

        enum Types { Insturments, Fields, Parameters };
        int noOfInstruments = 0;
        int noOfFields = 0;
        int noOfParameters = 0;

        const double inputWidth = 280;
        const double inputHeight = 30;
        const double textboxHeight = 24;
        const double removeButtonWidth = textboxHeight;

        // Instrument & Field
        List<Label> instrumentLabel;
        List<Label> fieldLabel;
        List<AutoSuggestControl.AutoSuggestBox> instrumentTextBox;
        List<AutoSuggestControl.AutoSuggestBox> fieldTextBox;
        List<Button> instrumentRemoveButton;
        List<Button> fieldRemoveButton;

        const double textboxWidth = 220;

        // Parameters
        List<Label> parameterLabel;
        List<WrapPanel> parameterWrapPanel;
        List<AutoSuggestControl.AutoSuggestBox> parameterKeyTextBox;
        List<Label> parameterSeparatorLabel;
        List<AutoSuggestControl.AutoSuggestBox> parameterValueTextBox;
        List<Label> parameterDescriptionLabel;
        List<Button> parameterRemoveButton;

        const double textboxParameterKeyWidth = 50;
        const double labelParameterSaparatorWidth = 12;
        const double textboxParameterValueWidth = 50;
        const double labelParameterDescriptionWidth = 150;

        public MainWindow()
        {
            InitializeComponent();
            m_childsWhoAreListeningToMe = new List<IWillHearMyParent>();
            m_parameter = new CParameters();

            // Instrument & Fields
            instrumentLabel = new List<Label>();
            fieldLabel = new List<Label>();

            instrumentTextBox = new List<AutoSuggestControl.AutoSuggestBox>();
            fieldTextBox = new List<AutoSuggestControl.AutoSuggestBox>();

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

            test();
        }

        public void test()
        {
            m_parameter.AddInstrument("EUR=");
            m_parameter.AddInstrument("JPY=");
            m_parameter.AddInstrument("TRI.N");
            m_parameter.AddInstrument("MSFT.O");
            m_parameter.AddField("BID");
            m_parameter.AddField("RI.ID.RIC");
            m_parameter.AddParameter("CH", "In");
            m_parameter.AddParameter("RH", "Fd");
            
            foreach (string l_each in m_parameter.Instruments)
            {
                Console.WriteLine(l_each);
            }
            foreach (string l_each in m_parameter.Fields)
            {
                Console.WriteLine(l_each);
            }
            foreach (KeyValuePair<string, List<string>> l_each in m_parameter.Parameters)
            {
                Console.WriteLine(l_each.Key);
                foreach (string l_eachValue in l_each.Value)
                {
                    Console.Write(l_eachValue + " ");
                }
            }
            this.OnMyChildToldsMeThatHeChangesParameters();
        }

        private void TriggerEvent_Click(object sender, RoutedEventArgs e)
        {
            this.OnMyChildToldsMeThatHeChangesParameters();
        }

        public void OnMyChildToldsMeThatHeChangesParameters()
        {
            // Tell childs
            foreach (IWillHearMyParent l_child in m_childsWhoAreListeningToMe)
            {
                l_child.OnParametersChange();
            }

            // Update Interpreter
            // This functions returns TYPE1 of FIELD_DES1 and FIELD_DES2 and TYPE2 of FIELD_DES1, FIELD_DES2 
            // for INS_DES1, INS_DES2 INS_TYPE_OF_1_2 and INS_DES3, INS_DES4 INS_TYPE_OF_3_4
            if (m_parameter.Instruments.Count != 0 && m_parameter.Fields.Count != 0)
            {

                string l_interpretedWord = "This function returns ";
                Dictionary<string, List<string>> l_groupOfField = m_parameter.Fields
                                                                  .GroupBy(x => DatabaseReader.FindFieldType(x))
                                                                  .ToDictionary(x => x.Key, x => x.Select(y => DatabaseReader.FindFieldDescription(y)).ToList());
                foreach (KeyValuePair<string, List<string>> l_elementsOfkey in l_groupOfField)
                {
                    l_interpretedWord += l_elementsOfkey.Key + " of ";
                    for (int i = 0; i < l_elementsOfkey.Value.Count; ++i)
                    {
                        l_interpretedWord += l_elementsOfkey.Value[i] + ", ";
                    }
                    l_interpretedWord = l_interpretedWord.Remove(l_interpretedWord.Length - ", ".Length);
                    l_interpretedWord += " and ";
                }
                if (l_interpretedWord.LastIndexOf(" and ") > 0)
                    l_interpretedWord = l_interpretedWord.Remove(l_interpretedWord.Length - " and ".Length);

                l_interpretedWord += " for ";
                Dictionary<string, List<string>> l_groupOfInstrument = m_parameter.Instruments
                                                              .GroupBy(x => DatabaseReader.FindInstrumentType(x))
                                                              .ToDictionary(x => x.Key, x => x.Select(y => DatabaseReader.FindInstrumentDescription(y)).ToList());
                foreach (KeyValuePair<string, List<string>> l_elementsOfkey in l_groupOfInstrument)
                {
                    for (int i = 0; i < l_elementsOfkey.Value.Count; ++i)
                    {
                        l_interpretedWord += l_elementsOfkey.Value[i] + ", ";
                    }
                    l_interpretedWord = l_interpretedWord.Remove(l_interpretedWord.Length - ", ".Length);
                    l_interpretedWord += " " + l_elementsOfkey.Key + " and ";
                }
                if (l_interpretedWord.LastIndexOf(" and ") > 0)
                    l_interpretedWord = l_interpretedWord.Remove(l_interpretedWord.Length - " and ".Length);

                l_interpretedWord += ".\nIt will ";

                if (m_parameter.Parameters.Count != 0)
                {
                    foreach (KeyValuePair<string, List<string>> l_each in m_parameter.Parameters)
                    {
                        foreach (string l_parameterValue in l_each.Value)
                        {
                            // Need to fill {0} with the description of a value of this parameter
                            // for example, display {0} as Column Header. {0} is In -> instrument
                            l_interpretedWord += DatabaseReader.FindParameterDescription(l_each.Key) + " and ";
                        }
                        l_interpretedWord = l_interpretedWord.Remove(l_interpretedWord.Length - " and ".Length);
                        l_interpretedWord += "\n";
                    }
                }
                InterpreterText.Text = l_interpretedWord;
            }
            else
            {
                InterpreterText.Text = "Please add Instrument and Fields :)";
            }
        }

        private void AddNewInputLine(Types type)
        {
            Label label = new Label();
            label.Width = inputWidth;
            label.Height = inputHeight;
            label.Padding = new Thickness(2);

            WrapPanel labelPanel = new WrapPanel();
            labelPanel.Orientation = Orientation.Horizontal;
            AutoSuggestControl.AutoSuggestBox textbox = new AutoSuggestControl.AutoSuggestBox();
            switch (type)
            {
                case Types.Insturments:
                    textbox.InitInstrumentAutoSuggest();
                    break;
                case Types.Fields:
                    textbox.InitFieldAutoSuggest();
                    break;
                default:
                    break;
            }

            textbox.Width = textboxWidth;
            textbox.Height = textboxHeight;

            Button removeButton = new Button();
            removeButton.Content = "-";
            removeButton.Width = removeButtonWidth;
            removeButton.Height = textboxHeight;

            labelPanel.Children.Add(textbox);
            labelPanel.Children.Add(removeButton);
            label.Content = labelPanel;
            
            if (type == Types.Insturments)
            {
                label.Tag = "InstrumentLabel" + noOfInstruments;
                textbox.Tag = "InstrumentTextbox" + noOfInstruments;
                removeButton.Tag = "InstrumentRemoveButton" + noOfInstruments;
                removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Instruments_MouseDownEvent);
                
                // To remove
                //textbox.Text = "" + noOfInstruments;
                //

                instrumentLabel.Add(label);
                instrumentTextBox.Add(textbox);
                instrumentRemoveButton.Add(removeButton);

                while (InstrumentsGrid.RowDefinitions.Count() > noOfInstruments)
                {
                    InstrumentsGrid.RowDefinitions.RemoveAt(InstrumentsGrid.RowDefinitions.Count() - 1);
                }

                Grid.SetColumn(label, 0);
                Grid.SetRow(label, noOfInstruments);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                InstrumentsGrid.RowDefinitions.Insert(noOfInstruments, rowDef);

                InstrumentsGrid.Children.Add(label);
                ++noOfInstruments;
            }
            if (type == Types.Fields)
            {
                label.Tag = "FieldLabel" + noOfFields;
                textbox.Tag = "FieldTextbox" + noOfFields;
                removeButton.Tag = "FieldRemoveButton" + noOfFields;
                removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Fields_MouseDownEvent);

                fieldLabel.Add(label);
                fieldTextBox.Add(textbox);
                fieldRemoveButton.Add(removeButton);

                if (FieldsGrid.RowDefinitions.Count() <= noOfFields)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    FieldsGrid.RowDefinitions.Add(rowDef);
                }
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, noOfFields);

                FieldsGrid.Children.Add(label);
                ++noOfFields;
            }
           
        }

        private void AddNewInputParametersLine()
        {
            Label label = new Label();
            label.Width = inputWidth;
            label.Height = inputHeight;
            label.Padding = new Thickness(2);

            WrapPanel labelPanel = new WrapPanel();
            labelPanel.Orientation = Orientation.Horizontal;
            labelPanel.Width = inputWidth;
            labelPanel.Height = inputHeight;

            AutoSuggestControl.AutoSuggestBox textboxKey = new AutoSuggestControl.AutoSuggestBox();
            textboxKey.InitOptionAutoSuggest();
            textboxKey.Width = textboxParameterKeyWidth;
            textboxKey.Height = textboxHeight;

            Label labelSeparator = new Label();
            labelSeparator.Width = labelParameterSaparatorWidth;
            labelSeparator.Height = textboxHeight;
            labelSeparator.Content = ":";

            AutoSuggestControl.AutoSuggestBox textboxValue = new AutoSuggestControl.AutoSuggestBox();
            //textboxValue.InitOptionValueAutoSuggest();
            textboxValue.Width = textboxParameterValueWidth;
            textboxValue.Height = textboxHeight;

            Label labelDescription = new Label();
            labelDescription.Width = labelParameterDescriptionWidth;
            labelDescription.Height = textboxHeight;

            Button removeButton = new Button();
            removeButton.Content = "-";
            removeButton.Width = removeButtonWidth;
            removeButton.Height = textboxHeight;

            labelPanel.Children.Add(textboxKey);
            labelPanel.Children.Add(removeButton);
            label.Content = labelPanel;

            label.Tag = "ParameterLabel" + noOfParameters;
            labelPanel.Tag = "ParameterWrapPanel" + noOfInstruments;
            textboxKey.Tag = "ParameterKeyTextbox" + noOfParameters;
            labelSeparator.Tag = "ParameterSeparatorLabel" + noOfParameters;
            textboxValue.Tag = "ParameterValueTextbox" + noOfParameters;
            labelDescription.Tag = "ParamterDescriptionLabel" + noOfParameters;
            removeButton.Tag = "ParameterRemoveButton" + noOfParameters;

            removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Parameters_MouseDownEvent);
            textboxKey.PreviewKeyUp += new KeyEventHandler(keyTextbox_Parameters_KeyInput);
            
            parameterLabel.Add(label);
            parameterWrapPanel.Add(labelPanel);
            parameterKeyTextBox.Add(textboxKey);
            parameterSeparatorLabel.Add(labelSeparator);
            parameterValueTextBox.Add(textboxValue);
            parameterDescriptionLabel.Add(labelDescription);
            parameterRemoveButton.Add(removeButton);

            while (ParametersGrid.RowDefinitions.Count() > noOfParameters)
            {
                ParametersGrid.RowDefinitions.RemoveAt(noOfParameters - 1);
            }

            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            ParametersGrid.RowDefinitions.Add(rowDef);

            Grid.SetColumn(label, 0);
            Grid.SetRow(label, noOfParameters);

            ParametersGrid.Children.Add(label);
            ++noOfParameters;
        }

        private void InstrumentsAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewInputLine(Types.Insturments);
            //InstrumentsAddButton.Focus();
            //InstrumentsAddButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
            //Keyboard.Focus(instrumentTextBox[instrumentTextBox.Count - 1]);       
        }

        private void FieldsAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewInputLine(Types.Fields);
        }

        private void ParametersAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewInputParametersLine();
        }

        private void removeButton_Instruments_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String buttonName = ((Button)sender).Tag.ToString();
            int instrumentIndex;
            bool result = Int32.TryParse(buttonName.Substring(22), out instrumentIndex); //InstrumentRemoveButton{index}

            //instrumentLabel[instrumentIndex].Tag = null;
            //instrumentTextBox[instrumentIndex].Tag = null;
            //instrumentRemoveButton[instrumentIndex].Tag = null;

            instrumentLabel.RemoveAt(instrumentIndex);
            instrumentTextBox.RemoveAt(instrumentIndex);
            instrumentRemoveButton.RemoveAt(instrumentIndex);

            --noOfInstruments;

            for (int i = instrumentIndex; i < noOfInstruments; ++i)
            {
                instrumentLabel[i].Tag = "InstrumentLabel" + i;
                instrumentTextBox[i].Tag = "InstrumentTextbox" + i;
                instrumentRemoveButton[i].Tag = "InstrumentRemoveButton" + i;
            }

            //if (instrumentIndex == noOfInstruments)
            //{
            InstrumentsGrid.Children.RemoveAt(instrumentIndex);
            //}
            //InstrumentsGrid.RowDefinitions.RemoveAt(instrumentIndex);
        }

        private void removeButton_Fields_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String buttonName = ((Button)sender).Tag.ToString();
            int fieldIndex;
            bool result = Int32.TryParse(buttonName.Substring(17), out fieldIndex); //FieldRemoveButton{index}

            fieldLabel[fieldIndex].Tag = null;
            fieldTextBox[fieldIndex].Tag = null;
            fieldRemoveButton[fieldIndex].Tag = null;

            fieldLabel.RemoveAt(fieldIndex);
            fieldTextBox.RemoveAt(fieldIndex);
            fieldRemoveButton.RemoveAt(fieldIndex);

            --noOfFields;

            for (int i = fieldIndex; i < noOfFields; ++i)
            {
                fieldLabel[i].Name = "FieldLabel" + i;
                fieldTextBox[i].Name = "FieldTextbox" + i;
                fieldRemoveButton[i].Name = "FieldRemoveButton" + i;
            }

            FieldsGrid.Children.RemoveAt(fieldIndex);
            //FieldsGrid.RowDefinitions.RemoveAt(fieldIndex);
        }

        private void removeButton_Parameters_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String buttonName = ((Button)sender).Tag.ToString();
            int parameterIndex;
            bool result = Int32.TryParse(buttonName.Substring(21), out parameterIndex); //ParameterRemoveButton{index}

            parameterLabel[parameterIndex].Tag = null;
            parameterWrapPanel[parameterIndex].Tag = null;
            parameterKeyTextBox[parameterIndex].Tag = null;
            parameterSeparatorLabel[parameterIndex].Tag = null;
            parameterValueTextBox[parameterIndex].Tag = null;
            parameterDescriptionLabel[parameterIndex].Tag = null;
            parameterRemoveButton[parameterIndex].Tag = null;

            parameterLabel.RemoveAt(parameterIndex);
            parameterWrapPanel.RemoveAt(parameterIndex);
            parameterKeyTextBox.RemoveAt(parameterIndex);
            parameterSeparatorLabel.RemoveAt(parameterIndex);
            parameterValueTextBox.RemoveAt(parameterIndex);
            parameterDescriptionLabel.RemoveAt(parameterIndex);
            parameterRemoveButton.RemoveAt(parameterIndex);

            --noOfParameters;

            for (int i = parameterIndex; i < noOfParameters; ++i)
            {
                parameterLabel[i].Tag = "ParameterLabel" + i;
                parameterWrapPanel[i].Tag = "ParameterWrapPanel" + i;
                parameterKeyTextBox[i].Tag = "ParameterKeyTextbox" + i;
                parameterSeparatorLabel[i].Tag = "ParameterSeparatorLabel" + i;
                parameterValueTextBox[i].Tag = "ParameterValueTextbox" + i;
                parameterDescriptionLabel[i].Tag = "ParamterDescriptionLabel" + i;
                parameterRemoveButton[i].Tag = "ParameterRemoveButton" + i;
            }

            ParametersGrid.Children.RemoveAt(parameterIndex);
            //ParametersGrid.RowDefinitions.RemoveAt(parameterIndex);
        }

        private void keyTextbox_Parameters_KeyInput(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                String textboxName = ((TextBox)sender).Tag.ToString();
                int parameterIndex;
                bool result = Int32.TryParse(textboxName.Substring(19), out parameterIndex); //ParameterKeyTextbox{index}

                parameterWrapPanel[parameterIndex].Children.RemoveAt(1);
                parameterWrapPanel[parameterIndex].Children.Add(parameterSeparatorLabel[parameterIndex]);
                parameterWrapPanel[parameterIndex].Children.Add(parameterValueTextBox[parameterIndex]);
                parameterWrapPanel[parameterIndex].Children.Add(parameterRemoveButton[parameterIndex]);
            }
        }



    }
}
