using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Studio_Photo_Collage.Infrastructure.Services;
using Studio_Photo_Collage.ViewModels.PopUpsViewModels;

namespace Studio_Photo_Collage.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var services = new ServiceCollection();

            //services.AddTransient<MainPageViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<StartPageViewModel>();
            services.AddSingleton<TemplatePageViewModel>();
            services.AddSingleton<MainPageViewModel>();

            services.AddSingleton<SettingsDialogViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        public MainPageViewModel MainPageInstance => Ioc.Default.GetService<MainPageViewModel>();
        public StartPageViewModel StarPageInstance => Ioc.Default.GetService<StartPageViewModel>();
        public TemplatePageViewModel TemplatePageInstance => Ioc.Default.GetService<TemplatePageViewModel>();
        public SettingsDialogViewModel  SettingsDialogInstance => Ioc.Default.GetService<SettingsDialogViewModel>();

    }
}
