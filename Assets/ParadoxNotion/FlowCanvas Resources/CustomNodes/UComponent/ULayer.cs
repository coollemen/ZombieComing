using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region LayerMask

    [Name("nameToLayer")]
    [Category("UnityEngine/LayerMask")]
    [Description("将层的名称转换成int数值")]
    public class G_NameToLayer : PureFunctionNode<int, string>
    {
        public override int Invoke(string layerName)
        {
            return LayerMask.NameToLayer(layerName);
        }
    }

    [Name("layerToName")]
    [Category("UnityEngine/LayerMask")]
    [Description("将层的int数值转换成层的名称")]
    public class G_LayerToName : PureFunctionNode<string, int>
    {
        public override string Invoke(int layerIndex)
        {
            return LayerMask.LayerToName(layerIndex);
        }
    }


    #endregion
}