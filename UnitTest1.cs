using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V133.Network;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace _1_test;

public class Tests
{
    public IWebDriver driver;
    public WebDriverWait wait;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    private void Autorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");

        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("");

        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("");

        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));
    }

    [Test]
    public void AuthorizationTest()
    {
        Autorization();

        Assert.That(driver.Title, Does.Contain("Новости"), "после авторизации не обнаружен заголовок Новости");

    }

    [Test]
    public void NavigationTest()
    {
        Autorization();

        var burger = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        burger.Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='SidePageBody']")));

        var communites = driver.FindElement(By.CssSelector("[data-tid='SidePageBody'] [data-tid='Community']"));
        communites.Click();
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/communities"));

        var titlePageElement = driver.FindElement(By.CssSelector("[data-tid='Title']"));

        Assert.That(titlePageElement.Text, Does.Contain("Сообщества"), "при переходе на вкладку сообщества не обнаружили заголовок Сообщества");

    }

    [Test]
    public void CommentTest()
    {
        Autorization();

        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities/e8ce0b22-dd03-4669-b21d-53c986425976?tab=discussions&id=6e670e6a-bdec-4265-b0d8-e1ead2e95b48");

        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Return']")));

        var comment = driver.FindElement(By.CssSelector("[data-tid='AddComment']"));
        comment.Click();
        var inpt = driver.FindElement(By.CssSelector("[data-tid='CommentInput']"));
        inpt.SendKeys("тест");
        var send = driver.FindElement(By.CssSelector("[data-tid='SendComment']"));
        send.Click();

        var text = driver.FindElement(By.CssSelector("[data-tid='TextComment']"));
        Assert.That(text.Text, Does.Contain("тест"), "после нажатия на кнопку отправить, на странице обсуждения не появился комментарий с текстом 'тест'");

    }

    [Test]
    public void CreateCommunityTest()
    {
        Autorization();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));

        //не смогла привязаться к другим селекторам
        var create = driver.FindElement(By.ClassName("sc-juXuNZ"));
        create.Click();

        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='ModalPageHeader']")));

        var name = driver.FindElement(By.CssSelector("[data-tid='Name']"));
        name.Click();
        name.SendKeys("Название");

        var button_create = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        button_create.Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));
        var settings = driver.FindElement(By.CssSelector("[data-tid='SettingsTabWrapper']"));
        Assert.That(settings.Text, Does.Contain("Основные настройки"), "после создания сообщества не обнаружен заголовок 'Основные настройки'");

    }

    [Test]
    public void ChangeThemeTest()
    {
        Autorization();

        var popap = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']"));
        popap.Click();

        var settings = driver.FindElement(By.CssSelector("[data-tid='Settings']"));
        settings.Click();

        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='modal-content']")));

        //не смогла привязаться к другим селекторам
        var checkbox = driver.FindElement(By.ClassName("react-ui-toggle-handle"));
        checkbox.Click();

        //снова нет других подходящих селекторов
        var save = driver.FindElement(By.ClassName("react-ui-1m5qr6w"));


        save.Click();

        //и снова нашла только такой селектор
        var lamp = driver.FindElement(By.ClassName("sc-dvUynV"));
        Assert.That(lamp.Displayed, "Элемент с классом 'sc-dvUynV eIUTfe' не найден на странице.");

    }


}
