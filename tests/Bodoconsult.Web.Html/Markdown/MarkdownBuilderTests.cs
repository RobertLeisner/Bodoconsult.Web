// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Html.Markdown;
using NUnit.Framework;
using System;

namespace Bodoconsult.Web.Html.Test.Markdown
{
    [TestFixture]
    internal class MarkdownBuilderTests
    {

        [Test]
        public void Ctor_DefaultSetup_PropsSetCorrectly()
        {
            // Arrange 
            
            // Act  
            var builder = new MarkdownBuilder();

            // Assert
            Assert.That(builder.GetContent().Length, Is.EqualTo(0));
        }

        [Test]
        public void AddHeadline1_ValidContent_HeadlineAdded()
        {
            // Arrange 
            var builder = new MarkdownBuilder();
            const string content = "Test";

            // Act  
            builder.AddHeadline1(content);

            // Assert
            Assert.That(builder.GetContent().Length, Is.Not.EqualTo(0));
            Assert.That(builder.GetContent(), Is.EqualTo($"# {content}{Environment.NewLine}{Environment.NewLine}"));
        }

        // ToDo: add separate tests for all new methods

        // ToDo: add a test using multiple method for creating a demo markdown file. This test will be used as demo code for MarkDownBuilder class docu

        // ToDo: write docu for MarkDownBuilder class in /doc/Bodoconsult.Web.Html/readme.md. Add code sample from demo code above
    }
}
