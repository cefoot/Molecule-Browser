using RestSharp;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace molecula_shared
{
    public class App
    {
        private Dictionary<int, Material> _atomMaterialMap = new Dictionary<int, Material>();

        private List<AtomData> _molecules = new List<AtomData>();

        private const int BndClr = -1;

        public float MoleculeScale = 0.1f;

        public float AtomDiameter = 0.05f;
        private StringBuilder _errorMsg = new StringBuilder();
        private DateTime _errorTime = DateTime.Now;
        private readonly TextStyle _errorTextStyl = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 0f, 0f));
        private readonly TextStyle _normalTextStyle = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 1f, 1f));

        public class AtomData
        {
            public Model Model;
            public Pose Pose;
            public string Name;
            public void Draw()
            {
                UI.Handle($"molecule_{Name}", ref Pose, Model.Bounds);
                //UI.Text(Model.Visuals[0].Name, TextAlign.Center);
                Model.Draw(Pose.ToMatrix());
                Text.Add(Name, Pose.ToMatrix(), offY: 3f * U.cm, offZ: -5f * U.cm);
            }
        }

        public void Loop()
        {
            Init();
            // Create assets used by the app
            Pose windowPose = new Pose(.4f, 0, -.5f, Quat.LookDir(Input.Head.Forward * -1));

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;
            var mat = Default.MaterialUI.Copy();
            mat.SetColor("color", Color.HSV(0f, 0f, 1f, 0.3f));
            mat.Transparency = Transparency.Blend;
            _atomMaterialMap[BndClr] = mat;

            LoadMolecule("Methanol");
            var moleculeSearchTxt = "";
            // Core application loop
            while (SK.Step(() =>
            {
                if (SK.System.displayType == Display.Opaque)
                    Default.MeshCube.Draw(floorMaterial, floorTransform);
                for (int i = 0; i < _molecules.Count; i++)
                {
                    _molecules[i].Draw();
                }
                MoleculeSearchWindow(ref windowPose, ref moleculeSearchTxt);
                //Hierarchy.Push(Matrix.T(0, 0, -0.3f));
            })) ;
        }

        private void Init()
        {
            //_errorTextStyl.Material.SetColor("color", new Color(1f, 0f, 0f));
            //_normalTextStyle.Material.SetColor("color", Color.White);
        }

        private void MoleculeSearchWindow(ref Pose windowPose, ref string moleculeSearchTxt)
        {
            UI.WindowBegin("Molecule search", ref windowPose, new Vec2(25, 0) * U.cm, UIWin.Normal);
            UI.PushTextStyle(_normalTextStyle);
            UI.Label("Name:");
            UI.SameLine();
            UI.Input("inpMolecule", ref moleculeSearchTxt);//, new Vec2(10, 2) * U.mm);
            if (UI.Button("Search") && !string.IsNullOrEmpty(moleculeSearchTxt))
            {
                LoadMolecule(moleculeSearchTxt);
                moleculeSearchTxt = "";
            }
            if ((DateTime.Now - _errorTime).TotalSeconds < 5d)
            {
                UI.PushTextStyle(_errorTextStyl);
                UI.Text(_errorMsg.ToString(), TextAlign.Center);
                UI.PopTextStyle();
            }
            else
            {
                _errorMsg.Clear();
            }
            UI.WindowEnd();
        }

        public async /*Task<Model>*/ void LoadMolecule(string moleculeName)
        {//https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/name/water/record/JSON/?record_type=3d
            try
            {

                var client = new RestClient("https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/name/");
                //client.Authenticator = new HttpBasicAuthenticator("username", "password");

                var request = new RestRequest($"{moleculeName}/record/JSON/?record_type=3d", DataFormat.Json);

                var response = await client.GetAsync<RecordData>(request);
                if (response == null || response.PC_Compounds == null)
                {
                    AddErrorMessage($"no molecule found with name '{moleculeName}'");
                    return;
                }
                System.Diagnostics.Debug.WriteLine(string.Join(";", response.PC_Compounds[0].AtomList));
                var mdl = new Model();
                foreach (var atom in response.PC_Compounds[0].AtomList)
                {
                    if (!_atomMaterialMap.ContainsKey(atom.ElementOrderNumber))
                    {
                        var mat = Default.MaterialUI.Copy();
                        mat.SetColor("color", atom.GetColor());
                        _atomMaterialMap[atom.ElementOrderNumber] = mat;
                    }
                    var atmPos = atom.Position * MoleculeScale;
                    var atmMdl = mdl.AddNode(atom.GetName(), new Pose(atmPos, Quat.Identity).ToMatrix(), Mesh.GenerateSphere(AtomDiameter), _atomMaterialMap[atom.ElementOrderNumber]);
                    foreach (var bndData in atom.Bonds)
                    {
                        var bndPos = bndData.Key.Position * MoleculeScale;
                        var bndDir = bndPos - atmPos;
                        var mddlPnt = atmPos + bndDir / 2f;
                        var bndDirNorm = Vec3.Cross(bndDir, new Vec3(bndDir.z, bndDir.x, bndDir.y)).Normalized * AtomDiameter * .5f;
                        for (int i = 0; i < bndData.Value; i++)
                        {
                            var tmpPnt = mddlPnt;
                            if (bndData.Value > 1)
                            {
                                tmpPnt += bndDirNorm * i;
                            }
                            //Quat.FromAngles(0f, 0f, 360f / bndData.Value).
                            mdl.AddNode(
                                $"{atom.GetSymbol()}_{bndData.Key.GetSymbol()}",
                                Matrix.TRS(
                                    tmpPnt,
                                    Quat.LookDir(bndDir),
                                    new Vec3(.1f, .1f, 1f)
                                    ),
                                Mesh.GenerateSphere(Vec3.Distance(atom.Position * MoleculeScale, bndData.Key.Position * MoleculeScale)),
                                _atomMaterialMap[BndClr]);
                        }
                    }
                }
                var pos = Input.Head.position + Input.Head.Forward;
                _molecules.Add(new AtomData { Model = mdl, Pose = new Pose(pos, Quat.LookAt(pos, Input.Head.position, Vec3.Up)), Name = moleculeName });
                System.Diagnostics.Debug.WriteLine($"Molecule '{moleculeName}' loaded");
            }
            catch (Exception e)
            {
                AddErrorMessage(e.Message);
            }
        }

        private void AddErrorMessage(string msg)
        {
            _errorTime = DateTime.Now;
            _errorMsg.AppendLine(msg);
        }
    }
}
