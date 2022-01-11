using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Database
{
    public abstract class Model
    {
        public abstract void createModel(MySqlConnection context);
        public abstract string getAllRows();
        public abstract string getRowById(int id);
        public abstract string insertRow(params string[] prms);
        public abstract string insertRowById(int id, params string[] prms);
        public abstract string deleteAllRows();
        public abstract string deleteRowById(int id);
        public abstract string getAllRowsByFields(Dictionary<string, string> flds);
        public abstract string getRowsByFields(string[] selectedRows, Dictionary<string, string> flds);

    }
}
