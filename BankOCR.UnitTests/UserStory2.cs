using NUnit.Framework;
using System.Linq;

namespace BankOcrKata
{
    [TestFixture]
    public class UserStory2
    {
        [TestCase("711111111", true)]
        [TestCase("123456789", true)]
        [TestCase("490867715", true)]
        [TestCase("888888888", false)]
        [TestCase("490067715", false)]
        [TestCase("012345678", false)]
        public void Tests(string accountNumber, bool isValid)
        {
            var account = new AccountNumber(accountNumber);
            Assert.AreEqual(isValid, account.HasValidChecksum());
        }
    }
}