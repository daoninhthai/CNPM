﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNPM
{
    public partial class dangnhap : Form
    {
        int dem;//đếm số lần đăng nhập sai
        string ten;// tên nhân viên đăng nhập
        string chucvu;//chức vụ nhân viên đăng nhập


        private string StringConnect = @"Data Source=THETUYEN\SQLEXPRESS;Initial Catalog=QuanLyCuaHangGiay;Integrated Security=True";
        private SqlConnection Connect = null;
        public dangnhap()
        {
            InitializeComponent();
        }
        private void loadtkmk()
        {
            tbmatkhau.Text = "";
            tbtaikhoan.Text = "";
            tbtaikhoan.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connect = new SqlConnection(StringConnect); //Khởi tạo kết nối với đường dẫn StringConnect
            Connect.Open();
            quenmatkhau.Hide();
            dem = 0;
        }

        private void btdangnhap_Click(object sender, EventArgs e)
        {
            bool x;
            x=checktkmk(tbtaikhoan.Text, tbmatkhau.Text,ref dem,out ten,out chucvu);
            if (x)
            {
                if (chucvu == "Quản lý")
                {
                    Form a = new homeql(StringConnect);
                    a.Show();
                    this.Hide();
                }
                else
                {
                    Form a = new homenv(ten,StringConnect);
                    a.Show();
                    this.Hide();
                }
            }
            else
                loadtkmk();
        }

        private void dangnhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void quenmatkhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form a = new quenmk();
            a.Show();
            this.Hide();
        }
        private bool checktkmk(string tk,string mk,ref int sai,out string ten, out string cv)
        {
            cv = "";
            ten = "";
            bool x = false;
            string dem="";//đệm
            DataTable tkmk = new DataTable();
            string query = "select Usename,Pass from NV where Usename='"+tk+"'";
            SqlDataAdapter a = new SqlDataAdapter(query,Connect);
            a.Fill(tkmk);
            foreach (DataRow dr in tkmk.Rows)
            {
                dem = dr["Usename"].ToString();
            }
            if (dem == "")
            {
                MessageBox.Show("Tài khoản không chính xác", "Warning", MessageBoxButtons.OK);
                sai++;
            }
            else
            {
                dem = "";
                query = "select Pass from NV where Usename='" + tk + "'";
                a = new SqlDataAdapter(query, Connect);
                a.Fill(tkmk);
                foreach (DataRow dr in tkmk.Rows)
                {
                    dem = dr["Pass"].ToString();
                }
                if (dem != tbmatkhau.Text)
                {
                    MessageBox.Show("Mật khẩu không chính xác", "Warning", MessageBoxButtons.OK);
                    sai++;
                }
                else
                {
                    x = true;
                    query = "select Ten_NV, ChucVu from NV where Usename='" + tk + "'";
                    a = new SqlDataAdapter(query, Connect);
                    a.Fill(tkmk);
                    foreach (DataRow dr in tkmk.Rows)
                    {
                        cv = dr["ChucVu"].ToString();
                        ten = dr["Ten_NV"].ToString();
                    }
                }
            }
            return x;
        }
    }
}
