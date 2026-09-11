// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Mail.Converters;
using Bodoconsult.Web.Mail.Mailers;
using Bodoconsult.Web.Mail.Models;
using Bodoconsult.Web.Mail.Test.App;
using Bodoconsult.Web.Mail.Test.Helpers;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Mail;

namespace Bodoconsult.Web.Mail.Test.Mailers;

[TestFixture]
public class SmtpMailerTests
{
    //private string _baseUrl = Path.Combine(TestHelper.TestDataPath, @"TestData\HtmlLocalData\");
    private readonly string _docUrl = Path.Combine(TestHelper.TestDataPath, @"TestData\HtmlLocalData\Sample.txt");

    [Test]
    public void SendMail_ValidParameters_MailSent()
    {
        var account = TestHelper.GetTestMailAccount();

        var smtp = new SmtpMailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);
        smtp.Init();

        smtp.SendMail(TestHelper.GetTestReceiver(), "Testmail", "dgdgdgdgdgs sfsgdgs sshshsh");

        Assert.That(true);
    }

    [Test]
    public void SendMail_ValidMailMessage_MailSent()
    {
        var account = TestHelper.GetTestMailAccount();

        // New message as HTML mail
        var msg = new MailMessage {IsBodyHtml = true};

        // From
        var add = new MailAddress(account.MailAddressSender);
        msg.From = add;

        // To
        add = new MailAddress(TestHelper.GetTestReceiver());
        msg.To.Add(add);

        // Subject and mail body
        msg.Subject = "Bodoconsult.Core.Web.Mail: test mail";
        msg.Body = "<p>ajHA SADad asd AS Ddad</p>";

        // Send the mail
        var smtp = new SmtpMailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);
        smtp.Init();

        smtp.SendMail(msg);

        Assert.That(true);
    }

    [Test]
    public void SendMail_ValidMassMailItem_MailSent()
    {

        var c = new HtmlToMailConverter { DocUrl = _docUrl };
        c.LoadDocument();
        c.FindImages();
        c.ProcessContent();

        var account = TestHelper.GetTestMailAccount();

        var m = new SmtpMailer(Globals.Instance.Logger);
        m.LoadMailAccount(account);

        var mmi = new MassMailItem
        {
            From = "noreply@bodoconsult.de",
            Subject = $"Testmail {DateTime.Now:s}",
            Body = c.Content
        };

        mmi.To.Add(new MailReceiver { EmailAddress = "robert.leisner@bodoconsult.de" });
        mmi.To.Add(new MailReceiver { EmailAddress = "info@bodoconsult.de" });
        mmi.To.Add(new MailReceiver { EmailAddress = "support@bodoconsult.de" });

        mmi.Images.AddRange(c.Images);

        m.SendMails(mmi);
    }
}