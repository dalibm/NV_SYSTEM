using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using NV_SYSTEM.Models;
using NV_SYSTEM.Models.NV_SYSTEM_CAISSE;
using NV_SYSTEM.Structure;
using WebMatrix.WebData;

namespace NV_SYSTEM.Controllers
{
    [Authorize]
    public class HomeController : BaseXpoController
    {
        private string _CurrentTransaction
        {
            get
            {
                return (string)Session["_CurrentTransaction"];
            }
            set
            {
                Session["_CurrentTransaction"] = value;
            }
        }

        public ActionResult Index()
        {
            return View();    
        }

        public ActionResult Callback()
        {
            try
            {
                var transaction = GetOrCreateTransaction(WebSecurity.CurrentUserId);
                var model = XpoSession.Query<TransactionArticle>().Where(x => x.Transaction.ID == transaction.ID).Select(x => new
                {
                    ID = x.ID,
                    Article = x.Article.LibArticle,
                    PrixUnitaire = x.Article.PrixUnitaire,
                    Qte = x.Qte,
                    Total = x.Qte * x.Article.PrixUnitaire
                }).ToList();
                return PartialView("_GridPartial", model);
            }
            catch (Exception e)
            {
                NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                ViewData["EditError"] = "Erreur Systeme\n" + e.ToString();
                return PartialView("_GridPartial");
            }
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                var tra = XpoSession.GetObjectByKey<TransactionArticle>(ID);
                var transaction = GetOrCreateTransaction(WebSecurity.CurrentUserId);
                NVLogger.Write("SuppArticle", WebSecurity.CurrentUserName, string.Format("Article: {0} | Poids {1} | Transaction {2}", tra.Article.LibArticle, tra.Qte, tra.Transaction.UniqueID), XpoSession);
                XpoSession.BeginTransaction();
                tra.Delete();
                XpoSession.CommitTransaction();
                var model = XpoSession.Query<TransactionArticle>().Where(x => x.Transaction.ID == transaction.ID).Select(x => new
                {
                    ID = x.ID,
                    Article = x.Article.LibArticle,
                    PrixUnitaire = x.Article.PrixUnitaire,
                    Qte = x.Qte,
                    Total = x.Qte * x.Article.PrixUnitaire
                }).ToList();
                return PartialView("_GridPartial", model);
            }
            catch (Exception e)
            {
                NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                ViewData["EditError"] = "Erreur Systeme\n" + e.ToString();
                return PartialView("_GridPartial");
            }
        }

