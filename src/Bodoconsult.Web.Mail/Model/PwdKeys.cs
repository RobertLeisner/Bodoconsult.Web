// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Web.Mail.Model;

public class PwdKeys
{

    public string Key1 { get; set; } = "abc";
    public string Key2 { get; set; } = "def";
    public string Key3 { get; set; } = "ghi";

    public byte[] Salt { get; set; }

    /// <summary>
    /// Number of iterations
    /// </summary>
    public static int Iterations { get; set; } = 255;
}