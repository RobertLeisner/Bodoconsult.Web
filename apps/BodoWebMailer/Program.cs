// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using BodoWebMailer.Business;
using BodoWebMailer.Business.App;
using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using Bodoconsult.App.Extensions;
using BodoWebMailer.Business.Services;

namespace BodoWebMailer;

[SupportedOSPlatform("windows10.0")]
internal class Program
{

    private static readonly ILog Logger;

    private static int Main(string[] args)
    {
        //XmlDocument log4netConfig = new XmlDocument();
        //log4netConfig.Load(File.OpenRead("log4net.config"));

        //var repo = LogManager.CreateRepository(
        //    Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

        //XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

        //Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        //Logger.Info("Starts...");

        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "/p": // Ask user for password, encrypt it and copy it to clipboard
                    ConsoleHelper.EncryptPassword();
                    return 0;
                default:
                    break;
            }

        }

        Debug.Print("Hello, World!");

        Console.WriteLine("ConsoleApp1 initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

        const string performanceToken = "--PERF";

        if (args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        IAppBuilder builder = new BodoWebMailerAppBuilder(globals);
#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

        // Load basic app metadata
        builder.LoadBasicSettings();

        // Process the config file
        builder.ProcessConfiguration();

        // Now load the globally needed settings
        builder.LoadGlobalSettings();

        // Write first log entry with default logger
        ArgumentNullException.ThrowIfNull(globals.Logger);
        globals.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
        globals.StatusMessageDelegate = ShowStatus;

        Console.WriteLine("Logging started...");

        // App is ready now for doing something
        Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

        Console.WriteLine("");
        Console.WriteLine("");

        Console.WriteLine($"App name loaded: {param.AppName}");
        Console.WriteLine($"App version loaded: {param.AppVersion}");
        Console.WriteLine($"App path loaded: {param.AppPath}");

        Console.WriteLine("");
        Console.WriteLine("");

        Console.WriteLine($"Logging config: {ObjectHelper.GetObjectPropertiesAsString(Globals.Instance.LoggingConfig)}");

        // Prepare the DI container package
        builder.LoadDiContainerServiceProviderPackage();
        builder.RegisterDiServices();
        builder.FinalizeDiContainerSetup();

        // Now finally start the app and wait
        builder.StartApplication();

        Environment.Exit(0);



        //// Load configuration
        ////var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        ////XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        //Status("BodoWebMailer started...");

        //GlobalValues.LoadAppSettings();
        //var appSettings = GlobalValues.CurrentAppSettings;

        ////log4net.Config.BasicConfigurator.Configure();



        //Status(appSettings.ConnectionString);


        //ShowStatus("Get mails to send...");
        //var mailhandler = new MailHandler(new DbMailService(appSettings.ConnectionString), appSettings.CurrentMailAccount)
        //{
        //    AdminMailAddress = appSettings.AdminMailAddress,
        //};

        //mailhandler.StatusChanged += ShowStatus;
        //mailhandler.StartMailing();
        //ShowStatus("Mails sent. Program quits...");

        ////Logger.Info("Done!");

        return 0;
    }

    internal static void ShowStatus(string message)
    {
        Console.WriteLine(message);
        //Logger.Info(message);
    }
}