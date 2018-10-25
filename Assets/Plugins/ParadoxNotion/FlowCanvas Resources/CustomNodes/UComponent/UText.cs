using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.UI;

namespace FlowCanvas.Nodes
{

    #region Text

    [Name("setText")]
    [Category("UnityEngine/Text")]
    [Description("设置UI Text 字符显示")]
    public class S_Text : CallableFunctionNode<Text,Text, string>
    {
        public override Text Invoke(Text text, string value)
        {
            text.text = value;
            return text;
        }
    }

    [Name("setTextColor")]
    [Category("UnityEngine/Text")]
    [Description("设置UI Text 字符颜色")]
    public class S_TextColor : CallableFunctionNode<Text,Text, Color>
    {
        public override Text Invoke(Text text, Color color)
        {
            text.color = color;
            return text;
        }
    }

    [Name("setFontSize")]
    [Category("UnityEngine/Text")]
    [Description("设置UI Text 字符大小")]
    public class S_TextSize : CallableFunctionNode<Text,Text, int>
    {
        public override Text Invoke(Text text, int size)
        {
            text.fontSize = size;
            return text;
        }
    }

    [Name("getText")]
    [Category("UnityEngine/Text")]
    [Description("获取UI Text 字符显示")]
    public class G_Text : PureFunctionNode<string, Text>
    {
        public override string Invoke(Text text)
        {
            return text.text;
        }
    }

    [Name("getFontSize")]
    [Category("UnityEngine/Text")]
    [Description("获取UI Text 字符显示")]
    public class G_TextSize : PureFunctionNode<int, Text>
    {
        public override int Invoke(Text text)
        {
            return text.fontSize;
        }
    }

    [Name("getTextColor")]
    [Category("UnityEngine/Text")]
    [Description("获取UI Text 字符颜色")]
    public class G_TextColor : PureFunctionNode<Color, Text>
    {
        public override Color Invoke(Text text)
        {
            return text.color;
        }
    }

    #endregion
}