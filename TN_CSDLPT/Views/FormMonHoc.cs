using System;
using System.Collections;
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

        private Boolean checkThem = false;
        private Boolean checkSua = false;
        public static Boolean checkSave = true;
        private ArrayList arrPhucHoi = new ArrayList();
        private int viTri = 0;

        private String truocKhiUpdate;
        private String sauKhiUpdate;
        //private PhucHoi phucHoi;
        public FormMonHoc()
        {
            InitializeComponent();
        }

        //private void frmMonHoc_Load(object sender, EventArgs e)
        //{
        //    this.ControlBox = false;
        //    TNDataSet.EnforceConstraints = false;
        //    gcMH.UseDisabledStatePainter = false;
        //    // TODO: This line of code loads data into the 'tNDataSet.BANGDIEM' table. You can move, or remove it, as needed.
        //    this.tbBangDiemADT.Connection.ConnectionString = Program.connstr;
        //    this.tbBangDiemADT.Fill(this.TNDataSet.BANGDIEM);
        //    // TODO: This line of code loads data into the 'tNDataSet.BODE' table. You can move, or remove it, as needed.
        //    this.tbBoDeADT.Connection.ConnectionString = Program.connstr;
        //    this.tbBoDeADT.Fill(this.TNDataSet.BODE);
        //    // TODO: This line of code loads data into the 'tNDataSet.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
        //    this.tbGVDKyADT.Connection.ConnectionString = Program.connstr;
        //    this.tbGVDKyADT.Fill(this.TNDataSet.GIAOVIEN_DANGKY);

        //    this.tbMonHoc.Connection.ConnectionString = Program.connstr;
        //    this.tbMonHoc.Fill(this.TNDataSet.MONHOC);
        //    // phân quyền
        //    // nhóm CoSo thì ta chỉ cho phép toàn quyền làm việc trên cơ sở  đó , không được log vào cơ sở  khác,   
        //    if (Program.mGroup == "Coso")
        //    {
        //        btnRedo.Visibility = btnHuy.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        //        btnThemMH.Visibility = btnSuaMH.Visibility = btnGhiMH.Visibility = btnXoaMH.Visibility = btnPhucHoiMH.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        //    }
        //    //Truong thì login đó có thể đăng nhập vào bất kỳ phân mảnh  nào để xem dữ liệu 
        //    else if (Program.mGroup == "Truong")
        //    {
        //        btnRedo.Visibility = btnHuy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //        btnThemMH.Visibility = btnSuaMH.Visibility = btnGhiMH.Visibility = btnXoaMH.Visibility = btnPhucHoiMH.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //    }

        //    if (viTri <= 0)
        //    {
        //        btnPhucHoiMH.Enabled = btnRedo.Enabled = false;
        //    }
        //    else if (viTri < arrPhucHoi.Count)
        //    {
        //        btnRedo.Enabled = true;
        //    }
        //    else
        //    {
        //        btnPhucHoiMH.Enabled = true;
        //    }
        //}

        private void mONHOCBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.mONHOCBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void FormMonHoc_Load(object sender, EventArgs e)
        {
            DS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.MONHOC' table. You can move, or remove it, as needed.
            this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
            this.mONHOCTableAdapter.Fill(this.DS.MONHOC);

            

        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //try
            //{
            //    bdsMonHoc.AddNew();
            //    gcMH.Enabled = false;
            //    edtTenMH.Enabled = edtMaMH.Enabled = true;
            //    edtMaMH.Focus();
            //    btnThemMH.Enabled = btnSuaMH.Enabled = btnTaiLaiMH.Enabled = btnXoaMH.Enabled = btnTim.Enabled = edtTim.Enabled = false;
            //    checkThem = true;
            //    checkSave = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi thêm môn học " + ex.Message, "", MessageBoxButtons.OK);
            //}
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
