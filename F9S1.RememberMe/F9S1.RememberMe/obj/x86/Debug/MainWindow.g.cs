﻿#pragma checksum "..\..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CB84EDAF65B3FB05578C82C421BF3D77"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace F9S1.RememberMe {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 38 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox helpBox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label displayBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inputBox;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataGrid1;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordBox1;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label passwordLabel;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox userBox;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label userLabel;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button syncButton;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button helpButton;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button settingsButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/F9S1.RememberMe;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\..\MainWindow.xaml"
            ((F9S1.RememberMe.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\MainWindow.xaml"
            ((F9S1.RememberMe.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.OnClose);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\MainWindow.xaml"
            ((F9S1.RememberMe.MainWindow)(target)).StateChanged += new System.EventHandler(this.OnStateChanged);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\MainWindow.xaml"
            ((F9S1.RememberMe.MainWindow)(target)).IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 7 "..\..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.CommandBinding_CanExecute);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.undoExecuted);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 10 "..\..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.CommandBinding_CanExecute);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.redoExecuted);
            
            #line default
            #line hidden
            return;
            case 4:
            this.helpBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.displayBox = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.inputBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 40 "..\..\..\MainWindow.xaml"
            this.inputBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.inputBox_KeyDown);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\MainWindow.xaml"
            this.inputBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.inputBox_TextChanged);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\MainWindow.xaml"
            this.inputBox.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.inputBox_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\MainWindow.xaml"
            this.inputBox.GotFocus += new System.Windows.RoutedEventHandler(this.inputBox_GotFocus);
            
            #line default
            #line hidden
            return;
            case 7:
            this.dataGrid1 = ((System.Windows.Controls.DataGrid)(target));
            
            #line 41 "..\..\..\MainWindow.xaml"
            this.dataGrid1.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.dataGrid1_CellEditEnding);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\MainWindow.xaml"
            this.dataGrid1.BeginningEdit += new System.EventHandler<System.Windows.Controls.DataGridBeginningEditEventArgs>(this.dataGrid1_BeginningEdit);
            
            #line default
            #line hidden
            return;
            case 11:
            this.passwordBox1 = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 12:
            this.passwordLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 13:
            this.userBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 77 "..\..\..\MainWindow.xaml"
            this.userBox.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.userBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 14:
            this.userLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 15:
            this.syncButton = ((System.Windows.Controls.Button)(target));
            
            #line 79 "..\..\..\MainWindow.xaml"
            this.syncButton.Click += new System.Windows.RoutedEventHandler(this.syncButton_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.helpButton = ((System.Windows.Controls.Button)(target));
            
            #line 84 "..\..\..\MainWindow.xaml"
            this.helpButton.Click += new System.Windows.RoutedEventHandler(this.helpButton_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.settingsButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 8:
            
            #line 46 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Primitives.ToggleButton)(target)).Click += new System.Windows.RoutedEventHandler(this.ToggleButton_Click);
            
            #line default
            #line hidden
            break;
            case 9:
            
            #line 58 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Archive_Button_Click);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 67 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Delete_Button_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

