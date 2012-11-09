using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace TinkerBell
{
    class DatabaseReader
    {
        static string s_databaseXmlName = "TinkerBell.xml";

        public DatabaseReader()
        {
        }

        static public string FindInstrumentDescription(string a_instrument)
        {
            return FindAttribute("Instrument", a_instrument, "Description");
        }

        static public string FindInstrumentType(string a_instrument)
        {
            return FindAttribute("Instrument", a_instrument, "Type");
        }

        static public string FindFieldDescription(string a_field)
        {
            return FindAttribute("Field", a_field, "Description");
        }

        static public string FindFieldType(string a_field)
        {
            return FindAttribute("Field", a_field, "Type");
        }

        static public string FindParameterDescription(string a_parameter)
        {
            return FindAttribute("Parameter", a_parameter, "Description1");
        }

        static private string FindAttribute(string a_element, string a_code, string a_attribute)
        {
            XmlDocument l_doc = new XmlDocument();
            l_doc.Load(s_databaseXmlName);
            string l_query = "/metadata/" + a_element + "/item[@Code='" + a_code + "']";
            XmlNode l_xmlNode = l_doc.SelectSingleNode(l_query);
            if (l_xmlNode == null)
                return "";

            XmlAttribute l_attribute = l_xmlNode.Attributes[a_attribute];
            if (l_attribute == null)
                return "";

            return l_attribute.Value;
        }
    }
}
