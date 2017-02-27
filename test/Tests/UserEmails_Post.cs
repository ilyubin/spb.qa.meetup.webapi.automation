using System.Collections.Generic;
using System.Linq;
using System.Net;
using AT.Github.Automation.Framework;
using NUnit.Framework;

namespace AT.Github.Automation.Tests
{
    public class UserEmails_Post : FixtureBase
    {
        [Test] [Smoke]
        public void Should_add_email_to_profile()
        {
            var data = new[]
            {
                Faker.Internet.Gmail(),
                Faker.Internet.Gmail()
            };
            var response = GithubApi.PostAuth("user/emails", data);
            Assert.That(response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                "Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created)");
        }

        [Test]
        public void Should_add_one_email_to_profile()
        {
            var data = "igor" + Faker.Internet.Gmail();
            var response = GithubApi.PostAuth("user/emails", data);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            response = GithubApi.GetAuth("user/emails");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var emails = response.DeserializeTo<List<UserEmailsResponse>>();
            Assert.That(emails, Has.Some.Property("Email").EqualTo(data));
        }

        [Test]
        public void Should_add_double_email_to_profile()
        {
            var email = "double"+Faker.Internet.Gmail();
            var data = new[] {email,email};
            var response = GithubApi.PostAuth("user/emails", data);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            response = GithubApi.GetAuth("user/emails");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var emails = response.DeserializeTo<List<UserEmailsResponse>>();
            Assert.That(emails.Count(x => x.Email.Equals(email)),
                Is.EqualTo(1));
        }

        public static object[] InvalidEmails = new object[]
        {
            123,
            1234,
            null
        };

        [Test]
        [TestCaseSource(nameof(InvalidEmails))]
        public void Should_add_double_email_to_profil(object data)
        {
            var response = GithubApi.PostAuth("user/emails", data);
            Assert.That(response.StatusCode, Is.EqualTo((HttpStatusCode) 422));
        }
    }
}
