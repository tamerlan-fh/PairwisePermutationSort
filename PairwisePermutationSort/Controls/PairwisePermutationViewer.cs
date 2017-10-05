using PairwisePermutationSort.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PairwisePermutationSort.Controls
{
    class PairwisePermutationViewer : FrameworkElement
    {

        public static readonly DependencyProperty PairwisePermutationDataProperty = DependencyProperty.Register("PairwisePermutationData", typeof(SortingResult), typeof(PairwisePermutationViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(BuildingsPropertyChanged)));
        private static void BuildingsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //  (d as PairwisePermutationViewer).ПрименениеНовыхЗначений(e.NewValue as SortingResult);
            (d as PairwisePermutationViewer).Void();
        }

        public SortingResult PairwisePermutationData
        {
            get { return (SortingResult)GetValue(PairwisePermutationDataProperty); }
            set { SetValue(PairwisePermutationDataProperty, value); }
        }

        private const double d = 8;
        private const double dx = d;
        private const double dy = d;
        private const double dk = d / 5f;

        private Pen pen1 = new Pen(Brushes.Red, d / 10f);
        private Pen pen2 = new Pen(Brushes.Green, d / 10f);
        private Typeface typeface = new Typeface("Verdana");
        private void Void()
        {
            visuals.Clear();
            if (PairwisePermutationData == null || !PairwisePermutationData.Success) return;

            var visual = new DrawingVisual();

            var h = 3 * dy * PairwisePermutationData.Iterations.Length;
            var w = dx * PairwisePermutationData.Iterations.FirstOrDefault().NumbersArray.Length;

            this.Height = 2 * Math.Max(h, w);
            this.Width = 2 * Math.Max(h, w);

            var startPoint = new Point((Width - w) / 2, (Height - h) / 2);

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, Width, Height));

                for (int j = 0; j < PairwisePermutationData.Iterations.Length; j++)
                {
                    var iteration = PairwisePermutationData.Iterations[j];

                    for (int i = 0; i < iteration.NumbersArray.Length; i++)
                    {
                        var point = startPoint + new Vector(dx * i, 0);
                        var text = new FormattedText(iteration.NumbersArray[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, d, Brushes.Black);
                        dc.DrawText(text, point + new Vector(0.2 * dx, 0));
                        //  dc.DrawRectangle(null, pen, new Rect(point, new Size(dx, dy)));
                    }

                    if (iteration.IsLast) continue;

                    var point1 = startPoint + new Vector(iteration.IndexOfLeft * dx, dy + dk);
                    dc.DrawLine(pen1, point1, point1 + new Vector(2 * dx, 0));
                    dc.DrawLine(pen1, point1 + new Vector(0, pen1.Thickness / 2), point1 + new Vector(0, -dk));
                    dc.DrawLine(pen1, point1 + new Vector(2 * dx, pen1.Thickness / 2), point1 + new Vector(2 * dx, -dk));

                    var point2 = startPoint + new Vector(iteration.IndexOfRight * dx, dy + dk);
                    dc.DrawLine(pen2, point2, point2 + new Vector(2 * dx, 0));
                    dc.DrawLine(pen2, point2 + new Vector(0, pen2.Thickness / 2), point2 + new Vector(0, -dk));
                    dc.DrawLine(pen2, point2 + new Vector(2 * dx, pen2.Thickness / 2), point2 + new Vector(2 * dx, -dk));

                    var point3 = startPoint + new Vector(iteration.IndexOfLeft * dx, 3 * dy - dk);
                    dc.DrawLine(pen2, point3, point3 + new Vector(2 * dx, 0));
                    dc.DrawLine(pen2, point3 + new Vector(0, -pen2.Thickness / 2), point3 + new Vector(0, dk));
                    dc.DrawLine(pen2, point3 + new Vector(2 * dx, -pen2.Thickness / 2), point3 + new Vector(2 * dx, dk));

                    var point4 = startPoint + new Vector(iteration.IndexOfRight * dx, 3 * dy - dk);
                    dc.DrawLine(pen1, point4, point4 + new Vector(2 * dx, 0));
                    dc.DrawLine(pen1, point4 + new Vector(0, -pen1.Thickness / 2), point4 + new Vector(0, dk));
                    dc.DrawLine(pen1, point4 + new Vector(2 * dx, -pen1.Thickness / 2), point4 + new Vector(2 * dx, dk));

                    var vector = new Vector(dx, 0);
                    dc.DrawLine(pen1, point1 + vector, point4 + vector);
                    dc.DrawLine(pen2, point2 + vector, point3 + vector);

                    startPoint += new Vector(0, 3 * dy);
                }
            }

            visuals.Add(visual);
        }

        public PairwisePermutationViewer()
        {
            this.visuals = new VisualCollection(this);
        }

        private VisualCollection visuals;
        protected override int VisualChildrenCount { get { return visuals.Count; } }
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visuals.Count) { throw new ArgumentOutOfRangeException(); }

            return visuals[index];
        }
    }
}
