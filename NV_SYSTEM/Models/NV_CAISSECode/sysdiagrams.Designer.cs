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

    public partial class sysdiagrams : XPLiteObject
    {
        string fname;
        [Size(128)]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        int fprincipal_id;
        public int principal_id
        {
            get { return fprincipal_id; }
            set { SetPropertyValue<int>("principal_id", ref fprincipal_id, value); }
        }
        int fdiagram_id;
        [Key(true)]
        public int diagram_id
        {
            get { return fdiagram_id; }
            set { SetPropertyValue<int>("diagram_id", ref fdiagram_id, value); }
        }
        int fversion;
        public int version
        {
            get { return fversion; }
            set { SetPropertyValue<int>("version", ref fversion, value); }
        }
        byte[] fdefinition;
        [Size(SizeAttribute.Unlimited)]
        public byte[] definition
        {
            get { return fdefinition; }
            set { SetPropertyValue<byte[]>("definition", ref fdefinition, value); }
        }
    }

}