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
        
        public void AddInstrument(string a_instrument)
        {
            m_instruments.Add(a_instrument);
        }

        public void RemoveInstrument(string a_instrument)
        {
            m_instruments.Remove(a_instrument);
        }

        public void ClearMember()
        {
            m_instruments.Clear();
            m_fields.Clear();
            m_parameters.Clear();
        }

        public List<string> Instruments
        {
            get { return m_instruments; }
        }

        public void AddField(string a_field)
        {
            m_fields.Add(a_field);
        }

        public void RemoveField(string a_field)
        {
            m_fields.Remove(a_field);
        }

        public List<string> Fields
        {
            get { return m_fields; }
        }

        public void AddParameter(string a_parameterName, string a_parameterValue)
        {
            if (!m_parameters.ContainsKey(a_parameterName))
            {
                m_parameters[a_parameterName] = new List<string>();
            }
            m_parameters[a_parameterName].Add(a_parameterValue);
        }

        public void RemoveParameterByName(string a_parameterName)
        {
            m_parameters.Remove(a_parameterName);
        }

        public void RemoveParameterValue(string a_parameterName, string a_value)
        {
            if (m_parameters.ContainsKey(a_parameterName))
            {
                m_parameters[a_parameterName].Remove(a_value);
            }
        }

        public Dictionary<string, List<string>> Parameters
        {
            get { return m_parameters; }
        }

        private List<string> m_instruments;
        private List<string> m_fields;
        private Dictionary<string, List<string>> m_parameters;
    }
}
