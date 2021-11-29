using System;
using System.Collections.Generic;
using System.Text;

namespace Molecula.Molecula
{
    public class IdentifierList
    {
        public List<int> CID { get; set; }
    }

    public class CIDIdentifierList
    {
        public IdentifierList IdentifierList { get; set; }
    }
}
