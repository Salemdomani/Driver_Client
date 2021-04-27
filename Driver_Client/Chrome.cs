using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;

namespace Driver_Client
{
    class Chrome
    {
        ChromeDriver driver;
        ChromeOptions options = new ChromeOptions();
        int currentProfile = 1;
        public List<int> profiles;

        public Chrome()
        {
            profiles = SQLserver.GetProfiles();
            options.AddArguments(
                "--start-maximized",
                "--disable-notifications",
                @"--user-data-dir=C:\Users\vms" + ShouldBeService.VmNum + @"\AppData\Local\Google\Chrome\User Data",
                "--profile-directory=Profile " + currentProfile);
        }

        public void StartProfile(int Id)
        {
            currentProfile = Id;
            options.AddArgument("--profile-directory=Profile " + Id);
            Console.WriteLine("Starting profile  : " + currentProfile);
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void Close()
        {
            driver.Dispose();
        }

        public bool driverIsActive()
        {
            if (driver != null)
                return true;
            return false;
        }

        #region actions
        public void Like(string id)
        {
            driver.Navigate().GoToUrl("https://mbasic.facebook.com/" + id);
            try
            {
                if (driver.FindElement(By.LinkText("أعجبني")).GetAttribute("aria-pressed") == "true")
                    throw new AlreadyLikedException("Already Liked profile : " + currentProfile);
                driver.FindElement(By.LinkText("أعجبني")).Click();
            }
            catch (NoSuchElementException)
            {
                try
                {
                    if (driver.FindElement(By.LinkText("Like")).GetAttribute("aria-pressed") == "true")
                        throw new AlreadyLikedException("Already Liked profile : "+currentProfile);
                    driver.FindElement(By.LinkText("Like")).Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        CheckIfBlocked();
                    }
                    catch (NoSuchElementException)
                    {
                        throw new NotDoneException("not done profile : "+currentProfile);
                    }

                }
            }
        }

        public void Unlike(string id)
        {
            driver.Navigate().GoToUrl("https://mbasic.facebook.com/" + id);
            try
            {
                if (driver.FindElement(By.LinkText("أعجبني")).GetAttribute("aria-pressed") == "true")
                {
                    driver.FindElement(By.LinkText("أعجبني")).Click();
                    driver.FindElementByPartialLinkText("أعجبني").Click();
                }
                    
               

            }
            catch (NoSuchElementException)
            {
                try
                {
                    if (driver.FindElement(By.LinkText("Like")).GetAttribute("aria-pressed") == "true")
                    {
                        driver.FindElement(By.LinkText("Like")).Click();
                        driver.FindElementByPartialLinkText("Like").Click();
                    }
                        
                 

                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        CheckIfBlocked();
                    }
                    catch (NoSuchElementException)
                    {
                        throw new NotDoneException("not done profile : " + currentProfile);
                    }

                }
            }
        }

        public void Comment(string id, string comment)
        {

            try
            {
                driver.Navigate().GoToUrl("https://mbasic.facebook.com/" + id);
                driver.FindElement(By.LinkText("تعليق")).Click();
                driver.FindElement(By.Name("comment_text")).SendKeys(comment);
                driver.FindElement(By.Name("post")).Click();
            }
            catch (NoSuchElementException)
            {
                try
                {
                    driver.FindElement(By.LinkText("Comment")).Click();
                    driver.FindElement(By.Name("comment_text")).SendKeys(comment);
                    driver.FindElement(By.Name("post")).Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        CheckIfBlocked();
                    }
                    catch (NoSuchElementException)
                    {
                        throw new NotDoneException("not done profile : " + currentProfile);
                    }

                }
            }

        }

        public void Share(string id, string text)
        {
            driver.Navigate().GoToUrl("https://mbasic.facebook.com/" + id);
            try
            {
                driver.FindElement(By.LinkText("مشاركة")).Click();
                driver.FindElement(By.Name("xc_message")).SendKeys(text);
                driver.FindElement(By.Name("view_post")).Click();
            }
            catch (NoSuchElementException)
            {
                try
                {
                    driver.FindElement(By.LinkText("Share")).Click();
                    driver.FindElement(By.Name("xc_message")).SendKeys(text);
                    driver.FindElement(By.Name("view_post")).Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        CheckIfBlocked();
                    }
                    catch (NoSuchElementException)
                    {
                        throw new NotDoneException("not done profile : " + currentProfile);
                    }

                }
            }

        }

        public void ShareToGroub(string Post_Id, string Groub_Id)
        {
            driver.Navigate().GoToUrl("https://mbasic.facebook.com/groups/" + Groub_Id);
            driver.FindElement(By.Name("xc_message")).SendKeys("www.facebook.com/" + Post_Id);
            driver.FindElement(By.Name("view_post")).Click();
        }
        #endregion

        void Login(string email, string password)
        {
            driver.Navigate().GoToUrl("https://mbasic.facebook.com/");
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("pass")).SendKeys(password);
            driver.FindElement(By.Name("login")).Click();
        }

        private void CheckIfBlocked()
        {
            try
            {
                driver.FindElement(By.PartialLinkText("تسجيل الخروج")).Click();
                driver.FindElement(By.LinkText("تسجيل الخروج")).Click();
                driver.Dispose();
                throw new BlockedException("Blocked profile : " + currentProfile);
            }
            catch (NoSuchElementException)
            {
                try
                {
                    driver.FindElement(By.PartialLinkText("Log out")).Click();
                    driver.FindElement(By.LinkText("Log Out")).Click();
                    driver.Dispose();
                    throw new BlockedException("Blocked profile : " + currentProfile);
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        driver.FindElementByPartialLinkText("تسجيل");
                        throw new BlockedException("Blocked profile : "+currentProfile);
                    }
                    catch (NoSuchElementException)
                    {

                        driver.FindElementByPartialLinkText("Sign Up");
                        throw new BlockedException("Blocked profile : " + currentProfile);
                    }
                }
            }
            catch (Exception) { Console.WriteLine("Error Blocking"); }
        }

        public void JoinedGroups()
        {
            string gr = "C:\\Users\\vms" + ShouldBeService.VmNum + "\\Desktop\\Groups";
            if (!Directory.Exists(gr))
            {
                Directory.CreateDirectory(gr);
                foreach (var p in profiles)
                    if (!File.Exists(gr + "\\" + p + ".txt"))
                    {
                        StartProfile(p);
                        List<string> groups = new List<string>();
                        driver.Navigate().GoToUrl("https://mbasic.facebook.com/groups/groups/?seemore");
                        var nodes = driver.FindElements(By.XPath("//li//a"));
                        foreach (var node in nodes)
                        {
                            var href = node.GetAttribute("href");
                            groups.Add(href.Substring(href.IndexOf("groups/") + 7, href.IndexOf("/?") - href.IndexOf("groups/") - 7));
                        }
                        File.WriteAllLines(gr + "\\" + p + ".txt", groups);
                        Close();
                    }
            }
        }

    }

}


