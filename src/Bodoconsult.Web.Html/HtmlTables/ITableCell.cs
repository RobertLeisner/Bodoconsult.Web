// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;

namespace Bodoconsult.Web.Html.HtmlTables;

public interface ITableCell
{
    IDictionary<string, string> Attributes { get; set; }

    string Text { get; set; }

    string RenderIt();
}