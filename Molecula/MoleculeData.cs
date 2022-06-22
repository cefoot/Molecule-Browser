using Molecula;
using Molecula.Molecula;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace molecula_shared
{
    public class MoleculeData
    {
        public Model Model;
        public Pose Pose;
        public string Name;
        public PCCompound RawData;
        public int CID;

        public string Description { get; private set; } = "...Loading...";

        public List<AtomData> SingleElements;

        public Property Props { get; private set; }

        public static float MoleculeScale = 0.05f;

        public static float AtomDiameter = 0.03f;

        private static Dictionary<int, Material> _atomMaterialMap = new Dictionary<int, Material>();
        private static Mesh _atomMesh;
        private const int BndClr = -1;
        private static TextStyle _errorTextStyl;
        private static TextStyle _activeTextStyl;
        private static TextStyle _normalTextStyle;
        private const string PRE_STRING_CID = "cid";
        private const string PRE_STRING_NAME = "name";

        public string ID { get; private set; }
        public MoleculeMenu MenuInfo { get; internal set; }

        static MoleculeData()
        {
            var mat = new Material(Shader.Default);
            mat.SetColor("color", Color.HSV(0f, 0f, 1f, 0.3f));
            mat.Transparency = Transparency.Blend;
            _atomMaterialMap[BndClr] = mat;
            var fnt = Font.FromFile(@"c:\work\Roboto\Roboto-Regular.ttf") ?? Font.Default;
            _errorTextStyl = Text.MakeStyle(fnt, 2f * U.cm, new Color(1f, 0f, 0f));
            _activeTextStyl = Text.MakeStyle(fnt, 2f * U.cm, new Color(0f, 1f, 0f, 0.7f));
            _activeTextStyl.Material.Transparency = Transparency.Blend;
            _normalTextStyle = Text.MakeStyle(fnt, 2f * U.cm, new Color(1f, 1f, 1f, 0.5f));
            _normalTextStyle.Material.Transparency = Transparency.Blend;
        }

        public async void LoadInfo_Async(App caller)
        {
            try
            {
                var moleculeProps = await PubChemUtils.GetData<MoleculeProps>($"cid/{CID}/property/MolecularWeight,MolecularFormula/JSON");
                var moleculeInfoRequest = await PubChemUtils.GetData<Root_InformationData>($"cid/{CID}/description/JSON");

                var info = moleculeInfoRequest;
                Name = info.InformationList.Information.Where(i => !string.IsNullOrEmpty(i.Title)).FirstOrDefault()?.Title;
                Description = info.InformationList.Information.Where(i => !string.IsNullOrEmpty(i.Description)).FirstOrDefault()?.Description;
                Props = (moleculeProps).PropertyTable?.Properties.Where(p => !string.IsNullOrEmpty(p.MolecularFormula)).FirstOrDefault();
            }
            catch (Exception e)
            {
                caller.AddErrorMessage(e.Message);
            }
        }


        public static async Task<MoleculeData> CreateMolecule(string moleculeName, App caller, bool isCid = false)
        {
            if (_atomMesh == null)
            {
                _atomMesh = Mesh.GenerateSphere(AtomDiameter);
            }
            var cid = moleculeName;
            System.Diagnostics.Debug.WriteLine($"loading molecule data from pubchem [{System.Threading.Thread.CurrentThread.ManagedThreadId}]");
            if (!isCid)
            {
                var cids = await PubChemUtils.GetData<CIDIdentifierList>($"name/{moleculeName}/cids/JSON");
                cid = cids.IdentifierList.CID[0].ToString();
            }
            var recData = await PubChemUtils.GetData<RecordData>($"cid/{cid}/record/JSON/?record_type=3d");
            if (recData == null || recData.PC_Compounds == null)
            {
                //try 2d instead (will just be flat in 3d should be fine)
                recData = await PubChemUtils.GetData<RecordData>($"cid/{cid}/record/JSON/?record_type=2d");

                if (recData == null || recData.PC_Compounds == null)
                {//well.. that did not work
                    throw new ArgumentException($"no molecule found with name '{moleculeName}'");
                }
            }
            System.Diagnostics.Debug.WriteLine($"loading info from pubchem [{System.Threading.Thread.CurrentThread.ManagedThreadId}]");
            var molecule = await BuildMoleculeModel(recData);
            molecule.Name = moleculeName;
            molecule.LoadInfo_Async(caller);
            return molecule;
        }

        private static async Task<MoleculeData> BuildMoleculeModel(RecordData recData)
        {
            var molecule = new MoleculeData();
            Exception hadException = null;
            App.MainThreadCtxt.Send(m =>
            {
                try
                {

                    var mdl = new Model();
                    var atoms = new List<AtomData>();
                    var idx = 1;
                    System.Diagnostics.Debug.WriteLine($"building model [{System.Threading.Thread.CurrentThread.ManagedThreadId}]");
                    foreach (var atom in recData.PC_Compounds[0].AtomList)
                    {
                        if (!_atomMaterialMap.ContainsKey(atom.ElementOrderNumber))
                        {
                            System.Diagnostics.Debug.WriteLine($"creating material for '{atom.ElementOrderNumber}' [{System.Threading.Thread.CurrentThread.ManagedThreadId}]");

                            var mat = new Material(Shader.Default);
                            mat.SetColor("color", atom.GetColor());
                            _atomMaterialMap[atom.ElementOrderNumber] = mat;
                        }
                        var atmPos = atom.Position * MoleculeScale;
                        atoms.Add(new AtomData
                        {
                            Atom = atom.GetAtomInfo(),
                            RelativePosition = atmPos
                        });
                        var atmMdl = mdl.AddNode($"{idx++}_{atom.GetName()}", new Pose(atmPos, Quat.Identity).ToMatrix(), _atomMesh, _atomMaterialMap[atom.ElementOrderNumber]);
                        CreateBonds(mdl, atom, atmPos);
                    }
                    System.Diagnostics.Debug.WriteLine($"assembling modelinfo [{System.Threading.Thread.CurrentThread.ManagedThreadId}]");

                    var pos = Input.Head.position + Input.Head.Forward;
                    ((MoleculeData)m).Model = mdl;
                    ((MoleculeData)m).Pose = new Pose(pos, Quat.LookAt(pos, Input.Head.position, Vec3.Up));
                    ((MoleculeData)m).CID = recData.PC_Compounds[0].id.id.cid;
                    ((MoleculeData)m).RawData = recData.PC_Compounds[0];
                    ((MoleculeData)m).SingleElements = atoms;
                    ((MoleculeData)m).ID = Guid.NewGuid().ToString();
                }
                catch (Exception e)
                {
                    hadException = e;
                }
            }, molecule);
            if (hadException != null) throw hadException;
            return molecule;
        }

        private static void CreateBonds(Model mdl, SingleAtom atom, Vec3 atmPos)
        {
            foreach (var bndData in atom.Bonds)
            {
                var bndPos = bndData.Key.Position * MoleculeScale;
                var bndDir = bndPos - atmPos;
                var mddlPnt = atmPos + bndDir / 2f;
                var bndDirNorm = Vec3.Cross(bndDir, Input.Head.Forward).Normalized * AtomDiameter * .5f;
                for (var i = 0; i < bndData.Value; i++)
                {
                    var tmpPnt = mddlPnt;
                    if (bndData.Value > 1)
                    {
                        tmpPnt += bndDirNorm * (-0.5f + i);
                    }
                    //Quat.FromAngles(0f, 0f, 360f / bndData.Value).
                    System.Diagnostics.Debug.WriteLine($"creating model node for bonding ({atom.GetSymbol()}-{bndData.Key.GetSymbol()})'{atom.ElementOrderNumber}' [{System.Threading.Thread.CurrentThread.ManagedThreadId}]");

                    mdl.AddNode(
                        $"{atom.GetSymbol()}_{bndData.Key.GetSymbol()}_{Guid.NewGuid()}",
                        Matrix.TRS(
                            tmpPnt,
                            Quat.LookDir(bndDir),
                            new Vec3(.1f, .1f, 1f)
                            ),
                        _atomMesh/*Mesh.GenerateSphere(Vec3.Distance(atom.Position * MoleculeScale, bndData.Key.Position * MoleculeScale))*/,
                        _atomMaterialMap[BndClr]);
                }
            }
        }

        public void Draw(App caller)
        {
            var isMoving = UI.Handle(ID, ref Pose, Model.Bounds);
            Model.Draw(Pose.ToMatrix());
            //var isMoving = UI.Handle($"molecule_{Name}", ref Pose, Model.Bounds);
            //UI.Text(Model.Visuals[0].Name, TextAlign.Center);
            var txtMatrix = new Pose(Pose.position, Quat.LookAt(Pose.position, Input.Head.position));
            if (!isMoving)
            {
                //Text.Add(Name, txtMatrix.ToMatrix(), _normalTextStyle, offY: 3f * U.cm, offZ: -5f * U.cm);
                Text.Add(Name, txtMatrix.ToMatrix(), offY: 3f * U.cm, offZ: -5f * U.cm);
            }
            else
            {
                this.SelectMolecule();
                //Text.Add(Name, txtMatrix.ToMatrix(), _activeTextStyl, offY: 3f * U.cm, offZ: -5f * U.cm);
                Text.Add(Name, txtMatrix.ToMatrix(), offY: 3f * U.cm, offZ: -5f * U.cm);
                foreach (var elem in SingleElements)
                {
                    var atomPos = Pose.orientation * elem.RelativePosition;
                    //Text.Add(elem.Atom.Symbol, new Pose(Pose.position + atomPos, Quat.LookAt(Pose.position, Input.Head.position)).ToMatrix(), _activeTextStyl, offY: 0f * U.cm, offZ: -5f * U.cm);
                    Text.Add(elem.Atom.Symbol, new Pose(Pose.position + atomPos, Quat.LookAt(Pose.position, Input.Head.position)).ToMatrix(), offY: 0f * U.cm, offZ: -5f * U.cm);
                }

            }
            this.DrawMenu(caller);
            //UI.HandleEnd();
        }
    }
}
