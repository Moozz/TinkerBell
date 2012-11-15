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
using System.Collections.Generic;
using PLAutoSuggestBox;

namespace TinkerBellInput
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum Types {Insturments, Fields, Parameters };

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
        List<TextBox> instrumentTextBox;
        List<TextBox> fieldTextBox;
        List<Button> instrumentRemoveButton;
        List<Button> fieldRemoveButton;
       
        const double textboxWidth = 220;

        // Parameters
        List<Label> parameterLabel;
        List<WrapPanel> parameterWrapPanel;
        List<TextBox> parameterKeyTextBox;
        List<Label> parameterSeparatorLabel;
        List<TextBox> parameterValueTextBox;
        List<Label> parameterDescriptionLabel;
        List<Button> parameterRemoveButton;

        const double textboxParameterKeyWidth = 50;
        const double labelParameterSaparatorWidth = 12;
        const double textboxParameterValueWidth = 50;
        const double labelParameterDescriptionWidth = 150;

        public MainWindow()
        {
            InitializeComponent();
            
            // Instrument & Fields
            instrumentLabel = new List<Label>();
            fieldLabel = new List<Label>();

            instrumentTextBox = new List<TextBox>();
            fieldTextBox = new List<TextBox>();

            instrumentRemoveButton = new List<Button>();
            fieldRemoveButton = new List<Button>();

            // Paramters
            parameterLabel = new List<Label>();
            parameterWrapPanel = new List<WrapPanel>();
            parameterKeyTextBox = new List<TextBox>();
            parameterSeparatorLabel = new List<Label>();
            parameterValueTextBox = new List<TextBox>();
            parameterDescriptionLabel = new List<Label>();
            parameterRemoveButton = new List<Button>();

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
            switch(type)
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
                //instrumentTextBox.Add(textbox);
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
                //fieldTextBox.Add(textbox);
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

            TextBox textboxKey = new TextBox();
            textboxKey.Width = textboxParameterKeyWidth;
            textboxKey.Height = textboxHeight;

            Label labelSeparator = new Label();
            labelSeparator.Width = labelParameterSaparatorWidth;
            labelSeparator.Height = textboxHeight;
            labelSeparator.Content = ":";

            TextBox textboxValue = new TextBox();
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

            for (int i = instrumentIndex; i < noOfInstruments; ++i )
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

            for (int i = parameterIndex; i < noOfParameters; ++i )
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
