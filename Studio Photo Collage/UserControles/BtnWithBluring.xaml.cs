﻿using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Studio_Photo_Collage.UserControles
{
    public sealed partial class BtnWithBluring : UserControl
    {
        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Project.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(BtnWithBluring), new PropertyMetadata(null));



        public ICommand MyCommand
        {
            get { return (ICommand)GetValue(MyCommandProperty); }
            set { SetValue(MyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyCommandProperty =
            DependencyProperty.Register("MyCommand", typeof(ICommand), typeof(BtnWithBluring), new PropertyMetadata(null));



        public object MyCommandParameter
        {
            get { return (object)GetValue(MyCommandParametrProperty); }
            set { SetValue(MyCommandParametrProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCommandParametr.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyCommandParametrProperty =
            DependencyProperty.Register("MyCommandParametr", typeof(object), typeof(BtnWithBluring), new PropertyMetadata(0));



        public BtnWithBluring()
        {
            this.InitializeComponent();
            this.DataContextChanged += (o, e) => {  };
        }
    }
}