// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.Web.Mail.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test;

/// <summary>
/// Test app
/// </summary>
[SetUpFixture]
public class AppSetup
{
    /// <summary>
    /// Load initial data for testing
    /// </summary>
    [OneTimeSetUp]
    public void LoadApp()
    {
        PasswordHandler.Key1 = "Mail2020";
        PasswordHandler.Key2 = "2020Mail";
        PasswordHandler.Key3 = "20Mail20";
        PasswordHandler.Salt = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];
    }

    ///// <summary>
    ///// Unlooad app
    ///// </summary>
    //[OneTimeTearDown]
    //public void UnloadApp()
    //{

    //}
}
