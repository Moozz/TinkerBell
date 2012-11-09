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

namespace TinkerBell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWillHearMyChilds
    {
        private List<IWillHearMyParent> m_childsWhoAreListeningToMe;
        private CParameters m_parameter;

        public MainWindow()
        {
            InitializeComponent();
            m_childsWhoAreListeningToMe = new List<IWillHearMyParent>();
            m_parameter = new CParameters();
            test();
        }

        public void test()
        {
            m_parameter.AddInstrument("EUR=");
            m_parameter.AddInstrument("JPY=");
            m_parameter.AddInstrument("TRI.N");
            m_parameter.AddInstrument("MSFT.O");
            m_parameter.AddInstrument("PTT.BK");
            m_parameter.AddInstrument("AAPL.O");
            m_parameter.AddInstrument("THB=");
            m_parameter.AddField("BID");
            m_parameter.AddField("ASK");
            m_parameter.AddField("CURRENCY");
            m_parameter.AddField("CLOSE");
            m_parameter.AddField("RF.G.Compname");
            m_parameter.AddField("RF.IS.NetSales");
            m_parameter.AddField("RI.ID.RIC");

            
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
            // Tell another childs
            /*
            foreach (IWillHearMyParent l_child in m_childsWhoAreListeningToMe)
            {
                l_child.OnParametersChange();
            }
            */

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

                InterpreterText.Text = l_interpretedWord + ".\n";
                /*
                if (m_parameter.Parameters.Count != 0)
                {
                    foreach (KeyValuePair<string, List<string>> item in m_parameter.Parameters)
	                {
		 
	                } 
                }*/
            }
            else
            {
                InterpreterText.Text = "Please add Instrument and Fields :)";
            }
        }
    }
}
