using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TN_CSDLPT.Views;

namespace TN_CSDLPT
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }


        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void barButtonDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormDangNhap));
            if (frm != null) frm.Activate();
            else
            {
                FormDangNhap f = new FormDangNhap();
                f.MdiParent = this;
                f.Show();
            }

        }

        private void skinRibbonGalleryBarItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnGiaoVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FromGiangVien));
            if (frm != null) frm.Activate();
            else
            {
                FromGiangVien f = new FromGiangVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnMonHoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormMonHoc));
            if (frm != null) frm.Activate();
            else
            {
                FormMonHoc f = new FormMonHoc();
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}
