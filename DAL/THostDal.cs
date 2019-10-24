using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data.SQLite;
using System.Data;

namespace DAL
{
    public class THostDal
    {
        public bool AddServer(THost tHost)
        {
            bool b = false;
            string sql = @"INSERT INTO t_host (name,ip,war,username,password,instructions) VALUES(@name,@ip,@war,@username,@password,@instructions)";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",tHost.name),
                new SQLiteParameter("@ip",tHost.ip),
                new SQLiteParameter("@war",tHost.war),
                new SQLiteParameter("@username",tHost.username),
                new SQLiteParameter("@password",tHost.password),
                new SQLiteParameter("@instructions",tHost.instructions),
            };
            if (SQLiteHelper.ExecuteSql(sql, parameters) != 1)
            {
                b = false;
            }
            else
            {
                b = true;            
            }
                      
            return b;
        }

        public DataSet SelectAllServer()
        {
            
            string sql = @"select id,name,ip,war,username,password,instructions from t_host";  
                     
            return SQLiteHelper.Query(sql);

        }

        public DataSet SelectNameServer(string name)
        {

            string sql = string.Format("select id,name,ip,war,username,password,instructions from t_host where name like '%{0}%'", name);
            return SQLiteHelper.Query(sql);

        }

        public DataSet SelectIdServer(string id)
        {

            string sql = string.Format("select name,ip,war,username,password,instructions from t_host where id={0}", id);
            return SQLiteHelper.Query(sql);

        }

        public bool DeleteServer(string id)
        {
            
            string sql = @"delete from t_host where id = @id";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@id", id),
            };
            try
            {
                return SQLiteHelper.Exists(sql, parameters);
            }
            catch (Exception)
            {

                throw;
            }          
            
        }

        public bool UpdateServer(THost tHost)
        {
            bool b = false;
            string sql = @"UPDATE t_host SET name=@name,ip=@ip,war=@war,username=@username,password=@password,instructions=@instructions WHERE id=@id";
            SQLiteParameter[] parameters =
            {                
                new SQLiteParameter("@name",tHost.name),
                new SQLiteParameter("@ip",tHost.ip),
                new SQLiteParameter("@war",tHost.war),
                new SQLiteParameter("@username",tHost.username),
                new SQLiteParameter("@password",tHost.password),
                new SQLiteParameter("@instructions",tHost.instructions),
                new SQLiteParameter("@id", tHost.id),
            };
            if (SQLiteHelper.ExecuteSql(sql, parameters) != 1)
            {
                b = false;
            }
            else
            {
                b = true;
            }

            return b;

        }
    }
}
