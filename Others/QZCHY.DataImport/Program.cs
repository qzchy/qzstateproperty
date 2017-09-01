using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.DataImport
{
    class Program
    {       
        static void Main(string[] args)
        {
            var filePath = @"F:\国有资产展示系统\QZCHY.QZStatePropertyManagementSystem\Presentation\QZCHY.API\App_Data\import.xls";

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            System.Data.OleDb.OleDbDataAdapter myCommand = null;
            System.Data.DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
            ds = new System.Data.DataSet(); myCommand.Fill(ds, "table1");

            var table = ds.Tables[0];
            for (var i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];

                string governmentName = row[1].ToString();
                string parentGovernmentName = row[2].ToString();

                //判断是否存在
                var government = new GovernmentUnit { };

                var property = new Property {
                   
                };
            }
        }
    }
}
