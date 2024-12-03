using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MikroTikAdmin.Views;
using WpfAppTemplate.ViewModels;

namespace WpfAppTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHostBuilder hostBuilder;
        private IHost host;

        public App()
        {
            hostBuilder = Host.CreateDefaultBuilder();
        }

        internal string? StartupPage { get; set; }
        internal FlowDirection InitialFlowDirection { get; set; }

        public static IServiceProvider ServiceProvider { get; private set; }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            host = hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
            }).ConfigureServices((context, services) => ConfigureServices(context, services))
                .Build();


            ServiceProvider = host.Services;

            await host.StartAsync();

            MainWindow mainWindow = host.Services.GetRequiredService<MainWindow>();

            base.OnStartup(e);

            mainWindow.Show();

            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddOptions();

            services.AddLogging();

            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<CultureViewModel>();
            services.AddTransient<ContentControl>();
            services.AddTransient<ContentViewModel>();
        }

        private void Dispatcher_UnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs ex)
        {
            Trace.WriteLine(
                $"Dispatcher_UnhandledException: {ex.Exception.Message}{Environment.NewLine}{ex.Exception}");
            ex.Handled = true;
            string message = $"{ex.Exception.Message}{Environment.NewLine}{ex.Exception}";
            WriteLogMessage(message);
        }

        private void WriteLogMessage(string message)
        {
            string fileName = string.Concat("app_crash_report.txt");

            try
            {
                File.AppendAllText(fileName, $"\n{DateTime.Now} {message}");
            }
            catch (Exception e)
            {
            }
        }
    }
}
