using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel; 

namespace ExcelManager
{
    public class CExcelManager
    {
        private Excel.Application m_xlApp = null;
        
        private List<String> m_instrumentList;
        private List<String> m_fieldList;
        public String InstrumentList
        {
            set
            {
                m_instrumentList.Add(value);
            }
        }

        public String FieldList
        {
            set
            {
                m_fieldList.Add(value);
            }
        }

        public CExcelManager()
        {
            m_instrumentList = new List<String>();
            m_fieldList = new List<String>();
            try
            {
                //Get reference to Excel.Application from the ROT.
                m_xlApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                Excel.Range l_activeCell = m_xlApp.ActiveCell;
                l_activeCell.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent3;
            }
            catch (Exception)
            {

            }
        }

        public void RemoveExcelApp()
        {
            m_xlApp = null;
        }

        public void RefreshInstrument()
        {
            if (m_xlApp != null)
            {
                Excel.Range l_activeCell = m_xlApp.ActiveCell ;
                for (int l_index = 0; l_index < m_instrumentList.Count; ++l_index)
                {
                    l_activeCell.get_Offset(l_index + 1, 0).Value = m_instrumentList[l_index];
                }
            }
        }

        public void ResetInstrument(int a_index)
        {
            if (m_xlApp != null)
            {
                Excel.Range l_start = m_xlApp.ActiveCell.get_Offset(1, 0);
                Excel.Range l_end = m_xlApp.ActiveCell.get_Offset(m_instrumentList.Count + 1, 0);
                Excel.Range l_clearRange = m_xlApp.get_Range(l_start, l_end);
                l_clearRange.ClearContents();
            }

            if (m_instrumentList.Count != 0 && m_instrumentList.Count >= a_index)
            {
                m_instrumentList.RemoveAt(a_index);
            }
        }

        public void RefreshField()
        {
            if (m_xlApp != null)
            {
                Excel.Range l_activeCell = m_xlApp.ActiveCell;
                for (int l_index = 0; l_index < m_fieldList.Count; ++l_index)
                {
                    l_activeCell.get_Offset(0, l_index + 1).Value = m_fieldList[l_index];
                }
            }
        }
    }
}