        [HttpPost]
        public ActionResult PushArticle(string barcode)
        {
            try
            {
                if (string.IsNullOrEmpty(barcode) || barcode.Length != 13)
                {
                    NVLogger.Write("Erreur", WebSecurity.CurrentUserName, string.Format("Code invalide: {0}", barcode), XpoSession);
                    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1501, Message = "Code invalide" }), "application/json");
                }
                else
                {
                    string _balance = barcode.Substring(0, 2);
                    string _article = barcode.Substring(2, 5);
                    string _poids = barcode.Substring(7, 5);
                    string _flag = barcode.Substring(12, 1);
                    decimal qte;
                    Article a = XpoSession.Query<Article>().Where(x => x.CodeArticle == _article).FirstOrDefault();
                    if(a.Unite == "UN")
                    {
                        qte = decimal.Parse(string.Format("{0}", _poids), System.Globalization.NumberStyles.Number);
                    }
                    else
                    {
                        qte = decimal.Parse(string.Format("{0},{1}", _poids.Substring(0, 2), _poids.Substring(2, 3)), System.Globalization.NumberStyles.Number);
                    }
                    if (a != null)
                    {
                        //check if current transaction
                        Transaction current = GetOrCreateTransaction(WebSecurity.CurrentUserId);
                        //log action
                        NVLogger.Write("ScanArticle", WebSecurity.CurrentUserName, string.Format("Article: {0} | Poids/Qte {1} | Transaction {2}", a.LibArticle, qte, current.UniqueID), XpoSession);
                        //update transaction
                        TransactionArticle tra = new TransactionArticle(XpoSession);
                        tra.Article = a;
                        tra.Transaction = current;
                        tra.Qte = qte;
                        tra.CodeBalance = _balance;
                        XpoSession.BeginTransaction();
                        tra.Save();
                        XpoSession.CommitTransaction();
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
                    }
                    else
                    {
                        NVLogger.Write("Erreur", WebSecurity.CurrentUserName, string.Format("Article non trouvé, {0}", barcode), XpoSession);
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1515, Message = "Article non trouvé" }), "application/json");
                    }
                }
            }
            catch (Exception e)
            {
                NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
            }
        }

        [HttpPost]
        public ActionResult AnnulerCommande()
        {
            try
            {
                var transaction = GetOrCreateTransaction(WebSecurity.CurrentUserId);
                XPCollection<TransactionArticle> tras = transaction.TransactionArticles;
                TmpTransactionCaissier tmp = XpoSession.Query<TmpTransactionCaissier>().Where(x => x.CompoundKey1.Caissier == WebSecurity.CurrentUserId).FirstOrDefault();
                NVLogger.Write("SuppressionTransaction", WebSecurity.CurrentUserName, string.Format("Transaction {0} | Articles {1}", transaction.UniqueID, string.Join(", ", transaction.UniqueID, tras.Select(x => x.Article.CodeArticle))), XpoSession);
                XpoSession.BeginTransaction();
                tmp.Delete();
                XpoSession.Delete(tras);
                transaction.Delete();
                XpoSession.CommitTransaction();
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
            }
            catch (Exception e)
            {
                NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
            }
        }

        [NonAction]
        private Transaction GetOrCreateTransaction(int userid)
        {
            TmpTransactionCaissier tmp = XpoSession.Query<TmpTransactionCaissier>().Where(x => x.CompoundKey1.Caissier == userid).FirstOrDefault();
            if(tmp == null)
            {
                Transaction tr = new Transaction(XpoSession);
                tr.UniqueID = Guid.NewGuid();
                tr.Client = "Passager";
                tr.DateTransaction = DateTime.Now;
                tr.Caissier = XpoSession.Query<Caissier>().Where(x => x.UserProfile.UserId == userid).FirstOrDefault();
                XpoSession.BeginTransaction();
                tr.Save();
                XpoSession.CommitTransaction();
                tmp = new TmpTransactionCaissier(XpoSession);
                tmp.CompoundKey1.Caissier = userid;
                tmp.CompoundKey1.Transaction = tr.ID;
                tmp.DateCreation = DateTime.Now;
                XpoSession.BeginTransaction();
                tmp.Save();
                XpoSession.CommitTransaction();
                NVLogger.Write("DebutTransaction", WebSecurity.CurrentUserName, string.Format("UniqueId = {0}", tr.UniqueID), XpoSession);
                return tr;
            }
            else
            {
                Transaction tr = XpoSession.GetObjectByKey<Transaction>(tmp.CompoundKey1.Transaction);
                return tr;
            }
        }
        [HttpPost]
        public ActionResult ValiderCommande(Decimal mt,int mp,string ch)
        {
            switch (mp)
            {
                case 1:
                    try
                    {
                        TmpTransactionCaissier tmp = XpoSession.Query<TmpTransactionCaissier>().Where(x => x.CompoundKey1.Caissier == WebSecurity.CurrentUserId).FirstOrDefault();
                        Transaction transaction = XpoSession.GetObjectByKey<Transaction>(tmp.CompoundKey1.Transaction);
                        // NVLogger.Write("ValiderTransaction", WebSecurity.CurrentUserName, string.Format("Transaction {0} | Articles {1}", transaction.UniqueID, string.Join(", ", transaction.UniqueID, XpoSession);
                        TypePaiement tp= XpoSession.GetObjectByKey<TypePaiement>(mp);
                        XpoSession.Delete(tmp);
                        transaction.MontantTotal = mt;
                        transaction.TypePaiement = tp;
                        transaction.Save();
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
                    }
                    catch (Exception e)
                    {
                        NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
                    }
                case 2:
                    try
                    {
                        TmpTransactionCaissier tmp = XpoSession.Query<TmpTransactionCaissier>().Where(x => x.CompoundKey1.Caissier == WebSecurity.CurrentUserId).FirstOrDefault();
                        Transaction transaction = XpoSession.GetObjectByKey<Transaction>(tmp.CompoundKey1.Transaction);
                        // NVLogger.Write("ValiderTransaction", WebSecurity.CurrentUserName, string.Format("Transaction {0} | Articles {1}", transaction.UniqueID, string.Join(", ", transaction.UniqueID, XpoSession);
                        XpoSession.Delete(tmp);
                        transaction.MontantTotal = mt;
                        transaction.Save();
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
                    }
                    catch (Exception e)
                    {
                        NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
                    }
                case 3:
                    try
                    {
                        TmpTransactionCaissier tmp = XpoSession.Query<TmpTransactionCaissier>().Where(x => x.CompoundKey1.Caissier == WebSecurity.CurrentUserId).FirstOrDefault();
                        Transaction transaction = XpoSession.GetObjectByKey<Transaction>(tmp.CompoundKey1.Transaction);
                        // NVLogger.Write("ValiderTransaction", WebSecurity.CurrentUserName, string.Format("Transaction {0} | Articles {1}", transaction.UniqueID, string.Join(", ", transaction.UniqueID, XpoSession);
                        XpoSession.Delete(tmp);
                        transaction.MontantTotal = mt;
                        transaction.Save();
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
                    }
                    catch (Exception e)
                    {
                        NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
                    }
                case 4:
                     try
                    {
                        TmpTransactionCaissier tmp = XpoSession.Query<TmpTransactionCaissier>().Where(x => x.CompoundKey1.Caissier == WebSecurity.CurrentUserId).FirstOrDefault();
                        Transaction transaction = XpoSession.GetObjectByKey<Transaction>(tmp.CompoundKey1.Transaction);
                        // NVLogger.Write("ValiderTransaction", WebSecurity.CurrentUserName, string.Format("Transaction {0} | Articles {1}", transaction.UniqueID, string.Join(", ", transaction.UniqueID, XpoSession);
                        XpoSession.Delete(tmp);
                        transaction.MontantTotal = mt;
                        transaction.Save();
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
                    }
                    catch (Exception e)
                    {
                        NVLogger.Write("ErreurSysteme", WebSecurity.CurrentUserName, e.ToString().Replace(Environment.NewLine, ";"), XpoSession);
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
                    }
                default:
                    try
                    {   return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 2000, Message = "OK !" }), "application/json");
                    }
                    catch (Exception e)
                    {
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { Result = 1503, Message = e.ToString() }), "application/json");
                    }

            }

        }
    }
}