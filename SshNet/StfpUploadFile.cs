using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SshNet
{
    public class StfpUploadFile
    {
        public void FileUpload(string localPath, string remotePath, string fileName, string ip, string username, string password)
        {
            SFTPHelper sftp = new SFTPHelper(ip, username, password);
            sftp.Put(localPath, remotePath, fileName);
        }
    }
}
