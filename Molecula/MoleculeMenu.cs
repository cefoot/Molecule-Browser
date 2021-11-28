using System;
using System.Collections.Generic;
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

        public static void DrawMenu(this MoleculeData molecule)
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
            _curMenu.Draw();
        }


    }

    public class MoleculeMenu
    {
        public MoleculeData MoleculeInfo;

        public Pose _menuPose;

        public MoleculeMenu(MoleculeData moleculeInfo)
        {
            _menuPose = new Pose(new Vec3(0f, -0.5f, -0.1f), Quat.Identity);
            MoleculeInfo = moleculeInfo;
            MoleculeInfo.MenuInfo = this;
        }

        public void Draw()
        {
            Hierarchy.Push(MoleculeInfo.Pose.ToMatrix());
            UI.WindowBegin(MoleculeInfo.Name, ref _menuPose, new Vec2(25, 0) * U.cm, UIWin.Normal);
            UI.Label("Formula:");
            UI.Text(MoleculeInfo.Props?.MolecularFormula);
            UI.Label("Description:");
            UI.Text(MoleculeInfo.Description);
            //UI.PushTextStyle(_normalUiTextStyle);
            UI.WindowEnd();
            Hierarchy.Pop();
        }
    }
}
