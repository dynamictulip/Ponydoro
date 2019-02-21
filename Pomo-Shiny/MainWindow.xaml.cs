using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

namespace Pomo_Shiny
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(new ApplicationAccessor(),
                new CountdownTimer(new TimerFacade(), new SoundProvider()));
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}