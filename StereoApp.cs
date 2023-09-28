using molecula_shared;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Molecula
{
    public class StereoKitApp
    {
        public SKSettings Settings => new SKSettings
        {
            appName = "Molecula",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

        Pose cubePose = new Pose(0, 0, -0.5f, Quat.Identity);
        Pose cubePose1 = new Pose(0.5f, 0, -0.5f, Quat.Identity);
        Model cube;
        Matrix4x4 floorTransform = Matrix.TS(new Vector3(0, -1.5f, 0), new Vector3(30, 0.1f, 30));
        private List<MoleculeData> _molecules = new List<MoleculeData>();
        Material floorMaterial;
        private Pose _windowPose;
        private string _moleculeSearchTxt;
        private Sprite _keyboardSprite;
        private Sprite _pubchemLogo;
        private StringBuilder _errorMsg = new StringBuilder();
        private DateTime _errorTime = DateTime.Now;
        private TextStyle _errorUiTextStyle;
        private TextStyle _normalUiTextStyle;
        private bool _playErrorSound = false;

        public static System.Threading.SynchronizationContext MainThreadCtxt { get; private set; }

        public void Init()
        {

            _keyboardSprite = Sprite.FromFile("keyboard-outline.png", SpriteType.Single);
            _pubchemLogo = Sprite.FromFile("pubchem_logo.png", SpriteType.Single);
            // Create assets used by the app
            cube = Model.FromMesh(
                Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
                Default.MaterialUI);

            //keyboardSprite =

            floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            _windowPose = new Pose(.4f, 0, -.5f, Quat.LookDir(Input.Head.Forward * -1));
            _moleculeSearchTxt = "";

            _errorUiTextStyle = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 0f, 0f));
            _normalUiTextStyle = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 1f, 1f));

            MainThreadCtxt = new System.Threading.SynchronizationContext();
            //LoadMolecule("butin");
        }

        private void JustActiveHandler(InputSource arg1, BtnState arg2, Pointer arg3)
        {
            System.Diagnostics.Debug.WriteLine(arg1 + ":" + arg2);
        }

        public void Step()
        {
            if (SK.System.displayType == Display.Opaque)
                Default.MeshCube.Draw(floorMaterial, floorTransform);
            try
            {

                for (int i = 0; i < _molecules.Count; i++)
                {
                    _molecules[i].Draw(this);
                }
                MoleculeSearchWindow(ref _windowPose, ref _moleculeSearchTxt);
            }
            catch (Exception e)
            {
                AddErrorMessage(e.Message);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e);
            }

            //UI.Handle("Cube", ref cubePose, cube.Bounds);
            //UI.Handle("Cube1", ref cubePose1, cube.Bounds);
            //cube.Draw(cubePose.ToMatrix());
            //cube.Draw(cubePose1.ToMatrix());
        }

        private void MoleculeSearchWindow(ref Pose windowPose, ref string moleculeSearchTxt)
        {
            Keyboard.HandleKeyboard(ref moleculeSearchTxt);
            UI.WindowBegin("Molecule search", ref windowPose, new Vec2(40, 0) * U.cm, UIWin.Normal);
            //UI.PushTextStyle(_normalUiTextStyle);
            UI.Label("Name:");
            UI.Input("inpMolecule", ref moleculeSearchTxt, new Vec2(20, 0) * U.cm);
            UI.SameLine();
            //UI.ButtonRound("btnKey", keyboardSprite);
            if (UI.ButtonRound("keyboard", _keyboardSprite)/*UI.Button("⌨️", new Vec2(5, 0) * U.cm)*/)
            {
                Keyboard.ToogleKeyboard();
            }
            var inputBounds = UI.LayoutLast;
            var btnPressed = false;
            UI.SameLine();
            if (UI.Button("Search") && !string.IsNullOrEmpty(moleculeSearchTxt))
            {
                LoadMolecule(moleculeSearchTxt);
                moleculeSearchTxt = "";
                btnPressed = true;
            }
            UI.Label("Samples:");
            if (UI.Button("Fructose"))
            {
                LoadMolecule("Fructose");
                btnPressed = true;
            }
            UI.SameLine();
            if (UI.Button("Butin"))
            {
                LoadMolecule("butin");
                btnPressed = true;
            }
            if (!btnPressed)
            {
                if (UI.IsInteracting(Handed.Left) && inputBounds.Contains(Input.Hand(Handed.Left)[FingerId.Index, JointId.Tip].position))
                {
                    moleculeSearchTxt = "left" + Guid.NewGuid().ToString();
                }
                if (UI.IsInteracting(Handed.Right) && inputBounds.Contains(Input.Hand(Handed.Right)[FingerId.Index, JointId.Tip].position))
                {
                    moleculeSearchTxt = "right" + Guid.NewGuid().ToString();
                }
            }
            if ((DateTime.Now - _errorTime).TotalSeconds < 5d)
            {
                if (_playErrorSound)
                {
                    _playErrorSound = false;
                    Sound.Click.Play(windowPose.position);
                }
                UI.PushTextStyle(_errorUiTextStyle);
                UI.Text(_errorMsg.ToString());
                UI.PopTextStyle();
            }
            else
            {
                _errorMsg.Clear();
            }
            UI.Label("powered by:");
            UI.SameLine();
            var imgWidth = 20F;
            var imgAspect = 3.214765100671141F;
            UI.Image(_pubchemLogo, new Vec2(imgWidth, imgWidth / imgAspect) * U.cm);
            UI.WindowEnd();
        }

        public async /*Task<Model>*/ void LoadMolecule(string moleculeName, bool isCid = false)
        {//https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/name/water/record/JSON/?record_type=3d
            try
            {
                System.Diagnostics.Debug.WriteLine($"trying to load '{moleculeName}'[{System.Threading.Thread.CurrentThread.ManagedThreadId}]");
                var molecule = await MoleculeData.CreateMolecule(moleculeName, this, isCid);
                _molecules.Add(molecule);
                System.Diagnostics.Debug.WriteLine($"Molecule '{moleculeName}' loaded[{System.Threading.Thread.CurrentThread.ManagedThreadId}]");
            }
            catch (Exception e)
            {
                AddErrorMessage(e.Message);
            }
        }

        public void AddErrorMessage(string msg)
        {
            _playErrorSound = true;
            _errorTime = DateTime.Now;
            _errorMsg.AppendLine(msg);
        }
    }
}
