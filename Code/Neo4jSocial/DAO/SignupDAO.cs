using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4j.Driver.V1;
using Neo4jSocial.Models;

namespace Neo4jSocial.DAO
{
    public class SignupDAO : BaseDAO
    {
        public bool CreateAccount(string user, string pass)
        {
            using (var session = Driver.Session())
            {
                // ... function check user are ready?
                var check = session.Run("MATCH (n:Account {User:'"+user+ "'}) RETURN id(n)").ToList().Count();
                if (check==0)
                {
                    session.WriteTransaction(
                    tx =>
                    {
                        var id = tx.Run("CREATE (n:Account {User:'" + user + "',Password:'" + pass + "'})");
                    }
                );
                    return true;
                }
                return false;                
            }
        }
    }
}