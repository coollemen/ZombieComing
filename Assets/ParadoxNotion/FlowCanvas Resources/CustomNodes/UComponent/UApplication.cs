using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region Application

    [Name("quit")]
    [Category("UnityEngine/Application")]
    [Description("退出程序")]
    public class S_Quit : CallableActionNode
    {
        public override void Invoke()
        {
            Application.Quit();
        }
    }

    [Name("openURL")]
    [Category("UnityEngine/Application")]
    [Description("打开网页网址")]
    public class S_OpenURL : CallableActionNode<string>
    {
        public override void Invoke(string url)
        {
            Application.OpenURL(url);
        }
    }

    [Name("setTargetFrameRate")]
    [Category("UnityEngine/Application")]
    [Description("设置程序运行目标帧数")]
    public class G_targetFrameRate : CallableActionNode<int>
    {
        public override void Invoke(int targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }

    [Name("getIsFocused")]
    [Category("UnityEngine/Application")]
    [Description("程序是否被选中")]
    public class G_IsFocused : PureFunctionNode<bool>
    {
        public override bool Invoke()
        {
            return Application.isFocused;
        }
    }

    [Name("getStreamingAssetsPath")]
    [Category("UnityEngine/Application")]
    [Description("获得streamingAssetsPath")]
    public class G_StreamingAssetsPath : PureFunctionNode<string>
    {
        public override string Invoke()
        {
            return Application.streamingAssetsPath;
        }
    }

    #endregion
}