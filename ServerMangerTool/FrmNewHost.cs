using Models;
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

namespace ServerMangerTool
{
    public partial class FrmNewHost : Form
    {
        private THostDal thostDal = new THostDal();

        public FrmNewHost()
        {
            InitializeComponent();
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            string warList = "";
            foreach (Control item in groupBoxWar.Controls)
            {
                if (((CheckBox)item).Checked)
                {
                    warList += (item.Text.Trim()+",");
                }
            }

            THost tHost = new THost() {
                name = this.txtName.Text.Trim(),
                ip = this.txtIp.Text.Trim(),
                war = warList,
                username = this.txtUserName.Text.Trim(),
                password = this.txtPassWord.Text.Trim(),
                instructions = this.txtInstructions.Text.Trim()
            };
            try
            {

                if (thostDal.AddServer(tHost) == true)
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

                MessageBox.Show("添加失败"+ ex.Message);
            }

        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
