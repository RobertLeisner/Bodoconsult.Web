// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Web.Mail.Helpers;
using BodoWebMailer.Business.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace BodoWebMailer.Test;

[TestFixture]
public class PasswordHandlerTests
{
    [Test]
    public void TestMethod_PlainPassword()
    {

        //const string password = "XN9FXSH7EQSGV36R";

        const string password = "Krumm2021Robert";

        var encryptedPassword = PasswordHandler.Encrypt(password);


        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password==decryptedPassword);
    }


    [Test]
    public void TestMethod_DecryptPassword()
    {
        var encryptedPassword = "cccc";
        var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);


        Debug.Print(decryptedPassword);
        Assert.That(!string.IsNullOrEmpty(decryptedPassword));

    }


    [Test]
    public void TestMethod_PlainPassword3()
    {

        //const string password = "XN9FXSH7EQSGV36R";

        const string password = "Test";

        var encryptedPassword = PasswordHandler.Encrypt3(password);


        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt3(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password == decryptedPassword);
    }



    [Test]
    public void TestMethod_HashPassword()
    {

        //const string password = "XN9FXSH7EQSGV36R";

        const string password = "Test";

        const int saltBytes = 128;
        const int hashBytes = 128;
        const int iterations = 10000;

        var salt = PasswordHandler.CreateSalt(saltBytes);



        var hashedPassword = PasswordHandler.CreateHash(password, salt, hashBytes, iterations);


        Debug.Print(hashedPassword);

        Assert.That(hashedPassword.Length>0);

        var erg = PasswordHandler.ValidateHash(password, salt, hashedPassword, iterations);

        //var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);

        //Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(erg);
    }


    [Test]
    public void TestMethod_HashPasswordLong()
    {

        //const string password = "XN9FXSH7EQSGV36R";

        const string password = "Test";

        const int saltBytes = 512;
        const int hashBytes = 512;
        const int iterations = 10000;

        var salt = PasswordHandler.CreateSalt(saltBytes);

        var hashedPassword = PasswordHandler.CreateHash(password, salt, hashBytes, iterations);

        Debug.Print(hashedPassword);

        Assert.That(hashedPassword.Length > 0);

        var erg = PasswordHandler.ValidateHash(password, salt, hashedPassword, iterations);

        //var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);

        //Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(erg);
    }


    [Test]
    public void TestMethod_ValdidateReal()
    {

        //const string password = "XN9FXSH7EQSGV36R";

        const string password = "Test123";
        const int iterations = 10000;

        var salt = "yO+U3TLU/4xb6a5drs6u8IJGDkGk5pzpw1PRD4wvhpPF564Rwu/em8u9/Oi/fdA7yEAjuJ7jJCH6aIe1+Ixc+qK9qOpy1TYRYwXrzj2sq/h8W6NaBMRm2oya2RSShAi0e9EaR5ugNx29iZiG927A9V03TYx/rhfy6Be5VsGzl3/A5jebh9C2LN9YpU0NisU5nU0sxM/IwzJXYHWUz5CbvEqAfMSeIHuqyxOXIQ0HqeP/rWMNrrVX38i5ChL63FCqOAjDzLlP+AUZXSUlMHM04faBbJaB7YRZQ7FC3LbCQB8malPGOXFzDhDCVD4nYfpVOZUVBcCQGuyh/qnHR5rSZoIRVt3IBzOunvA2horU10zxMEqrtHFl2eEz4BjPxkLUOrhs7z0JqxWhUWxpvkOYCytDA1P/PpkuEa3GD4ve4/BDgpdYqEYLLPDsz7VGlRUE6mZtjKXVjH0YMBwGbCmagrj2dq9n5HTObppFvUuuVX9uVgjlNuCNaDM5k+WE0vDGHxIgPBd/bdlmTq5C7bx8RE7drtNhCQYd0RdT/ncZNDix8bhkToY0p0Hk79Ql+iyBlpWH3F9xCEv1OFgX2wi7cfWpGxF8xgnDmx5y5ZoOqhJiQ9KtUWu44wLXYF6qqGBnqPgrh5v2dVFAgc8+iIgQ9W6uTwoA3sECOVNUH+KwNHU=";

        var hashedPassword1 = PasswordHandler.CreateHash(password, salt, 512, iterations);


        Debug.Print(hashedPassword1);

        var hashedPassword = "ngbDrmOkSmNlnvfKme9Hl/D9gZm87+1xKeEt7eMuA/cziB+bCNstal2/z8k/0YVld/dUooxa3XD88wNDhmOl6HB0Mxf2YoatfnJGPvxdzts45sOvuOwu1sUw1+J6pBciYYtvUP107sN/rwE+a0TPbw3Crpll/d3iAVlf1yHEcBvBGY7GNZFp4aDfADg4HMvpDpzBgLSh711aiU50nHXDu6U9ksUwfT/4DjCiUGBxgfeR2xRmQMp/d+Ee9/SU+qbkofnsAVvIt3O5ngZsfdOh9+1I+E/V00RNfmjZwqdhT/3WCdJw1pedLhmn72VYyBoMyxvwt1kXKzx08yDzNFrRJJ69rxOoPQxk66Cqb9qFxmRW5n6W2+h48cCO1pKccN1G3k+42LYkYYH4KqX5RDcJrRre520Lb/p/QxK+GXQzmFBeFPud79TDtGirOF8YDeabS5PbkAk7EUAs1pP5rIu4X5UFJ6T3eHzzxR8C0S1Oi15FnMcFasGcKJQ3EgdXnm32+00u6mOvEuT5JbY2SrE8oT3PDnuFTreLXq4dY7NL+vFLMQBGgIiCMAglQOCfxzvf23pHtwu6SaiR4fXLLjRiqA/i64l6U9RVqfeTLYmAIdTd9rEGx4vmmhyVKWSWO3aIdbMVJcjOyVsIL1UnrN3SwDuUsYkgTqWLzL0ZXT8Kseo=";
        Debug.Print(hashedPassword);

        Assert.That(hashedPassword == hashedPassword1);

        var erg = PasswordHandler.ValidateHash(password, salt, hashedPassword, iterations);

        //var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);

        //Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(erg);
    }

        
}