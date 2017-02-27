using AT.Github.Automation.Framework;
using NUnit.Framework;

namespace AT.Github.Automation.Tests
{
    public class UsersUsername_Get : FixtureBase
    {
        [Test] [Smoke]
        public void Should_info()
        {
            GithubApi.Get("users/ilyubin");
        }
    }
}
