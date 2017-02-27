using System;
using AT.Github.Automation.Tests;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace AT.Github.Automation.Framework
{
    public abstract class SharedFixtureBase
    {
        protected string TestCaseId => TestContext.CurrentContext.Test.Properties.Get("TestCaseId").ToString();
        protected string SeqUrl(string baseUrl) => $"{baseUrl}/#/events?filter=TestCaseId%20%3D%3D%20%22{TestCaseId}%22";

        private IDisposable _logContext;

        [SetUp]
        public void SharedBeforeEach()
        {
            TestContext.CurrentContext.Test.Properties.Set("TestCaseId", Guid.NewGuid().ToString("N"));
            _logContext = LogContext.PushProperties(new PropertyEnricher("TestCaseId", TestCaseId), new PropertyEnricher("TestName", TestContext.CurrentContext.Test.FullName));
            Log.Information("Test Start|{TestName}", TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void SharedAfterEach()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
                Log.Information("Test End|{TestStatus}|{TestName}", TestContext.CurrentContext.Result.Outcome.Status, TestContext.CurrentContext.Test.FullName);
            else
                Log.Error("Test End|{TestName}|{@TestResult}", TestContext.CurrentContext.Test.FullName, TestContext.CurrentContext.Result);
            _logContext.Dispose();

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed) return;
            Assert.Fail(SeqUrl(FixtureSetup.Configuration["seq:url"]));
        }
    }
}
