#pragma checksum "..\..\MazeStartUpWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B0356F35BCB3408356FC11C2766471D2A91526A5DB1CE3FAD1848D301D472A5C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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


namespace Mazer {
    
    
    /// <summary>
    /// MazeStartUpWindow
    /// </summary>
    public partial class MazeStartUpWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\MazeStartUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerateMaze;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\MazeStartUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSolveMaze;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\MazeStartUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnQuit;
        
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
            System.Uri resourceLocater = new System.Uri("/Mazer;component/mazestartupwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MazeStartUpWindow.xaml"
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
            this.btnGenerateMaze = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\MazeStartUpWindow.xaml"
            this.btnGenerateMaze.Click += new System.Windows.RoutedEventHandler(this.btnGenerateMaze_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnSolveMaze = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\MazeStartUpWindow.xaml"
            this.btnSolveMaze.Click += new System.Windows.RoutedEventHandler(this.btnSolveMaze_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnQuit = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\MazeStartUpWindow.xaml"
            this.btnQuit.Click += new System.Windows.RoutedEventHandler(this.btnQuit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

