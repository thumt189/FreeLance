﻿using BookStore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStore.Sale {
    public partial class frmCart : Form {
        DBConnect db = new DBConnect();

        public frmCart() {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            myTable = new DataTable();
            myTableOrder = new DataTable();
            tableCus = new DataTable();
            myBindS = new BindingSource();
            lbNameStaff.Text = DBConnect.TENNHANVIEN;

        }

        private DataTable tableCus;
        private DataTable myTable;
        private DataTable myTableOrder;
        private BindingSource myBindS;
        private string[] headerText = { "Mã sách", "Tên sách", "Tên danh mục", "Tên tác giả", "Mô tả", "Tên NXB", "Số lượng", "Đơn giá" };
        private int[] size = { 15, 25, 20, 20, 15, 15, 10, 20 };

        private string[] headerTextOrder = { "Mã sách", "Tên sách", "Số lượng", "Đơn giá" };
        private int[] sizeOrder = { 15, 25, 20, 20 };


        public void GetData() {
            // Lấy dữ liệu từ CSDL
            string strCL = "SELECT MASACH, TENSACH, TENDANHMUC, TENTACGIA, MOTA, TENNHAXUATBAN, SOLUONGTON, DONGIABAN, ANHBIA FROM SACH"
                         + " INNER JOIN DANHMUC ON DANHMUC.MADANHMUC = SACH.MADANHMUC"
                         + " INNER JOIN TACGIA ON TACGIA.MATACGIA = SACH.MATACGIA"
                         + " INNER JOIN NHAXUATBAN ON NHAXUATBAN.MANHAXUATBAN = SACH.MANHAXUATBAN";
            myTable = db.GetData(strCL);

            // Gán dl bảng vào binding, binding gán vào dgv
            myBindS.DataSource = myTable;
            dgvData.DataSource = myBindS;

            // Set size + headerText cho dgv
            for (int i = 0; i < headerText.Length; i++) {
                dgvData.Columns[i].HeaderText = headerText[i];
                dgvData.Columns[i].Width = ((dgvData.Width) / 100) * size[i];
            }

            this.dgvData.Columns[7].Visible = false;

            // Chọn theo chế độ cả hàng
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void frmCart_Load(object sender, EventArgs e) {
            if (DBConnect.BOPHAN == DBConnect.BANHANG) {

            }

            //Lấy dữ liệu từ bảng sách
            GetData();

            //Lấy tên nhân viên đăng nhập
            // Gán nhân viên
            //string strCmd = "Select TENNHANVIEN from NHANVIEN where MANHANVIEN = '" + IdStaff + "'";
            //string nameStaff = db.GetName(strCmd);
            //lbNameStaff.Text = nameStaff;

            //Đổ dữ liệu khách hàng cho cbb
            string sqlCus = "Select MAKHACHHANG, TENKHACHHANG from KHACHHANG";
            DataTable dt = db.GetData(sqlCus);

            cbbCustomer.DisplayMember = "TENKHACHHANG";
            cbbCustomer.ValueMember = "MAKHACHHANG";
            cbbCustomer.DataSource = dt;

            DataRow dr = dt.NewRow();
            dr["TENKHACHHANG"] = "-- Chọn khách hàng --";
            dr["MAKHACHHANG"] = 0;

            dt.Rows.InsertAt(dr, 0);
            cbbCustomer.SelectedIndex = 0;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e) {
            // Lấy vị trí hàng đang được chọn
            int choose_ing = dgvData.SelectedRows[0].Index;

            if (choose_ing >= 0) {
                // Đặt lại ảnh
                string urlImage = dgvData.Rows[choose_ing].Cells["ANHBIA"].Value.ToString();
                ptbBook.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Resources\\" + urlImage);

            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            //Lấy ra ID
            string id = dgvData.Rows[dgvData.SelectedRows[0].Index].Cells[0].Value.ToString();
            //Select data theo ID vừa lấy
            string strCL = "SELECT MASACH, TENSACH, SOLUONGTON, DONGIABAN FROM SACH"
                         + " WHERE MASACH = '" + id + "'";
            myTableOrder = db.GetData(strCL);
            GetData();
            //Đổ vào dgvOrder
            // Gán dl bảng vào binding, binding gán vào dgv
            myBindS.DataSource = myTableOrder;
            dgvOrder.DataSource = myBindS;

            // Set size + headerText cho dgv
            for (int i = 0; i < headerTextOrder.Length; i++) {
                dgvOrder.Columns[i].HeaderText = headerTextOrder[i];
                dgvOrder.Columns[i].Width = ((dgvOrder.Width) / 100) * sizeOrder[i];
            }
            //dgvOrder.Columns["SOLUONGTON"].DefaultCellStyle.NullValue = "1";

            // Chọn theo chế độ cả hàng
            dgvOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ptbAddCustomer_Click(object sender, EventArgs e) {
            frmCustomer fc = new frmCustomer();
            if (fc.ShowDialog() == DialogResult.OK) {
                // Update dl
                string sql = "Select MAKHACHHANG, TENKHACHHANG from KHACHHANG";
                tableCus = db.GetData(sql);
                cbbCustomer.DataSource = tableCus;
                cbbCustomer.DisplayMember = "TENKHACHHANG";
                cbbCustomer.ValueMember = "MAKHACHHANG";

                // Set khách hàng là khách vừa thêm
                cbbCustomer.SelectedIndex = cbbCustomer.Items.Count - 1;
            }
            string sqlUpdate = "Select MAKHACHHANG, TENKHACHHANG from KHACHHANG";
            tableCus = db.GetData(sqlUpdate);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            string loc = string.Format("TENSACH like '%{0}%' or TENNHAXUATBAN like '%{1}%' or TENTACGIA like '%{2}%' or TENDANHMUC like '%{3}%'",
                                       txtSearch.Text, txtSearch.Text, txtSearch.Text, txtSearch.Text);
            myBindS.Filter = loc;
            dgvData.DataSource = myBindS;
        }
    }
}