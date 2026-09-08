// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Mail.Model;

namespace BodoWebMailer.Business.App;

/// <summary>
/// Application settings
/// </summary>
public sealed class AppSettings
{

    public string ConnectionString { get; set; }


    public MailAccount CurrentMailAccount { get; set; }


    public string AdminMailAddress { get; set; }

}