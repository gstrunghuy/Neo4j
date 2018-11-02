using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4j.Driver.V1;
using Neo4jSocial.Models;

namespace Neo4jSocial.DAO
{
    public class FriendDao:BaseDAO
    {
        public List<Profile> FindFriend(string key, string info)
        {
            ReplaceInput(ref info);
            ReplaceInput(ref key);
            info= info.Replace(" ", "");

            List<Profile> lst = new List<Profile>();
            using (var session = Driver.Session())
            {
                lst = session.ReadTransaction(tx =>
                {
                    var result = tx.Run("MATCH (p:Profile) WHERE p."+ info+ "=~'.*"+key+ ".*' RETURN p.Avatar, p.Birthday, p.City, p.Company, p.Email, p.FirstName, p.Gender, p.Interest, p.Job, p.LastName, p.School, p.User");
                    foreach (var item in result)
                    {
                        Profile p = new Profile();
                        p.Avatar = item.Values["p.Avatar"].As<string>();
                        p.Birthday = item.Values["p.Birthday"].As<LocalDate>();
                        p.City = item.Values["p.City"].As<string>();
                        p.Company = item.Values["p.Company"].As<string>();
                        p.Email = item.Values["p.Email"].As<string>();
                        p.FirstName = item.Values["p.FirstName"].As<string>();
                        p.Gender = item.Values["p.Gender"].As<string>();
                        p.Interest = item.Values["p.Interest"].As<string>();
                        p.Job = item.Values["p.Job"].As<string>();
                        p.LastName = item.Values["p.LastName"].As<string>();
                        p.School = item.Values["p.School"].As<string>();
                        p.User = item.Values["p.User"].As<string>();
                        lst.Add(p);
                    }
                    return lst;
                });
                return lst;
            }
        }

        public void AddFriend(string user, string me)
        {
            using (var session = Driver.Session())
            {
                session.Run("MATCH (u:Profile {User:'" + user+ "'}),(m:Profile {User:'" + me + "'}) CREATE (m)-[f:FRIEND]->(u)");
            }
        }
        public bool CheckFriend(string user, string me)
        {
            try
            {
                using (var session = Driver.Session())
                {
                    int id = session.Run("MATCH (u:Profile {User:'" + user + "'})-[f:FRIEND]->(m:Profile {User:'" + me + "'}) RETURN id(f) as id").FirstOrDefault().Values["id"].As<int>();
                    if (id > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string IsFriend(string user, string me)
        {
            string f = "NotFriend";
            bool c1 = CheckFriend(user, me);
            bool c2 = CheckFriend(me, user);
            if (c1 || c2)
            {
                f = "Request";
            }
            if (c1 && c2)
            {
                f = "Friend";
            }
            return f;
        }

        
    }
}