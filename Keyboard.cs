using System;
using System.Collections.Generic;
using System.Text;
using StereoKit;

namespace Molecula
{
    public static class Keyboard
    {
        private static bool _isShown = false;
        private static Vec3 _keyboardPos = new Vec3(0f, -0.3f, -0.3f);
        private static TextStyle _textStyle = Text.MakeStyle(Font.Default, 2f * U.cm, new Color(1f, 1f, 1f));

        public static void ToogleKeyboard()
        {
            _isShown = !_isShown;
        }

        public static void HandleKeyboard(ref string inputTxt)
        {
            //if (!_isShown) return;
            var keyboardPose = new Pose(_keyboardPos, Quat.LookAt(_keyboardPos, Input.Head.position, Vec3.Up));
            Hierarchy.Push(Matrix.T(Input.Head.position));

            UI.WindowBegin("Keyboard", ref keyboardPose, new Vec2(32, 0) * U.cm, UIWin.Normal);
            CreateKeyBtnLine("QWERTZUIOP",ref inputTxt);
            CreateKeyBtnLine("ASDFGHJKL", ref inputTxt);
            CreateKeyBtnLine("YXC", ref inputTxt);
            UI.SameLine();
            if (UI.Button(" ", new Vec2(8f, 2f) * U.cm))
            {
                inputTxt += " ";
            }
            UI.SameLine();
            CreateKeyBtnLine("VBNM", ref inputTxt);
            UI.WindowEnd();
            Hierarchy.Pop();

            _keyboardPos = keyboardPose.position;
        }

        private static void CreateKeyBtnLine(string keyLine, ref string inputTxt)
        {
            var isFirst = true;
            foreach (var chr in keyLine.ToCharArray())
            {
                if (isFirst) { isFirst = false; } else { UI.SameLine(); }
                if (UI.Button(chr.ToString(), new Vec2(2f, 2f) * U.cm))
                {
                    inputTxt += chr;
                }
            }
        }
    }
}
