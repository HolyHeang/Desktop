#pragma checksum "..\..\..\Windows\ShowListStudentToPrint.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1AA9AEA651E49A5097DBAC1E4B8121D1352A2A55138EAFA2C7BB75CCB4DFAB48"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CamemisOffLine.Windows;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using RootLibrary.WPF.Localization;
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
using System.Windows.Shell;


namespace CamemisOffLine.Windows {
    
    
    /// <summary>
    /// ShowListStudentToPrint
    /// </summary>
    public partial class ShowListStudentToPrint : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStudentName;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon iconClose;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkAll;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DGStudentName;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNamePrint;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Windows\ShowListStudentToPrint.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtNotification;
        
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
            System.Uri resourceLocater = new System.Uri("/CamemisOffLine;component/windows/showliststudenttoprint.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            
            #line 9 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            ((CamemisOffLine.Windows.ShowListStudentToPrint)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtStudentName = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.txtStudentName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtStudentName_TextChanged);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.txtStudentName.MouseEnter += new System.Windows.Input.MouseEventHandler(this.txtStudentName_MouseEnter);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.txtStudentName.MouseLeave += new System.Windows.Input.MouseEventHandler(this.txtStudentName_MouseLeave);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.txtStudentName.GotFocus += new System.Windows.RoutedEventHandler(this.txtStudentName_GotFocus);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.txtStudentName.LostFocus += new System.Windows.RoutedEventHandler(this.txtStudentName_LostFocus);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.txtStudentName.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.txtStudentName_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.iconClose = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            
            #line 17 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.iconClose.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.iconClose_MouseUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.checkAll = ((System.Windows.Controls.CheckBox)(target));
            
            #line 20 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.checkAll.Click += new System.Windows.RoutedEventHandler(this.checkAll_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DGStudentName = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.btnNamePrint = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.btnNamePrint.Click += new System.Windows.RoutedEventHandler(this.btnNamePrint_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.txtNotification = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 27 "..\..\..\Windows\ShowListStudentToPrint.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.checkStu_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

