// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindingPropertyDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Slash.Unity.DataBind.Core.Presentation;
using UnityEditor;
using UnityEngine;

namespace Slash.Unity.DataBind.Editor.PropertyDrawers
{
    /// <summary>
    ///     Property Drawer for <see cref="DataBinding" />.
    /// </summary>
    [CustomPropertyDrawer(typeof (DataBinding))]
    public class DataBindingPropertyDrawer : PropertyDrawer
    {
        private const float LineHeight = 16f;

        private const float LineSpacing = 2f;

        private SerializedProperty constantProperty;

        private SerializedProperty pathProperty;

        private SerializedProperty providerProperty;

        private SerializedProperty referenceProperty;

        private Rect selectGameObjectComponentRect;

        private SerializedProperty targetTypeProperty;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            this.targetTypeProperty = property.FindPropertyRelative("Type");
            this.providerProperty = property.FindPropertyRelative("Provider");
            this.constantProperty = property.FindPropertyRelative("Constant");
            this.referenceProperty = property.FindPropertyRelative("Reference");
            this.pathProperty = property.FindPropertyRelative("Path");

            var targetType = (DataBindingType) this.targetTypeProperty.enumValueIndex;
            float targetTypeHeight = 0;
            switch (targetType)
            {
                case DataBindingType.Context:
                    targetTypeHeight = EditorGUI.GetPropertyHeight(this.pathProperty);
                    break;
                case DataBindingType.Provider:
                    targetTypeHeight = EditorGUI.GetPropertyHeight(this.providerProperty);
                    break;
                case DataBindingType.Constant:
                    targetTypeHeight = EditorGUI.GetPropertyHeight(this.constantProperty);
                    break;
                case DataBindingType.Reference:
                    targetTypeHeight = EditorGUI.GetPropertyHeight(this.referenceProperty);
                    break;
            }

            return LineHeight + targetTypeHeight + LineSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            var contentPosition = EditorGUI.PrefixLabel(position, label);
            if (this.targetTypeProperty != null)
            {
                // Type selection.
                contentPosition.height = LineHeight;
                EditorGUI.PropertyField(contentPosition, this.targetTypeProperty, GUIContent.none);
                position.y += LineHeight + LineSpacing;

                // Draw type specific fields.
                EditorGUI.indentLevel++;
                var targetType = (DataBindingType) this.targetTypeProperty.enumValueIndex;
                switch (targetType)
                {
                    case DataBindingType.Context:
                    {
                        var rect = new Rect(position) {height = LineHeight};
                        EditorGUI.PropertyField(rect, this.pathProperty);
                    }
                        break;
                    case DataBindingType.Provider:
                    {
                        var rect = new Rect(position) {height = LineHeight};
                        EditorGUI.PropertyField(rect, this.providerProperty, new GUIContent("Provider"));
                    }
                        break;
                    case DataBindingType.Constant:
                    {
                        var rect = new Rect(position) {height = LineHeight};
                        EditorGUI.PropertyField(rect, this.constantProperty, new GUIContent("Constant"));
                    }
                        break;
                    case DataBindingType.Reference:
                    {
                        var rect = new Rect(position) {height = LineHeight};
                        GameObjectComponentSelectionField(rect, new GUIContent("Reference"), this.referenceProperty,
                            ref this.selectGameObjectComponentRect);
                    }
                        break;
                }
            }
            --EditorGUI.indentLevel;

            EditorGUI.EndProperty();
        }

        private static void GameObjectComponentSelectionField(Rect rect, GUIContent label,
            SerializedProperty serializedProperty,
            ref Rect popupWindowRect)
        {
            var reference = serializedProperty.objectReferenceValue;
            var newReference = EditorGUI.ObjectField(rect, label, reference, typeof (Object), true);
            if (newReference != reference)
            {
                var gameObjectReference = newReference as GameObject;
                if (gameObjectReference != null)
                {
                    // Let user select component reference if he wants to.
                    PopupWindow.Show(popupWindowRect,
                        new SelectGameObjectComponentPopupWindowContent(gameObjectReference,
                            selectedReference =>
                            {
                                serializedProperty.objectReferenceValue = selectedReference;
                                serializedProperty.serializedObject.ApplyModifiedProperties();
                            }));
                }
                else
                {
                    serializedProperty.objectReferenceValue = newReference;
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                popupWindowRect = GUILayoutUtility.GetLastRect();
            }
        }

        private class SelectGameObjectComponentPopupWindowContent : PopupWindowContent
        {
            private readonly System.Action<Object> callback;

            private readonly GameObject gameObject;

            public SelectGameObjectComponentPopupWindowContent(GameObject gameObject, System.Action<Object> callback)
            {
                this.gameObject = gameObject;
                this.callback = callback;
            }

            public override void OnGUI(Rect rect)
            {
                if (GUILayout.Button("Use Game Object", GUILayout.Width(200)))
                {
                    this.callback(this.gameObject);
                    this.editorWindow.Close();
                }

                var components = this.gameObject.GetComponents<Component>();
                if (components.Length > 0)
                {
                    EditorGUILayout.LabelField("Component:", EditorStyles.boldLabel);
                    foreach (var component in components)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(component.GetType().Name, GUILayout.Width(100));
                        if (GUILayout.Button("Choose", GUILayout.Width(100)))
                        {
                            this.callback(component);
                            this.editorWindow.Close();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}