using System.Windows;

namespace Optimus.Wpf;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel; // Aici legăm butoanele din design de codul din fundal
    }
}