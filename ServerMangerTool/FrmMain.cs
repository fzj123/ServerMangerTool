using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using System.IO;
using SshNet;
using System.Diagnostics;
using System.Configuration;

namespace ServerMangerTool
{
    public partial class FrmMain : Form
    {
        private THostDal thostDal = new THostDal();
        public static string serverId { get; set; }
        private StfpUploadFile stfpUploadFile = new StfpUploadFile();
        private SshUpdateFile sshUpdateFile = new SshUpdateFile();

        public FrmMain()
        {
            InitializeComponent();

            refreshServer();


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmNewHost frmNewHost = new FrmNewHost();
            frmNewHost.Show();
        }


        private void txtBoxServerName_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.txtBoxServerName.Text == "请输入服务器名称")
            {
                this.txtBoxServerName.Text = "";
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string name = this.txtBoxServerName.Text;
            DataTable dtSelectAllList = thostDal.SelectNameServer(name).Tables[0];
            this.dgvServerList.DataSource = dtSelectAllList;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否删除？", "删除询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string id = this.dgvServerList.SelectedCells[0].Value.ToString();
                thostDal.DeleteServer(id);
                dgvServerList.Rows.Remove(dgvServerList.CurrentRow);
                MessageBox.Show("删除成功");

            }

        }

        public void refreshServer()
        {
            //显示全部服务器信息
            DataTable dtSelectAllList = thostDal.SelectAllServer().Tables[0];
            this.dgvServerList.DataSource = dtSelectAllList;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshServer();
        }

        private void btnEditor_Click(object sender, EventArgs e)
        {
            FrmMain.serverId = this.dgvServerList.SelectedCells[0].Value.ToString();
            FrmUpdateHost frmUpdateHost = new FrmUpdateHost();
            frmUpdateHost.Show();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            //上传文件
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "所有文件(*.*)|*.*";
            try
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string[] filePaths = dialog.FileNames;

                    foreach (string filePath in filePaths)
                    {
                        string[] condition = { @"\" };
                        string[] result = filePath.Split(condition, StringSplitOptions.None);

                        //"F:\\测试素材\\《新闻联播》201804132100.mp4"
                        DataTable dtSelectAllList = thostDal.SelectIdServer(this.dgvServerList.SelectedCells[0].Value.ToString()).Tables[0];
                        foreach (DataRow dataRow in dtSelectAllList.Rows)
                        {
                            string name = dataRow["name"].ToString();
                            string ip = dataRow["ip"].ToString();
                            string userName = dataRow["username"].ToString();
                            string password = dataRow["password"].ToString();

                            string localPath = filePath.Replace(result.Last(), "");
                            string cmsimagesIP = ConfigurationManager.ConnectionStrings["tomcatPath"].ToString();
                            string remotePath = cmsimagesIP;
                            string fileName = result.Last();
                            //string ip = "100.0.10.242";
                            //string userName = "root";
                            //string password = "Admin@123";
                            stfpUploadFile.FileUpload(localPath, remotePath, fileName, ip, userName, password);
                        }
                    }
                    

                    MessageBox.Show("上传成功");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("上传失败" +ex.Message);
            }           

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string batFile = "startup.bat";
            string shfile = "startup.sh";
            executeBatFile(batFile, shfile);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            string batFile = "shutdown.bat";
            string shfile = "shutdown.sh";
            executeBatFile(batFile, shfile);
        }

        private void btnTomcat_Click(object sender, EventArgs e)
        {
            DataTable dtSelectAllList = thostDal.SelectIdServer(this.dgvServerList.SelectedCells[0].Value.ToString()).Tables[0];
            foreach (DataRow dataRow in dtSelectAllList.Rows)
            {
                string name = dataRow["name"].ToString();
                string ip = dataRow["ip"].ToString();
                string userName = dataRow["username"].ToString();
                string password = dataRow["password"].ToString();
                string shellCommand = @"ps -ef | grep 'tomcat' | grep -v 'grep' | awk '{print $1}'";
                string str = sshUpdateFile.shUpdate(ip, userName, password, shellCommand);
                if (sshUpdateFile.shUpdate(ip, userName, password, shellCommand) == "")
                {
                    MessageBox.Show("tomcat服务没有运行");
                }
                else
                {
                    MessageBox.Show("tomcat服务运行中");
                }
                
            }
        }


        public void executeBatFile(string batFile,string shfile)
        {
            //start.bat文件写入数据
            DataTable dtSelectAllList = thostDal.SelectIdServer(this.dgvServerList.SelectedCells[0].Value.ToString()).Tables[0];
            foreach (DataRow dataRow in dtSelectAllList.Rows)
            {
                string name = dataRow["name"].ToString();
                string ip = dataRow["ip"].ToString();
                string userName = dataRow["username"].ToString();
                string password = dataRow["password"].ToString();

                //FileInfo myFile = new FileInfo(@"sh\start.bat");
                //StreamWriter sw5 = myFile.CreateText();
                string batStr = "@echo off" + "," + "set USERIP=\"" + ip + "\"" + "," + "set user=\"" + userName + "\"" + "," + "set password=\"" + password + "\"" + "," + "putty.exe -ssh -pw %password% -m " +shfile+" %user%@%USERIP%";
                string[] batStrArray = batStr.Split(',');

                File.WriteAllLines(@"sh\" + batFile, batStrArray);
            }
            //执行start.bat文件
            Process proc = null;
            try
            {
                string targetDir = string.Format(@"sh\");
                proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = batFile;
                proc.StartInfo.Arguments = string.Format("10");
                //proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示，经实践可行
                proc.Start();
                proc.WaitForExit();
                MessageBox.Show("执行成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }

        }

        
    }
}
