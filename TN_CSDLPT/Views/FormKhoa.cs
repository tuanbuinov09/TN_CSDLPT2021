using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TN_CSDLPT.Views;

namespace TN_CSDLPT.Views
{
    public partial class FormKhoa : Form
    {
        int vitri;
        String macs;

        public FormKhoa()
        {
            InitializeComponent();
        }

        private void kHOABindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKhoa.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void FormKhoa_Load(object sender, EventArgs e)
        {
            DS.EnforceConstraints = false;
            gcKhoa.UseDisabledStatePainter = false;
            panelControl.Enabled = false;
            // TODO: This line of code loads data into the 'dS.KHOA' table. You can move, or remove it, as needed.
            this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
            this.KhoaTableAdapter.Fill(this.DS.KHOA);

            // TODO: This line of code loads data into the 'DS.GIAOVIEN' table. You can move, or remove it, as needed.
            this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
            this.GiaoVienTableAdapter.Fill(this.DS.GIAOVIEN);
            // TODO: This line of code loads data into the 'DS.LOP' table. You can move, or remove it, as needed.
            this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
            this.LopTableAdapter.Fill(this.DS.LOP);

            macs = ((DataRowView)bdsKhoa[0])["MACS"].ToString();
            cbxCoSo.DataSource = Program.bds_DanhSachPhanManh;
            cbxCoSo.DisplayMember = "TENCS";
            cbxCoSo.ValueMember = "TENSERVER";
            cbxCoSo.SelectedItem = Program.mCoSo;


            // phân quyền
            // nhóm COSO thì ta chỉ cho phép toàn quyền làm việc trên cơ sở  đó , không được log vào cơ sở  khác,   
            if (Program.mGroup == "COSO")
            {
                cbxCoSo.Enabled = false;
                btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;


            }
            //TRUONG thì login đó có thể đăng nhập vào bất kỳ phân mảnh  nào để xem dữ liệu 
            else if (Program.mGroup == "TRUONG")
            {
                cbxCoSo.Enabled = true;
                btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnUndo.Enabled = true;
            }


        }

        private void btnLop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormSinhVien formSinhVien = new FormSinhVien();
            formSinhVien.ShowDialog();

        }

        private void kHOABindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKhoa.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsKhoa.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            panelControl.Enabled = true;
            bdsKhoa.AddNew();
            txtMaCS.Text = macs;

            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            //btnHuy.Enabled = 
            btnUndo.Enabled = btnGhi.Enabled = true;
            gcKhoa.Enabled = false;
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsKhoa.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            panelControl.Enabled = true;


            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcKhoa.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaKH.Text.Trim() == "")
            {
                MessageBox.Show("Mã Khoa không được bỏ trống!", "Lỗi", MessageBoxButtons.OK);
                txtMaKH.Focus();
                return;
            }
            if (txtTenKH.Text.Trim() == "")
            {
                MessageBox.Show("Tên Khoa không được bỏ trống", "Lỗi", MessageBoxButtons.OK);
                txtTenKH.Focus();
                return;
            }


            //MAKH không được trùng trên các phân mảnh ?
            //nhớ viết SP để kiểm tra trùng

            try
            {
                bdsKhoa.EndEdit(); //kết thúc quá trình hiệu chỉnh, ghi vào bdsGiaovien
                bdsKhoa.ResetCurrentItem(); //Đưa thông tin đó lên bảng
                //Cập nhật lại cơ sở dữ liệu trên DB
                this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                this.KhoaTableAdapter.Update(this.DS.KHOA);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi Khoa\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }

            gcKhoa.Enabled = true;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;

            panelControl.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsKhoa.CancelEdit();
            //Nếu hồi nãy đang thêm khi nhấn undo thì nó sẽ set btnAdd là false
            //và gán vị trí dòng sẽ bằng vị trí trước khi thêm
            if (btnThem.Enabled == false) bdsKhoa.Position = vitri;

            gcKhoa.Enabled = true;
            panelControl.Enabled = false;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                this.KhoaTableAdapter.Fill(this.DS.KHOA);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string makh = "";
            //MessageBox.Show(bdsBangDiem.Count + "", "");
            if (bdsLop.Count > 0)
            {
                MessageBox.Show("Không thể xóa Khoa này do có trong Lớp", "", MessageBoxButtons.OK);
                return;
            }

            if (bdsGiaoVien.Count > 0)
            {
                MessageBox.Show("Không thể xóa Khoa này do có trong Giáo Viên", "", MessageBoxButtons.OK);
                return;
            }



            if (MessageBox.Show("Bạn có muốn xóa môn học này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //giữ lại để nếu xóa lỗi thì phục hồi lại tại vị trí đó
                    makh = ((DataRowView)bdsKhoa[bdsKhoa.Position])["MAKH"].ToString();
                    bdsKhoa.RemoveCurrent(); //xóa ở trong bảng hiện tại

                    //Cập nhật lại cơ sở dữ liệu trên DB
                    this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.KhoaTableAdapter.Update(this.DS.KHOA);

                    // TODO: This line of code loads data into the 'DS.GIAOVIEN' table. You can move, or remove it, as needed.
                    this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.GiaoVienTableAdapter.Fill(this.DS.GIAOVIEN);
                    // TODO: This line of code loads data into the 'DS.LOP' table. You can move, or remove it, as needed.
                    this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.LopTableAdapter.Fill(this.DS.LOP);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Khoa\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);

                    //phục hồi lại dữ liệu
                    this.KhoaTableAdapter.Fill(this.DS.KHOA);

                    //bảng sẽ tự động đến dòng đó
                    bdsKhoa.Position = bdsKhoa.Find("MAKH", makh);
                    return;
                }
            }
            else return;


            if (bdsKhoa.Count == 0) btnXoa.Enabled = false;
        }

        private void cbxCoSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCoSo.SelectedValue.ToString() == "System.Data.DataRowView") return;
            Program.servername = cbxCoSo.SelectedValue.ToString();


            //MessageBox.Show("servername " + Program.servername);
            //MessageBox.Show("selected value " + cbxCoSo.SelectedValue.ToString());
            if (cbxCoSo.SelectedItem.ToString() != Program.mCoSo)
            {
                Program.mlogin = Program.remoteLogin;
                Program.password = Program.remotePassword;
            }

            else
            {
                Program.mlogin = Program.mLoginDN;
                Program.password = Program.passwordDN;
            }

            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Lỗi kết nối về cơ sở mới", "Lỗi", MessageBoxButtons.OK);
            }
            else
            {
                DS.EnforceConstraints = false;
                gcKhoa.UseDisabledStatePainter = false;
                panelControl.Enabled = false;

                // TODO: This line of code loads data into the 'dS.KHOA' table. You can move, or remove it, as needed.
                this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                this.KhoaTableAdapter.Fill(this.DS.KHOA);

                // TODO: This line of code loads data into the 'DS.GIAOVIEN' table. You can move, or remove it, as needed.
                this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                this.GiaoVienTableAdapter.Fill(this.DS.GIAOVIEN);
                // TODO: This line of code loads data into the 'DS.LOP' table. You can move, or remove it, as needed.
                this.KhoaTableAdapter.Connection.ConnectionString = Program.connstr;
                this.LopTableAdapter.Fill(this.DS.LOP);

                macs = ((DataRowView)bdsKhoa[0])["MACS"].ToString();
            }

        }
    }
}
