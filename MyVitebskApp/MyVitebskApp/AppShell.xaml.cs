using MyVitebskApp.ViewModels;
using MyVitebskApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyVitebskApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("//LoginPage", typeof(LoginPage));
        }

    }
}
