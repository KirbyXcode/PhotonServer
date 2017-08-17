using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using yukiServer.Table;

namespace yukiServer
{
    class UserManager : IUserManager
    {
        public void Add(User user)
        {
            //ISession session = NHibernateHelper.OpenSession();
            //session.Save(user);
            //session.Close();

            using(ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public void Update(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }

        public void Remove(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }

        public User GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.Get<User>(id);
                if(user == null)
                {
                    Console.WriteLine("id does not exist");
                    return null;
                }
                else return user;
            }
        }

        public User GetByUsername(string username)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                //ICriteria criteria = session.CreateCriteria(typeof(User));
                //criteria.Add(Restrictions.Eq("Username", username));
                //User user = criteria.UniqueResult<User>();
                //return user;

                User user = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", username)).UniqueResult<User>();
                return user;
            }
        }

        public ICollection<User> GetAllUsers()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<User> users = session.CreateCriteria(typeof(User)).List<User>();
                return users;
            }
        }


        public bool VerifyAccount(string username, string password)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User)).
                    Add(Restrictions.Eq("Username", username)).
                    Add(Restrictions.Eq("Password",password)).
                    UniqueResult<User>();
                if (user == null) return false;
                return true;
            }
        }
    }
}
