// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Send a mail via SMTP to a lot of receivers
/// </summary>
public sealed class MassSmtpMailer: BaseMailer, IMassMailer
{
    private SmtpMailAccount _currentMailAccount;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="logger">Current logger</param>
    public MassSmtpMailer(IAppLoggerProxy logger): base(logger)
    {
        To = new List<MailReceiver>();
        DefaultSalutation = "Sehr geehrte Damen und Herren";
    }

    /// <summary>
    /// HTML-Body für eMails
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Subject for the mail
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Sending mail address
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Default salutation tu use: default is "Sehr geehrte Damen und Herren"
    /// </summary>
    public string DefaultSalutation { get; set; }

    /// <summary>
    /// All mail addresses the mail will be sent to
    /// </summary>
    public IList<MailReceiver> To { get; set; }

    /// <summary>
    /// Contains all found images in the document
    /// </summary>
    public IList<ImageMetaData> Images { get; set; }

    /// <summary>
    /// Send mail to all mail addresses registered in <see cref="To"/>
    /// </summary>
    public void SendMails()
    {
        var smtpClient = new SmtpClient(_currentMailAccount.SmtpServer)
        {
            Credentials = new NetworkCredential(_currentMailAccount.SmtpAccountName, _currentMailAccount.SmtpPassword),
            EnableSsl = _currentMailAccount.UseSecureConnection
        };

        // 1. Send emails to receivers with no salutation
        if (To.Any(x => string.IsNullOrEmpty(x.Salutation)))
        {
            SendWithNoSalutation(smtpClient);
        }
        
        // 2. Send emails to receivers with salutation
        SendWithSalutation(smtpClient);

        smtpClient.Dispose();
    }

    private void SendWithSalutation(SmtpClient smtpClient)
    {
        foreach (var mailReciever1 in To.Where(x => !string.IsNullOrEmpty(x.Salutation)))
        {
            var msg = new MailMessage
            {
                From = new MailAddress(From),
                Subject = Subject,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };

            msg.Bcc.Add(mailReciever1.EmailAddress);

            //string txtBody = "See this email online here: " + messageURL; 
            //AlternateView plainView = AlternateView.CreateAlternateViewFromString(txtBody, null, "text/plain"); 

            var htmlView = AlternateView.CreateAlternateViewFromString(Body.Replace("??address??", mailReciever1.Salutation), null, "text/html");
            AddImages(Images, htmlView);
            msg.AlternateViews.Add(htmlView);

            // Send the mail
            smtpClient.Send(msg);
        }
    }

    private static void AddImages(IList<ImageMetaData> images, AlternateView htmlView)
    {
        foreach (var image in images)
        {
            var imagelink = new LinkedResource(image.Url)
            {
                ContentId = image.ContentId,
                //ContentLink = new Uri("cid:" + image.ContentId),
                //TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };

            htmlView.LinkedResources.Add(imagelink);
        }
    }

    private void SendWithNoSalutation(SmtpClient smtpClient)
    {
        // build the message to send
        var msg = new MailMessage
        {
            From = new MailAddress(From)
        };
            
        foreach (var mailReciever in To.Where(x => string.IsNullOrEmpty(x.Salutation)))
        {
            msg.Bcc.Add(mailReciever.EmailAddress);
        }

        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.BodyEncoding = Encoding.UTF8;

        //string txtBody = "See this email online here: " + messageURL; 
        //AlternateView plainView = AlternateView.CreateAlternateViewFromString(txtBody, null, "text/plain"); 

        var htmlView = AlternateView.CreateAlternateViewFromString(Body.Replace("??address??", DefaultSalutation), null, "text/html");
        AddImages(Images, htmlView);
        msg.AlternateViews.Add(htmlView);

        // Send the mail
        smtpClient.Send(msg);
    }

    /// <summary>
    /// Load mail account data from a JSON string
    /// </summary>
    /// <param name="json">JSON string with fail account data</param>
    public override void LoadMailAccount(string json)
    {
        var ad = JsonHelper.LoadJsonFromString<SmtpMailAccount>(json);
        CurrentMailAccount = ad;
        _currentMailAccount = ad;
    }

    /// <summary>
    /// Load mail account
    /// </summary>
    /// <param name="mailAccount">Mail account instance</param>
    public override void LoadMailAccount(IMailAccount mailAccount)
    {
        CurrentMailAccount = mailAccount;

        if (mailAccount is not SmtpMailAccount o365)
        {
            throw new ArgumentException("mailAccount is not SmtpMailAccount");
        }

        _currentMailAccount = o365;
    }
}