using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ponydoro_Common;

namespace Ponydoro
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider ServiceProvider { get; set; }
        private IConfiguration Configuration { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            Configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IApplicationAccessor, ApplicationAccessor>();
            services.AddTransient<ICountdownTimer, CountdownTimer>();
            services.AddTransient<ITimerFacade, TimerFacade>();
            services.AddTransient<ISoundProvider, SoundProvider>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>();

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        }
    }
}