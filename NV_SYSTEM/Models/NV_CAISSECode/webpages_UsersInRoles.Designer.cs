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

    public partial class webpages_UsersInRoles : XPLiteObject
    {
        public struct CompoundKey1Struct
        {
            [Persistent("UserId")]
            public int UserId { get; set; }
            [Persistent("RoleId")]
            public int RoleId { get; set; }
        }
        [Key, Persistent]
        public CompoundKey1Struct CompoundKey1;
    }

}
