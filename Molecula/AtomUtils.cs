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
{001, new AtomInfo{Symbol ="H", Color = Color.Hex(0xFFFFFFFF),Name = "Hydrogen"}},
{002, new AtomInfo{Symbol ="He", Color = Color.Hex(0xD9FFFFFF), Name = "Helium"}},
{003, new AtomInfo{Symbol ="Li", Color = Color.Hex(0xCC80FFFF), Name = "Lithium"}},
{004, new AtomInfo{Symbol ="Be", Color = Color.Hex(0xC2FF00FF), Name = "Beryllium"}},
{005, new AtomInfo{Symbol ="B", Color = Color.Hex(0xFFB5B5FF), Name = "Boron"}},
{006, new AtomInfo{Symbol ="C", Color = Color.Hex(0x909090FF), Name = "Carbon"}},
{007, new AtomInfo{Symbol ="N", Color = Color.Hex(0x3050F8FF), Name = "Nitrogen"}},
{008, new AtomInfo{Symbol ="O", Color = Color.Hex(0xFF0D0DFF), Name = "Oxygen"}},
{009, new AtomInfo{Symbol ="F", Color = Color.Hex(0x90E050FF), Name = "Fluorine"}},
{010, new AtomInfo{Symbol ="Ne", Color = Color.Hex(0xB3E3F5FF), Name = "Neon"}},
{011, new AtomInfo{Symbol ="Na", Color = Color.Hex(0xAB5CF2FF), Name = "Sodium"}},
{012, new AtomInfo{Symbol ="Mg", Color = Color.Hex(0x8AFF00FF), Name = "Magnesium"}},
{013, new AtomInfo{Symbol ="Al", Color = Color.Hex(0xBFA6A6FF), Name = "Aluminium"}},
{014, new AtomInfo{Symbol ="Si", Color = Color.Hex(0xF0C8A0FF), Name = "Silicon"}},
{015, new AtomInfo{Symbol ="P", Color = Color.Hex(0xFF8000FF), Name = "Phosphorus"}},
{016, new AtomInfo{Symbol ="S", Color = Color.Hex(0xFFFF30FF), Name = "Sulfur"}},
{017, new AtomInfo{Symbol ="Cl", Color = Color.Hex(0x1FF01FFF), Name = "Chlorine"}},
{018, new AtomInfo{Symbol ="Ar", Color = Color.Hex(0x80D1E3FF), Name = "Argon"}},
{019, new AtomInfo{Symbol ="K", Color = Color.Hex(0x8F40D4FF), Name = "Potassium"}},
{020, new AtomInfo{Symbol ="Ca", Color = Color.Hex(0x3DFF00FF), Name = "Calcium"}},
{021, new AtomInfo{Symbol ="Sc", Color = Color.Hex(0xE6E6E6FF), Name = "Scandium"}},
{022, new AtomInfo{Symbol ="Ti", Color = Color.Hex(0xBFC2C7FF), Name = "Titanium"}},
{023, new AtomInfo{Symbol ="V", Color = Color.Hex(0xA6A6ABFF), Name = "Vanadium"}},
{024, new AtomInfo{Symbol ="Cr", Color = Color.Hex(0x8A99C7FF), Name = "Chromium"}},
{025, new AtomInfo{Symbol ="Mn", Color = Color.Hex(0x9C7AC7FF), Name = "Manganese"}},
{026, new AtomInfo{Symbol ="Fe", Color = Color.Hex(0xE06633FF), Name = "Iron"}},
{027, new AtomInfo{Symbol ="Co", Color = Color.Hex(0xF090A0FF), Name = "Cobalt"}},
{028, new AtomInfo{Symbol ="Ni", Color = Color.Hex(0x50D050FF), Name = "Nickel"}},
{029, new AtomInfo{Symbol ="Cu", Color = Color.Hex(0xC88033FF), Name = "Copper"}},
{030, new AtomInfo{Symbol ="Zn", Color = Color.Hex(0x7D80B0FF), Name = "Zinc"}},
{031, new AtomInfo{Symbol ="Ga", Color = Color.Hex(0xC28F8FFF), Name = "Gallium"}},
{032, new AtomInfo{Symbol ="Ge", Color = Color.Hex(0x668F8FFF), Name = "Germanium"}},
{033, new AtomInfo{Symbol ="As", Color = Color.Hex(0xBD80E3FF), Name = "Arsenic"}},
{034, new AtomInfo{Symbol ="Se", Color = Color.Hex(0xFFA100FF), Name = "Selenium"}},
{035, new AtomInfo{Symbol ="Br", Color = Color.Hex(0xA62929FF), Name = "Bromine"}},
{036, new AtomInfo{Symbol ="Kr", Color = Color.Hex(0x5CB8D1FF), Name = "Krypton"}},
{037, new AtomInfo{Symbol ="Rb", Color = Color.Hex(0x702EB0FF), Name = "Rubidium"}},
{038, new AtomInfo{Symbol ="Sr", Color = Color.Hex(0x00FF00FF), Name = "Strontium"}},
{039, new AtomInfo{Symbol ="Y", Color = Color.Hex(0x94FFFFFF), Name = "Yttrium"}},
{040, new AtomInfo{Symbol ="Zr", Color = Color.Hex(0x94E0E0FF), Name = "Zirconium"}},
{041, new AtomInfo{Symbol ="Nb", Color = Color.Hex(0x73C2C9FF), Name = "Niobium"}},
{042, new AtomInfo{Symbol ="Mo", Color = Color.Hex(0x54B5B5FF), Name = "Molybdenum"}},
{043, new AtomInfo{Symbol ="Tc", Color = Color.Hex(0x3B9E9EFF), Name = "Technetium"}},
{044, new AtomInfo{Symbol ="Ru", Color = Color.Hex(0x248F8FFF), Name = "Ruthenium"}},
{045, new AtomInfo{Symbol ="Rh", Color = Color.Hex(0x0A7D8CFF), Name = "Rhodium"}},
{046, new AtomInfo{Symbol ="Pd", Color = Color.Hex(0x006985FF), Name = "Palladium"}},
{047, new AtomInfo{Symbol ="Ag", Color = Color.Hex(0xC0C0C0FF), Name = "Silver"}},
{048, new AtomInfo{Symbol ="Cd", Color = Color.Hex(0xFFD98FFF), Name = "Cadmium"}},
{049, new AtomInfo{Symbol ="In", Color = Color.Hex(0xA67573FF), Name = "Indium"}},
{050, new AtomInfo{Symbol ="Sn", Color = Color.Hex(0x668080FF), Name = "Tin"}},
{051, new AtomInfo{Symbol ="Sb", Color = Color.Hex(0x9E63B5FF), Name = "Antimony"}},
{052, new AtomInfo{Symbol ="Te", Color = Color.Hex(0xD47A00FF), Name = "Tellurium"}},
{053, new AtomInfo{Symbol ="I", Color = Color.Hex(0x940094FF), Name = "Iodine"}},
{054, new AtomInfo{Symbol ="Xe", Color = Color.Hex(0x429EB0FF), Name = "Xenon"}},
{055, new AtomInfo{Symbol ="Cs", Color = Color.Hex(0x57178FFF), Name = "Caesium"}},
{056, new AtomInfo{Symbol ="Ba", Color = Color.Hex(0x00C900FF), Name = "Barium"}},
{057, new AtomInfo{Symbol ="La", Color = Color.Hex(0x70D4FFFF), Name = "Lanthanum"}},
{058, new AtomInfo{Symbol ="Ce", Color = Color.Hex(0xFFFFC7FF), Name = "Cerium"}},
{059, new AtomInfo{Symbol ="Pr", Color = Color.Hex(0xD9FFC7FF), Name = "Praseodymium"}},
{060, new AtomInfo{Symbol ="Nd", Color = Color.Hex(0xC7FFC7FF), Name = "Neodymium"}},
{061, new AtomInfo{Symbol ="Pm", Color = Color.Hex(0xA3FFC7FF), Name = "Promethium"}},
{062, new AtomInfo{Symbol ="Sm", Color = Color.Hex(0x8FFFC7FF), Name = "Samarium"}},
{063, new AtomInfo{Symbol ="Eu", Color = Color.Hex(0x61FFC7FF), Name = "Europium"}},
{064, new AtomInfo{Symbol ="Gd", Color = Color.Hex(0x45FFC7FF), Name = "Gadolinium"}},
{065, new AtomInfo{Symbol ="Tb", Color = Color.Hex(0x30FFC7FF), Name = "Terbium"}},
{066, new AtomInfo{Symbol ="Dy", Color = Color.Hex(0x1FFFC7FF), Name = "Dysprosium"}},
{067, new AtomInfo{Symbol ="Ho", Color = Color.Hex(0x00FF9CFF), Name = "Holmium"}},
{068, new AtomInfo{Symbol ="Er", Color = Color.Hex(0x00E675FF), Name = "Erbium"}},
{069, new AtomInfo{Symbol ="Tm", Color = Color.Hex(0x00D452FF), Name = "Thulium"}},
{070, new AtomInfo{Symbol ="Yb", Color = Color.Hex(0x00BF38FF), Name = "Ytterbium"}},
{071, new AtomInfo{Symbol ="Lu", Color = Color.Hex(0x00AB24FF), Name = "Lutetium"}},
{072, new AtomInfo{Symbol ="Hf", Color = Color.Hex(0x4DC2FFFF), Name = "Hafnium"}},
{073, new AtomInfo{Symbol ="Ta", Color = Color.Hex(0x4DA6FFFF), Name = "Tantalum"}},
{074, new AtomInfo{Symbol ="W", Color = Color.Hex(0x2194D6FF), Name = "Tungsten"}},
{075, new AtomInfo{Symbol ="Re", Color = Color.Hex(0x267DABFF), Name = "Rhenium"}},
{076, new AtomInfo{Symbol ="Os", Color = Color.Hex(0x266696FF), Name = "Osmium"}},
{077, new AtomInfo{Symbol ="Ir", Color = Color.Hex(0x175487FF), Name = "Iridium"}},
{078, new AtomInfo{Symbol ="Pt", Color = Color.Hex(0xD0D0E0FF), Name = "Platinum"}},
{079, new AtomInfo{Symbol ="Au", Color = Color.Hex(0xFFD123FF), Name = "Gold"}},
{080, new AtomInfo{Symbol ="Hg", Color = Color.Hex(0xB8B8D0FF), Name = "Mercury"}},
{081, new AtomInfo{Symbol ="Tl", Color = Color.Hex(0xA6544DFF), Name = "Thallium"}},
{082, new AtomInfo{Symbol ="Pb", Color = Color.Hex(0x575961FF), Name = "Lead"}},
{083, new AtomInfo{Symbol ="Bi", Color = Color.Hex(0x9E4FB5FF), Name = "Bismuth"}},
{084, new AtomInfo{Symbol ="Po", Color = Color.Hex(0xAB5C00FF), Name = "Polonium"}},
{085, new AtomInfo{Symbol ="At", Color = Color.Hex(0x754F45FF), Name = "Astatine"}},
{086, new AtomInfo{Symbol ="Rn", Color = Color.Hex(0x428296FF), Name = "Radon"}},
{087, new AtomInfo{Symbol ="Fr", Color = Color.Hex(0x420066FF), Name = "Francium"}},
{088, new AtomInfo{Symbol ="Ra", Color = Color.Hex(0x007D00FF), Name = "Radium"}},
{089, new AtomInfo{Symbol ="Ac", Color = Color.Hex(0x70ABFAFF), Name = "Actinium"}},
{090, new AtomInfo{Symbol ="Th", Color = Color.Hex(0x00BAFFFF), Name = "Thorium"}},
{091, new AtomInfo{Symbol ="Pa", Color = Color.Hex(0x00A1FFFF), Name = "Protactinium"}},
{092, new AtomInfo{Symbol ="U", Color = Color.Hex(0x008FFFFF), Name = "Uranium"}},
{093, new AtomInfo{Symbol ="Np", Color = Color.Hex(0x0080FFFF), Name = "Neptunium"}},
{094, new AtomInfo{Symbol ="Pu", Color = Color.Hex(0x006BFFFF), Name = "Plutonium"}},
{095, new AtomInfo{Symbol ="Am", Color = Color.Hex(0x545CF2FF), Name = "Americium"}},
{096, new AtomInfo{Symbol ="Cm", Color = Color.Hex(0x785CE3FF), Name = "Curium"}},
{097, new AtomInfo{Symbol ="Bk", Color = Color.Hex(0x8A4FE3FF), Name = "Berkelium"}},
{098, new AtomInfo{Symbol ="Cf", Color = Color.Hex(0xA136D4FF), Name = "Californium"}},
{099, new AtomInfo{Symbol ="Es", Color = Color.Hex(0xB31FD4FF), Name = "Einsteinium"}},
{100, new AtomInfo{Symbol ="Fm", Color = Color.Hex(0xB31FBAFF), Name = "Fermium"}},
{101, new AtomInfo{Symbol ="Md", Color = Color.Hex(0xB30DA6FF), Name = "Mendelevium"}},
{102, new AtomInfo{Symbol ="No", Color = Color.Hex(0xBD0D87FF), Name = "Nobelium"}},
{103, new AtomInfo{Symbol ="Lr", Color = Color.Hex(0xC70066FF), Name = "Lawrencium"}},
{104, new AtomInfo{Symbol ="Rf", Color = Color.Hex(0xCC0059FF), Name = "Rutherfordium"}},
{105, new AtomInfo{Symbol ="Db", Color = Color.Hex(0xD1004FFF), Name = "Dubnium"}},
{106, new AtomInfo{Symbol ="Sg", Color = Color.Hex(0xD90045FF), Name = "Seaborgium"}},
{107, new AtomInfo{Symbol ="Bh", Color = Color.Hex(0xE00038FF), Name = "Bohrium"}},
{108, new AtomInfo{Symbol ="Hs", Color = Color.Hex(0xE6002EFF), Name = "Hassium"}},
{109, new AtomInfo{Symbol ="Mt", Color = Color.Hex(0xEB0026FF), Name = "Meitnerium"}},
{110, new AtomInfo{Symbol ="Ds", Color = Color.Hex(0xAAAAAAFF), Name = "Darmstadtium"}},
{111, new AtomInfo{Symbol ="Rg", Color = Color.Hex(0xAAAAAAFF), Name = "Roentgenium"}},
{112, new AtomInfo{Symbol ="Cn", Color = Color.Hex(0xAAAAAAFF), Name = "Copernicium"}},
{113, new AtomInfo{Symbol ="Nh", Color = Color.Hex(0xAAAAAAFF), Name = "Nihonium"}},
{114, new AtomInfo{Symbol ="Fl", Color = Color.Hex(0xAAAAAAFF), Name = "Flerovium"}},
{115, new AtomInfo{Symbol ="Mc", Color = Color.Hex(0xAAAAAAFF), Name = "Moscovium"}},
{116, new AtomInfo{Symbol ="Lv", Color = Color.Hex(0xAAAAAAFF), Name = "Livermorium"}},
{117, new AtomInfo{Symbol ="Ts", Color = Color.Hex(0xAAAAAAFF), Name = "Tennessine"}},
{118, new AtomInfo{Symbol ="Og", Color = Color.Hex(0xAAAAAAFF), Name = "Oganesson"}},
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