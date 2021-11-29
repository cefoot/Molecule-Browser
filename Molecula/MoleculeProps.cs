using System;
using System.Collections.Generic;
using System.Text;

namespace Molecula.Molecula
{
    public class Property
    {
        public int CID { get; set; }
        public string MolecularFormula { get; set; }
        public string MolecularWeight { get; set; }
    }

    public class PropertyTable
    {
        public List<Property> Properties { get; set; }
    }

    public class MoleculeProps
    {
        public PropertyTable PropertyTable { get; set; }
    }
}
