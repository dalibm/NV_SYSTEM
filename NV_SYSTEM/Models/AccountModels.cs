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
using System.ComponentModel.DataAnnotations;

namespace NV_SYSTEM.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Ce champs est requis")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Ce champs est requis")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public static class ModelHelpers
    {
        public static IEnumerable<TypePaiement> GetTypePaiment(Session XpoSession)
        {
            try
            {
                return XpoSession.Query<TypePaiement>().ToList();
            }
            catch (Exception e)
            {
                return new List<TypePaiement>();
            }
        }
    }

}