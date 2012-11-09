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
using System.Xml;

using TinkerBell;

namespace TinkerBell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWillHearMyChilds
    {
        private List<IWillHearMyParent> m_childsWhoAreListeningToMe;

        public MainWindow()
        {
            InitializeComponent();
            m_childsWhoAreListeningToMe = new List<IWillHearMyParent>();
        }

        public void test()
        {
            CParameters l_parameter = new CParameters();
            l_parameter.AddInstrument("EUR=");
            l_parameter.AddField("ASK");
            foreach (string l_each in l_parameter.Instruments)
            {
                Console.WriteLine(l_each);
            }
            foreach (string l_each in l_parameter.Fields)
            {
                Console.WriteLine(l_each);
            }
            foreach (KeyValuePair<string, List<string>> l_each in l_parameter.Parameters)
            {
                Console.WriteLine(l_each.Key);
                foreach (string l_eachValue in l_each.Value)
                {
                    Console.Write(l_eachValue + " ");
                }
            }
        }

        private void TriggerEvent_Click(object sender, RoutedEventArgs e)
        {
            this.OnMyChildToldsMeThatHeChangesParameters();
        }

        public void OnMyChildToldsMeThatHeChangesParameters()
        {
            // Tell another childs
            /*
            foreach (IWillHearMyParent l_child in m_childsWhoAreListeningToMe)
            {
                l_child.OnParametersChange();
            }
            */

            // Update Interpreter

            string l_interpretedWord = "This functions returns TYPE1 of FIELD_DES1, FIELD_DES2 and TYPE2 of FIELD_DES1, FIELD_DES2 for INS_DES1, INS_DES2 INS_TYPE_OF_1_2 and INS_DES3, INS_DES4 INS_TYPE_OF_3_4";
            
            XmlReader l_xml = XmlReader.Create("D:\\TinkerBell\\Database");

            InterpreterText.Text = l_interpretedWord;
        }
    }
}
