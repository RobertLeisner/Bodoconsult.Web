// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bodoconsult.Web.Mail.Model;
using BodoWebMailer.Business.Model;
using BodoWebMailer.Business.Service;
using log4net;
using Newtonsoft.Json;

namespace BodoWebMailer.Business;

/// <summary>
/// Handles the mails received from a database source
/// </summary>
public sealed class MailHandler
{

    public event StatusMessage StatusChanged;

    private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


    private readonly Mailer _mailer;

    private readonly IMailService _service;

    private readonly IList<string> _tempFiles = new List<string>();


    public string AdminMailAddress;

    public MailAccount CurrentMailAccount { get; private set; }

    public MailHandler(IMailService service, MailAccount currentMailAccount)
    {
        _service = service;
        CurrentMailAccount = currentMailAccount;
        _mailer = new Mailer(currentMailAccount);
    }



    public void StartMailing()
    {
        Status("Open database...");
        var mailItems = _service.GetMails();

        if (!mailItems.Any()) return;

        _mailer.Init();


        Status("Database opened...");
        _logger.Info("database opened");


        // Mails verarbeiten
        foreach (var mailItem in mailItems)
        {
            var item = mailItem;

            var msg = $"Mail to {item.To}: {item.Subject}";
            _logger.Info(msg);
            Status(msg);


            PrepareQueries(item);

            _mailer.CurrentMailAccount.MailAddressSender = item.From;
            try
            {
                var erg = _mailer.SendMail(item);

                if (!erg) _service.ArchiveMail(item);

            }
            catch (Exception ex)
            {
                msg = "BodoWebMailer:Error:" + item.MailGuid + ":" + msg;

                Status(msg);
                _logger.Error(msg, ex);

                try
                {
                    var adminMail = new MailItem
                    {
                        MailGuid = Guid.NewGuid().ToString(),
                        To = AdminMailAddress,
                        Subject = "BodoWebMailer:Error",
                        Body = msg
                    };

                    _mailer.SendMail(adminMail);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }

                _service.SetError(item);
            }
        }

        // Delete temp files
        foreach (var path in _tempFiles)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                // ignored
            }
        }

        Status("All mails sent...");
    }

    private void PrepareQueries(MailItem item)
    {

        if (string.IsNullOrEmpty(item.Queries))
        {
            return;
        }

        var queries = (Dictionary<string, string>)JsonConvert.DeserializeObject(item.Queries, typeof(Dictionary<string, string>));

        if (queries == null)
        {
            return;
        }

        // Save Query as file
        if (queries.ContainsKey("ToCsvFile"))
        {

            foreach (var query in queries.Where(x => x.Key != "ToCsvFile"))
            {
                var path = _service.RunSqlQueryToCsvFile(query.Key, query.Value);

                item.Attachments += $"{path};";
                _tempFiles.Add(path);
            }

            return;
        }

        var content = "";

        foreach (var query in queries)
        {
            content += _service.RunSqlQueryAsHtml(query.Key, query.Value);
        }


        item.Body = string.IsNullOrEmpty(item.Body) ?
            content :
            item.Body += content;
    }

    private void Status(string message)
    {
        var x = StatusChanged;
        x?.Invoke(message);
    }


    public delegate void StatusMessage(string message);
}