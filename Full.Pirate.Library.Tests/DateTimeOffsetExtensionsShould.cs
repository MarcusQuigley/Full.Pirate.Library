using System;
using Xunit;

namespace Full.Pirate.Library.Tests
{
    public class DateTimeOffsetExtensionsShould
    {
        [Theory]
        [InlineData("1/2/73",47)]
        [InlineData("1/14/73", 47)]
        [InlineData("1/15/73", 46)]
        [InlineData("10/15/73", 46)]
        public void ReturnCorrectAges(string dob, int expectedAge)
        {
            DateTimeOffset parsedAge = DateTimeOffset.Parse(dob);
            Assert.Equal(expectedAge, parsedAge.GetAge());
        }
    }
}
