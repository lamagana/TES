using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using static TES.Enumerations;

namespace TES
{
    public class TesHub : Hub
    {
        public TesHub()
        {
        }

        #region CookiesMethod
        public void GetPrimaryKeyCookie()
        {
            if(Cookies.PrimaryKey.Value == null)
            {
                Clients.Caller.checkPk("");
            }
            else
            {
            Clients.Caller.checkPk(Cookies.PrimaryKey.Value.ToString());
            }
        }

        public void ClearCookies()
        {
            Cookies.PrimaryKey.Value = string.Empty;
        }
        #endregion

        #region NewUser.html
        public void SendNewUser(object firstName, object lastName, object email, object password)
        {
            bool result = DatabaseAccess.InsertNewUser_SQL(firstName.ToString(), lastName.ToString(), email.ToString(), password.ToString(), out int UserID, out string errorMessage);
            if (!result)
                Clients.Caller.sendAlert(errorMessage);
            else
            {
                Cookies.PrimaryKey.Value = UserID.ToString();
                Clients.Caller.submitForm();
            }
        }
        #endregion

        #region Login.html
        public void VerifyLogin(object email, object password)
        {
            Console.WriteLine("VerifyLoginMethod");
            bool result = DatabaseAccess.SelectUser_SQL(email.ToString(), password.ToString(), out int UserID, out int Role, out string errorMessage);
            if (!result)
                Clients.Caller.sendAlert(errorMessage);
            else
            {
                Cookies.PrimaryKey.Value = UserID.ToString();
                if (Role == 0)
                {
                    Clients.Caller.submitForm("UnassignedUser.aspx");
                }
                else if(Role == 1)
                {
                    Clients.Caller.submitForm("AdminHomepage.aspx");
                }
                else if(Role == 3)
                {
                    Clients.Caller.submitForm("StudentHomepage.aspx");
                }
            }
        }
        #endregion

        #region UnassignedHomepage.html
        public void SubmitRequest(object role)
        {
            string errorMessage = string.Empty;
            Enum.TryParse<RoleTypes>(role.ToString(), out RoleTypes roleType);
            int.TryParse(Cookies.PrimaryKey.Value, out int pk);
            if(!DatabaseAccess.InsertRoleRequest_SQL((int)roleType, pk, out errorMessage))
            {
                Clients.Caller.sendAlert(errorMessage);
                return;
            }

            Clients.Caller.roleEntered();
        }

        public void CheckRequest()
        {
            string errorMessage = string.Empty;
            int.TryParse(Cookies.PrimaryKey.Value, out int pk);
            if (DatabaseAccess.SelectRoleRequest_SQL(pk, out errorMessage))
            {
                Clients.Caller.roleEntered();
            }
        }
        #endregion
    }
}