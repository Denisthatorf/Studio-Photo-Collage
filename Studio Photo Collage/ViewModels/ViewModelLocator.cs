using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Studio_Photo_Collage.Infrastructure.Services;
using Studio_Photo_Collage.ViewModels.PopUpsViewModels;
using Studio_Photo_Collage.ViewModels.SidePanels;

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

            services.AddSingleton<FramesPageViewModel>();
            services.AddSingleton<BackgroundPageViewModel>();
            services.AddSingleton<FiltersPageViewModel>();
            services.AddSingleton<RecentPageViewModel>();
            services.AddSingleton<TemplatePageViewModel>();
            services.AddSingleton<TransformPageViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        public MainPageViewModel MainPageInstance => Ioc.Default.GetService<MainPageViewModel>();
        public StartPageViewModel StarPageInstance => Ioc.Default.GetService<StartPageViewModel>();
        public TemplatePageViewModel TemplatePageInstance => Ioc.Default.GetService<TemplatePageViewModel>();

        public SettingsDialogViewModel  SettingsDialogInstance => Ioc.Default.GetService<SettingsDialogViewModel>();

        public FramesPageViewModel FramesPageInstance => Ioc.Default.GetService<FramesPageViewModel>();
        public BackgroundPageViewModel BackgroundPageInstance => Ioc.Default.GetService<BackgroundPageViewModel>();
        public FiltersPageViewModel FiltersPageInstance => Ioc.Default.GetService<FiltersPageViewModel>();
        public RecentPageViewModel RecentPageInstance => Ioc.Default.GetService<RecentPageViewModel>();
        public TemplatePageViewModel TemplateSidePageInstance => Ioc.Default.GetService<TemplatePageViewModel>();
        public TransformPageViewModel TransformPageInstance => Ioc.Default.GetService<TransformPageViewModel>();

    }
}
