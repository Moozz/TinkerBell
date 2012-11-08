using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinkerBell
{
    class CParameters
    {
        public CParameters()
        {
            m_instruments = new List<string>();
            m_fields = new List<string>();
            m_parameters = new Dictionary<string, List<string>>();
        }
              
        private List<string> m_instruments;
        public void AddInstrument(string a_instrument)
        {
            m_instruments.Add(a_instrument);
        }
        public List<string> Instruments
        {
            get { return m_instruments; }
        }

        private List<string> m_fields;
        public void AddField(string a_field)
        {
            m_fields.Add(a_field);
        }
        public List<string> Fields
        {
            get { return m_fields; }
        }

        private Dictionary<string, List<string>> m_parameters;
        public void AddParameter(string a_parameterName, string a_parameterValue)
        {
            if (!m_parameters.ContainsKey(a_parameterName))
            {
                m_parameters[a_parameterName] = new List<string>();
            }
            m_parameters[a_parameterName].Add(a_parameterValue);
        }
        public Dictionary<string, List<string>> Parameters
        {
            get { return m_parameters; }
        }
    }
}
