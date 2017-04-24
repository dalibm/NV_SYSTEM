using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NV_SYSTEM.API.Structure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Windows.Forms;

namespace NV_SYSTEM.API.Controllers
{
    public class AdminController : BaseXpoController
    {
        string rep=@"C:\wamp64\www\nv\model\tmp\";
  //debut gestion pdv
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<dynamic> GetPdv()
        {
            var data = XpoSession.ExecuteQuery(@"SELECT [ID]
      ,[Nom]
      ,[Adresse]
      ,[TEL]
        FROM [dbo].[PDV]").ResultSet[0].Rows.Select(x => new
            {
                ID = x.Values[0],
                NOM = (string)x.Values[1],
                ADRESSE = (string)x.Values[2],
                TEL = (string)x.Values[3],
            }).ToList();
            return data;
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<dynamic> Getlistepdv()
        {
            var data = XpoSession.ExecuteQuery(@"SELECT [ID],[Nom]
  FROM [NV_SYSTEM_BO].[dbo].[PDV]").ResultSet[0].Rows.Select(x => new {
                ID = x.Values[0],
                NOM = (string)x.Values[1]}).ToList();
            return data;
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void AddPDV()
        {
            string uri = rep + "AddPDV.json";
            string json = File.ReadAllText(uri);
            JObject parsed = JObject.Parse(json);
            string NOM = "";
            string ADRESSE = "";
            string TEL = "";
            foreach (var obj in parsed)
            {
                if (obj.Key.Equals("NOM")) { NOM = obj.Value.ToString(); }
                else if (obj.Key.Equals("ADRESSE")) { ADRESSE = obj.Value.ToString(); }
                else if (obj.Key.Equals("TEL")) { TEL = obj.Value.ToString(); }
            }
            var data = XpoSession.ExecuteQuery(@"INSERT INTO [dbo].[PDV]
           ([ID]
           ,[Nom]
           ,[Adresse]
           ,[TEL])
     VALUES
           (NEWID(),'" + NOM + "','" + ADRESSE + "','" + TEL + "')");
            System.IO.File.Delete(uri);
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void DeletePDV()
        {
            string uri = rep + "DelPDV.json";
            string json = File.ReadAllText(uri);
            JObject parsed = JObject.Parse(json);
            string ID = "";
            foreach (var obj in parsed)
            {
                if (obj.Key.Equals("ID")) { ID = obj.Value.ToString(); }
            }
            var data = XpoSession.ExecuteQuery(@"DELETE FROM [dbo].[PDV]
      WHERE ID LIKE '" + ID + "'");
            System.IO.File.Delete(uri);
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void UpdatePDV()
        {
            string uri = rep + "UpdatePDV.json";
            string json = File.ReadAllText(uri);
            JObject parsed = JObject.Parse(json);
            string ID = "";
            string NOM = "";
            string ADRESSE = "";
            string TEL = "";
            foreach (var obj in parsed)
            {
                if (obj.Key.Equals("ID")) { ID = obj.Value.ToString(); }
                else if (obj.Key.Equals("NOM")) { NOM = obj.Value.ToString(); }
                else if (obj.Key.Equals("ADRESSE")) { ADRESSE = obj.Value.ToString(); }
                else if (obj.Key.Equals("TEL")) { TEL = obj.Value.ToString(); }
            }
            var data = XpoSession.ExecuteQuery(@"UPDATE [dbo].[PDV] SET [Nom] ='" + NOM + "',[Adresse] = '" + ADRESSE + "',[TEL] = '" + TEL + "' WHERE ID LIKE '" + ID + "'");
            System.IO.File.Delete(uri);
        }

    //fin gestion pdv

        //debut gestion des articles

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<dynamic> GetArticles()
        {
            var data = XpoSession.ExecuteQuery(@"SELECT [ID]
      ,[LibArticle]
      ,[CodeArticle]
      ,[PrixUnitaire]
      ,[Unite]
      ,[DateCreation]
      ,[Nom]
  FROM [NV_SYSTEM_BO].[dbo].[VW_Articles]").ResultSet[0].Rows.Select(x => new
            {
                ID = x.Values[0],
                LibArticle = (string)x.Values[1],
                CodeArticle = (string)x.Values[2],
                PrixUnitaire = (decimal)x.Values[3],
                Unite = (string)x.Values[4],
                DateCreation = (DateTime)x.Values[5],
                Pdv = x.Values[6],
            }).ToList();
            return data;
        }
   
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void AddArticle()
        {
            string uri = rep + "AddArticle.json";
            string json = File.ReadAllText(uri);
            JObject parsed = JObject.Parse(json);
            string LibArticle = "";
            string CodeArticle = "";
            string PrixUnitaire = "";
            string Unite = "";
            string DateCreation = "";
             string Pdv = "";
            foreach (var obj in parsed)
            {
                if (obj.Key.Equals("LibArticle")) { LibArticle = obj.Value.ToString(); }
                else if (obj.Key.Equals("CodeArticle")) { CodeArticle = obj.Value.ToString(); }
                else if (obj.Key.Equals("PrixUnitaire")) { PrixUnitaire = obj.Value.ToString(); }
                else if (obj.Key.Equals("Unite")) { Unite = obj.Value.ToString(); }
                else if (obj.Key.Equals("DateCreation")) { DateCreation = obj.Value.ToString(); }
                else if (obj.Key.Equals("Pdv")) { Pdv = obj.Value.ToString(); }
            }
            var data = XpoSession.ExecuteQuery(@"INSERT INTO [dbo].[Article]
           ([LibArticle]
            ,[CodeArticle]
            ,[PrixUnitaire]
            ,[Unite]
            ,[DateCreation]
            ,[Pdv])
     VALUES
           ('" + LibArticle + "','" + CodeArticle + "','" + PrixUnitaire + "','" + Unite + "','" + DateTime.Now + "','" + Pdv + "')");
            System.IO.File.Delete(uri);
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void DeleteArticle()
        {
            string uri = rep + "Delarticle.json";
            string json = File.ReadAllText(uri);
            JObject parsed = JObject.Parse(json);
            string ID = "";
            foreach (var obj in parsed)
            {
                if (obj.Key.Equals("ID")) { ID = obj.Value.ToString(); }
            }
            var data = XpoSession.ExecuteQuery(@"DELETE FROM [dbo].[Article]
      WHERE ID LIKE '" + ID + "'");
            System.IO.File.Delete(uri);
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void UpdateArticle()
        {
            string uri = rep + "UpdateArticle.json";
            string json = File.ReadAllText(uri);
            JObject parsed = JObject.Parse(json);
            string ID = "";
            string LibArticle = "";
            string CodeArticle = "";
            string PrixUnitaire = "";
            string Unite = "";
            string DateCreation = "";
            string Pdv = "";
            foreach (var obj in parsed)
            {
                if (obj.Key.Equals("ID")) { ID = obj.Value.ToString(); }
                else if (obj.Key.Equals("LibArticle")) { LibArticle = obj.Value.ToString(); }
                else if (obj.Key.Equals("CodeArticle")) { CodeArticle = obj.Value.ToString(); }
                else if (obj.Key.Equals("PrixUnitaire")) { PrixUnitaire = obj.Value.ToString(); }
                else if (obj.Key.Equals("Unite")) { Unite = obj.Value.ToString(); }
                else if (obj.Key.Equals("DateCreation")) { DateCreation = obj.Value.ToString(); }
                else if (obj.Key.Equals("Pdv")) { Pdv = obj.Value.ToString(); }
            }
            var data = XpoSession.ExecuteQuery(@"UPDATE [dbo].[Article] SET [LibArticle] ='" + LibArticle + "',[CodeArticle] = '" + CodeArticle + "',[PrixUnitaire] = '" + PrixUnitaire + "',[Unite] = '" + Unite + "',[Pdv] = '" + Pdv + "' WHERE ID LIKE '" + ID + "'");
            System.IO.File.Delete(uri);
        }



        //fin gestion des articles
        
        //debut Historique des caissier
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<dynamic> GetLogsCaissier()
        {
            var data = XpoSession.ExecuteQuery(@"SELECT [ID_LC]
              ,[Nom]
              ,[ID]
              ,[UserName]
              ,[TypeAction]
              ,[DateHeure]
              ,[Description]
          FROM [dbo].[VW_Logs]").ResultSet[0].Rows.Select(x => new
            {
                                                   ID_LC = (int)x.Values[0],
                                                   PDV = (string)x.Values[1],
                                                   CAISSIER = (string)x.Values[3],
                                                   TYPEACTION = (string)x.Values[4],
                                                   DATE = (DateTime)x.Values[5],
                                                   DESCRIPTION = (string)x.Values[6],
            }).ToList();
            return data;
        }

        //fin historique des caissier

        public IEnumerable<dynamic> GetTransactionCaissier()
        {
            var data = XpoSession.ExecuteQuery(@"SELECT [ID]
      ,[UniqueID]
      ,[Client]
      ,[DateTransaction]
      ,[MontantTotal]
      ,[Caissier]
      ,[TypePaiement]
      ,[Flag]
  FROM [dbo].[Transaction]").ResultSet[0].Rows.Select(x => new
            {
                ID = (int)x.Values[0],
                Client = (string)x.Values[1],
                DateTransaction = (DateTime)x.Values[2],
                MontantTotal = (string)x.Values[3],
                Caissier = (string)x.Values[4],
                TypePaiement = (string)x.Values[5],
            }).ToList();
            return data;
        }
    

        public void AddCaissier() { }

        //rapport journalier
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<dynamic> Getch(string Nom) {
            System.Threading.Thread.Sleep(200);
            string date = @"2017-04-03";
            var data = XpoSession.ExecuteQuery(@"SELECT [total]
                  ,[Designation]
              FROM [NV_SYSTEM_BO].[dbo].[VW_rj]
            WHERE [dateop] LIKE '"+date+"' AND NOM LIKE '"+Nom+"'").ResultSet[0].Rows.Select(x => new
                        {
                            total = string.Format("{0,12:N3}", (decimal)x.Values[0]),
                Designation = (string)x.Values[1],
            }).ToList();
            return data;
        }
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<KeyValuePair<string, dynamic>> Getrj()
        {
            string date =@"2017-04-03";
            List< KeyValuePair < string, dynamic >> data =new List<KeyValuePair<string, dynamic>>();
            var pdv = XpoSession.ExecuteQuery(@"SELECT [ID],[Nom]
  FROM [NV_SYSTEM_BO].[dbo].[PDV]").ResultSet[0].Rows.Select(x => new {
                ID = x.Values[0],
                NOM = (string)x.Values[1]
            }).ToList();
            var lc = XpoSession.ExecuteQuery(@"SELECT [Nom]
                  ,[Prenom]
                  ,[PDV]
                  ,[Caissier]
                  ,[dateop]
                  FROM [NV_SYSTEM_BO].[dbo].[VW_pdvc]
                  WHERE  [dateop] LIKE  '" + date + "'").ResultSet[0].Rows.Select(x => new
                            {
                                NOMC = (string)x.Values[0] + " " + (string)x.Values[1],
                                PDV =x.Values[2],
                                ID = (int)x.Values[3],
                            }).ToList();
            foreach (var pd in pdv)
            {
                List<KeyValuePair<string, dynamic>> lcl = new List<KeyValuePair<string, dynamic>>();

                foreach (var c in lc)
                {
                    

                    if (c.PDV.Equals(pd.ID))
                    {
                        var la = XpoSession.ExecuteQuery(@"SELECT [Qte]
                                                          ,[LibArticle]
                                                          ,[PrixUnitaire]
                                                          ,[Caissier]
                                                          FROM [NV_SYSTEM_BO].[dbo].[VW_rc]
                                                          WHERE [datetransaction] LIKE  '" + date + "' AND [Caissier] LIKE '" + c.ID + "'").ResultSet[0].Rows.Select(x => new
                        {
                            QTE = (decimal)x.Values[0],
                            LIB = (string)x.Values[1],
                            TOTAL = string.Format("{0,6:N3}", (decimal)x.Values[2] * (decimal)x.Values[0]),
                        }).ToList();
                        if (la.LongCount() > 0)
                        {
                            lcl.Add(new KeyValuePair<string, dynamic>(c.NOMC, la));



                        }

                    }
                }
                    if (lcl.LongCount() > 0)
                    {
                       data.Add(new KeyValuePair<string, dynamic>(pd.NOM, lcl));
                    }
                
            }


                return data;
        }
     
    }

}
                                              