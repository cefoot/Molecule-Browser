using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace molecula_shared
{// RecordData myDeserializedClass = JsonConvert.DeserializeObject<RecordData>(myJsonResponse); 
    public class Id2
    {
        public int cid { get; set; }
        public Id id { get; set; }
    }

    public class SingleAtom
    {
        public int AtomId { get; set; }

        public int ElementOrderNumber { get; set; }

        public Dictionary<SingleAtom, int> Bonds { get; set; }

        public Vec3 Position { get; set; }

        public override string ToString()
        {
            return AtomUtils.GetSymbol(ElementOrderNumber);
        }
    }

    public class Atoms
    {
        public List<int> aid { get; set; }
        public List<int> element { get; set; }
    }

    public class Bonds
    {
        public List<int> aid1 { get; set; }
        public List<int> aid2 { get; set; }
        public List<int> order { get; set; }
    }

    public class Urn
    {
        public string label { get; set; }
        public string name { get; set; }
        public int datatype { get; set; }
        public string version { get; set; }
        public string software { get; set; }
        public string source { get; set; }
        public string release { get; set; }
        public string parameters { get; set; }
    }

    public class Value
    {
        public string sval { get; set; }
        public double? fval { get; set; }
        public List<string> slist { get; set; }
        public List<double> fvec { get; set; }
        public List<int> ivec { get; set; }
    }

    public class Datum
    {
        public Urn urn { get; set; }
        public Value value { get; set; }
    }

    public class Conformer
    {
        public List<double> x { get; set; }
        public List<double> y { get; set; }
        public List<double> z { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Coord
    {
        public List<int> type { get; set; }
        public List<int> aid { get; set; }
        public List<Conformer> conformers { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Prop
    {
        public Urn urn { get; set; }
        public Value value { get; set; }
    }

    public class Count
    {
        public int heavy_atom { get; set; }
        public int atom_chiral { get; set; }
        public int atom_chiral_def { get; set; }
        public int atom_chiral_undef { get; set; }
        public int bond_chiral { get; set; }
        public int bond_chiral_def { get; set; }
        public int bond_chiral_undef { get; set; }
        public int isotope_atom { get; set; }
        public int covalent_unit { get; set; }
        public int tautomers { get; set; }
    }

    public class PCCompound
    {
        public Id id { get; set; }
        public Atoms atoms { get; set; }
        public Bonds bonds { get; set; }
        public List<Coord> coords { get; set; }
        public List<Prop> props { get; set; }
        public Count count { get; set; }

        private List<SingleAtom> _atomList;

        public List<SingleAtom> AtomList
        {
            get
            {
                if (_atomList == null)
                {
                    _atomList = FindAtoms();
                }
                return _atomList;
            }
        }

        private List<SingleAtom> FindAtoms()
        {
            var atomList = new Dictionary<int, SingleAtom>();
            var idx = 0;
            foreach (var currentAid in atoms.aid)
            {
                atomList[currentAid] = new SingleAtom
                {
                    AtomId = currentAid,
                    ElementOrderNumber = atoms.element[idx++],
                    Bonds = new Dictionary<SingleAtom, int>()
                };
                var coordIdx = coords[0].aid.Select((aid, aidIdx) => new { AtomId = aid, Index = aidIdx }).Where(e => e.AtomId == currentAid).Select(e => e.Index).First();
                atomList[currentAid].Position = new Vec3(
                    x: (float)coords[0].conformers[0].x[coordIdx],
                    y: (float)coords[0].conformers[0].y[coordIdx],
                    z: (float)coords[0].conformers[0].z[coordIdx]
                    );

            }
            idx = 0;
            foreach (var aid in bonds.aid1)
            {
                for (int i = 0; i < bonds.order[idx]; i++)
                {
                    var other = atomList[bonds.aid2[idx]];
                    if (atomList[aid].Bonds.ContainsKey(other))
                    {
                        atomList[aid].Bonds[other]++;
                    }
                    else
                    {
                        atomList[aid].Bonds[other] = 1;
                    }
                }
                idx++;
            }
            return atomList.Values.ToList();
        }
    }

    public class RecordData
    {
        public List<PCCompound> PC_Compounds { get; set; }
    }


}