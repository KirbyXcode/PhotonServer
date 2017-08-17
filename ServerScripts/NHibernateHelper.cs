using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;

namespace yukiServer
{
    class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure(); //解析hibernate.cfg.xml
                    configuration.AddAssembly("yukiServer"); // 解析映射文件 Users.hbm.xml

                    sessionFactory = configuration.BuildSessionFactory();
                }   
                return sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
