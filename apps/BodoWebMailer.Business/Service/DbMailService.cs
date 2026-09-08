// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Web.Html.Html;
using Bodoconsult.Web.Html.HtmlTables;
using BodoWebMailer.Business.Helpers;
using BodoWebMailer.Business.Model;

namespace BodoWebMailer.Business.Service;

/// <summary>
/// Service for mail handling in a SqlServer database
/// </summary>
public sealed class DbMailService : IMailService
{

    private readonly IConnManager _db;

    public DbMailService(string connection)
    {
        _db = SqlClientConnManager.GetConnManager(connection);

#if DEBUG
        _db.Exec("INSERT INTO [dbo].[tMail] ([M_From],[M_To],[M_Subject],[M_Body]) VALUES ('noreply@bodoconsult.de','test@bodoconsult.de','Test','Testbody')");
#endif
    }


    public IList<MailItem> GetMails()
    {
        var erg = new List<MailItem>();

        // Mails einlesen
        var dt = _db.GetDataTable("EXEC dbo.spMail_FetchAll");
        foreach(System.Data.DataRow row in dt.Rows)
        {
            var mi = new MailItem
            {
                MailGuid = row["M_ID"].ToString(),
                From = row["M_From"].ToString(),
                To = row["M_To"].ToString(),
                Subject = row["M_Subject"].ToString(),
                Body = row["M_Body"].ToString(),
                Logo = row["M_LogoPath"].ToString(),
                Attachments = row["M_Attachments"].ToString(),
                SignatureTemplate = row["M_SignatureTemplate"].ToString(),
                Archive = Convert.ToBoolean(row["M_Archive"]),
                Queries = row["M_Queries"].ToString(),
                Zip= Convert.ToBoolean(row["M_Zip"].ToString()),
                ZipPassword = PasswordHandler.Decrypt(row["M_ZipPassword"].ToString()),
            };

            erg.Add(mi);
        }
        dt.Dispose();

        return erg;
    }


    public void ArchiveMail(MailItem mailItem)
    {
        var sql = (mailItem.Archive)
            ? $"EXEC dbo.spMail_MoveToArchive '{mailItem.MailGuid}'"
            : $"EXEC dbo.spMail_Delete '{mailItem.MailGuid}'";
        _db.Exec(sql);
    }


    public void SetError(MailItem mailItem)
    {
        var sql = $"EXEC dbo.spMail_SetError '{mailItem.MailGuid}'";
        _db.Exec(sql);
    }

    public void AddMailItem(MailItem mailItem)
    {
        var sql = "INSERT INTO [dbo].[tMail] " +
                  "([M_ID] " +
                  ",[M_From] " +
                  ",[M_To] " +
                  ",[M_Subject] " +
                  ",[M_Body] " +
                  ",[M_Error] " +
                  ",[M_LogoPath] " +
                  ",[M_SignatureTemplate] " +
                  ",[M_Attachments] " +
                  ",[M_Archive] " +
                  ",[M_Queries]) " +
                  "VALUES " +
                  "({0} " +
                  ",{1} " +
                  ",{2} " +
                  ",{3} " +
                  ",{4} " +
                  ", 0 " +
                  ",{5} " +
                  ",{6} " +
                  ",{7} " +
                  ", 0 " +
                  ",{8}) ";


        sql = string.Format(sql,
            GetValue(mailItem.MailGuid),
            GetValue(mailItem.From),
            GetValue(mailItem.To),
            GetValue(mailItem.Subject),
            GetValue(mailItem.Body),
            GetValue(mailItem.Logo),
            GetValue(mailItem.SignatureTemplate),
            GetValue(mailItem.Attachments),
            GetValue(mailItem.Queries));

        _db.Exec(sql);
    }

    private static string GetValue(string value)
    {
        return string.IsNullOrEmpty(value) ? "null" : "'" + value + "'";
    }


    public string RunSqlQueryAsHtml(string title, string sql)
    {
        var erg = $"<h1>{title}<h1>";

        var style = new Stylesheet();
        style.CssClassTable("bordered");

        erg += TableFormatter.FormatAsHtml(_db.GetDataTable(sql), true, style);

        return erg;
    }

    public string RunSqlQueryToCsvFile(string fileName, string sql)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), fileName + ".csv");

        if (File.Exists(tempPath))
        {
            File.Delete(tempPath);
        }

        var content = TableFormatter.FormatAsCsv(_db.GetDataTable(sql), true);

        // us-ascii
        var sw = new StreamWriter(tempPath, false, Encoding.GetEncoding("utf-8"));
        sw.Write(content);
        sw.Close();

        return tempPath;
    }

    public void ClearMailItems()
    {
#if DEBUG
        _db.Exec("delete from dbo.tMail");
#endif
    }
}