using molecula_shared;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Molecula
{
    public class App
    {
        public SKSettings Settings => new SKSettings
        {
            appName = "Molecula",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

        Pose cubePose = new Pose(0, 0, -0.5f, Quat.Identity);
        Model cube;
        Matrix4x4 floorTransform = Matrix.TS(new Vector3(0, -1.5f, 0), new Vector3(30, 0.1f, 30));
        private List<MoleculeData> _molecules = new List<MoleculeData>();
        Material floorMaterial;
        private Pose _windowPose;
        private string _moleculeSearchTxt;
        private StringBuilder _errorMsg = new StringBuilder();
        private DateTime _errorTime = DateTime.Now;
        private TextStyle _errorUiTextStyl;
        private TextStyle _normalUiTextStyle;

        public void Init()
        {
            // Create assets used by the app
            cube = Model.FromMesh(
                Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
                Default.MaterialUI);

            floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            _windowPose = new Pose(.4f, 0, -.5f, Quat.LookDir(Input.Head.Forward * -1));
            _moleculeSearchTxt = "";

            _errorUiTextStyl = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 0f, 0f));
            _normalUiTextStyle = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 1f, 1f));

            //LoadMolecule("butin");
        }

        public void Step()
        {
            if (SK.System.displayType == Display.Opaque)
                Default.MeshCube.Draw(floorMaterial, floorTransform);
            for (int i = 0; i < _molecules.Count; i++)
            {
                _molecules[i].Draw();
            }
            MoleculeSearchWindow(ref _windowPose, ref _moleculeSearchTxt);

            UI.Handle("Cube", ref cubePose, cube.Bounds);
            cube.Draw(cubePose.ToMatrix());
        }

        private void MoleculeSearchWindow(ref Pose windowPose, ref string moleculeSearchTxt)
        {
            UI.WindowBegin("Molecule search", ref windowPose, new Vec2(25, 0) * U.cm, UIWin.Normal);
            //UI.PushTextStyle(_normalUiTextStyle);
            UI.Label("Name:");
            UI.SameLine();
            UI.Input("inpMolecule", ref moleculeSearchTxt);//, new Vec2(10, 2) * U.mm);
            if (UI.Button("Search") && !string.IsNullOrEmpty(moleculeSearchTxt))
            {
                LoadMolecule(moleculeSearchTxt);
                moleculeSearchTxt = "";
            }
            UI.SameLine();
            if (UI.Button("Fructose"))
            {
                LoadMolecule("Fructose");
            }
            UI.SameLine();
            if (UI.Button("butin"))
            {
                LoadMolecule("butin");
            }
            if ((DateTime.Now - _errorTime).TotalSeconds < 5d)
            {
                UI.PushTextStyle(_errorUiTextStyl);
                UI.Text(_errorMsg.ToString());
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