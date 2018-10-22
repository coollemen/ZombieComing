namespace Slash.Unity.DataBind.Editor.Editors
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEditor;

    [CustomEditor(typeof(DataProvider), true)]
    public class DataProviderEditor : Editor
    {
        #region Public Methods and Operators

        public override void OnInspectorGUI()
        {
            var dataProvider = (DataProvider)this.target;

            if (dataProvider.isActiveAndEnabled)
            {
                EditorGUILayout.LabelField("Current Value: " + dataProvider.Value);
            }

            base.OnInspectorGUI();
        }

        #endregion
    }
}