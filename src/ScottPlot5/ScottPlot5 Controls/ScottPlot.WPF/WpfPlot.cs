using System.Windows;
using SkiaSharp;

namespace ScottPlot.WPF
{
    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    [TemplatePart(Name = PART_SKElement, Type = typeof(SkiaSharp.Views.WPF.SKElement))]
    public class WpfPlot : WpfPlotBase
    {
        private const string PART_SKElement = "PART_SKElement";

        private SkiaSharp.Views.WPF.SKElement? SKElement;
        protected override FrameworkElement PlotFrameworkElement => SKElement!;

        public override GRContext GRContext => null!;

        static WpfPlot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                forType: typeof(WpfPlot),
                typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlot)));
        }

        public override void OnApplyTemplate()
        {
            SKElement = Template.FindName(PART_SKElement, this) as SkiaSharp.Views.WPF.SKElement;

            if (SKElement == null)
                return;

            SKElement.PaintSurface += (sender, e) =>
            {
                float width = (float)e.Surface.Canvas.LocalClipBounds.Width;
                float height = (float)e.Surface.Canvas.LocalClipBounds.Height;
                PixelRect rect = new(0, width, height, 0);
                Multiplot.Render(e.Surface.Canvas, rect);
            };

            SKElement.MouseDown += SKElement_MouseDown;
            SKElement.MouseUp += SKElement_MouseUp;
            SKElement.MouseMove += SKElement_MouseMove;
            SKElement.MouseWheel += SKElement_MouseWheel;
            SKElement.KeyDown += SKElement_KeyDown;
            SKElement.KeyUp += SKElement_KeyUp;
            SKElement.LostMouseCapture += SKElement_LostFocus;
            SKElement.LostKeyboardFocus += SKElement_LostFocus;
        }

        public override void Refresh()
        {
            if (!CheckAccess())
            {
                Dispatcher.BeginInvoke(Refresh);
                return;
            }

            SKElement?.InvalidateVisual();
        }

        // CurrentInstance: provides a property e.g. to use in MVVM view models to avoid having
        // to add code-behind for forwarding the control instance.

        public WpfPlot() : base()
        {
            // Defer updating the property value because WPF will set the bound property value
            // to null on load / creation at InitializeComponent.
            // This InvokeAsync will update it afterwards to the correct value.
            Application.Current?.Dispatcher.InvokeAsync(
                () => { SetValue(CurrentInstanceProperty, this); },
                System.Windows.Threading.DispatcherPriority.Loaded
            );
        }

        public static readonly DependencyProperty CurrentInstanceProperty =
           DependencyProperty.Register(nameof(CurrentInstance), typeof(WpfPlot), typeof(WpfPlot),
               new FrameworkPropertyMetadata(defaultValue: null));

        public WpfPlot? CurrentInstance
        {
            get
            {
                return this;
            }
            set
            {
                SetValue(CurrentInstanceProperty, this);
            }
        }
    }
}
