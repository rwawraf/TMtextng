﻿#pragma checksum "..\..\..\..\KeyboardPages\LowerABC.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3AF5DFB43AFF4FCFF5C83F1C08C9BA58F6291BC477D2CD5628FB3BAEB8B2D9AA"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
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
using TMtextng.KeyboardPages;
using TMtextng.Properties;


namespace TMtextng.KeyboardPages {
    
    
    /// <summary>
    /// LowerABC
    /// </summary>
    public partial class LowerABC : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\KeyboardPages\LowerABC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LowerABCgrid;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\KeyboardPages\LowerABC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid TextBoxGrid;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\KeyboardPages\LowerABC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxContent;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\KeyboardPages\LowerABC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ControlGrid;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\KeyboardPages\LowerABC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid WordSuggestionGrid;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\KeyboardPages\LowerABC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LettersGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/TMtextng;component/keyboardpages/lowerabc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\KeyboardPages\LowerABC.xaml"
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
            this.LowerABCgrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.TextBoxGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.TextBoxContent = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\..\KeyboardPages\LowerABC.xaml"
            this.TextBoxContent.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextChangedEventHandler);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ControlGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.WordSuggestionGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.LettersGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

