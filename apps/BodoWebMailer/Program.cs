// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Runtime.Versioning;
using BodoWebMailer.Business;
using BodoWebMailer.Business.App;
using BodoWebMailer.Business.Service;
using log4net;

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

        // Load configuration
        //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        Status("BodoWebMailer started...");

        GlobalValues.LoadAppSettings();
        var appSettings = GlobalValues.CurrentAppSettings;

        //log4net.Config.BasicConfigurator.Configure();



        Status(appSettings.ConnectionString);


        Status("Get mails to send...");
        var mailhandler = new MailHandler(new DbMailService(appSettings.ConnectionString), appSettings.CurrentMailAccount)
        {
            AdminMailAddress = appSettings.AdminMailAddress,
        };

        mailhandler.StatusChanged += Status;
        mailhandler.StartMailing();
        Status("Mails sent. Program quits...");

        //Logger.Info("Done!");

        return 0;
    }

    internal static void Status(string message)
    {
        Console.WriteLine(message); 
        //Logger.Info(message);
    }
}