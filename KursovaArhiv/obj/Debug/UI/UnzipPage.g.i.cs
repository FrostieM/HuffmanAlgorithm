﻿#pragma checksum "..\..\..\UI\UnzipPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C19F4194464F187C94A77DB2F38493CD3D2A18AD"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using KursovaArhiv.UI;
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


namespace KursovaArhiv.UI {
    
    
    /// <summary>
    /// UnzipPage
    /// </summary>
    public partial class UnzipPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel UnzipPanelChoose;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock_Copy2;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UnzipPathTextBox;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UnzipPathChooseButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock_Copy3;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UnzipFileTextBox;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UnzipFileChooseButton;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UnzipNextButton;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\UI\UnzipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UnzipBackButton;
        
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
            System.Uri resourceLocater = new System.Uri("/KursovaArhiv;component/ui/unzippage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UI\UnzipPage.xaml"
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
            this.UnzipPanelChoose = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 2:
            this.textBlock_Copy2 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.UnzipPathTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.UnzipPathChooseButton = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\UI\UnzipPage.xaml"
            this.UnzipPathChooseButton.Click += new System.Windows.RoutedEventHandler(this.UnzipPathChooseButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.textBlock_Copy3 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.UnzipFileTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.UnzipFileChooseButton = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\UI\UnzipPage.xaml"
            this.UnzipFileChooseButton.Click += new System.Windows.RoutedEventHandler(this.UnzipFileChooseButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.UnzipNextButton = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\UI\UnzipPage.xaml"
            this.UnzipNextButton.Click += new System.Windows.RoutedEventHandler(this.UnzipNextButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.UnzipBackButton = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\UI\UnzipPage.xaml"
            this.UnzipBackButton.Click += new System.Windows.RoutedEventHandler(this.UnzipBackButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

