using AT.Github.Automation.Framework;
using Microsoft.Extensions.Configuration;

namespace AT.Github.Automation.Tests
{
    public abstract class FixtureBase : SharedFixtureBase
    {
        public static IConfiguration Configuration => FixtureSetup.Configuration;
        protected static HttpClient GithubApi => new HttpClient(Configuration["api:github:url"]);
    }
}
