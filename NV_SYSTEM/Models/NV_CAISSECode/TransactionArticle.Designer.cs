﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NV_SYSTEM.Models.NV_SYSTEM_CAISSE
{

    public partial class TransactionArticle : XPLiteObject
    {
        int fID;
        [Key(true)]
        public int ID
        {
            get { return fID; }
            set { SetPropertyValue<int>("ID", ref fID, value); }
        }
        Article fArticle;
        [Association(@"TransactionArticleReferencesArticle")]
        public Article Article
        {
            get { return fArticle; }
            set { SetPropertyValue<Article>("Article", ref fArticle, value); }
        }
        Transaction fTransaction;
        [Association(@"TransactionArticleReferencesTransaction")]
        public Transaction Transaction
        {
            get { return fTransaction; }
            set { SetPropertyValue<Transaction>("Transaction", ref fTransaction, value); }
        }
        decimal fQte;
        public decimal Qte
        {
            get { return fQte; }
            set { SetPropertyValue<decimal>("Qte", ref fQte, value); }
        }
        string fCodeBalance;
        [Size(50)]
        public string CodeBalance
        {
            get { return fCodeBalance; }
            set { SetPropertyValue<string>("CodeBalance", ref fCodeBalance, value); }
        }
    }

}
