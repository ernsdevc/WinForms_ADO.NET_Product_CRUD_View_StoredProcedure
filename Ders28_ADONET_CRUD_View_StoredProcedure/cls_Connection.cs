using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ders28_ADONET_CRUD_View_StoredProcedure
{
    public class cls_Connection
    {
        public static SqlConnection baglanti
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=MACHINEX;Database=NORTHWND;Trusted_Connection=True");
                return sqlcon;
            }
        }
    }
}
