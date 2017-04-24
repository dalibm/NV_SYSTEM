using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NV_SYSTEM.Models.NV_SYSTEM_CAISSE
{

    public partial class Transaction
    {
        public Transaction(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
