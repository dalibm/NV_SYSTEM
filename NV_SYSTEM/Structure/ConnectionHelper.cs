using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo.DB;

namespace NV_SYSTEM.Structure
{
    public static class ConnectionHelper
    {
        public static void Connect(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
        {
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(GetConnectionString(), autoCreateOption);
            XpoDefault.Session = null;
        }
        public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
        {
            return XpoDefault.GetConnectionProvider(GetConnectionString(), autoCreateOption);
        }
        public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            return XpoDefault.GetConnectionProvider(GetConnectionString(), autoCreateOption, out objectsToDisposeOnDisconnect);
        }
        public static IDataLayer GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
        {
            return XpoDefault.GetDataLayer(GetConnectionString(), autoCreateOption);
        }
        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["XpoConnection"].ConnectionString;
        }
    }

    public static class XpoHelper
    {
        public static Session GetNewSession()
        {
            return new Session(DataLayer);
        }
        public static UnitOfWork GetNewUnitOfWork()
        {
            return new UnitOfWork(DataLayer);
        }

        private readonly static object lockObject = new object();

        static IDataLayer fDataLayer;
        public static IDataLayer DataLayer
        {
            get
            {
                if (fDataLayer == null)
                {
                    lock (lockObject)
                    {
                        //fDataLayer = GetDataLayer(ConnectionHelper.GetConnectionString());
                        fDataLayer = XpoDefault.GetDataLayer(ConnectionHelper.GetConnectionString(), AutoCreateOption.None);
                    }
                }
                return fDataLayer;
            }
        }

        //private static IDataLayer GetDataLayer(string ConnectionString, AutoCreateOption autoCreateOption = AutoCreateOption.DatabaseAndSchema)
        //{
        //    XpoDefault.Session = null;
        //    XPDictionary dict = new ReflectionDictionary();
        //    IDataStore store = XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
        //    Assembly[] assemblies = new Assembly[]
        //    {
        //        typeof(DAO.Agent).Assembly,
        //        typeof(DAO.Astreinte).Assembly,
        //        typeof(DAO.Combinaison).Assembly,
        //        typeof(DAO.Equipe).Assembly,
        //        typeof(DAO.Permanence).Assembly,
        //        typeof(DAO.PlanningRotation).Assembly,
        //        typeof(DAO.Quart).Assembly,
        //        typeof(DAO.Utilisateur).Assembly,
        //    };
        //    dict.GetDataStoreSchema(assemblies);
        //    return new ThreadSafeDataLayer(dict, store);
        //}
    }


}
