namespace TreeViewDemo
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class GeometryTools
    {
        static GeometryTools()
        {
        }


        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <param name="iconColor">Icon Farbe</param>
        /// <returns></returns>
        public static System.Windows.Shapes.Path GetPathGeometry(string iconString, Color iconColor, int size = 24)
        {
            return new System.Windows.Shapes.Path
            {
                Height = size,
                Width = size,
                Fill = new SolidColorBrush(iconColor),
                Data = Geometry.Parse(iconString)
            };
        }

        /// <summary>
        /// Icon aus String für PathGeometry erstellen
        /// </summary>
        /// <param name="iconString">Icon String</param>
        /// <returns></returns>
        public static System.Windows.Shapes.Path GetPathGeometry(string iconString, int size = 24)
        {
            return GetPathGeometry(iconString, Colors.Black, size);
        }

        public static System.Windows.Shapes.Path GetPathGeometry(string iconString)
        {
            return GetPathGeometry(iconString, Colors.Blue);
        }

        public static ImageSource GeometryToImageSource(Geometry geometry, int iconSize, Brush foregroundColor)
        {
            Rect renderBounds = geometry.GetRenderBounds(new Pen(Brushes.Black, 0.0));
            double num = ((renderBounds.Width > renderBounds.Height) ? renderBounds.Width : renderBounds.Height);
            double num2 = (double)iconSize / num;
            Geometry geometry2 = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, new ScaleTransform(num2, num2));
            renderBounds = geometry2.GetRenderBounds(new Pen(Brushes.Black, 0.0));
            Geometry geometry3 = Geometry.Combine(geometry2, geometry2, GeometryCombineMode.Intersect, new TranslateTransform(((double)iconSize - renderBounds.Width) / 2.0 - renderBounds.Left, ((double)iconSize - renderBounds.Height) / 2.0 - renderBounds.Top));
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(iconSize, iconSize, 96.0, 96.0, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawGeometry(foregroundColor, null, geometry3);
            }

            renderTargetBitmap.Render(drawingVisual);
            MemoryStream memoryStream = new MemoryStream();
            PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            pngBitmapEncoder.Save(memoryStream);
            return GetImg(memoryStream);
        }

        public static ImageSource GeometryToImageSource(Geometry geometry)
        {
            return GeometryToImageSource(geometry, 16, Brushes.Black);
        }

        public static ImageSource GeometryToImageSource(Geometry geometry, Brush foregroundColor)
        {
            return GeometryToImageSource(geometry, 16, foregroundColor);
        }

        public static Image GeometryToImage(Geometry geometry, Brush foregroundColor)
        {
            ImageSource imageSource = GeometryToImageSource(geometry, 16, foregroundColor);
            Image img = new Image();
            img.Source = imageSource;
            return img;
        }

        public static Image GeometryToImage(Geometry geometry, int iconSize, Brush foregroundColor)
        {
            ImageSource imageSource = GeometryToImageSource(geometry, iconSize, foregroundColor);
            Image img = new Image();
            img.Source = imageSource;
            return img;
        }

        private static BitmapImage GetImg(MemoryStream ms)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
