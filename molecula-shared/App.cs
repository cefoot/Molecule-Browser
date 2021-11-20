using RestSharp;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace molecula_shared
{
    public partial class App
    {

        private List<MoleculeData> _molecules = new List<MoleculeData>();

        private const int BndClr = -1;
        private StringBuilder _errorMsg = new StringBuilder();
        private DateTime _errorTime = DateTime.Now;
        private static readonly TextStyle _errorTextStyl = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 0f, 0f));
        private static readonly TextStyle _normalTextStyle = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 1f, 1f));

        public void Loop()
        {
            Init();
            // Create assets used by the app
            Pose windowPose = new Pose(.4f, 0, -.5f, Quat.LookDir(Input.Head.Forward * -1));

            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

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
            MoleculeData.MoleculeScale = 0.1f;
            MoleculeData.AtomDiameter = 0.05f;
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
                var molecule = await MoleculeData.CreateMolecule(moleculeName);
                _molecules.Add(molecule);
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
