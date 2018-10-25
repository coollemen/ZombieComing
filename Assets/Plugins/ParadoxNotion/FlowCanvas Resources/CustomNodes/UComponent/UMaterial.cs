using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region Material

    [Name("getColor(Material)")]
    [Category("UnityEngine/Material")]
    [Description("获取材质颜色")]
    public class G_MaterialColor : PureFunctionNode<Color, Material>
    {
        public override Color Invoke(Material material)
        {
            return material.color;
        }
    }

    [Name("setColor(Material)")]
    [Category("UnityEngine/Material")]
    [Description("设置材质颜色")]
    public class S_MaterialColor : CallableFunctionNode<Material,Material, Color>
    {
        public override Material Invoke(Material material, Color color)
        {
            material.color = color;
            return material;
        }
    }

    [Name("setMainTexture")]
    [Category("UnityEngine/Material")]
    [Description("设置材质主贴图")]
    public class S_MaterialTexture : CallableFunctionNode<Material,Material, Texture>
    {
        public override Material Invoke(Material material, Texture texture)
        {
            material.mainTexture = texture;
            return material;
        }
    }

    #endregion
}