using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SshNet
{
    public class SShClientHelper
    {
        #region 字段或属性
        private SshClient sshClient;
        //private string cmsimagesIP = ConfigurationManager.ConnectionStrings["cmsimagesIP"].ToString();
        //private string cmsimagesName = ConfigurationManager.ConnectionStrings["cmsimagesName"].ToString();
        //private string cmsimagesPwd = ConfigurationManager.ConnectionStrings["cmsimagesPwd"].ToString();

        /// <summary>
        /// SFTP连接状态
        /// </summary>
        public bool Connected { get { return sshClient.IsConnected; } }
        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public SShClientHelper(string sIp, string sName, string sPwd)
        {
            sshClient = new SshClient(sIp, 22, sName, sPwd);
        }
        #endregion

        #region 连接SSH
        /// <summary>
        /// 连接SFTP
        /// </summary>
        /// <returns>true成功</returns>
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    sshClient.Connect();
                }
                return true;
            }
            catch (Exception ex)
            {
                // TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("连接SFTP失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("连接ssh失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region 断开SSH
        /// <summary>
        /// 断开SFTP
        /// </summary> 
        public void Disconnect()
        {
            try
            {
                if (sshClient != null && Connected)
                {
                    sshClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                // TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("断开SFTP失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("断开ssh失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region SSH上传文件

        public string Put(string shellCommand)
        {
            try
            {
                Connect();
                
                using (var cmd = sshClient.CreateCommand(shellCommand))
                {
                    var res = cmd.Execute();

                    string results = cmd.Result;
                    Console.Write(results);
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region SSH上传文件全部

        public string Puts(string shellCommand)
        {
            try
            {
                Connect();
                while (true)
                {                   
                    if (string.IsNullOrWhiteSpace(shellCommand)) continue;
                    var cmd = sshClient.RunCommand(shellCommand);
                    if (!string.IsNullOrWhiteSpace(cmd.Error))
                        Console.WriteLine(cmd.Error);
                    else
                        Console.WriteLine(cmd.Result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("失败，原因：{0}", ex.Message));
            }
        }
        #endregion
    }
}
