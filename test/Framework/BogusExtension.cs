using System;
using System.Globalization;
using Bogus;
using Bogus.DataSets;

namespace AT.Github.Automation.Framework
{
    public class Faker
    {
        public static Bogus.Faker Self => new Bogus.Faker(locale: "ru");
        public static Internet Internet => Self.Internet;
        public static Person Person => Self.Person;
        public static PhoneNumbers Phone => Self.Phone;
        public static Address Address => Self.Address;
        public static Company Company => Self.Company;
        public static Date Date => Self.Date;
        public static Finance Finance => Self.Finance;
        public static Hacker Hacker => Self.Hacker;
        public static Images Image => Self.Image;
        public static Lorem Lorem => Self.Lorem;
        public static Name Name => Self.Name;
        public static Randomizer Random => Self.Random;
    }

    public static class BogusExtension
    {
        public static string InRussia(this PhoneNumbers obj) => "+7990" + Faker.Random.ReplaceNumbers("#######");
        public static string Invalid(this PhoneNumbers obj) => "+7990" + Faker.Random.ReplaceNumbers("##");

        public static string Gmail(this Internet obj) => $"spbmeetup+{Faker.Lorem.AlwaysNewString()}{Faker.Random.ReplaceNumbers("###")}@gmail.com";
        public static string Password(this Internet obj) => Faker.Person.FirstName + Faker.Random.ReplaceNumbers("########");

        public static string RandomSex(this Person obj) => new Random().Next(2) == 0 ? "male" : "female";
        public static string Id(this Person obj) => Guid.NewGuid().ToString();

        public static string AlwaysNewString(this Lorem obj) => (DateTime.Now - DateTime.MinValue).TotalMilliseconds.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", "");

        public static string Id(this Randomizer obj) => Guid.NewGuid().ToString();
        public static string InvalidValue(this Randomizer obj)
        {
            var arr = new[]
                            {
                    "'", "\"", "’", "«", "»",
                    "<!--", "--",
                    "</body>", "?>",
                    "&#",
                    "/*", "*/",
                    "(", ")", "[", "]", "{", "}", "|", "\\", "/",
                    "~", "@", "$", "%", "^", ",", ".", "!",
                    ".*", "*.", "?", "*",                  // regexp
                    "+", "-", "=",
                    "åëåêòðîííà",
                    "\n", "\r", "^M", "\t",
                    "♣", "☺", "♂"
                };
            new Random().Shuffle(arr);
            return string.Join("", arr);
        }
    }
}
