using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using molecula_shared;
using StereoKit;

namespace Molecula.Molecula
{
    public static class MoleculeMenuExtensions
    {
        private static MoleculeData _curMoleculeData;
        public static MoleculeMenu _curMenu;

        public static void SelectMolecule(this MoleculeData molecule)
        {
            _curMoleculeData = molecule;
        }

        public static void DrawMenu(this MoleculeData molecule, App caller)
        {
            if (molecule != _curMoleculeData)
            {//only draw for selected menu
                return;
            }
            if (_curMenu == null || _curMenu.MoleculeInfo != _curMoleculeData)
            {
                if (_curMoleculeData.MenuInfo != null)
                {
                    _curMenu = _curMoleculeData.MenuInfo;
                }
                else
                {
                    _curMenu = new MoleculeMenu(_curMoleculeData);
                }
            }
            _curMenu.Draw(caller);
        }


    }

    public class MoleculeMenu
    {
        public MoleculeData MoleculeInfo;

        public Vec3 _menuLoc;
        private List<KeyValuePair<int, string>> _similarMolecules;

        public MoleculeMenu(MoleculeData moleculeInfo)
        {
            _menuLoc = new Vec3(0f, -0.5f, -0.1f);
            MoleculeInfo = moleculeInfo;
            MoleculeInfo.MenuInfo = this;
        }

        public void Draw(App caller)
        {
            Hierarchy.Push(Matrix.T(MoleculeInfo.Pose.position));
            var menuPose = new Pose(_menuLoc, Quat.LookAt(MoleculeInfo.Pose.position + _menuLoc, Input.Head.position));
            UI.WindowBegin(MoleculeInfo.Name, ref menuPose, new Vec2(25, 0) * U.cm, UIWin.Normal, UIMove.PosOnly);
            UI.Label("Formula:");
            UI.Text(MoleculeInfo.Props?.MolecularFormula);
            UI.Label("Description:");
            UI.Text(MoleculeInfo.Description);
            UI.Label("Options");
            if (_similarMolecules == null)
            {
                if (UI.Button("Find similar"))
                {
                    FindSimilar(caller);
                }
            }
            else
            {
                if (_similarMolecules.Count < 1)
                {
                    UI.Label("..Loading similar..");
                }
                foreach (var similarMolecule in _similarMolecules)
                {
                    if (UI.Button(similarMolecule.Value))
                    {
                        caller.LoadMolecule(similarMolecule.Key.ToString(), true);
                    }
                }
            }
            UI.WindowEnd();
            _menuLoc = menuPose.position;
            Hierarchy.Pop();
        }

        private async void FindSimilar(App caller)
        {
            _similarMolecules = new List<KeyValuePair<int, string>>();
            try
            {

                var res = await PubChemUtils.GetData<CIDIdentifierList>($"fastsimilarity_3d/cid/{MoleculeInfo.CID}/cids/JSON");
                var cids = res.IdentifierList.CID.Take(3).AsParallel();
                foreach (var cid in cids)
                {
                    var info = await PubChemUtils.GetData<Root_InformationData>($"cid/{cid}/description/JSON");
                    _similarMolecules.Add(
                        new KeyValuePair<int, string>(cid,
                        info.InformationList.Information.Where(i => !string.IsNullOrEmpty(i.Title)).First().Title)
                        );
                }
            }
            catch (Exception e)
            {
                _similarMolecules = null;
                caller.AddErrorMessage(e.Message);
            }
        }
    }
}
