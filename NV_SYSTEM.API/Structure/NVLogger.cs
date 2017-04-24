//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.IO;
//using DevExpress.Xpo;
//using NV_SYSTEM.Models.NV_SYSTEM_CAISSE;
//namespace NV_SYSTEM.API
//{
//    public static class NVLogger
//    {
//        public static void Write(string type, string userid, string description, Session XpoSession)
//        {
//            string logDir = System.Configuration.ConfigurationManager.AppSettings["LogDir"].ToString();
//            string fullLogDir = string.Format(@"{0}\{1}\{2}", logDir, DateTime.Today.Year, DateTime.Today.Month);
//            string fullLogFile = string.Format(@"{0}\{1:D2}_{2}.txt", fullLogDir, DateTime.Today.Day, userid);
//            if (!Directory.Exists(fullLogDir))
//                Directory.CreateDirectory(fullLogDir);
//            if (!File.Exists(fullLogFile))
//                File.Create(fullLogFile).Close();
//            string text = string.Format("[{0}] | {1} | {2} | {3}{4}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), type, userid, description, Environment.NewLine);
//            File.AppendAllText(fullLogFile, text);
//            ActionCaissier ac = new ActionCaissier(XpoSession);
//            ac.Caissier = userid;
//            ac.TypeAction = type;
//            ac.Description = description;
//            ac.DateHeure = DateTime.Now;
//            XpoSession.BeginTransaction();
//            ac.Save();
//            XpoSession.CommitTransaction();
//        }
//    }
//}