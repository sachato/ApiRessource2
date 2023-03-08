using ApiRessource2.Services;
using System.Text.RegularExpressions;


namespace ApiRessource2.Test
{
    public class TelephoneNumberTest
    {

        [Theory]
        [InlineData("06 12 34 56 78", false)]
        [InlineData("+33612345678", true)]
        [InlineData("+r", false)]
        [InlineData("0612345678", true)]
        [InlineData("+33eeeeeeeee", false)]
        [InlineData("+33 6 12 34 56 78", false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("+330625256595", false)]
        [InlineData("+53625256599", false)]
        [InlineData(null, false)]
        public void IsValidPhoneNumber(string telephoneNumber, bool expected)
        {
            //Initialisation
            bool actual = Tools.IsValidPhoneNumber(telephoneNumber);

            //Assert
            
            Assert.Equal(expected, actual);

        }
    }
}