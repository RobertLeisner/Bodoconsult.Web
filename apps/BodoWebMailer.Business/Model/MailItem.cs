// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoWebMailer.Business.Model;

/// <summary>
/// Represents data for an email to send
/// </summary>
public sealed class MailItem
{
    public string MailGuid;
    public string From;
    public string To;
    public string Subject;
    public string Body;
    public string Logo;
    public string Attachments;
    public string SignatureTemplate;
    public bool Archive;
    public string Queries;
    /// <summary>
    /// Zip the attachments
    /// </summary>
    public bool Zip;

    /// <summary>
    /// Use this password for protecting the ZIP file with attachments
    /// </summary>
    public string ZipPassword;
}