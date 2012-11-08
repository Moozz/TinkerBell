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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CParameters l_parameter = new CParameters();
            l_parameter.AddField("BID");
            l_parameter.AddInstrument("EUR=");
            l_parameter.AddParameter("CH", "In");
            l_parameter.AddParameter("CH", "Fd");
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
    }
}
