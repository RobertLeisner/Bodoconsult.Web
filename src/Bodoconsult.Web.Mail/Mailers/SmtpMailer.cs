// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Zip;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Send an email via SMTP unsecured or via SSL secured
/// </summary>
public sealed class SmtpMailer: BaseMailer
{
    private SmtpClient _smtpClient;
    private readonly string _htmlMailTemplate;

    private SmtpMailAccount _currentMailAccount;

    /// <summary>
    /// Default SMTP mailer
    /// </summary>
    /// <param name="logger">Current logger</param>
    public SmtpMailer(IAppLoggerProxy logger): base(logger)
    {
        _htmlMailTemplate = MailHelper.GetTemplate("HtmlMail");
    }

    /// <summary>
    /// Initialize SMTP client before sending a mail
    /// </summary>
    public override void Init()
    {
        ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, sslPolicyErrors) => true;

        if (string.IsNullOrEmpty(_currentMailAccount.SmtpAccountName))
        {
            _smtpClient = new SmtpClient(_currentMailAccount.SmtpServer)
            {
                Credentials = CredentialCache.DefaultNetworkCredentials,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _currentMailAccount.UseSecureConnection,
                    
            };
        }
        else
        {
            _smtpClient = new SmtpClient(_currentMailAccount.SmtpServer)
            {
                Credentials = new NetworkCredential(_currentMailAccount.SmtpAccountName, _currentMailAccount.SmtpPassword),
                EnableSsl = _currentMailAccount.UseSecureConnection
            };
        }
    }

    /// <summary>
    /// Send an email based on an HTML body string
    /// </summary>
    /// <param name="to">Mail receiver</param>
    /// <param name="subject">Subject of the mail</param>
    /// <param name="body">HTML encoded text to send as mail</param>
    public override void SendMail(string to, string subject, string body)
    {
        try
        {
            var msg = new MailMessage
            {
                From = new MailAddress(CurrentMailAccount.MailAddressSender),
                Subject = subject,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = body
            };
            msg.To.Add(to);

            _smtpClient.Send(msg);
        }
        catch (Exception ex)
        {
            throw new Exception("Smtp mailing error", ex);
        }
    }

    //private static string StripHtml(string input)
    //{
    //    return Regex.Replace(input, "<.*?>", string.Empty);
    //}

    /// <summary>
    /// Send mail based on a <see cref="MailMessage"/> object
    /// </summary>
    /// <param name="message"></param>
    public void SendMail(MailMessage message)
    {
        try
        {
            _smtpClient.Send(message);
        }
        catch (Exception ex)
        {
            throw new Exception("Smtp mailing error", ex);
        }
    }

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on error else false</returns>
    public override bool SendMail(MailItem mailItem)
    {
        //return  string.IsNullOrEmpty(logoPath) ? SendMailPlain(to, subject, body, signatureTemplate, attachments) : SendMailLogo(to, subject, body, logoPath, signatureTemplate, attachments);

        //try
        //{
        var msg = new MailMessage
        {
            From = new MailAddress(CurrentMailAccount.MailAddressSender),
            Subject = mailItem.Subject,
            IsBodyHtml = true
        };

        msg.To.Add(mailItem.To.Replace(";", ","));
        msg.Subject = mailItem.Subject;
        msg.IsBodyHtml = true;


        if (!string.IsNullOrEmpty(mailItem.Logo))
        {

            // Plain HTML ink
            if (mailItem.Logo.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                msg.Body = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, mailItem.Logo, _htmlMailTemplate);

            }
            // Local file as logo
            else
            {
                var inlineLogo = new LinkedResource(mailItem.Logo) { ContentId = Guid.NewGuid().ToString() };

                var body = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, $"cid:{inlineLogo.ContentId}", _htmlMailTemplate);
                msg.BodyEncoding = Encoding.UTF8;

                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                msg.AlternateViews.Add(view);
            }
        }
        else
        {
            msg.Body = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, string.Empty, _htmlMailTemplate);
        }


        if (!string.IsNullOrEmpty(mailItem.Attachments))
        {
            var files = mailItem.Attachments.Split([';'], StringSplitOptions.RemoveEmptyEntries);

            if (mailItem.Zip)
            {
                var zipFileStream = new MemoryStream();

                // Password handling missing
                var zh = new ZipHandler(files);
                zh.GenerateZip(zipFileStream);

                var att = new Attachment(zipFileStream, new ContentType("application/zip"))
                {
                    Name = "data.zip"
                };

                msg.Attachments.Add(att);
            }
            else
            {
                foreach (var file in files.Where(file => !string.IsNullOrEmpty(file)))
                {
                    msg.Attachments.Add(new Attachment(file));
                }
            }
        }


        try
        {
            SendMail(msg);
            return false;
        }
        catch (Exception ex)
        {
            Logger.LogError("Sending message failed", ex);
            return true;
        }
        //}
        //catch (Exception e)
        //{
        //    _logger.Error("SendMailException", e);
        //    return true;
        //}
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

    /// <summary>
    /// Dispose smtp client
    /// </summary>
    public override void Dispose()
    {
        try
        {
            _smtpClient.Dispose();
        }
        catch
        {
            // ignored
        }
    }
}