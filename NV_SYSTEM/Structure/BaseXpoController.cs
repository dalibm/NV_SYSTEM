using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NV_SYSTEM.Structure
{
    public class BaseXpoController : System.Web.Mvc.Controller
    {
        Session fXpoSession;

        public BaseXpoController()
            : base()
        {
            fXpoSession = CreateSession();
            ViewBag.Xposession = fXpoSession;
        }

        public Session XpoSession
        {
            get { return fXpoSession; }
            private set { fXpoSession = value; }
        }


        protected virtual Session CreateSession()
        {
            return XpoHelper.GetNewSession();
        }
    }
}