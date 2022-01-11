using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Assets.Database.Models
{
    public class accounts : Model
    {
        MySqlConnection context;

        public int idAccount { get; set; }
        public string accountLogin { get; set; }
        public byte[] accountPassword { get; set; }
        public byte[] accountSalt { get; set; }

        public override void createModel(MySqlConnection context)
        {
            this.context = context;
        }

        public override string deleteAllRows()
        {
            throw new NotImplementedException();
        }

        public override string deleteRowById(int id)
        {
            throw new NotImplementedException();
        }

        public string getRowsByFields(string[] selectedRows)
        {
            string queryText = "SELECT ";
            foreach (var field in selectedRows.Select((value, i) => new { i, value }))
            {
                queryText += (field.i == selectedRows.Length) ? field.value : field.value + ", ";
            }
            queryText += " FROM accounts WHERE 1";

            return queryText;
        }

        public override string getAllRows()
        {
            throw new NotImplementedException();
        }

        public override string getAllRowsByFields(Dictionary<string, string> flds)
        {
            throw new NotImplementedException();
        }

        public override string getRowById(int id)
        {
            throw new NotImplementedException();
        }

        public override string getRowsByFields(string[] selectedRows, Dictionary<string, string> flds)
        {
            string queryText = "SELECT ";
            foreach (var field in selectedRows.Select((value, i) => new { i, value }))
            {
                queryText += (field.i == selectedRows.Length-1) ? field.value : field.value + ", ";
            }
            queryText += " FROM accounts WHERE ";

            foreach (var field in flds)
            {
                queryText += field.Key + " = '" + field.Value + ((flds.Keys.ToList().IndexOf(field.Key) == flds.Count-1) ? "'": "', ");
            }

            return queryText;
        }

        public override string insertRow(params string[] prms)
        {
            throw new NotImplementedException();
        }

        public override string insertRowById(int id, params string[] prms)
        {
            throw new NotImplementedException();
        }
    }
}
