using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Neo4jSocial.DAO
{
    public class BaseDAO : IDisposable
    {
        public IDriver Driver { get; }
        public BaseDAO()
        {
            string uri = ConfigurationManager.AppSettings["uri"];
            string userNeo4j = ConfigurationManager.AppSettings["userNeo4j"];
            string passNeo4j = ConfigurationManager.AppSettings["passNeo4j"];
            Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(userNeo4j, passNeo4j));
        }

        public void Dispose()
        {
            Driver?.Dispose();
        }

        public void ReplaceInput(ref string str)
        {
            //str = str.Replace(" ", "");
            str = str.Replace("'", "");
            str = str.Replace("/", "");
            str = str.Replace("\\", "");
            //str = str.Replace(".", "");
            //str = str.Replace(",", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("?", "");
            str = str.Replace("`", "");
            str = str.Replace("!", "");
            str = str.Replace("@", "");
            str = str.Replace("#", "");
            str = str.Replace("&", "");
            str = str.Replace("*", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("-", "");
            str = str.Replace("+", "");
            str = str.Replace("_", "");
            str = str.Replace(":", "");
            str = str.Replace(";", "");
            str = str.Replace("~", "");
            str = str.Replace("=", "");
            str = str.Replace("^", "");
            str = str.Replace("%", "");
            str = str.Replace("$", "");
            str = str.Replace("\"", "");
        }
    }
}