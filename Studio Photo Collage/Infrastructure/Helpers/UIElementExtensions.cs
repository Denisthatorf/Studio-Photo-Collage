using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    namespace UIElementClone
    {
        public static class UIElementExtensions
        {
            public static T DeepClone<T>(this T source) where T : UIElement
            {

                T result;

                // Get the type
                var type = source.GetType();

                // Create an instance
                result = Activator.CreateInstance(type) as T;

                CopyProperties<T>(source, result, type);

                DeepCopyChildren<T>(source, result);

                return result;
            }

            private static void DeepCopyChildren<T>(T source, T result) where T : UIElement
            {
                // Deep copy children.
                var sourcePanel = source as Panel;
                if (sourcePanel != null)
                {
                    var resultPanel = result as Panel;
                    if (resultPanel != null)
                    {
                        foreach (var child in sourcePanel.Children)
                        {
                            // RECURSION!
                            var childClone = DeepClone(child);
                            resultPanel.Children.Add(childClone);
                        }
                    }
                }
            }

            private static void CopyProperties<T>(T source, T result, Type type) where T : UIElement
            {
                // Copy all properties.

                var properties = type.GetRuntimeProperties();

                foreach (var property in properties)
                {
                    if (property.Name != "Name") // do not copy names or we cannot add the clone to the same parent as the original.
                    {
                        if ((property.CanWrite) && (property.CanRead))
                        {
                            object sourceProperty = property.GetValue(source);

                            var element = sourceProperty as UIElement;
                            if (element != null)
                            {
                                var propertyClone = element.DeepClone();
                                property.SetValue(result, propertyClone);
                            }
                            else
                            {
                                try
                                {
                                    property.SetValue(result, sourceProperty);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
