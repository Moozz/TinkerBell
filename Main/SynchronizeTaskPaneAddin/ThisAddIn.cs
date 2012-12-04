using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace SynchronizeTaskPaneAddin
{
    public partial class ThisAddIn
    {
        private TinkerBell.MainWindow taskPaneControl1;
        private Microsoft.Office.Tools.CustomTaskPane taskPaneValue;

        public Microsoft.Office.Tools.CustomTaskPane TaskPane
        {
            get
            {
                return taskPaneValue;
            }
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            taskPaneControl1 = new TinkerBell.MainWindow();

            UserControl userControl1 = new UserControl();

            // Create the ElementHost control for hosting the 
            // WPF UserControl.
            ElementHost host = new ElementHost();
            host.Dock = DockStyle.Fill;
            host.Child = taskPaneControl1;

            userControl1.Controls.Add(host);

            taskPaneValue = this.CustomTaskPanes.Add(
                userControl1, "TinkerBell");
            taskPaneValue.Width = 450;
            taskPaneValue.VisibleChanged +=
                new EventHandler(taskPaneValue_VisibleChanged);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void taskPaneValue_VisibleChanged(object sender, System.EventArgs e)
        {
            if (taskPaneValue.Visible == true)
            {
                taskPaneControl1.init();
            }
            Globals.Ribbons.ManageTaskPaneRibbon.toggleButton1.Checked =
                    taskPaneValue.Visible;
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
