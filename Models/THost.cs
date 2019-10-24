using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 服务器信息实体类
    /// </summary>
    public class THost
    {
        public int id { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public string war { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string instructions { get; set; }
    }
}
