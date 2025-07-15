// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using Bodoconsult.Web.Html.Html;
using Bodoconsult.Web.Html.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Html.Test.Html
{
    [TestFixture]
    public class TableFormatterTests
    {



        [Test]
        public void TestFormatAsCsv()
        {

            var dt = TestHelper.GetDataTable("LineChart.xml");
            var erg = TableFormatter.FormatAsCsv(dt, true);

            Debug.Print(erg);

            Assert.IsTrue(erg.Contains("\r\n"));
            Assert.IsTrue(erg.Contains(";"));
        }


        [Test]
        public void TestFormatAsHtml()
        {

            var dt = TestHelper.GetDataTable("LineChart.xml");
            var erg = TableFormatter.FormatAsHtml(dt, true);

            Debug.Print(erg);

            Assert.IsTrue(erg.Contains("<table>"));
            Assert.IsTrue(erg.Contains("</table>"));
        }

    }
}
