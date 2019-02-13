using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Pomo_Shiny
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [ExcludeFromCodeCoverage]
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(new ApplicationAccessor(), new CountdownTimer());
            InitializeComponent();
        }
    }
}