﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TN_CSDLPT.Views
{
    public partial class FromGiangVien : Form
    {
        int vitri; //lưu vị trí của một dòng trong bảng
        String macs;//lưu mã cơ sở;

        public FromGiangVien()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            //giaovienTableAdapter.Fill(DS.GIAOVIEN);
            // This line of code is generated by Data Source Configuration Wizard
            //giaovienTableAdapter.Fill(DS.GIAOVIEN);
        }



        private void gIAOVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsGiaoVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void FromGiangVien_Load(object sender, EventArgs e)
        {
            DS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.GIAOVIEN' table. You can move, or remove it, as needed.
            //this.giaovienTableAdapter.Connection.ConnectionString = Program.connstr;
            //this.giaovienTableAdapter.Fill(this.DS.GIAOVIEN);
            

            // TODO: This line of code loads data into the 'DS.GIAOVIEN' table. You can move, or remove it, as needed.
            this.giaovienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.giaovienTableAdapter.Fill(this.DS.GIAOVIEN);

            // TODO: This line of code loads data into the 'DS.BODE' table. You can move, or remove it, as needed.
            this.bODETableAdapter.Connection.ConnectionString = Program.connstr;
            this.bODETableAdapter.Fill(this.DS.BODE);

            // TODO: This line of code loads data into the 'DS.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
            this.gIAOVIEN_DANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIAOVIEN_DANGKYTableAdapter.Fill(this.DS.GIAOVIEN_DANGKY);

            //macs = ((DataRowView)bdsGiangVien[0])["MACS"].ToString();
            cbxCoSo.DataSource = Program.bds_DanhSachPhanManh;
            cbxCoSo.DisplayMember = "MACS";
            cbxCoSo.ValueMember = "TENSERVER";
            cbxCoSo.SelectedValue = Program.mCoSo;

            //Phân quyền nếu là nhóm TRUONG thì
            if (Program.mGroup == "TRUONG")
            {
                cbxCoSo.Enabled = true; //cho phép chọn cơ sở
                btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
                btnGhi.Enabled = btnUndo.Enabled = false;
            }
            else //các trường hợp còn lại
            {
                cbxCoSo.Enabled = false;
                btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
                btnGhi.Enabled = btnUndo.Enabled = false;
            }

        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsGiaoVien.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            groupBox.Enabled = true;
            bdsGiaoVien.AddNew();

            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnUndo.Enabled = btnGhi.Enabled = true;
            gcGiaoVien.Enabled = false;

        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsGiaoVien.CancelEdit();

            //Nếu hồi nãy đang thêm khi nhấn undo thì nó sẽ set btnAdd là false
            //và gán vị trí dòng sẽ bằng vị trí trước khi thêm
            if (btnThem.Enabled == false) bdsGiaoVien.Position = vitri;
            gcGiaoVien.Enabled = true;
            groupBox.Enabled = true;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsGiaoVien.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            groupBox.Enabled = true;
            

            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcGiaoVien.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.giaovienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.giaovienTableAdapter.Fill(this.DS.GIAOVIEN);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload " + ex.Message, "Lỗi Reload", MessageBoxButtons.OK);
                return;
            }

        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string magv = "";
            MessageBox.Show(bdsBoDe.Count+"","");
            if (bdsBoDe.Count > 0)
            {
                MessageBox.Show("Không thể xóa giáo viên này do đã tạo đề", "", MessageBoxButtons.OK);
                return;
            }

            if (bdsGiaoVien_DangKy.Count > 0)
            {
                MessageBox.Show("Không thể xóa giáo viên này do đã đăng ký môn thi", "", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa giáo viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //giữ lại để nếu xóa lỗi thì phục hồi lại tại vị trí đó
                    magv = ((DataRowView)bdsGiaoVien[bdsGiaoVien.Position])["MAGV"].ToString();
                    bdsGiaoVien.RemoveCurrent(); //xóa ở trong bảng hiện tại

                    //Cập nhật lại cơ sở dữ liệu trên DB
                    this.giaovienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.giaovienTableAdapter.Update(this.DS.GIAOVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa giáo viên\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);

                    //phục hồi lại dữ liệu
                    this.giaovienTableAdapter.Fill(this.DS.GIAOVIEN);

                    //bảng sẽ tự động đến dòng đó
                    bdsGiaoVien.Position = bdsGiaoVien.Find("MAGV", magv);
                    return;
                }
            }

            if (bdsGiaoVien.Count == 0) btnXoa.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaGV.Text.Trim() == "")
            {
                MessageBox.Show("Mã giáo viên không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtMaGV.Focus();
                return;
            }
            if (txtHo.Text.Trim() == "")
            {
                MessageBox.Show("Họ giáo viên không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (txtTen.Text.Trim() == "")
            {
                MessageBox.Show("Tên giáo viên không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtTen.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim() == "")
            {
                MessageBox.Show("Dịa chỉ không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtDiaChi.Focus();
                return;
            }
            if (txtMaKH.Text.Trim() == "")
            {
                MessageBox.Show("Mã khoa không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtMaKH.Focus();
                return;
            }

            //MAGV không được trùng trên các phân mảnh ?
            //==> không cần thiết do giáo viên là nhân bản

            try
            {
                bdsGiaoVien.EndEdit(); //kết thúc quá trình hiệu chỉnh, ghi vào bdsGiaovien
                bdsGiaoVien.ResetCurrentItem(); //Đưa thông tin đó lên bảng
                //Cập nhật lại cơ sở dữ liệu trên DB
                this.giaovienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.giaovienTableAdapter.Update(this.DS.GIAOVIEN);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi giáo viên\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }

            gcGiaoVien.Enabled = true;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;

            groupBox.Enabled = false;
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
