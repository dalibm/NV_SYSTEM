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

    public partial class webpages_OAuthMembership : XPLiteObject
    {
        int fUserId;
        public int UserId
        {
            get { return fUserId; }
            set { SetPropertyValue<int>("UserId", ref fUserId, value); }
        }
        public struct CompoundKey1Struct
        {
            [Size(30)]
            [Persistent("Provider")]
            public string Provider { get; set; }
            [Persistent("ProviderUserId")]
            public string ProviderUserId { get; set; }
        }
        [Key, Persistent]
        public CompoundKey1Struct CompoundKey1;
    }

}
