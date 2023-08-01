using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;

namespace BetBasketBallChecker
{
    class CheckCondition : Form
    {

        private List<IWebElement> checkedList = new List<IWebElement>();
        StringBuilder sb;

        public void Start()
        {
            new Thread(() =>
            {
                try
                {
                    StartChecker();
                }
                catch (Exception e)
                {
                    StartChecker();
                }
            }).Start();
        }
        private void StartChecker()
        {
            checkedList = new List<IWebElement>();
            // Initialize the Chrome Driver
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            using (var driver = new ChromeDriver(driverService, new ChromeOptions()))
            {
                        WebDriverWait load = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                        load.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(InvalidElementStateException));
                        // Go to the home page
                        driver.Navigate().GoToUrl("https://www.bet365.com/home/?lng=1");
                        var enBtn = driver.FindElementByXPath("//a[@href='https://www.bet365.com/home/?lng=1']");
                        enBtn.Click();
                        load.Until(ExpectedConditions.ElementExists(By.XPath("//a[contains(@class, 'hm-BigButton')][contains (text(),'In-Play')]")));
                        var ipBtn = driver.FindElementByXPath("//a[contains(@class, 'hm-BigButton')][contains (text(),'In-Play')]");
                        ipBtn.Click();

                        load.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'ip-ControlBar_BBarItem')][contains (text(),'Event View')]")));
                        var evBtn = driver.FindElementByXPath("//div[contains(@class, 'ip-ControlBar_BBarItem')][contains (text(),'Event View')]");
                        evBtn.Click();

                        load.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'ipn-ClassificationButton_Classification-18')]")));

                while (true)
                {
                    try
                    {
                        sb = new StringBuilder();
                        var basketballBtn = driver.FindElementByXPath("//div[contains(@class, 'ipn-ClassificationButton_Classification-18')]");
                        basketballBtn = basketballBtn.FindElement(By.XPath(".."));
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].scrollIntoView()", basketballBtn);
                        if (basketballBtn.GetAttribute("class").Contains("closed"))
                        {
                            basketballBtn.Click();
                            Thread.Sleep(1000);
                        }
                        var competitionList = basketballBtn.FindElements(By.XPath(".//div[contains(@class, 'ipn-CompetitionButton')]"));
                        foreach (var competition in competitionList)
                        {
                            //js.ExecuteScript("arguments[0].scrollIntoView()", competition);
                            var parent = competition.FindElement(By.XPath(".."));
                            if (parent.GetAttribute("class").Contains("closed"))
                            {
                                competition.Click();
                                Thread.Sleep(1000);
                            }
                            var gameList = parent.FindElements(By.XPath(".//div[contains(@class, 'ipn-FixtureButton')]"));
                            foreach (var item in gameList)
                            {
                                try
                                {

                                    if (item.GetAttribute("class").Trim().EndsWith("ipn-FixtureButton") && !checkedList.Contains(item))
                                    {
                                        item.Click();
                                        Thread.Sleep(2000);
                                        String s = item.Text;
                                        sb.Append(s.Substring(0, s.IndexOf('\n')));
                                        load.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'ipe-ScoreGridContainer')]")));
                                        var container = driver.FindElementByXPath("//div[contains(@class, 'ipe-ScoreGridContainer')]");
                                        var valueList = container.FindElements(By.XPath(".//div[contains(@class, 'ipe-ScoreGridColumn ipe-ScoreGridColumn_Width30')]"));
                                        var item1 = valueList[0];
                                        var f1 = checkCondition(driver, item1, ".//div[contains(@class, 'ipe-ScoreGridCell')]");
                                        var item2 = valueList[1];
                                        var f2 = checkCondition(driver, item2, ".//div[contains(@class, 'ipe-ScoreGridCell')]");
                                        var item3 = valueList[2];
                                        var f3 = checkCondition(driver, item3, ".//div[contains(@class, 'ipe-ScoreGridCell')]");
                                        //if (((f1 == 1 || f1 == 2) && (f2 == 1 || f2 == 2) && (f3 == 1 || f3 == 2)) || ((f1 == 0 || f1 == 2) && (f2 == 0 || f2 == 2) && (f3 == 0 || f3 == 2)))
                                        if (((f1 == 1) && (f2 == 1) && (f3 == 1)) || ((f1 == 0) && (f2 == 0) && (f3 == 0)))
                                        {
                                            var item4 = valueList[3];
                                            int ret = checkCondition(driver, item4, ".//div[contains(@class, 'ipe-ScoreGridCell')]");
                                            if (ret == -10)
                                            {
                                                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"alarm.wav");
                                                player.PlayLooping();
                                                MessageBox.Show("Please notce on betting.", "BasketBall Checker", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                                player.Stop();
                                                checkedList.Add(item);
                                                sb.Append("------Check");
                                            }
                                        }
                                        sb.Append("\n");
                                        File.AppendAllText("log.txt", sb.ToString());
                                        sb.Clear();

                                    }
                                }catch(Exception e)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        int checkCondition(ChromeDriver driver, IWebElement element, String by)
        {
            try
            {
                var valueList = element.FindElements(By.XPath(by));
                int first = Int32.Parse(valueList[2].Text);
                int second = Int32.Parse(valueList[4].Text);
                sb.Append(String.Format(":{0}-------{1}", first, second));
                if (first == 0 && second == 0)
                {
                    return -10;
                }
                if (first - second > 0)
                    return 1;
                else if (first == second)
                    return 2;
                else 
                    return 0;
            }
            catch (Exception ex)
            {
                return 5;
            }
        }
    }
}
