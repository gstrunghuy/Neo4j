using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jSocial.Models;

namespace Neo4jSocial.DAO
{
    public class StatusDAO:BaseDAO
    {
        public Profile GetProfile(string user)
        {
            
            using (var session = Driver.Session())
            {
                var profile = session.Run("MATCH (n:Profile {User:'" + user + "'}) RETURN n").FirstOrDefault();
                Profile p = new Profile();
                if (profile!=null)
                {
                    var node = profile["n"].As<INode>();
                    p.Avatar = node["Avatar"].As<string>();
                    p.Birthday = node["Birthday"].As<LocalDate>();
                    p.City = node["City"].As<string>();
                    p.Company = node["Company"].As<string>();
                    p.Email = node["Email"].As<string>();
                    p.FirstName = node["FirstName"].As<string>();
                    p.Gender = node["Gender"].As<string>();
                    p.Interest = node["Interest"].As<string>();
                    p.Job = node["Job"].As<string>();
                    p.LastName = node["LastName"].As<string>();
                    p.School = node["School"].As<string>();
                    p.User = node["User"].As<string>();

                }
                return p;
                
            }
        }

        public bool Edit(Profile p)
        {
            string avt = "";
            if (p.Avatar!="" && p.Avatar!=null)
            {
                avt = "', p.Avatar = '" + p.Avatar;
            }
            try
            {
                using (var session = Driver.Session())
                {
                    session.Run("MATCH (p:Profile {User:'" + p.User + "'}) SET  p.Birthday = date('" + p.Birthday + "'), p.City = '" + p.City + "', p.LastName = '" + p.LastName + "', p.FirstName = '" + p.FirstName + "', p.Interest = '" + p.Interest + "', p.Email = '" + p.Email + "', p.Gender = '" + p.Gender + "', p.Job = '" + p.Job + "', p.Company = '" + p.Company + avt + "', p.School = '" + p.School + "' ");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Add(Profile p)
        {
            try
            {
                using (var session = Driver.Session())
                {
                    session.Run("CREATE (p:Profile {User:'" + p.User + "'}) SET  p.Birthday = date('" + p.Birthday + "'), p.City = '" + p.City + "', p.LastName = '" + p.LastName + "', p.FirstName = '" + p.FirstName + "', p.Interest = '" + p.Interest + "', p.Email = '" + p.Email + "', p.Gender = '" + p.Gender + "', p.Job = '" + p.Job + "', p.Company = '" + p.Company + "', p.Avatar = '" + p.Avatar + "', p.School = '" + p.School + "' ");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Post(string user, string content, string avt, string displayName)
        {
           
            try
            {
                using (var session = Driver.Session())
                {
                    string idStt = session.WriteTransaction(tx => {
                        var result = tx.Run("MATCH (a:Profile {User:'" + user + "'}) CREATE (a)-[:POST]->(s:Status { Content:'" + content + "', Time: localdatetime({ timezone: '+07:00'}), Avatar:'"+avt+"', DisplayName:'"+displayName+"', UserPost:'" + user + "'}) RETURN id(s)");
                        return result.Single()[0].As<string>();
                    });
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public Status GetPost(int id)
        {
            Status stt = new Status();

            using (var session = Driver.Session())
            {
                stt = session.ReadTransaction(tx => {
                    var recod = tx.Run("MATCH (s:Status) where id(s)="+id+" RETURN s.Content,s.Time,s.UserPost,id(s) as idstt, s.Avatar, s.DisplayName ORDER BY s.Time DESC").FirstOrDefault();
                        stt.Id = recod.Values["idstt"].As<int>();
                        stt.Content = recod.Values["s.Content"].As<string>();
                        stt.UserPost = recod.Values["s.UserPost"].As<string>();
                        stt.AvatarPost = recod.Values["s.Avatar"].As<string>();
                        stt.DisplayName = recod.Values["s.DisplayName"].As<string>();
                        stt.TimePost = recod.Values["s.Time"].As<LocalDateTime>();
                    return stt;
                });
            }
            return stt;
        }
        public string GetAvt(string user)
        {
            string res = user.ToUpper()+"/";
            using (var session = Driver.Session())
            {
                var avt = session.Run("match (p:Profile {User:'"+user+"'}) return p.Avatar").FirstOrDefault().Values["p.Avatar"];
                if (avt!=null)
                {
                    res += avt.As<string>();
                }
                return res;
            }
        }
        public void Share(int id, string user)
        {
            using (var session = Driver.Session())
            {
                session.Run("MATCH (s:Status),(p:Profile {User:'" + user + "'}) WHERE id(s)=" + id + "  CREATE (p)-[:SHARE {Time:localdatetime({ timezone: '+07:00'})}]->(s)");
            }
        }
        public bool Comment(string user, string content, string avt, string displayName, int idstt)
        {
            string query = "MATCH (s:Status) WHERE id(s)=" + idstt.ToString() + " CREATE (s)-[:CMT]->(c:Comment { Content:'" + content + "', Time: localdatetime({ timezone: '+07:00'}), Avatar:'" + avt + "', Name:'" + displayName + "', User:'" + user + "'})";
            try
            {
                using (var session = Driver.Session())
                {
                    session.Run(query);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public List<Comment> GetAllComment(string idstt)
        {
            List<Comment> lst = new List<Comment>();

            using (var session = Driver.Session())
            {
                lst = session.ReadTransaction(tx => {
                    var result = tx.Run("MATCH (s:Status)-[r:CMT]->(c:Comment) WHERE id(s)=" + idstt.ToString() + " RETURN c.Content,c.Time,c.User, c.Avatar, c.Name ORDER BY c.Time ASC");
                    foreach (var recod in result)
                    {
                        Comment cmt = new Comment();
                        cmt.Content = recod.Values["c.Content"].As<string>();
                        cmt.User = recod.Values["c.User"].As<string>();
                        cmt.Avatar = recod.Values["c.Avatar"].As<string>();
                        cmt.Name = recod.Values["c.Name"].As<string>();
                        cmt.Time = recod.Values["c.Time"].As<LocalDateTime>();
                        lst.Add(cmt);
                    }
                    return lst;
                });
            }
            return lst;
        }
    }
}