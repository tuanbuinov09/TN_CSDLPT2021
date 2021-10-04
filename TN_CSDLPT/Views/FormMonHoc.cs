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
    public partial class FormMonHoc : Form
    {
        int vitri;

        public FormMonHoc()
        {
            InitializeComponent();
        }

        private void mONHOCBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsMonHoc.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void FormMonHoc_Load(object sender, EventArgs e)
        {
            DS.EnforceConstraints = false;
            gcMonHoc.UseDisabledStatePainter = false;
            panelControl.Enabled = false;
            // TODO: This line of code loads data into the 'dS.MONHOC' table. You can move, or remove it, as needed.
            this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
            this.MonHocTableAdapter.Fill(this.DS.MONHOC);
            // TODO: This line of code loads data into the 'dS.BANGDIEM' table. You can move, or remove it, as needed.
            this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
            this.BangDiemTableAdapter.Fill(this.DS.BANGDIEM);
            // TODO: This line of code loads data into the 'dS.BODE' table. You can move, or remove it, as needed.
            this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
            this.BoDeTableAdapter.Fill(this.DS.BODE);
            // TODO: This line of code loads data into the 'dS.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
            this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
            this.GiaoVien_DangKyTableAdapter.Fill(this.DS.GIAOVIEN_DANGKY);

            // phân quyền
            // nhóm CoSo thì ta chỉ cho phép toàn quyền làm việc trên cơ sở  đó , không được log vào cơ sở  khác,   
            if (Program.mGroup == "COSO")
            {
                
                //btnRedo.Visibility = btnHuy.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //btnThemMH.Visibility = btnSuaMH.Visibility = btnGhiMH.Visibility = btnXoaMH.Visibility = btnPhucHoiMH.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            //Truong thì login đó có thể đăng nhập vào bất kỳ phân mảnh  nào để xem dữ liệu 
            else if (Program.mGroup == "TRUONG")
            {
                //btnRedo.Visibility = btnHuy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //btnThemMH.Visibility = btnSuaMH.Visibility = btnGhiMH.Visibility = btnXoaMH.Visibility = btnPhucHoiMH.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsMonHoc.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            panelControl.Enabled = true;
            bdsMonHoc.AddNew();

            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnHuy.Enabled = btnUndo.Enabled = btnGhi.Enabled = true;
            gcMonHoc.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                this.MonHocTableAdapter.Fill(this.DS.MONHOC);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnHuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                this.MonHocTableAdapter.Fill(this.DS.MONHOC);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maMH = "";
            //MessageBox.Show(bdsBangDiem.Count + "", "");
            if (bdsBoDe.Count > 0)
            {
                MessageBox.Show("Không thể xóa môn học này do có trong bộ đề", "", MessageBoxButtons.OK);
                return;
            }

            if (bdsBangDiem.Count > 0)
            {
                MessageBox.Show("Không thể xóa môn học này do có trong bảng điểm", "", MessageBoxButtons.OK);
                return;
            }

            if (bdsGiaoVien_DangKy.Count > 0)
            {
                MessageBox.Show("Không thể xóa môn học này do có giáo viên đã đăng ký", "", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa môn học này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //giữ lại để nếu xóa lỗi thì phục hồi lại tại vị trí đó
                    maMH = ((DataRowView)bdsMonHoc[bdsMonHoc.Position])["MAMH"].ToString();
                    bdsMonHoc.RemoveCurrent(); //xóa ở trong bảng hiện tại

                    //Cập nhật lại cơ sở dữ liệu trên DB
                    this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.MonHocTableAdapter.Update(this.DS.MONHOC);

                    // TODO: This line of code loads data into the 'dS.BANGDIEM' table. You can move, or remove it, as needed.
                    this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.BangDiemTableAdapter.Fill(this.DS.BANGDIEM);
                    // TODO: This line of code loads data into the 'dS.BODE' table. You can move, or remove it, as needed.
                    this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.BoDeTableAdapter.Fill(this.DS.BODE);
                    // TODO: This line of code loads data into the 'dS.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
                    this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.GiaoVien_DangKyTableAdapter.Fill(this.DS.GIAOVIEN_DANGKY);

                    this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.MonHocTableAdapter.Fill(this.DS.MONHOC);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa môn học\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);

                    //phục hồi lại dữ liệu
                    this.MonHocTableAdapter.Fill(this.DS.MONHOC);

                    //bảng sẽ tự động đến dòng đó
                    bdsMonHoc.Position = bdsMonHoc.Find("MAMH", maMH);
                    return;
                }
            }
            else return;
           

            if (bdsMonHoc.Count == 0) btnXoa.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaMH.Text.Trim() == "")
            {
                MessageBox.Show("Mã môn học không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtMaMH.Focus();
                return;
            }
            if (txtTenMH.Text.Trim() == "")
            {
                MessageBox.Show("Tên môn học không được thiếu", "Lỗi", MessageBoxButtons.OK);
                txtTenMH.Focus();
                return;
            }
            

            //MAMH không được trùng trên các phân mảnh ?
            //==> không cần thiết do môn học là nhân bản

            try
            {
               bdsMonHoc.EndEdit(); //kết thúc quá trình hiệu chỉnh, ghi vào bdsGiaovien
               bdsMonHoc.ResetCurrentItem(); //Đưa thông tin đó lên bảng
                //Cập nhật lại cơ sở dữ liệu trên DB
                this.MonHocTableAdapter.Connection.ConnectionString = Program.connstr;
                this.MonHocTableAdapter.Update(this.DS.MONHOC);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi môn học\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                return;
            }

            gcMonHoc.Enabled = true;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;

            panelControl.Enabled = false;
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsMonHoc.Position; //lưu vị trí dòng hiện tại đang trỏ đến trong bảng;
            panelControl.Enabled = true;


            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcMonHoc.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsMonHoc.CancelEdit();

            //Nếu hồi nãy đang thêm khi nhấn undo thì nó sẽ set btnAdd là false
            //và gán vị trí dòng sẽ bằng vị trí trước khi thêm
            if (btnThem.Enabled == false) bdsMonHoc.Position = vitri;
            gcMonHoc.Enabled = true;
            panelControl.Enabled = false;
            btnThem.Enabled = btnHieuChinh.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }
    }
}
