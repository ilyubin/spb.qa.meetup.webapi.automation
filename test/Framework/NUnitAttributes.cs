using System;
using NUnit.Framework;

namespace AT.Github.Automation.Framework
{
    [AttributeUsage(AttributeTargets.All)]
    public class SmokeAttribute : CategoryAttribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class CriticalAttribute : CategoryAttribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class ProdAttribute : CategoryAttribute { }
}
