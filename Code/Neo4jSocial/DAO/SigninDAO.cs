using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4j.Driver.V1;
using Neo4jSocial.Models;

namespace Neo4jSocial.DAO
{
    public class SigninDAO : BaseDAO
    {
        public bool LoginAccount(string user, string pass)
        {
            using (var session = Driver.Session())
            {
                //var check = session.Run("MATCH (n:Account {User:'"+user+ "',Password:'" + pass + "'}) RETURN id(n)").ToList().Count();
                var check = session.Run("MATCH (n:Account {User:'" + user + "',Password:'" + pass + "'}) RETURN n").FirstOrDefault();
                if (check != null)
                {
                    return true;
                }
                return false;                
            }
        }
    }
}