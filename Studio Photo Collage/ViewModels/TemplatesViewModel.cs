using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Studio_Photo_Collage.ViewModels
{
    public class TemplatesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public ICommand TemplateClickCommand { get; private set; }

        public TemplatesPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            TemplateClickCommand = new RelayCommand(() => _navigationService.NavigateTo("MainPage"));
          /*  FillByNormalTemplate(1, 1, false);

            FillByNormalTemplate(2, 2, false);
            FillByNormalTemplate(2, 3, true);

            FillByNormalTemplate(3, 2, false);
            FillByNormalTemplate(3, 2, true);*/
            TemplateCollection.Add(new Template(3, false, 0));
        }
        public ObservableCollection<Button> CollageTemplates1 { get; set; } = new ObservableCollection<Button>();
        public ObservableCollection<Template> TemplateCollection { get; set; } = new ObservableCollection<Template>();

        private void FillByNormalTemplate(int countOfPhoto, int rotationCount, bool IsSecondRow2Star)
        {
            for (int j = 0; j < rotationCount; j++)
            {
                var grid = new Grid();
                for (int i = 0; i < countOfPhoto; i++)
                {
                    if (i == 1 && IsSecondRow2Star)
                        grid.RowDefinitions.Add(new RowDefinition()
                        { Height = new Windows.UI.Xaml.GridLength(2, Windows.UI.Xaml.GridUnitType.Star) });
                    else
                        grid.RowDefinitions.Add(new RowDefinition());

                    var rect = new Rectangle();
                    rect.SetValue(Grid.RowProperty, i);

                    grid.Children.Add(rect);

                    grid.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                    grid.RenderTransform = new Windows.UI.Xaml.Media.RotateTransform() { Angle = 90 * j };
                }
                
                // grid.RenderTransform = new Windows.UI.Xaml.Media.RotateTransform() { Angle = 90 * j};
                Button btn = new Button()
                {
                    Content = grid,
                    Margin = new Windows.UI.Xaml.Thickness(10)
                };
                CollageTemplates1.Add(btn);
            }

        }
        #region
        /* 
        public ObservableCollection<Button> CollageTemplates1 { get; set; } = new ObservableCollection<Button>();
        ObservableCollection<Grid> CollageTemplates2 = new ObservableCollection<Grid>();
        ObservableCollection<Grid> CollageTemplates3 = new ObservableCollection<Grid>();

        private void FillCollagetemplates1()
        {
            var grid = CreateNewGrid(false);
            Rectangle rectangle = new Rectangle();
            grid.Children.Add(rectangle);
            Button b = new Button() { Command = TemplateClickCommand, Content = grid}; 
            CollageTemplates1.Add(b);
        }
        private void FillCollagetemplates2()
        {
            for (int j = 0; j < 2; j++)
            {
                var grid = CreateNewGrid(false);
                Rectangle rectangle = new Rectangle();

                grid.Children.Add(rectangle);

                CollageTemplates2.Add(grid);
            }

            for (int j = 0; j < 4; j++)
            {
                var grid = CreateNewGrid(false);
                CollageTemplates2.Add(grid);
            }
        }


        private Grid CreateNewGrid(bool IsSecondRow2Star)
        {
            Grid grid = new Grid()
            {
                Height = 120,
                Width = 120

            };
           // r.Height = new Windows.UI.Xaml.GridLength(2, Windows.UI.Xaml.GridUnitType.Star);

            grid.RowDefinitions.Add(new RowDefinition());
             if(IsSecondRow2Star)
                grid.RowDefinitions.Add(new RowDefinition() 
                { Height = new Windows.UI.Xaml.GridLength(2, Windows.UI.Xaml.GridUnitType.Star)}); //2*
            else
                grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            return grid;
        }
        private Grid FillNormalGrid(Grid grid)
        {
            return null;
        }
        private Grid FillGridWith2Star(Grid grid)
        {
            return null;
        } 
         */
        #endregion
    }
}
