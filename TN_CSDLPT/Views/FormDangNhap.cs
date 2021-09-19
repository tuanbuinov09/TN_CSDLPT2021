using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TN_CSDLPT
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            if (KetNoiCSDLGoc() == 0)
            {
                return;
            }
            LayDanhSachPhanManh("SELECT * FROM VIEW_DS_COSO");
            comboBoxCoSo.SelectedIndex = 1; comboBoxCoSo.SelectedIndex = 0;
        }
        private SqlConnection conn_publisher = new SqlConnection();
        
        private void LayDanhSachPhanManh(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn_publisher.State == ConnectionState.Closed)
            {
                conn_publisher.Open();
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd, conn_publisher);
            sda.Fill(dt);
            conn_publisher.Close();
            Program.bds_DanhSachPhanManh.DataSource = dt;
            comboBoxCoSo.DataSource = Program.bds_DanhSachPhanManh;
            comboBoxCoSo.DisplayMember = "MACS"; comboBoxCoSo.ValueMember = "TENSERVER";
        }
        private int KetNoiCSDLGoc()
        {
            if (conn_publisher !=null && conn_publisher.State == ConnectionState.Open)
            {
                conn_publisher.Close();
            }
            try
            {
                conn_publisher.ConnectionString = Program.connstr_publisher;
                conn_publisher.Open();
                return 1;
            } catch(Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n " + e.Message, "", MessageBoxButtons.OK);
            }
            return 0;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //btn dang nhap click
        {
            if(textBoxTenDangNhap.Text.Trim() == "" || textBoxMatKhau.Text.Trim() == "")
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không được bỏ trống", "", MessageBoxButtons.OK);
                return;
            }
            Program.mlogin = textBoxTenDangNhap.Text.Trim();
            Program.password = textBoxMatKhau.Text.Trim();
            if(Program.KetNoi() == 0)
            {
                return;
            }
            Program.mCoSo = comboBoxCoSo.Text;
            /*MessageBox.Show(Program.mCoSo , "", MessageBoxButtons.OK);*/
            Program.mLoginDN = Program.mlogin;
            Program.passwordDN = Program.password;
            String strLenh = "EXEC SP_THONGTINDANGNHAPGV '" + Program.mlogin + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);

            if (Program.myReader == null)
            {
                return;
            }
            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0);
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Ten dang nhap ban dung khong co quyen truy cap du lieu\nHay xem lai ten dang nhap va mat khau", "", MessageBoxButtons.OK);
                return;
            }
            Program.mHoTen = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.conn.Close();
            Program.formChinh.MAGV.Text = Program.username;
            Program.formChinh.HOTEN.Text = Program.mHoTen;
            Program.formChinh.NHOM.Text = Program.mGroup;
            if (radioButtonGiaoVien.Checked)
            {

            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.servername = comboBoxCoSo.SelectedValue.ToString();
            }catch(Exception ex)
            {

            }
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            Close();
            Program.formChinh.Close();
        }
    }
}
