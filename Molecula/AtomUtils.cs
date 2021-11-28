using StereoKit;
using System.Collections.Generic;

namespace molecula_shared
{// RecordData myDeserializedClass = JsonConvert.DeserializeObject<RecordData>(myJsonResponse); 
    public struct AtomInfo
    {
        public string Symbol;
        public string Name;
        public Color Color;
    }

    public static class AtomUtils
    {

        private static readonly Dictionary<int, AtomInfo> _dicSymbolNameOrderNumber = new Dictionary<int, AtomInfo>
        {
{1, new AtomInfo{Symbol ="H", Name = "Hydrogen", Color = Color.White}},
{2, new AtomInfo{Symbol ="He", Name = "Helium", Color = Color.White}},
{3, new AtomInfo{Symbol ="Li", Name = "Lithium", Color = Color.White}},
{4, new AtomInfo{Symbol ="Be", Name = "Beryllium", Color = Color.White}},
{5, new AtomInfo{Symbol ="B", Name = "Boron", Color = Color.White}},
{6, new AtomInfo{Symbol ="C", Name = "Carbon", Color = Color.HSV(0f,0f,0.3f)}},
{7, new AtomInfo{Symbol ="N", Name = "Nitrogen", Color = Color.White}},
{8, new AtomInfo{Symbol ="O", Name = "Oxygen", Color = Color.HSV(0f,1f,1f)}},
{9, new AtomInfo{Symbol ="F", Name = "Fluorine", Color = Color.White}},
{10, new AtomInfo{Symbol ="Ne", Name = "Neon", Color = Color.White}},
{11, new AtomInfo{Symbol ="Na", Name = "Sodium", Color = Color.White}},
{12, new AtomInfo{Symbol ="Mg", Name = "Magnesium", Color = Color.White}},
{13, new AtomInfo{Symbol ="Al", Name = "Aluminium", Color = Color.White}},
{14, new AtomInfo{Symbol ="Si", Name = "Silicon", Color = Color.White}},
{15, new AtomInfo{Symbol ="P", Name = "Phosphorus", Color = Color.White}},
{16, new AtomInfo{Symbol ="S", Name = "Sulfur", Color = Color.White}},
{17, new AtomInfo{Symbol ="Cl", Name = "Chlorine", Color = Color.White}},
{18, new AtomInfo{Symbol ="Ar", Name = "Argon", Color = Color.White}},
{19, new AtomInfo{Symbol ="K", Name = "Potassium", Color = Color.White}},
{20, new AtomInfo{Symbol ="Ca", Name = "Calcium", Color = Color.White}},
{21, new AtomInfo{Symbol ="Sc", Name = "Scandium", Color = Color.White}},
{22, new AtomInfo{Symbol ="Ti", Name = "Titanium", Color = Color.White}},
{23, new AtomInfo{Symbol ="V", Name = "Vanadium", Color = Color.White}},
{24, new AtomInfo{Symbol ="Cr", Name = "Chromium", Color = Color.White}},
{25, new AtomInfo{Symbol ="Mn", Name = "Manganese", Color = Color.White}},
{26, new AtomInfo{Symbol ="Fe", Name = "Iron", Color = Color.White}},
{27, new AtomInfo{Symbol ="Co", Name = "Cobalt", Color = Color.White}},
{28, new AtomInfo{Symbol ="Ni", Name = "Nickel", Color = Color.White}},
{29, new AtomInfo{Symbol ="Cu", Name = "Copper", Color = Color.White}},
{30, new AtomInfo{Symbol ="Zn", Name = "Zinc", Color = Color.White}},
{31, new AtomInfo{Symbol ="Ga", Name = "Gallium", Color = Color.White}},
{32, new AtomInfo{Symbol ="Ge", Name = "Germanium", Color = Color.White}},
{33, new AtomInfo{Symbol ="As", Name = "Arsenic", Color = Color.White}},
{34, new AtomInfo{Symbol ="Se", Name = "Selenium", Color = Color.White}},
{35, new AtomInfo{Symbol ="Br", Name = "Bromine", Color = Color.White}},
{36, new AtomInfo{Symbol ="Kr", Name = "Krypton", Color = Color.White}},
{37, new AtomInfo{Symbol ="Rb", Name = "Rubidium", Color = Color.White}},
{38, new AtomInfo{Symbol ="Sr", Name = "Strontium", Color = Color.White}},
{39, new AtomInfo{Symbol ="Y", Name = "Yttrium", Color = Color.White}},
{40, new AtomInfo{Symbol ="Zr", Name = "Zirconium", Color = Color.White}},
{41, new AtomInfo{Symbol ="Nb", Name = "Niobium", Color = Color.White}},
{42, new AtomInfo{Symbol ="Mo", Name = "Molybdenum", Color = Color.White}},
{43, new AtomInfo{Symbol ="Tc", Name = "Technetium", Color = Color.White}},
{44, new AtomInfo{Symbol ="Ru", Name = "Ruthenium", Color = Color.White}},
{45, new AtomInfo{Symbol ="Rh", Name = "Rhodium", Color = Color.White}},
{46, new AtomInfo{Symbol ="Pd", Name = "Palladium", Color = Color.White}},
{47, new AtomInfo{Symbol ="Ag", Name = "Silver", Color = Color.White}},
{48, new AtomInfo{Symbol ="Cd", Name = "Cadmium", Color = Color.White}},
{49, new AtomInfo{Symbol ="In", Name = "Indium", Color = Color.White}},
{50, new AtomInfo{Symbol ="Sn", Name = "Tin", Color = Color.White}},
{51, new AtomInfo{Symbol ="Sb", Name = "Antimony", Color = Color.White}},
{52, new AtomInfo{Symbol ="Te", Name = "Tellurium", Color = Color.White}},
{53, new AtomInfo{Symbol ="I", Name = "Iodine", Color = Color.White}},
{54, new AtomInfo{Symbol ="Xe", Name = "Xenon", Color = Color.White}},
{55, new AtomInfo{Symbol ="Cs", Name = "Caesium", Color = Color.White}},
{56, new AtomInfo{Symbol ="Ba", Name = "Barium", Color = Color.White}},
{71, new AtomInfo{Symbol ="Lu", Name = "Lutetium", Color = Color.White}},
{72, new AtomInfo{Symbol ="Hf", Name = "Hafnium", Color = Color.White}},
{73, new AtomInfo{Symbol ="Ta", Name = "Tantalum", Color = Color.White}},
{74, new AtomInfo{Symbol ="W", Name = "Tungsten", Color = Color.White}},
{75, new AtomInfo{Symbol ="Re", Name = "Rhenium", Color = Color.White}},
{76, new AtomInfo{Symbol ="Os", Name = "Osmium", Color = Color.White}},
{77, new AtomInfo{Symbol ="Ir", Name = "Iridium", Color = Color.White}},
{78, new AtomInfo{Symbol ="Pt", Name = "Platinum", Color = Color.White}},
{79, new AtomInfo{Symbol ="Au", Name = "Gold", Color = Color.White}},
{80, new AtomInfo{Symbol ="Hg", Name = "Mercury", Color = Color.White}},
{81, new AtomInfo{Symbol ="Tl", Name = "Thallium", Color = Color.White}},
{82, new AtomInfo{Symbol ="Pb", Name = "Lead", Color = Color.White}},
{83, new AtomInfo{Symbol ="Bi", Name = "Bismuth", Color = Color.White}},
{84, new AtomInfo{Symbol ="Po", Name = "Polonium", Color = Color.White}},
{85, new AtomInfo{Symbol ="At", Name = "Astatine", Color = Color.White}},
{86, new AtomInfo{Symbol ="Rn", Name = "Radon", Color = Color.White}},
{87, new AtomInfo{Symbol ="Fr", Name = "Francium", Color = Color.White}},
{88, new AtomInfo{Symbol ="Ra", Name = "Radium", Color = Color.White}},
{103, new AtomInfo{Symbol ="Lr", Name = "Lawrencium", Color = Color.White}},
{104, new AtomInfo{Symbol ="Rf", Name = "Rutherfordium", Color = Color.White}},
{105, new AtomInfo{Symbol ="Db", Name = "Dubnium", Color = Color.White}},
{106, new AtomInfo{Symbol ="Sg", Name = "Seaborgium", Color = Color.White}},
{107, new AtomInfo{Symbol ="Bh", Name = "Bohrium", Color = Color.White}},
{108, new AtomInfo{Symbol ="Hs", Name = "Hassium", Color = Color.White}},
{109, new AtomInfo{Symbol ="Mt", Name = "Meitnerium", Color = Color.White}},
{110, new AtomInfo{Symbol ="Ds", Name = "Darmstadtium", Color = Color.White}},
{111, new AtomInfo{Symbol ="Rg", Name = "Roentgenium", Color = Color.White}},
{112, new AtomInfo{Symbol ="Cn", Name = "Copernicium", Color = Color.White}},
{113, new AtomInfo{Symbol ="Nh", Name = "Nihonium", Color = Color.White}},
{114, new AtomInfo{Symbol ="Fl", Name = "Flerovium", Color = Color.White}},
{115, new AtomInfo{Symbol ="Mc", Name = "Moscovium", Color = Color.White}},
{116, new AtomInfo{Symbol ="Lv", Name = "Livermorium", Color = Color.White}},
{117, new AtomInfo{Symbol ="Ts", Name = "Tennessine", Color = Color.White}},
{118, new AtomInfo{Symbol ="Og", Name = "Oganesson", Color = Color.White}},
{57, new AtomInfo{Symbol ="La", Name = "Lanthanum", Color = Color.White}},
{58, new AtomInfo{Symbol ="Ce", Name = "Cerium", Color = Color.White}},
{59, new AtomInfo{Symbol ="Pr", Name = "Praseodymium", Color = Color.White}},
{60, new AtomInfo{Symbol ="Nd", Name = "Neodymium", Color = Color.White}},
{61, new AtomInfo{Symbol ="Pm", Name = "Promethium", Color = Color.White}},
{62, new AtomInfo{Symbol ="Sm", Name = "Samarium", Color = Color.White}},
{63, new AtomInfo{Symbol ="Eu", Name = "Europium", Color = Color.White}},
{64, new AtomInfo{Symbol ="Gd", Name = "Gadolinium", Color = Color.White}},
{65, new AtomInfo{Symbol ="Tb", Name = "Terbium", Color = Color.White}},
{66, new AtomInfo{Symbol ="Dy", Name = "Dysprosium", Color = Color.White}},
{67, new AtomInfo{Symbol ="Ho", Name = "Holmium", Color = Color.White}},
{68, new AtomInfo{Symbol ="Er", Name = "Erbium", Color = Color.White}},
{69, new AtomInfo{Symbol ="Tm", Name = "Thulium", Color = Color.White}},
{70, new AtomInfo{Symbol ="Yb", Name = "Ytterbium", Color = Color.White}},
{89, new AtomInfo{Symbol ="Ac", Name = "Actinium", Color = Color.White}},
{90, new AtomInfo{Symbol ="Th", Name = "Thorium", Color = Color.White}},
{91, new AtomInfo{Symbol ="Pa", Name = "Protactinium", Color = Color.White}},
{92, new AtomInfo{Symbol ="U", Name = "Uranium", Color = Color.White}},
{93, new AtomInfo{Symbol ="Np", Name = "Neptunium", Color = Color.White}},
{94, new AtomInfo{Symbol ="Pu", Name = "Plutonium", Color = Color.White}},
{95, new AtomInfo{Symbol ="Am", Name = "Americium", Color = Color.White}},
{96, new AtomInfo{Symbol ="Cm", Name = "Curium", Color = Color.White}},
{97, new AtomInfo{Symbol ="Bk", Name = "Berkelium", Color = Color.White}},
{98, new AtomInfo{Symbol ="Cf", Name = "Californium", Color = Color.White}},
{99, new AtomInfo{Symbol ="Es", Name = "Einsteinium", Color = Color.White}},
{100, new AtomInfo{Symbol ="Fm", Name = "Fermium", Color = Color.White}},
{101, new AtomInfo{Symbol ="Md", Name = "Mendelevium", Color = Color.White}},
{102, new AtomInfo{Symbol ="No", Name = "Nobelium", Color = Color.White}},
        };

        public static AtomInfo GetAtomInfo(this SingleAtom atom)
        {
            return GetAtomInfo(atom.ElementOrderNumber);
        }

        public static AtomInfo GetAtomInfo(int orderNum)
        {
            return _dicSymbolNameOrderNumber[orderNum];
        }

        public static string GetSymbol(this SingleAtom atom)
        {
            return GetSymbol(atom.ElementOrderNumber);
        }

        public static string GetSymbol(int orderNum)
        {
            return _dicSymbolNameOrderNumber[orderNum].Symbol;
        }

        public static string GetName(this SingleAtom atom)
        {
            return GetName(atom.ElementOrderNumber);
        }

        public static string GetName(int orderNum)
        {
            return _dicSymbolNameOrderNumber[orderNum].Name;
        }

        public static Color GetColor(this SingleAtom atom)
        {
            return GetColor(atom.ElementOrderNumber);
        }

        public static Color GetColor(int orderNum)
        {
            return _dicSymbolNameOrderNumber[orderNum].Color;
        }
    }


}