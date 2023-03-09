using ApiRessource2.Services;

namespace ApiRessource2.Test.Services
{
    public class IsValidPasswordTest
    {
        [Theory]
        [InlineData("MathisLeG@ot64", true)]
        [InlineData("tyhjkop@6432", false)]
        [InlineData("tyhjkopA6432", false)]
        [InlineData("tyhjkop@AAAAAA", false)]
        [InlineData("op@AAAAAA2", false)]
        [InlineData("AAAAAAA@6432", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void PasswordTest(string password, bool expected)
        {
            var actual = Tools.IsValidPassword(password);
            Assert.Equal(expected, actual);
        }
    }
}