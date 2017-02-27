using System.Collections.Generic;
using System.Linq;
using System.Net;
using AT.Github.Automation.Framework;
using NUnit.Framework;

namespace AT.Github.Automation.Tests
{
    public class UserEmails_Delete : FixtureBase
    {
        [Test]
        public void Should_return_NotFound_if_email_not_exists()
        {
            var data = new[] {Faker.Internet.Gmail()};
            var response = GithubApi.DeleteAuth("user/emails", data);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test] [Smoke]
        public void Should_return_NotFound_if_email_not_exists1()
        {
            var emails = GithubApi.GetAuth<List<UserEmailsResponse>>("user/emails");
            var email = emails.First(x => x.Email.Contains("@gmail.com")).Email;

            var data = new[] { email };
            var response = GithubApi.DeleteAuth("user/emails", data);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
