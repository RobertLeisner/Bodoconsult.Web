# What does the library

Bodoconsult.Core.Web.Mail library is intended for apps which have to send (but not receive) SMTP mails like console apps.

Mass mail handling features are in an experimental state or work is in progress.

# How to use the library

The source code contain a NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Send a simple mail with plain text

            var account = new MailAccount
            {
                SmtpServer = "smtp.test.de",
                SmtpPassword = "test123!",
                SmtpAccountName = "test@test.de",
                MailAddressSender = "noreply@test.de",
                UseSecureConnection = true
            };
			
            var smtp = new SmtpMailer(account);
            smtp.Init();

            smtp.SendMail("to@test.de", "Testmail", "dgdgdgdgdgs sfsgdgs sshshsh");

            Assert.IsTrue(true);

## Send a HTML mail

            var account = new MailAccount
            {
                SmtpServer = "smtp.test.de",
                SmtpPassword = "test123!",
                SmtpAccountName = "test@test.de",
                MailAddressSender = "noreply@test.de",
                UseSecureConnection = true
            };

            var msg = new MailMessage {IsBodyHtml = true};


            var add = new MailAddress(account.MailAddressSender);
            msg.From = add;

            add = new MailAddress(TestHelper.GetTestReceiver());
            msg.To.Add(add);

            msg.Subject = "Bodoconsult.Core.Web.Mail: test mail";
            msg.Body = "<p>ajHA SADad asd AS Ddad</p>";

            var smtp = new SmtpMailer(account);
            smtp.Init();

            smtp.SendMail(msg);


# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

