using MyEvernote.Common;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
           if(HttpContext.Current.Session["login"]!=null)
            {
                EverNoteUser user = HttpContext.Current.Session["login"] as EverNoteUser;
                return user.Username; 
            }

            //return null;
            return "fuatotomatik";
        }
    }
}