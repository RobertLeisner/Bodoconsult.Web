// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Text;

namespace Bodoconsult.Web.Html.Markdown;

/// <summary>
/// Build a markdown file
/// </summary>
public class MarkdownBuilder
{
    /// <summary>
    /// Internal field for the Markdown content. Should not be accessible from outside directly
    /// </summary>
    private readonly StringBuilder _builder = new();

    /// <summary>
    /// 
    /// </summary>
    private readonly string _tagEnd = $"{Environment.NewLine}{Environment.NewLine}";

    /// <summary>
    ///  Get the current Markdown as string
    /// </summary>
    /// <returns>Markdown as string</returns>
    public string GetContent()
    {
        return _builder.ToString();
    }

    /// <summary>
    /// Add a headline level 1
    /// </summary>
    /// <param name="content">Headline content</param>
    public void AddHeadline1(string content)
    {
        _builder.Append($"# {content}{_tagEnd}");
    }

    // ToDo: add all important markup tags but not image and link tags

    // ToDo: add image and link tags as static inline helper methods for creating content for the other methods

    // ToDo: add docu to all methods if not done already (use /// makro to let you help)

    // ToDo: add a method to add a raw string to the content (to make it possible to add plain markdown to the conent if needed)

    // ToDo: add saving content to a text file using System.IO.File object (overwrite existing file)

    // ToDo: add Clear() method to make MarkDownBuilder class reusable (calling _builder.Clear())
}