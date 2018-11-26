using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class MyGUITools
    {
        public static Color oldColor;
        public static Color bgOldColor;
        public static Color contentOldColor;
        public static void  SetColor(Color c)
        {
            oldColor = GUI.color;
            GUI.color = c;
        }
        public static void SetContentColor(Color c)
        {
            contentOldColor = GUI.contentColor;
            GUI.contentColor = c;
        }
        public static void SetBackgroundColor(Color c)
        {
            bgOldColor = GUI.backgroundColor;
            GUI.backgroundColor = c;
        }

        public static void RestoreColor()
        {
            GUI.color = oldColor;
        }
        public static void RestoreBackgroundColor()
        {
            GUI.backgroundColor = bgOldColor;
        }
        public static void RestoreContentColor()
        {
            GUI.contentColor = contentOldColor;
        }
    }
}