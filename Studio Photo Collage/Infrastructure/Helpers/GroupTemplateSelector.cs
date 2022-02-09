using Studio_Photo_Collage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public class GroupTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupTemplate { get; set; }
        public DataTemplate ProjectTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is GroupedTemplates)
                return GroupTemplate;
            if (item is Project)
                return ProjectTemplate;
            return null;
        }
    }
}
