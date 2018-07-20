using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitrum
{
    public class EiDatabaseFilter : Attribute
    {
        public Type typeFilter;
        public string pathFilter;

        public EiDatabaseFilter(Type type)
        {
            this.typeFilter = type;
        }

        public EiDatabaseFilter(string pathFilter)
        {
            this.pathFilter = pathFilter;
        }

        public EiDatabaseFilter(Type type, string pathFilter)
        {
            this.typeFilter = type;
            this.pathFilter = pathFilter;
        }
    }
}
