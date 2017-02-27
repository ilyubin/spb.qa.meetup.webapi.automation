using System.Collections.Generic;
using System.Linq;
using System.Net;
using AT.Github.Automation.Framework;
using NUnit.Framework;

namespace AT.Github.Automation.Tests
{
    public class UserEmails_Get : FixtureBase
    {
        [Test] [Smoke]
        public void Should_return_emails_list()
        {
            var response = GithubApi.GetAuth("user/emails");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var emails = response.DeserializeTo<List<UserEmailsResponse>>();
            Assert.That(emails.First(x => x.Primary).Email, Is.EqualTo("ilyubin@inbox.ru"));
            Assert.That(emails.First(x => x.Primary).Verified, Is.True);
        }
    }
}
