﻿using System.Threading.Tasks;
using MvvmNano.Forms;
using MvvmNanoDemo.Pages;
using MvvmNanoDemo.ViewModels;
using Xamarin.Forms;

namespace MvvmNanoDemo
{
    public class DemoPresenter : MvvmNanoFormsPresenter
    {
        public DemoPresenter(MvvmNanoApplication app) : base(app)
        {
        }

        protected override Task OpenPageAsync(Page page)
        {
            if (page is LoginPage)
            {
                return Task.FromResult(Application.MainPage = new MvvmNanoNavigationPage(page)); 
            }  

            if (page is MasterPage)
            {
                Application.MainPage = page;
                return NavigateToViewModelAsync<WelcomeViewModel>();
            } 

            if (page is AboutPage)
            {
                return CurrentPage.Navigation.PushModalAsync(new MvvmNanoNavigationPage(page));
            }

            return base.OpenPageAsync(page);
        }
    }
}
