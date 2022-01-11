using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Database.Exceptions
{
    public class MySqlExceptions : Exception
    {
        public MySqlExceptions(string message) : base(message) { }
    }
}
