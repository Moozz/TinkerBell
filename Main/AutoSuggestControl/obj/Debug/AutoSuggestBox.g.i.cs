﻿#pragma checksum "..\..\AutoSuggestBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FCB2BFB79E3877C4B7B2D95C7149935F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoSuggestControl;
using AutoSuggestControl.ViewModel;
using AutoSuggestTextBox;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AutoSuggestControl {
    
    
    /// <summary>
    /// AutoSuggestBox
    /// </summary>
    public partial class AutoSuggestBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 85 "..\..\AutoSuggestBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AutoSuggestTextBox.AutoSuggestTextBox commandBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PLAutoSuggestControl;component/autosuggestbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AutoSuggestBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 84 "..\..\AutoSuggestBox.xaml"
            ((System.Windows.Controls.Grid)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.Grid_GotFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.commandBox = ((AutoSuggestTextBox.AutoSuggestTextBox)(target));
            
            #line 92 "..\..\AutoSuggestBox.xaml"
            this.commandBox.ListItemSelected += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.commandBox_ListItemSelected);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
