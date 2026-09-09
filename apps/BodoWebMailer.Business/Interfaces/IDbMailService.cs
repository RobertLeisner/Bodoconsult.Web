// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using BodoWebMailer.Business.Models;

namespace BodoWebMailer.Business.Interfaces;

/// <summary>
/// Interface for mail services
/// </summary>
public interface IMailService
{
    /// <summary>
    /// Get all mails to send
    /// </summary>
    /// <returns></returns>
    IList<MailItem> GetMails();

    /// <summary>
    /// Archive a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    void ArchiveMail(MailItem mailItem);

    /// <summary>
    /// Set an error for a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    void SetError(MailItem mailItem);

    /// <summary>
    /// Add a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    void AddMailItem(MailItem mailItem);

    /// <summary>
    /// Run a SQL query to an HTML string
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="sql">SQL statement to export</param>
    /// <returns>Resulting HTML string</returns>
    string RunSqlQueryAsHtml(string title, string sql);

    /// <summary>
    /// Run SQL query 
    /// </summary>
    /// <param name="fileName">Filename</param>
    /// <param name="sql">SQL statement to export</param>
    /// <returns>Resulting CSV string</returns>
    string RunSqlQueryToCsvFile(string fileName, string sql);

    /// <summary>
    /// Clear all mail items
    /// </summary>
    void ClearMailItems();

}