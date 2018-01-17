using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Common
{
    public class ExceptionUtilityBase
    {
        public string Cause { get; set; }
        public string Namespace { get; set; }
        public string Source { get; set; }
        public string Method { get; set; }
        public System.Exception Exception { get; set; }
    }
}
