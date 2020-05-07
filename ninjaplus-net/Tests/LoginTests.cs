using NUnit.Framework;
using NinjaPlus.Pages;
using NinjaPlus.Common;

namespace NinjaPlus.Tests
{
    public class LoginTests : BaseTest
    {
        private LoginPage _login;
        private Sidebar _sidebar;

        [SetUp]
        public void Start()
        {
            _login = new LoginPage(Browser);
            _sidebar = new Sidebar(Browser);
        }

        [Test]
        [Category("Critical")]
        public void ShouldSeeLoggedUser()
        {
            _login.With("admin@ninjaplus.com", "pwd123");
            Assert.AreEqual("admin", _sidebar.LoggedUser());
        }

        // DDT = Teste Orientado a Dados (Data Driven Test)

        [TestCase("admin@ninjaplus.com", "123456", "Usuário e/ou senha inválidos")]
        [TestCase("test@ninjaplus.com", "pwd123", "Usuário e/ou senha inválidos")]
        [TestCase("", "pwd123", "Opps. Cadê o email?")]
        [TestCase("admin@ninjaplus.com", "", "Opps. Cadê a senha?")]
        public void ShouldSeeAlertMessage(string email, string pass, string expectMessage)
        {
            _login.With(email, pass);
            Assert.AreEqual(expectMessage, _login.AlertMessage());
        }
    }
}