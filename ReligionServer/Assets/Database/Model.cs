using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 

namespace Assets.Database
{
    public enum ESelectableMethod
    {
        Equal,
        Like,
        Date,
        DateTime
    }
    public interface IDbModel
    {
        IEnumerable<Model> GetModel(MySqlDataReader reader);
    }

    public abstract class Model : IDbModel
    {
        public abstract IEnumerable<Model> GetModel(MySqlDataReader reader);

        public T getForeignModels<T>(Dictionary<string, string> dictionary, Model classInfo, MySqlConnection context) where T: new()
        {
            string getCharacterInfoClassQuery = classInfo.getAllRowsByFields(dictionary, ESelectableMethod.Equal);
            MySqlCommand cmd = new MySqlCommand(getCharacterInfoClassQuery, context);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return (T)classInfo.GetModel(reader);
                }
                else return new T();
            }
        }

        protected virtual string modelName { get; }
        private string getEqualMethod(ESelectableMethod method)
        {
            switch (method)
            {
                case ESelectableMethod.Equal: return "=";
                case ESelectableMethod.Like: return "LIKE";
                default: return "=";
            }
        }
        public abstract void createModel(MySqlConnection context);
        public virtual string getRowById(int id, ESelectableMethod method = ESelectableMethod.Equal)
        {
            string queryText = $"SELECT * FROM {modelName} WHERE id {getEqualMethod(method)} '{id}' LIMIT 1";

            return queryText + ";";
        }
        public virtual string insertRow(Dictionary<string, object> flds)

        {
            string queryText = $"INSERT INTO {modelName} (";

            foreach(var field in flds)
            {
                queryText += $"{field.Key}" + (field.Key == flds.Keys.Last() ? ")" : ", ");
            }

            queryText += " VALUES (";

            foreach (var field in flds)
            {
                queryText += $"'{field.Value}'" + (field.Key == flds.Keys.Last() ? ")" : ", ");
            }

            return queryText + ";";
        }
        public virtual string insertRowById(int id, params string[] prms)
        {
            string queryText = "";

            return queryText;
        }
        public virtual string deleteRowById(int id, ESelectableMethod method = ESelectableMethod.Equal)
        {
            string queryText = $"DELETE FROM {modelName} WHERE id {getEqualMethod(method)} '{id}'";

            return queryText+";";
        }
        public virtual string getAllRows(int limit = 0)
        {
            string limit_c = limit > 0 ? $"1 LIMIT {limit}" : "1";
            string queryText = $"SELECT * FROM {modelName} WHERE {limit_c}";

            return queryText + ";";
        }
        public virtual string getAllRowsByFields(Dictionary<string, string> flds, ESelectableMethod method = ESelectableMethod.Equal, int limit = 0)
        {
            string limit_c = limit > 0 ? $" LIMIT {limit}" : "";
            string queryText = $"SELECT * FROM {modelName} WHERE ";
            foreach (var field in flds)
            {
                queryText += field.Key + " " + getEqualMethod(method) + " '" + field.Value + ((flds.Keys.ToList().IndexOf(field.Key) == flds.Count - 1) ? "'" : "', ");
            }

            queryText += limit_c;

            return queryText + ";";
        }

        public virtual string updateRowWithValuesByFields(Dictionary<string, object> flds, Dictionary<string, string> wFlds, ESelectableMethod method = ESelectableMethod.Equal)
        {
            string queryText = $"UPDATE {modelName} SET ";
            foreach (var field in flds)
            {
                queryText += $"{field.Key} = '{field.Value}{((flds.Keys.ToList().IndexOf(field.Key) == flds.Count - 1) ? "'" : "', ")}";
            }

            queryText += $" WHERE ";
            foreach (var field in wFlds)
            {
                queryText += field.Key + " " + getEqualMethod(method) + " '" + field.Value + ((wFlds.Keys.ToList().IndexOf(field.Key) == wFlds.Count - 1) ? "'" : "', ");
            }

            return queryText + ";";
        }

        /*public virtual string getRowsByFields(string[] selectedRows, Dictionary<string, string> flds, ESelectableMethod method = ESelectableMethod.Equal, int limit = 0)
        {
            string limit_c = limit > 0 ? $" LIMIT {limit}" : "";
            string queryText = "SELECT ";
            foreach (var field in selectedRows.Select((value, i) => new { i, value }))
            {
                queryText += (field.i == selectedRows.Length - 1) ? field.value : field.value + ", ";
            }
            queryText += $" FROM {modelName} WHERE ";

            foreach (var field in flds)
            {
                queryText += field.Key + " " + getEqualMethod(method) + " '" + field.Value + ((flds.Keys.ToList().IndexOf(field.Key) == flds.Count - 1) ? "'" : "', ");
            }

            queryText += limit_c;

            return queryText + ";";
        }*/

        public virtual string deleteAllRows()
        {
            return "";
        }

        public virtual string deleteRowsByFields()
        {
            return "";
        }
    }
}

#pragma warning restore IDE1006 // Стили именования
