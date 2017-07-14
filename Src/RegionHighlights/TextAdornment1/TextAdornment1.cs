using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace TextAdornment1
{
    ///<summary>
    ///TextAdornment1 places red boxes behind all the "A"s in the editor window
    ///</summary>
    public class TextAdornment1
    {
        IAdornmentLayer _layer;
        IWpfTextView _view;
        Brush _brush;
        Pen _pen;

        public TextAdornment1(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer("TextAdornment1");

            //Listen to any event that changes the layout (text changes, scrolling, etc)
            _view.LayoutChanged += OnLayoutChanged;
            _view.Caret.PositionChanged += Caret_PositionChanged;
            
            //Create the pen and brush to color the box behind the a's
            Brush brush = new SolidColorBrush(Color.FromArgb(20, 0, 0, 255));
            brush.Freeze();
            Brush penBrush = new SolidColorBrush(Color.FromArgb(148, 103, 121, 255));
            penBrush.Freeze();
            Pen pen = new Pen(penBrush, 0.8);
            pen.Freeze();
            
            _brush = brush;
            _pen = pen;
        }

        void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
        }

        /// <summary>
        /// On layout change add the adornment to any reformatted lines
        /// </summary>
        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                this.CreateVisuals(line);
            }
        }

        /// <summary>
        /// Within the given line add the scarlet box behind the a
        /// </summary>
        private void CreateVisuals(ITextViewLine line)
        {
            
            //grab a reference to the lines in the current TextView 
            IWpfTextViewLineCollection textViewLines = _view.TextViewLines;
            int start = line.Start;
            int end = line.End;

            //if (results > 0)
            //{
            //    var mySpan = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(results, results + 1));
            //    Geometry g = textViewLines.GetMarkerGeometry(mySpan);
            //    if (g != null)
            //    {
            //        GeometryDrawing drawing = new GeometryDrawing(_brush, _pen, g);
            //        drawing.Freeze();

            //        DrawingImage drawingImage = new DrawingImage(drawing);
            //        drawingImage.Freeze();

            //        Image image = new Image();
            //        image.Source = drawingImage;

            //        //Align the image with the top of the bounds of the text geometry
            //        Canvas.SetLeft(image, g.Bounds.Left);
            //        Canvas.SetTop(image, g.Bounds.Top);

            //        _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, mySpan, null, image, null);
            //    } 
            //}

            //Loop through each character, and place a box around any a 
            for (int i = start; (i+6 < end); ++i)
            {
                if (_view.TextSnapshot[i] == '#' && _view.TextSnapshot[i + 1] == 'r' && _view.TextSnapshot[i + 2] == 'e' && _view.TextSnapshot[i + 3] == 'g' && _view.TextSnapshot[i + 4] == 'i' && _view.TextSnapshot[i + 5] == 'o' && _view.TextSnapshot[i + 6] == 'n')
                {
                    var l = line.Left;
                    var r = line.Right;
                    var t = line.Top;
                    var b = line.Bottom;

                    var width = 500;


                    SnapshotSpan span = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(i, i + 6));
                    Geometry g = textViewLines.GetMarkerGeometry(span);
                    if (g != null)
                    {
                        GeometryDrawing drawing = new GeometryDrawing(_brush, _pen, g);
                        Border border = new Border() { Width = 1000, Height = b - t+2, BorderThickness = new Thickness(0, 2, 0, 0) };
                        border.BorderBrush = new SolidColorBrush(Color.FromArgb(148, 103, 121, 255));
                        border.Background = new SolidColorBrush(Color.FromArgb(11, 0, 0, 255));
                        Canvas.SetTop(border, g.Bounds.Top-1);
                        Canvas.SetLeft(border, g.Bounds.Left);
                        //drawing.Freeze();

                        //DrawingImage drawingImage = new DrawingImage(drawing);
                        //drawingImage.Freeze();

                        //Image image = new Image();
                        //image.Source = drawingImage;
                        //image.Width = r - l;

                        ////Align the image with the top of the bounds of the text geometry
                        //Canvas.SetLeft(image, g.Bounds.Left);
                        //Canvas.SetTop(image, g.Bounds.Top);

                        _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, border, null);
                    }
                } 
                
                if (_view.TextSnapshot[i] == '#' && _view.TextSnapshot[i + 1] == 'e' && _view.TextSnapshot[i + 2] == 'n' && _view.TextSnapshot[i + 3] == 'd' && _view.TextSnapshot[i + 4] == 'r' && _view.TextSnapshot[i + 5] == 'e' && _view.TextSnapshot[i + 6] == 'g'
                    && _view.TextSnapshot[i + 7] == 'i' && _view.TextSnapshot[i + 8] == 'o' && _view.TextSnapshot[i + 9] == 'n')
                {
                    var l = line.Left;
                    var r = line.Right;
                    var t = line.Top;
                    var b = line.Bottom;

                    var width = 500;


                    SnapshotSpan span = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(i, i + 6));
                    Geometry g = textViewLines.GetMarkerGeometry(span);
                    if (g != null)
                    {
                        GeometryDrawing drawing = new GeometryDrawing(_brush, _pen, g);
                        Border border = new Border() { Width = 1000, Height = b - t + 2, BorderThickness = new Thickness(0, 0, 0, 2) };
                        border.BorderBrush = new SolidColorBrush(Color.FromArgb(148, 103, 121, 255));
                        border.Background = new SolidColorBrush(Color.FromArgb(11, 0, 0, 255));
                        Canvas.SetTop(border, g.Bounds.Top);
                        Canvas.SetLeft(border, g.Bounds.Left);
                        //drawing.Freeze();

                        //DrawingImage drawingImage = new DrawingImage(drawing);
                        //drawingImage.Freeze();

                        //Image image = new Image();
                        //image.Source = drawingImage;
                        //image.Width = r - l;

                        ////Align the image with the top of the bounds of the text geometry
                        //Canvas.SetLeft(image, g.Bounds.Left);
                        //Canvas.SetTop(image, g.Bounds.Top);

                        _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, border, null);
                    }
                }
            }
        }
    }
}
