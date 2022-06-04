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

namespace Ders28_ADONET_CRUD_View_StoredProcedure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kategoriDoldur();
            markaDoldur();
            listviewDoldur();
        }
        SqlConnection sqlcon;
        SqlCommand sqlcmd;
        SqlDataReader sdr;

        void kategoriDoldur()
        {
            sqlcon = cls_Connection.baglanti;
            sqlcmd = new SqlCommand("SELECT * FROM Categories", sqlcon);

            sqlcon.Open();
            sdr = sqlcmd.ExecuteReader();
            foreach (var item in sdr)
            {
                cmb_kategori.Items.Add(sdr[1]);
            }
            sqlcon.Close();
        }

        void markaDoldur()
        {
            sqlcon = cls_Connection.baglanti;
            sqlcmd = new SqlCommand("SELECT * FROM Suppliers", sqlcon);

            sqlcon.Open();
            sdr = sqlcmd.ExecuteReader();
            foreach (var item in sdr)
            {
                cmb_marka.Items.Add(sdr[1]);
            }
            sqlcon.Close();
        }

        void listviewDoldur()
        {
            lst_urunler.Items.Clear();

            sqlcon = cls_Connection.baglanti;
            sqlcon.Open();
            sqlcmd = new SqlCommand();
            sqlcmd.CommandText = "SELECT * FROM vw_urunlistesi ORDER BY ProductID DESC";
            sqlcmd.Connection = sqlcon;

            sdr = sqlcmd.ExecuteReader();
            while (sdr.Read())
            {
                ListViewItem lv = new ListViewItem();

                lv.Text = sdr[0].ToString();
                lv.SubItems.Add(sdr[1].ToString());
                lv.SubItems.Add(sdr[2].ToString());
                lv.SubItems.Add(sdr[3].ToString());
                lv.SubItems.Add(sdr[4].ToString());
                lv.SubItems.Add(sdr[5].ToString());

                lst_urunler.Items.Add(lv);
            }
        }
        void temizle()
        {
            txt_urunAdi.Text = txt_Fiyat.Text = txt_Stok.Text = "";
            cmb_kategori.SelectedIndex = -1;
            cmb_marka.SelectedIndex = -1;
        }

        public static int listviewID;

        private void lst_urunler_Click(object sender, EventArgs e)
        {
            listviewID = Convert.ToInt32(lst_urunler.FocusedItem.SubItems[0].Text);
            txt_urunAdi.Text = lst_urunler.FocusedItem.SubItems[1].Text;
            txt_Fiyat.Text = lst_urunler.FocusedItem.SubItems[2].Text;
            txt_Stok.Text = lst_urunler.FocusedItem.SubItems[3].Text;
            cmb_kategori.Text = lst_urunler.FocusedItem.SubItems[4].Text;
            cmb_marka.Text = lst_urunler.FocusedItem.SubItems[5].Text;
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_urunAdi.Text != "" && txt_Fiyat.Text != "" && txt_Stok.Text != "" && cmb_kategori.Text != "" && cmb_marka.Text != "")
            {
                sqlcon = cls_Connection.baglanti;
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "sp_urun_insert";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ProductName", txt_urunAdi.Text);
                sqlcmd.Parameters.AddWithValue("@UnitPrice", txt_Fiyat.Text);
                sqlcmd.Parameters.AddWithValue("@UnitsInStock", txt_Stok.Text);
                sqlcmd.Parameters.AddWithValue("@CategoryID", cmb_kategori.SelectedIndex + 1);
                sqlcmd.Parameters.AddWithValue("@SupplierID", cmb_marka.SelectedIndex + 1);

                try
                {
                    sqlcon.Open();
                    sqlcmd.ExecuteNonQuery();
                    listviewDoldur();
                    MessageBox.Show("Ürün Başarıyla Kaydedildi.");
                    sqlcon.Close();
                    temizle();
                }
                catch (Exception)
                {

                    MessageBox.Show("Ürün Kaydedilemedi.");
                }
            }
            else
            {
                MessageBox.Show("Ürün bilgilerini giriniz!");
            }
        }

        private void btn_Guncelle_Click(object sender, EventArgs e)
        {
            if (listviewID > 0)
            {
                sqlcon = cls_Connection.baglanti;
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "sp_urun_update";
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@ProductName", txt_urunAdi.Text);
                sqlcmd.Parameters.AddWithValue("@UnitPrice", txt_Fiyat.Text);
                sqlcmd.Parameters.AddWithValue("@UnitsInStock", txt_Stok.Text);
                sqlcmd.Parameters.AddWithValue("@CategoryID", cmb_kategori.SelectedIndex + 1);
                sqlcmd.Parameters.AddWithValue("@SupplierID", cmb_marka.SelectedIndex + 1);
                sqlcmd.Parameters.AddWithValue("@ProductID", listviewID);

                try
                {
                    sqlcon.Open();
                    sqlcmd.ExecuteNonQuery();
                    listviewDoldur();
                    MessageBox.Show("Ürün Başarıyla Güncellendi.");
                    sqlcon.Close();
                    temizle();
                }
                catch (Exception)
                {

                    MessageBox.Show("Ürün Güncellenemedi.");
                }
            }
            else
            {
                MessageBox.Show("Listeden bir ürün seçmelisiniz!");
            }
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            if (listviewID > 0)
            {
                sqlcon = cls_Connection.baglanti;
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "sp_urun_delete";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ProductID", listviewID);

                try
                {
                    sqlcon.Open();
                    sqlcmd.ExecuteNonQuery();
                    listviewDoldur();
                    MessageBox.Show("Ürün Başarıyla Silindi.");
                    sqlcon.Close();
                    temizle();
                }
                catch (Exception)
                {

                    MessageBox.Show("Ürün Silinemedi.");
                }
            }
            else
            {
                MessageBox.Show("Listeden bir ürün seçmelisiniz!");
            }
        }

        private void btn_Detay_Click(object sender, EventArgs e)
        {
            if (listviewID > 0)
            {
                frm_Detay f = new frm_Detay(listviewID);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Listeden bir ürün seçmelisiniz!");
            }
        }
    }
}
