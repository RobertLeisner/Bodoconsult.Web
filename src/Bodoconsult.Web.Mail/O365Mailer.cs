// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bodoconsult.Web.Mail.Model;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Bodoconsult.Web.Mail;

/// <summary>
/// Implementation of an Office 365 Graph based mailer
/// </summary>
public class O365Mailer
{

    // Even if this is a console application here, a daemon application is a confidential client application
    private GraphServiceClient _app;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="mailAccount"></param>
    public O365Mailer(O365MailAccount mailAccount)
    {
        MailAccount = mailAccount;
    }

    /// <summary>
    /// Current O365 mail account
    /// </summary>
    public O365MailAccount MailAccount { get; }

    /// <summary>
    /// Login to O365 Graph API
    /// </summary>
    /// <returns>Awaitable task</returns>
    public void Login()
    {
        var tenantId = MailAccount.Tenant;

        var authenticationProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider(MailAccount.ClientId, MailAccount.ClientSecret, tenantId));

        _app = new GraphServiceClient(authenticationProvider);
    }

    /// <summary>
    /// Send an email over an O365 account
    /// </summary>
    /// <param name="to">Mail receiver separated by semmicolon</param>
    /// <param name="subject">Mail subject</param>
    /// <param name="content">Mail content with full HTML markup for a webpage</param>
    public void SendMail(string to, string subject, string content)
    {

        // Define a simple e-mail message.
        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = content
            },
        };

        var receips = to.Split(new[] { ';' }).Select(receiver => new Recipient { EmailAddress = new EmailAddress { Address = receiver } }).ToList();

        message.ToRecipients = receips;

        // Send mail as the given user. 
        SendMail(message);

    }

    /// <summary>
    /// Send a mail message via O365
    /// </summary>
    /// <param name="message">Message to be sent</param>
    public void SendMail(Message message)
    {
        // Send mail as the given user. 
        _app.Users[MailAccount.UserName].SendMail.PostAsync(new SendMailPostRequestBody
        {
            Message = message,
        }).GetAwaiter().GetResult();
    }

}

/// <summary>
/// Current token provider
/// </summary>
internal class TokenProvider : IAccessTokenProvider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tenantId;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <param name="clientSecret">Client secret</param>
    /// <param name="tenantId">Tenenat ID</param>
    public TokenProvider(string clientId, string clientSecret, string tenantId)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _tenantId = tenantId;
    }

    /// <summary>
    ///     This method is called by the <see cref="T:Microsoft.Kiota.Abstractions.Authentication.BaseBearerTokenAuthenticationProvider" /> class to get the access token.
    /// </summary>
    /// <param name="uri">The target URI to get an access token for.</param>
    /// <param name="additionalAuthenticationContext">Additional authentication context to pass to the authentication library.</param>
    /// <param name="cancellationToken">The cancellation token for the task</param>
    /// <returns>A Task that holds the access token to use for the request.</returns>
    public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object> additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        // Configure the MSAL client as a confidential client
        var app = ConfidentialClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority($"https://login.microsoftonline.com/{_tenantId}/v2.0")
            .WithClientSecret(_clientSecret)
            .Build();

        string[] scopes = ["https://graph.microsoft.com/.default"];

        var result = app.AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken).Result;

        return Task.FromResult(result.AccessToken);
    }

    /// <summary>
    /// Returns the <see cref="P:Microsoft.Kiota.Abstractions.Authentication.IAccessTokenProvider.AllowedHostsValidator" /> for the provider.
    /// </summary>
    public AllowedHostsValidator AllowedHostsValidator { get; } = new();
}