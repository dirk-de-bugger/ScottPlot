using ScottPlot;
using System.Windows;

namespace WPF_Demo.DemoWindows;

public partial class Quickstart : Window, IDemoWindow
{
    public string DemoTitle => "WPF Quickstart";
    public string Description => "Create a simple plot using the WPF control.";

    public Quickstart()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());
    }
}

public class QuickstartViewModel
{
    WpfPlot? _wpfPlot1 = null;
    public WpfPlot? WpfPlot1Property
    {
        get => _wpfPlot1;

        set
        {
            // Note: set will be called twice.
            // First with value==null on load / creation (at InitializeComponent)
            // Second by WpfPlot with the proper instance value (at DispatcherPriority.Loaded,
            // if Application.Current?.Dispatcher exists and runs).

            _wpfPlot1 = value;

            // Thus null-checks are necessary (also in any other places that read _wpfPlot1 or WpfPlot1Property):
            _wpfPlot1?.Plot.Add.Signal(ScottPlot.Generate.SquareWave(cycles:5, pointsPerCycle:10));
            _wpfPlot1?.Refresh();
        }
    }
}
