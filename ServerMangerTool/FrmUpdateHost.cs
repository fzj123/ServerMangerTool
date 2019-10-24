
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
using Models;

namespace ServerMangerTool
{
    public partial class FrmUpdateHost : Form
    {
        private THostDal thostDal = new THostDal();

        public FrmUpdateHost()
        {
            InitializeComponent();

            showInformation();
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(FrmMain.serverId);
            string warList = "";
            foreach (Control item in groupBoxWar.Controls)
            {
                if (((CheckBox)item).Checked)
                {
                    warList += (item.Text.Trim() + ",");
                }
            }

            THost tHost = new THost()
            {
                name = this.txtName.Text.Trim(),
                ip = this.txtIp.Text.Trim(),
                war = warList,
                username = this.txtUserName.Text.Trim(),
                password = txtPassWord.Text.Trim(),
                instructions = txtInstructions.Text.Trim(),
                id = Convert.ToInt32(FrmMain.serverId)
            };
            try
            {

                if (thostDal.UpdateServer(tHost) == true)
                {
                    MessageBox.Show("添加成功");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("输入有误");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("添加失败"+ex.Message);
            }

        }

        public void showInformation()
        {
            DataTable dtSelectAllList = thostDal.SelectIdServer(FrmMain.serverId).Tables[0];
            foreach (DataRow dataRow in dtSelectAllList.Rows)
            {
                this.txtName.Text = dataRow["name"].ToString();
                this.txtIp.Text = dataRow["ip"].ToString();
                this.txtUserName.Text = dataRow["username"].ToString();
                this.txtPassWord.Text = dataRow["password"].ToString();
                this.txtInstructions.Text = dataRow["username"].ToString();
                string war = dataRow["war"].ToString();
                string[] sArray = war.Split(',');
                foreach (string i in sArray)
                {
                    foreach (Control item in groupBoxWar.Controls)
                    {
                        if (((CheckBox)item).Text == i)
                        {
                            ((CheckBox)item).Checked = true;
                        }
                    }
                    //MessageBox.Show(i);
                } 
                
            }
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
