// Guids.cs
// MUST match guids.h
using System;

namespace Onway.AutoProjectFiles
{
    static class GuidList
    {
        public const string guidAutoProjectFilesPkgString = "7eb47d1f-9c44-482e-bf07-eda0f1ce873b";
        public const string guidAutoProjectFilesCmdSetString = "9daa1113-60ab-4974-9e9b-8d1b5adc95b2";

        public static readonly Guid guidAutoProjectFilesCmdSet = new Guid(guidAutoProjectFilesCmdSetString);
    };
}