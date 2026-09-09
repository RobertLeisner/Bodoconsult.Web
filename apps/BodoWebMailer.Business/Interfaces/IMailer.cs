// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using Bodoconsult.Web.Mail.Model;
using BodoWebMailer.Business.Models;

namespace BodoWebMailer.Business.Interfaces;

/// <summary>
/// Interface for mailer implemenations sending the mails via SMTP, ...
/// </summary>
public interface IMailer : IDisposable
{
    /// <summary>
    /// Current mail account
    /// </summary>
    MailAccount CurrentMailAccount { get; }

    /// <summary>
    /// Init the mail account
    /// </summary>
    void Init();

    /// <summary>
    /// Logon to mailserver
    /// </summary>
    void Logon();

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on success else false</returns>
    bool SendMail(MailItem mailItem);
}