using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using Streetwise.page_objects;
using AutomationCore.utility;

namespace Streetwise.page_assertions
{
    class LoginPageAssertions
    {
        private swLogin login;

        public LoginPageAssertions(swLogin login){
            this.login = login;
        }

        public void AssertPageElements()
        {
            HpgAssert.Exists(login.HealthtrustLogo, "The Healthtrust logo is missing or not displayed properly.");
            HpgAssert.Exists(login.CoretrustLogo, "The Coretrust logo is missing or not displayed properly.");
            HpgAssert.AreEqual(login.LoginTxt.Text, "Login", "The 'Login' text is missing or not displayed properly.");
            HpgAssert.AreEqual(login.EmailOrUserIDTxt.Text, "Email or User ID", "The 'Email or User ID' text is missing or not displayed properly.");
            HpgAssert.Exists(login.UserNameTxtField, "The username text field is missing or not displayed properly.");
            HpgAssert.AreEqual(login.PasswordTxt.Text, "Password Forgot password?", "The 'Password' text is missing or not displayed properly.");
            HpgAssert.AreEqual(login.ForgotPasswordLnk.Text, "Forgot password?", "The 'Forgot Password' text is missing or not displayed properly.");
            HpgAssert.Exists(login.PasswordTxtField, "The Password text field is missing or not displayed properly.");
            HpgAssert.Exists(login.RememberEmailOrUserIDCheckBox, "The 'Remember Email or User ID' check box is missing or not displayed properly.");
            HpgAssert.AreEqual(login.RememberEmailOrUserIDTxt.Text, "Remember Email or User ID", "The 'Remember Email or User ID' check box is missng or not displayed properly.");
            HpgAssert.Exists(login.loginButton, "The Login button is missing or not displayed properly.");
            HpgAssert.Exists(login.Footer, "The Footer is missing or does not exist.");
        }
    }
}