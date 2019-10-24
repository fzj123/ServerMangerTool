using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SshNet
{
    public class SshUpdateFile
    {
        public string shUpdate(string sIp, string sName, string sPwd, string shellCommand)
        {
            SShClientHelper ssh = new SShClientHelper(sIp, sName, sPwd);
            return ssh.Put(shellCommand);
            
        }
        
    }
}
