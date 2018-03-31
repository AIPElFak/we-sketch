﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WeSketch.App.Data.Shapes
{
    public class ShapeEllipse:ShapeComponent
    {
        public ShapeEllipse(int width, int height, System.Windows.Media.Color color) : base()
        {
            MyElement = new System.Windows.Shapes.Ellipse
            {
                Fill = new SolidColorBrush(color),
                IsHitTestVisible = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = this
            };

            Container = new ContentControl()
            {
                Width = width,
                Height = height,
                Content = MyElement,
                Template = Global.ResizeAndDragStyle
            };
        }

        public ShapeEllipse() : this(0, 0, System.Windows.Media.Color.FromRgb(0, 0, 0))
        {
   
        }
    }
}
