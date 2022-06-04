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
    public partial class frm_Detay : Form
    {
        public frm_Detay()
        {
            InitializeComponent();
        }

        int listviewID = 0;

        public frm_Detay(int listviewID) : this()
        {
            this.listviewID = listviewID;
        }

        SqlConnection sqlcon;
        SqlCommand sqlcmd;
        SqlDataReader sdr;

        private void frm_Detay_Load(object sender, EventArgs e)
        {
            lbl_productID.Text = listviewID.ToString();

            sqlcon = cls_Connection.baglanti;
            sqlcon.Open();
            sqlcmd = new SqlCommand();
            sqlcmd.CommandText = "SELECT * FROM vw_urun_detay";
            sqlcmd.Parameters.AddWithValue("@ProductID", listviewID);
            sqlcmd.Connection = sqlcon;

            sdr = sqlcmd.ExecuteReader();

            foreach (var item in sdr)
            {
                lbl_productname.Text = sdr[1].ToString();
                lbl_quantityperunit.Text = sdr[2].ToString();
                lbl_unitsonorder.Text = sdr[3].ToString();
                lbl_reorderlevel.Text = sdr[4].ToString();
                if (Convert.ToInt32(sdr[5]) == 0)
                {
                    lbl_discontinued.Text = "Pasif";
                }
                else
                {
                    lbl_discontinued.Text = "Aktif";
                }
            }
        }
    }
}
