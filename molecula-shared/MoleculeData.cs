using RestSharp;
using StereoKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace molecula_shared
{
    public partial class App
    {
        public class MoleculeData
        {
            public Model Model;
            public Pose Pose;
            public string Name;
            public PCCompound RawData;
            public int CID;

            public static float MoleculeScale = 0.1f;

            public static float AtomDiameter = 0.05f;

            private static Dictionary<int, Material> _atomMaterialMap = new Dictionary<int, Material>();


            static MoleculeData()
            {
                var mat = Default.MaterialUI.Copy();
                mat.SetColor("color", Color.HSV(0f, 0f, 1f, 0.3f));
                mat.Transparency = Transparency.Blend;
                _atomMaterialMap[BndClr] = mat;
            }

            public static async Task<MoleculeData> CreateMolecule(string moleculeName)
            {
                //client.Authenticator = new HttpBasicAuthenticator("username", "password");

                var recData = await PubChemUtils.GetData<RecordData>($"name/{moleculeName}/record/JSON/?record_type=3d");
                if (recData == null || recData.PC_Compounds == null)
                {
                    throw new System.ArgumentException($"no molecule found with name '{moleculeName}'");
                }
                var moleculeInfoRequest = PubChemUtils.GetData<Root_InformationData>($"cid/{recData.PC_Compounds[0].id.id.cid}/description/JSON");

                var mdl = new Model();
                foreach (var atom in recData.PC_Compounds[0].AtomList)
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
                var res = await moleculeInfoRequest;
                var pos = Input.Head.position + Input.Head.Forward;
                var molecule = new MoleculeData
                {
                    Model = mdl,
                    Pose = new Pose(pos, Quat.LookAt(pos, Input.Head.position, Vec3.Up)),
                    Name = res.InformationList.Information[0].Title,
                    CID = res.InformationList.Information[0].CID,
                    RawData = recData.PC_Compounds[0]
                };
                return molecule;
            }

            public void Draw()
            {
                var isMoving = UI.Handle($"molecule_{Name}", ref Pose, Model.Bounds);
                //UI.Text(Model.Visuals[0].Name, TextAlign.Center);
                Model.Draw(Pose.ToMatrix());
                var txtMatrix = new Pose(Pose.position, Quat.LookAt(Pose.position, Input.Head.position));
                if (!isMoving)
                {
                    Text.Add(Name, txtMatrix.ToMatrix(), _normalTextStyle, offY: 3f * U.cm, offZ: -5f * U.cm);
                }
                else
                {
                    Text.Add(Name, txtMatrix.ToMatrix(), _errorTextStyl, offY: 3f * U.cm, offZ: -5f * U.cm);
                }
            }
        }
    }
}
