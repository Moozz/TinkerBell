﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelManager
{
    public class CExcelManager
    {


        public class Parameter
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
            public override string ToString()
            {
                return "Name : " + Name + " Value : " + Value + " Type : " + Type;
            }
        }

        public enum ParameterType
        {
            Instrument = 0,
            Field = 1,
            RequestParam = 2,
            RefreshParam = 3,
            DisplayParam = 4
        }

        //A delegate type for select cell notification
        public delegate void CellSelectedEventHandler(CExcelManager a_excelMgr);
        public event CellSelectedEventHandler CellSelected;

        public Excel.Application m_xlApp = null;
        public Excel.Range m_activeCell = null;

        public List<String> m_instrumentList;
        public String m_instrumentRange;
        public List<String> m_fieldList;
        public String m_fieldRange;

        public List<Parameter> m_parameterList;

        public string m_displayParameterStr;
        public string m_refreshParameterStr;

#region Property implementation
        public String InstrumentList
        {
            set
            {
                m_instrumentList.Add(value);
            }
        }

        public List<String> RetreiveInstrument
        {
            get
            {
                return m_instrumentList;
            }
        }

        public String FieldList
        {
            set
            {
                m_fieldList.Add(value);
            }
        }

        public List<String> RetreiveField
        {
            get
            {
                return m_fieldList;
            }
        }

        public void AddParameter(string a_name, string a_type)
        {
            Parameter l_parameter = new Parameter();
            l_parameter.Name = a_name;
            l_parameter.Type = a_type;
            m_parameterList.Add(l_parameter);
        }

        public void AddParameterValue(string a_val)
        {
            m_parameterList.Last().Value = a_val;
        }

        public List<Parameter> RetreiveParameter
        {
            get
            {
                return m_parameterList;
            }
        }
#endregion

        public CExcelManager()
        {
            m_instrumentList = new List<String>();
            m_fieldList = new List<String>();
            m_parameterList = new List<Parameter>();
            m_displayParameterStr = "";
            m_refreshParameterStr = "";
            try
            {
                //Get reference to Excel.Application from the ROT.
                m_xlApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }
            catch (Exception)
            {

            }
        }


        public void InitActiveCell()
        {                 
            m_activeCell = m_xlApp.ActiveCell;
            m_activeCell.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent3;
            m_activeCell.Interior.Color = 2467823;
            m_activeCell.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
            m_activeCell.Font.TintAndShade = -0.499984740745262;
            m_activeCell.Font.Bold = true;

            m_xlApp.EnableEvents = true;
            m_xlApp.SheetSelectionChange += new Excel.AppEvents_SheetSelectionChangeEventHandler(m_xlApp_SheetSelectionChange);
        }

        public bool IsExcelReady()
        {
            if (m_xlApp != null)
            {            
                return true;
            }
            return false;
        }


        public void FinalizeExcelManager()
        {
            m_instrumentList.Clear();
            m_fieldList.Clear();
            m_instrumentRange = null;
            m_parameterList.Clear();
            m_displayParameterStr = "";
            m_refreshParameterStr = "";
            m_fieldRange = "";
            m_activeCell = null;
        }

        public void RemoveExcelApp()
        {
            m_xlApp = null;
        }

        public void RefreshInstrument()
        {
            if (m_xlApp != null)
            {
                for (int l_index = 0; l_index < m_instrumentList.Count; ++l_index)
                {
                    m_activeCell.get_Offset(l_index + 1, 0).Value = m_instrumentList[l_index];
                }
                if (m_instrumentList.Count > 0)
                {
                    Excel.Range l_start = m_activeCell.get_Offset(1, 0);
                    Excel.Range l_end = m_activeCell.get_Offset(m_instrumentList.Count, 0);
                    Excel.Range l_range = m_xlApp.get_Range(l_start, l_end);
                    l_range.Interior.Color = 2467823;
                    l_range.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    l_range.Font.TintAndShade = -0.499984740745262;
                    l_range.Font.Bold = true;
                    m_instrumentRange = l_range.get_Address();
                }
                else
                {
                    m_instrumentRange = null;
                }
                GenerateRDataFunction();
            }
        }

        public void ResetInstrument(int a_index)
        {
            if (m_xlApp != null)
            {
                Excel.Range l_start = m_activeCell.get_Offset(1, 0);
                Excel.Range l_end = m_activeCell.get_Offset(m_instrumentList.Count + 1, 0);
                Excel.Range l_clearRange = m_xlApp.get_Range(l_start, l_end);
                l_clearRange.Clear();
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
               
                for (int l_index = 0; l_index < m_fieldList.Count; ++l_index)
                {
                    m_activeCell.get_Offset(0, l_index + 1).Value = m_fieldList[l_index];
                }

                if (m_fieldList.Count > 0)
                {
                    Excel.Range l_start = m_activeCell.get_Offset(0, 1);
                    Excel.Range l_end = m_activeCell.get_Offset(0, m_fieldList.Count);
                    Excel.Range l_range = m_xlApp.get_Range(l_start, l_end);
                    l_range.Interior.Color = 2467823;
                    l_range.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    l_range.Font.TintAndShade = -0.499984740745262;
                    l_range.Font.Bold = true;

                    m_fieldRange = l_range.get_Address();
                }
                else
                {
                    m_fieldRange = null;
                }

                GenerateRDataFunction();
            }
        }

        public void ResetField(int a_index)
        {
            if (m_xlApp != null)
            {
                Excel.Range l_start = m_activeCell.get_Offset(0, 1);
                Excel.Range l_end = m_activeCell.get_Offset(0, m_fieldList.Count + 1);
                Excel.Range l_clearRange = m_xlApp.get_Range(l_start, l_end);
                l_clearRange.Clear();
            }

            if (m_fieldList.Count != 0 && m_fieldList.Count >= a_index)
            {
                m_fieldList.RemoveAt(a_index);
            }
        }

        public void RefreshParameter()
        {
            if (m_xlApp != null)
            {
                m_displayParameterStr = "";
                m_refreshParameterStr = "";
                foreach (Parameter l_parameter in m_parameterList)
                {
                    if (l_parameter.Type == "Display")
                    {
                        m_displayParameterStr += l_parameter.Name + ":" + l_parameter.Value + " ";
                    }
                    else if (l_parameter.Type == "Refresh")
                    {
                        m_refreshParameterStr += l_parameter.Name + ":" + l_parameter.Value + " ";
                    }
                    else
                    {
                        //Not support
                    }
                }

                GenerateRDataFunction();
            }
        }

        public void ResetParameter(int a_index)
        {
            if (m_parameterList.Count != 0 && m_parameterList.Count >= a_index)
            {
                m_parameterList.RemoveAt(a_index);
            }
        }

        private void GenerateRDataFunction()
        {
            String l_rDataFunctionText = @"=RData(";
            if (m_instrumentRange != null)
            {
                l_rDataFunctionText += m_instrumentRange;
            }
            l_rDataFunctionText += ",";
            if (m_fieldRange != null)
            {
                l_rDataFunctionText += m_fieldRange;
            }
            l_rDataFunctionText += ",";
            l_rDataFunctionText += ",";
            if (m_refreshParameterStr != "")
            {
                l_rDataFunctionText += @"""";
                l_rDataFunctionText += m_refreshParameterStr;
                l_rDataFunctionText += @"""";
            }

            l_rDataFunctionText += ",";
            if (m_displayParameterStr != "")
            {
                l_rDataFunctionText += @"""";
                l_rDataFunctionText += m_displayParameterStr;
                l_rDataFunctionText += @"""";
            }
            l_rDataFunctionText += ",";
            l_rDataFunctionText += m_activeCell.get_Offset(1, 1).get_Address();
            l_rDataFunctionText += @")";

            m_activeCell.Value = l_rDataFunctionText;
        }

#region RData pharser
        void m_xlApp_SheetSelectionChange(object Sh, Excel.Range Target)
        {
            FinalizeExcelManager();
            m_activeCell = m_xlApp.ActiveCell;
            string l_cellVal = Target.Formula;
            if (l_cellVal != null && l_cellVal.StartsWith(@"=RData("))
            {            
                string l_parameterStr = l_cellVal.Substring(l_cellVal.IndexOf("(")).Replace('\"', ' ');
                string[] l_parameterArray = l_parameterStr.Split(',');
                for (int l_index = 0; l_index < l_parameterArray.Length; ++l_index)
                {
                    switch (l_index)
                    {
                        case (int)ParameterType.Instrument:
                            InitInstrumentFromRange(l_parameterArray[l_index].Substring(1), Target.Worksheet);
                            break;
                        case (int)ParameterType.Field:
                            InitFieldFromRange(l_parameterArray[l_index], Target.Worksheet);
                            break;
                        case (int)ParameterType.RefreshParam:
                            InitRefreshParamFromRange(l_parameterArray[l_index], Target.Worksheet);
                            break;
                        case (int)ParameterType.DisplayParam:
                            InitDisplayParamFromRange(l_parameterArray[l_index], Target.Worksheet);
                            break;
                        default:
                            break;
                    }
                }
                RaiseCellSelectedSelectedEvent(this);
            }
            else
            {
                RaiseCellSelectedSelectedEvent(null);
            }
            
        }

        void InitInstrumentFromRange(string a_rangeInput, Excel.Worksheet a_sheet)
        {
            if (a_rangeInput.Length <= 0)
            {
                return;
            }
            Excel.Range l_instrumentRange = a_sheet.get_Range(a_rangeInput);
            if (l_instrumentRange.Rows.Count == 1 && l_instrumentRange.Columns.Count == 1)
            {
                string l_valueArrTmp = l_instrumentRange.get_Value();
                m_instrumentList.Add(l_valueArrTmp);
            }
            else
            {
                System.Array l_valueArrTmp = (System.Array)l_instrumentRange.get_Value();
                foreach (string l_value in l_valueArrTmp)
                {
                    m_instrumentList.Add(l_value);
                }
            }
        }


        void InitFieldFromRange(string a_rangeInput, Excel.Worksheet a_sheet)
        {
            if (a_rangeInput.Length <= 0)
            {
                return;
            }
            Excel.Range l_fieldRange = a_sheet.get_Range(a_rangeInput);
            if (l_fieldRange.Rows.Count == 1 && l_fieldRange.Columns.Count == 1)
            {
                string l_valueArrTmp = l_fieldRange.get_Value();
                m_fieldList.Add(l_valueArrTmp);
            }
            else
            {
                System.Array l_valueArrTmp = (System.Array)l_fieldRange.get_Value();
                foreach (string l_value in l_valueArrTmp)
                {
                    m_fieldList.Add(l_value);
                }
            }
        }

        void InitRefreshParamFromRange(string a_rangeInput, Excel.Worksheet a_sheet)
        {
            if (a_rangeInput.Length <= 0)
            {
                return;
            }
            string l_trimInput = a_rangeInput.Trim();
            foreach (String l_param in l_trimInput.Split(' '))
           {
               string[] l_nameAndVal = l_param.Split(':');
               AddParameter(l_nameAndVal[0].Trim(), "Refresh");
               AddParameterValue(l_nameAndVal[1].Trim());
           }
        }

        void InitDisplayParamFromRange(string a_rangeInput, Excel.Worksheet a_sheet)
        {
            if (a_rangeInput.Length <= 0)
            {
                return;
            }
            string l_trimInput = a_rangeInput.Trim();
            foreach (String l_param in l_trimInput.Split(' '))
            {
                string[] l_nameAndVal = l_param.Split(':');
                AddParameter(l_nameAndVal[0].Trim(), "Display");
                AddParameterValue(l_nameAndVal[1].Trim());
            }
        }


        // This method raises the Tap event 
        void RaiseCellSelectedSelectedEvent(CExcelManager a_excelMgr)
        {
            //Fire event to client
            if (CellSelected != null)
            {
                CellSelected(a_excelMgr);
            }
        }
        #endregion


        public void InsertInstrumentAtIndex(int a_index, string a_value)
        {
            //for()
            //{
            //}
        }
    }
}
