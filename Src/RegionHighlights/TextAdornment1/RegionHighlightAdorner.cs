using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace RegionHighlightAdornment
{
    /// <summary>
    /// RegionHighlightAdorner places a border behind region-like text.
    /// </summary>
    public class RegionHighlightAdorner
    {
        private static readonly Color BorderLineColor = Color.FromArgb(148, 103, 121, 255);
        private static readonly Color BorderBackgroundColor = Color.FromArgb(11, 0, 0, 255);
        private static readonly Brush BorderLineBrush;
        private static readonly Brush BorderBackgroundBrush;
        private static readonly Thickness BorderThicknessRegion = new Thickness(0, 2, 0, 0);
        private static readonly Thickness BorderThicknessEndRegion = new Thickness(0, 0, 0, 2);

        private readonly IAdornmentLayer _adornmentLayer;
        private readonly IWpfTextView _textView;

        static RegionHighlightAdorner()
        {
            BorderLineBrush = new SolidColorBrush(BorderLineColor);
            BorderLineBrush.Freeze();
            BorderBackgroundBrush = new SolidColorBrush(BorderBackgroundColor);
            BorderBackgroundBrush.Freeze();
        }

        public RegionHighlightAdorner(IWpfTextView view)
        {
            _textView = view;
            _textView.LayoutChanged += OnLayoutChanged;
            _adornmentLayer = view.GetAdornmentLayer("RegionHighlightAdornmentLayer");
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
        /// If line contains # region indicators, then add region adornment border to text.
        /// </summary>
        private void CreateVisuals(ITextViewLine currentLine)
        {
            //grab a reference to the lines in the current TextView 
            IWpfTextViewLineCollection textViewLines = _textView.TextViewLines;

            for (int i = currentLine.Start; (i + 6 < currentLine.End); ++i)
            {
                var matchType = TestRegionStringMatch(i);
                if (matchType == RegionMatchType.None) continue;

                var textSpan = new SnapshotSpan(_textView.TextSnapshot, Span.FromBounds(i, i + 6));
                var spanGeometry = textViewLines.GetMarkerGeometry(textSpan);
                if (spanGeometry == null) continue;

                AddBorderToText(currentLine, matchType, spanGeometry, textSpan);
            }
        }

        private void AddBorderToText(ITextViewLine line, RegionMatchType matchType, Geometry spanGeometry,
                                     SnapshotSpan textSpan)
        {
            var borderTop = matchType == RegionMatchType.Region || matchType == RegionMatchType.PragmaRegion
                ? spanGeometry.Bounds.Top - 1
                : spanGeometry.Bounds.Top;
            var borderLeft = spanGeometry.Bounds.Left;

            var border = new Border()
            {
                Width = 1000,
                Height = line.Bottom - line.Top + 2,
                BorderThickness = matchType == RegionMatchType.Region || matchType == RegionMatchType.PragmaRegion ? BorderThicknessRegion : BorderThicknessEndRegion,
                BorderBrush = BorderLineBrush,
                Background = BorderBackgroundBrush
            };
            Canvas.SetTop(border, borderTop);
            Canvas.SetLeft(border, borderLeft);
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.TextRelative, textSpan, null, border, null);
        }

        /// <summary>
        /// Test text at position for region-like text.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private RegionMatchType TestRegionStringMatch(int position)
        {
            if (TextMatchesRegionString(position)) return RegionMatchType.Region;
            if (TextMatchesEndRegionString(position)) return RegionMatchType.EndRegion;
            if (TextMatchesPragmaRegionString(position)) return RegionMatchType.PragmaRegion;
            if (TextMatchesPragmaEndRegionString(position)) return RegionMatchType.PragmaEndRegion;
            return RegionMatchType.None;
        }

        private bool TextMatchesRegionString(int i) => _textView.TextSnapshot[i] == '#' && _textView.TextSnapshot[i + 1] == 'r' && _textView.TextSnapshot[i + 2] == 'e' && _textView.TextSnapshot[i + 3] == 'g' && _textView.TextSnapshot[i + 4] == 'i' && _textView.TextSnapshot[i + 5] == 'o' && _textView.TextSnapshot[i + 6] == 'n';

        private bool TextMatchesPragmaRegionString(int i) => _textView.TextSnapshot[i] == '#' && _textView.TextSnapshot[i + 1] == 'p' && _textView.TextSnapshot[i + 2] == 'r' && _textView.TextSnapshot[i + 3] == 'a' && _textView.TextSnapshot[i + 4] == 'g' && _textView.TextSnapshot[i + 5] == 'm' && _textView.TextSnapshot[i + 6] == 'a' && _textView.TextSnapshot[i + 7] == ' ' && _textView.TextSnapshot[i + 8] == 'r' && _textView.TextSnapshot[i + 9] == 'e' && _textView.TextSnapshot[i + 10] == 'g' && _textView.TextSnapshot[i + 11] == 'i' && _textView.TextSnapshot[i + 12] == 'o' && _textView.TextSnapshot[i + 13] == 'n';

        private bool TextMatchesEndRegionString(int i) => _textView.TextSnapshot[i] == '#' && _textView.TextSnapshot[i + 1] == 'e' && _textView.TextSnapshot[i + 2] == 'n' && _textView.TextSnapshot[i + 3] == 'd' && _textView.TextSnapshot[i + 4] == 'r' && _textView.TextSnapshot[i + 5] == 'e' && _textView.TextSnapshot[i + 6] == 'g'
                                                          && _textView.TextSnapshot[i + 7] == 'i' && _textView.TextSnapshot[i + 8] == 'o' && _textView.TextSnapshot[i + 9] == 'n';
        private bool TextMatchesPragmaEndRegionString(int i) => _textView.TextSnapshot[i] == '#' && _textView.TextSnapshot[i + 1] == 'p' && _textView.TextSnapshot[i + 2] == 'r' && _textView.TextSnapshot[i + 3] == 'a' && _textView.TextSnapshot[i + 4] == 'g' && _textView.TextSnapshot[i + 5] == 'm' && _textView.TextSnapshot[i + 6] == 'a' && _textView.TextSnapshot[i + 7] == ' ' && _textView.TextSnapshot[i + 8] == 'e' && _textView.TextSnapshot[i + 9] == 'n' && _textView.TextSnapshot[i + 10] == 'd' && _textView.TextSnapshot[i + 11] == 'r' && _textView.TextSnapshot[i + 12] == 'e' && _textView.TextSnapshot[i + 13] == 'g' && _textView.TextSnapshot[i + 14] == 'i' && _textView.TextSnapshot[i + 15] == 'o' && _textView.TextSnapshot[i + 16] == 'n';

        private enum RegionMatchType
        {
            None,
            Region,
            EndRegion,
            PragmaRegion,
            PragmaEndRegion
        }
    }
}
