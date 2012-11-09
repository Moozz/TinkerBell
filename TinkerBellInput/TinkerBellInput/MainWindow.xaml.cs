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

        const double textboxParameterKeyWidth = 60;
        const double labelParameterSaparatorWidth = 10;
        const double textboxParameterValueWidth = 60;
        const double labelParameterDescriptionWidth = 120;

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

            TextBox textbox = new TextBox();
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
                label.Name = "InstrumentLabel" + noOfInstruments;
                textbox.Name = "InstrumentTextbox" + noOfInstruments;
                removeButton.Name = "InstrumentRemoveButton" + noOfInstruments;
                removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Instruments_MouseDownEvent);

                instrumentLabel.Add(label);
                instrumentTextBox.Add(textbox);
                instrumentRemoveButton.Add(removeButton);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                InstrumentsGrid.RowDefinitions.Add(rowDef);
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, noOfInstruments);

                InstrumentsGrid.Children.Add(label);
                ++noOfInstruments;
            }
            if (type == Types.Fields)
            {
                label.Name = "FieldLabel" + noOfFields;
                textbox.Name = "FieldTextbox" + noOfFields;
                removeButton.Name = "FieldRemoveButton" + noOfFields;
                removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Fields_MouseDownEvent);

                fieldLabel.Add(label);
                fieldTextBox.Add(textbox);
                fieldRemoveButton.Add(removeButton);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(inputHeight);
                FieldsGrid.RowDefinitions.Add(rowDef);
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

            label.Name = "ParameterLabel" + noOfParameters;
            labelPanel.Name = "ParameterWrapPanel" + noOfInstruments;
            textboxKey.Name = "ParameterKeyTextbox" + noOfParameters;
            labelSeparator.Name = "ParameterSeparatorLabel" + noOfParameters;
            textboxValue.Name = "ParameterValueTextbox" + noOfParameters;
            labelDescription.Name = "ParamterDescriptionLabel" + noOfParameters;
            removeButton.Name = "ParameterRemoveButton" + noOfParameters;
              
            removeButton.PreviewMouseDown += new MouseButtonEventHandler(removeButton_Parameters_MouseDownEvent);
            textboxKey.PreviewKeyUp += new KeyEventHandler(keyTextbox_Parameters_KeyInput);

            parameterLabel.Add(label);
            parameterWrapPanel.Add(labelPanel);
            parameterKeyTextBox.Add(textboxKey);
            parameterSeparatorLabel.Add(labelSeparator);
            parameterValueTextBox.Add(textboxValue);
            parameterDescriptionLabel.Add(labelDescription);
            parameterRemoveButton.Add(removeButton);

            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = new GridLength(inputHeight);
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
            String buttonName = ((Button)sender).Name;
            int instrumentIndex;
            bool result = Int32.TryParse(buttonName.Substring(22), out instrumentIndex); //InstrumentRemoveButton{index}

            //InstrumentsGrid.Children.RemoveAt(instrumentIndex);
            InstrumentsGrid.RowDefinitions.RemoveAt(instrumentIndex);
           
            instrumentLabel.RemoveAt(instrumentIndex);
            instrumentTextBox.RemoveAt(instrumentIndex);
            instrumentRemoveButton.RemoveAt(instrumentIndex);

            --noOfInstruments;

            for (int i = instrumentIndex; i < noOfInstruments; ++i )
            {
                instrumentLabel[i].Name = "InstrumentLabel" + i;
                instrumentTextBox[i].Name = "InstrumentTextbox" + i;
                instrumentRemoveButton[i].Name = "InstrumentRemoveButton" + i;
            }
        }

        private void removeButton_Fields_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            String buttonName = ((Button)sender).Name;
            int fieldIndex;
            bool result = Int32.TryParse(buttonName.Substring(17), out fieldIndex); //FieldRemoveButton{index}

            //FieldsGrid.Children.RemoveAt(fieldIndex);
            FieldsGrid.RowDefinitions.RemoveAt(fieldIndex);

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
        }

        private void removeButton_Parameters_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void keyTextbox_Parameters_KeyInput(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                String textboxName = ((TextBox)sender).Name;
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
