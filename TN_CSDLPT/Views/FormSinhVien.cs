using System;
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
    public partial class FormSinhVien : Form
    {
        int vitri;
        public FormSinhVien()
        {
            InitializeComponent();
        }

        private void sINHVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsSinhVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void sINHVIENBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsSinhVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void FormSinhVien_Load(object sender, EventArgs e)
        {

            DS.EnforceConstraints = false;
            gcSinhVien.UseDisabledStatePainter = false;
            panelControl.Enabled = false;
            // TODO: This line of code loads data into the 'dS.SINHVIEN' table. You can move, or remove it, as needed.
            this.SinhVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.SinhVienTableAdapter.Fill(this.DS.SINHVIEN);

            // TODO: This line of code loads data into the 'DS.BANGDIEM' table. You can move, or remove it, as needed.
            this.SinhVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.BangDiemTableAdapter.Fill(this.DS.BANGDIEM);

            // phân quyền
            // nhóm CoSo thì ta chỉ cho phép toàn quyền làm việc trên cơ sở  đó , không được log vào cơ sở  khác,   
            if (Program.mGroup == "COSO")
            {
                cbxCoSo.Enabled = false;


            }
            //Truong thì login đó có thể đăng nhập vào bất kỳ phân mảnh  nào để xem dữ liệu 
            else if (Program.mGroup == "TRUONG")
            {
                cbxCoSo.Enabled = true;

            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsSinhVien.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            panelControl.Enabled = true;
            bdsSinhVien.AddNew();

            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            //btnHuy.Enabled = 
            btnUndo.Enabled = btnGhi.Enabled = true;
            gcSinhVien.Enabled = false;
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsSinhVien.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            panelControl.Enabled = true;

            //btnHuy.Enabled = 
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcSinhVien.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaSV.Text.Trim() == "")
            {
                MessageBox.Show("Mã sinh viên không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtMaSV.Focus();
                return;
            }
            if (txtHo.Text.Trim() == "")
            {
                MessageBox.Show("Họ sinh viên không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (txtTen.Text.Trim() == "")
            {
                MessageBox.Show("Tên sinh viên không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtTen.Focus();
                return;
            }
            if (txtMaLop.Text.Trim() == "")
            {
                MessageBox.Show("Mã lớp không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtMaLop.Focus();
                return;
            }
            //if (txtNgaySinh.Text.Trim() == "")
            //{
            //    MessageBox.Show("Ngày sinh không được thiếu", "Lỗi", MessageBoxButtons.OK);
            //    txtNgaySinh.Focus();
            //    return;
            //}



            //MAMH không được trùng trên các phân mảnh ?
            //==> không cần thiết do môn học là nhân bản

            try
            {
                bdsSinhVien.EndEdit(); //kết thúc quá trình hiệu chỉnh, ghi vào bdsGiaovien
                bdsSinhVien.ResetCurrentItem(); //Đưa thông tin đó lên bảng
                //Cập nhật lại cơ sở dữ liệu trên DB
                this.SinhVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.SinhVienTableAdapter.Update(this.DS.SINHVIEN);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi sinh viên\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }

            gcSinhVien.Enabled = true;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;

            panelControl.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsSinhVien.CancelEdit();

            //Nếu hồi nãy đang thêm khi nhấn undo thì nó sẽ set btnAdd là false
            //và gán vị trí dòng sẽ bằng vị trí trước khi thêm
            if (btnThem.Enabled == false) bdsSinhVien.Position = vitri;
            gcSinhVien.Enabled = true;
            panelControl.Enabled = true;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maMH = "";
            //MessageBox.Show(bdsBangDiem.Count + "", "");
            if (bdsBangDiem.Count > 0)
            {
                MessageBox.Show("Không thể xóa sinh viên này do đã có trong bảng điểm", "", MessageBoxButtons.OK);
                return;
            }


            if (MessageBox.Show("Bạn có muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //giữ lại để nếu xóa lỗi thì phục hồi lại tại vị trí đó
                    maMH = ((DataRowView)bdsSinhVien[bdsSinhVien.Position])["MAMH"].ToString();
                    bdsSinhVien.RemoveCurrent(); //xóa ở trong bảng hiện tại

                    //Cập nhật lại cơ sở dữ liệu trên DB
                    this.SinhVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.SinhVienTableAdapter.Update(this.DS.SINHVIEN);

                    // TODO: This line of code loads data into the 'dS.BANGDIEM' table. You can move, or remove it, as needed.
                    this.SinhVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.BangDiemTableAdapter.Fill(this.DS.BANGDIEM);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa sinh viên\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);

                    //phục hồi lại dữ liệu
                    this.SinhVienTableAdapter.Fill(this.DS.SINHVIEN);

                    //bảng sẽ tự động đến dòng đó
                    bdsSinhVien.Position = bdsSinhVien.Find("MASV", maMH);
                    return;
                }
            }
            else return;


            if (bdsSinhVien.Count == 0) btnXoa.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'dS.SINHVIEN' table. You can move, or remove it, as needed.
                this.SinhVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.SinhVienTableAdapter.Fill(this.DS.SINHVIEN);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }
        }
    }
}
