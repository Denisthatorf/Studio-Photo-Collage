﻿using System;
using Studio_Photo_Collage.Views.SidePanelsViews;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public static class StringToFrameConverter
    {
        public static Type Convert(object value)
        {
            if (value != null)
            {
                var strEnum = value.ToString();
                var frame = new Frame();
                switch (strEnum)
                {
                    case "Filters":
                        return typeof(FiltersPage);
                    case "Templates":
                        return typeof(TemplatesSidePage);
                    case "Background":
                        return typeof(BackgroundPage);
                    case "Transform":
                        return typeof(TransformPage);
                    case "Frames":
                        return typeof(FramesPage);
                    case "Resents":
                        return typeof(RecentsPage);
                }
            }

            return null;
        }
    }
}
