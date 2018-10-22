// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextHolderEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Slash.Unity.DataBind.Core.Data;
using Slash.Unity.DataBind.Core.Presentation;
using Slash.Unity.DataBind.Core.Utils;
using Slash.Unity.DataBind.Editor.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Slash.Unity.DataBind.Editor.Editors
{
    /// <summary>
    ///     Custom editor for <see cref="ContextHolder" />.
    /// </summary>
    [CustomEditor(typeof (ContextHolder))]
    public class ContextHolderEditor : UnityEditor.Editor
    {
        #region Constants

        private const int MaxLevel = 5;

        #endregion

        #region Fields

        private readonly Dictionary<DataDictionary, object> newKeys = new Dictionary<DataDictionary, object>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Unity callback.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var contextHolder = this.target as ContextHolder;
            if (contextHolder == null)
            {
                return;
            }

            if (UnityEngine.Application.isPlaying)
            {
                var context = contextHolder.Context;
                var contextType = context != null ? context.GetType().ToString() : "null";
                EditorGUILayout.LabelField("Context", contextType);

                // Reflect data in context.
                this.DrawContextData(context);

                EditorUtility.SetDirty(contextHolder);
            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        #endregion

        #region Methods

        private void DrawContextData(object contextObject)
        {
            if (contextObject == null)
            {
                return;
            }

            var context = contextObject as Context;
            if (context != null)
            {
                this.DrawContextData(context, 1);
            }
            else
            {
                EditorGUILayout.TextField("Data", contextObject.ToString());
            }
        }

        private void DrawContextData(Context context, int level)
        {
            var prevIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = level;

            var contextMemberInfos = ContextTypeCache.GetMemberInfos(context.GetType());
            foreach (var contextMemberInfo in contextMemberInfos)
            {
                if (contextMemberInfo.Property != null)
                {
                    var memberValue = contextMemberInfo.Property.GetValue(context, null);
                    var newMemberValue = this.DrawContextData(contextMemberInfo.Name,
                        contextMemberInfo.Property.PropertyType, memberValue, level);
                    if (contextMemberInfo.Property.CanWrite && !Equals(newMemberValue, memberValue))
                    {
                        contextMemberInfo.Property.SetValue(context, newMemberValue, null);
                    }
                }
                else if (contextMemberInfo.Method != null)
                {
                    this.DrawContextMethod(context, contextMemberInfo.Method);
                }
            }

            EditorGUI.indentLevel = prevIndentLevel;
        }

        public class InvokeEventPopup : PopupWindowContent
        {
            private readonly Action<object[]> invokeAction;

            private readonly ParameterInfo[] parameterInfos;

            private readonly object[] parameterValues;

            public InvokeEventPopup(ParameterInfo[] parameterInfos, Action<object[]> invokeAction)
            {
                this.parameterInfos = parameterInfos;
                this.invokeAction = invokeAction;
                this.parameterValues = new object[parameterInfos.Length];
            }

            public override Vector2 GetWindowSize()
            {
                var lineCount = this.parameterInfos.Length + 2;
                return new Vector2(200, lineCount*EditorGUIUtility.singleLineHeight + 20);
            }

            public override void OnGUI(Rect rect)
            {
                GUILayout.Label("Parameters", EditorStyles.boldLabel);

                for (var index = 0; index < this.parameterInfos.Length; index++)
                {
                    var parameterInfo = this.parameterInfos[index];

                    var parameterValue = this.parameterValues[index];

                    this.parameterValues[index] = DrawValueField(ObjectNames.NicifyVariableName(parameterInfo.Name),
                        parameterInfo.ParameterType,
                        parameterValue);
                }

                if (GUILayout.Button("Invoke"))
                {
                    this.invokeAction(this.parameterValues);
                }
            }
        }

        private Rect invokeEventPopupRect;

        private void DrawContextMethod(Context context, MethodInfo method)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(method.Name));

            if (GUILayout.Button("Invoke"))
            {
                var parameterInfos = method.GetParameters();
                if (parameterInfos.Length > 0)
                {
                    var invokeEventPopup = new InvokeEventPopup(parameterInfos,
                        parameters => method.Invoke(context, parameters));
                    PopupWindow.Show(this.invokeEventPopupRect, invokeEventPopup);
                }
                else
                {
                    method.Invoke(context, null);
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                // Cover button with popup.
                var buttonRect = GUILayoutUtility.GetLastRect();
                this.invokeEventPopupRect = buttonRect;
                this.invokeEventPopupRect.position = new Vector2(buttonRect.position.x,
                    buttonRect.position.y - buttonRect.size.y);
            }

            EditorGUILayout.EndHorizontal();
        }

        private object DrawContextData(string memberName, Type memberType, object memberValue, int level)
        {
            if (level < MaxLevel)
            {
                var context = memberValue as Context;
                if (context != null)
                {
                    EditorGUILayout.LabelField(memberName, EditorStyles.boldLabel);
                    this.DrawContextData(context, level + 1);
                    return context;
                }

                var collection = memberValue as Collection;
                if (collection != null)
                {
                    EditorGUILayout.LabelField(memberName, EditorStyles.boldLabel);
                    this.DrawContextData(collection, level + 1);
                    return collection;
                }

                var dictionary = memberValue as DataDictionary;
                if (dictionary != null)
                {
                    EditorGUILayout.LabelField(memberName, EditorStyles.boldLabel);
                    this.DrawContextData(dictionary, level + 1);
                    return dictionary;
                }
            }

            // Draw data trigger.
            var dataTrigger = memberValue as DataTrigger;
            if (dataTrigger != null)
            {
                DrawDataTrigger(memberName, dataTrigger);
                return dataTrigger;
            }

            return DrawValueField(memberName, memberType, memberValue);
        }

        private static object DrawValueField(string memberName, Type memberType, object memberValue)
        {
            if (memberValue == null)
            {
                if (memberType.IsValueType)
                {
                    memberValue = Activator.CreateInstance(memberType);
                }
            }

            if (memberType == typeof (int))
            {
                return EditorGUILayout.IntField(memberName, (int) memberValue);
            }

            if (memberType == typeof (long))
            {
                return EditorGUILayout.LongField(memberName, (long) memberValue);
            }

            if (memberType == typeof (float))
            {
                return EditorGUILayout.FloatField(memberName, (float) memberValue);
            }

            if (memberType == typeof(bool))
            {
                return EditorGUILayout.Toggle(memberName, (bool)memberValue);
            }

            if (memberType == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(memberName, (Vector2)memberValue);
            }

            if (memberType == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(memberName, (Vector3)memberValue);
            }

            if (memberType == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(memberName, (Vector4)memberValue);
            }

            if (memberType == typeof (string))
            {
                return EditorGUILayout.TextField(memberName, (string) memberValue);
            }

            if (ReflectionUtils.IsEnum(memberType))
            {
                return EditorGUILayout.EnumPopup(memberName, (Enum) memberValue);
            }

            var unityObjectType = typeof (Object);
            if (unityObjectType.IsAssignableFrom(memberType))
            {
                return EditorGUILayout.ObjectField(memberName, (Object) memberValue, memberType, true);
            }

            EditorGUILayout.LabelField(memberName, memberValue != null ? memberValue.ToString() : "null");
            return memberValue;
        }

        private void DrawContextData(Collection collection, int level)
        {
            var prevIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = level;

            var index = 0;
            List<object> itemsToRemove = null;
            foreach (var item in collection)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Item " + index);
                if (GUILayout.Button("Remove"))
                {
                    if (itemsToRemove == null)
                    {
                        itemsToRemove = new List<object>();
                    }
                    itemsToRemove.Add(item);
                }
                EditorGUILayout.EndHorizontal();
                this.DrawContextData("Item " + index, collection.ItemType, item, level);
                ++index;
            }

            if (itemsToRemove != null)
            {
                foreach (var itemToRemove in itemsToRemove)
                {
                    collection.Remove(itemToRemove);
                }
            }

            if (GUILayout.Button("New Item"))
            {
                collection.AddNewItem();
            }

            EditorGUI.indentLevel = prevIndentLevel;
        }

        private void DrawContextData(DataDictionary dataDictionary, int level)
        {
            var prevIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = level;

            Dictionary<object, object> changedValues = null;
            foreach (var key in dataDictionary.Keys)
            {
                var value = dataDictionary[key];
                var newValue = this.DrawContextData("Item " + key, dataDictionary.ValueType, value, level);
                if (!Equals(value, newValue))
                {
                    if (changedValues == null)
                    {
                        changedValues = new Dictionary<object, object>();
                    }
                    changedValues[key] = newValue;
                }
            }

            if (changedValues != null)
            {
                foreach (var key in changedValues.Keys)
                {
                    dataDictionary[key] = changedValues[key];
                }
            }

            GUILayout.BeginHorizontal();

            object newKey;
            this.newKeys.TryGetValue(dataDictionary, out newKey);
            var keyType = dataDictionary.KeyType;
            const string NewKeyLabel = "New:";
            if (keyType == typeof (string))
            {
                this.newKeys[dataDictionary] = newKey = EditorGUILayout.TextField(NewKeyLabel, (string) newKey);
            }
            else if (keyType.IsEnum)
            {
                if (newKey == null)
                {
                    newKey = Enum.GetValues(keyType).GetValue(0);
                }

                this.newKeys[dataDictionary] = newKey = EditorGUILayout.EnumPopup(NewKeyLabel, (Enum) newKey);
            }

            if (GUILayout.Button("+") && newKey != null)
            {
                var valueType = dataDictionary.ValueType;
                dataDictionary.Add(newKey, valueType.IsValueType ? Activator.CreateInstance(valueType) : null);
            }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel = prevIndentLevel;
        }

        private static void DrawDataTrigger(string memberName, DataTrigger dataTrigger)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(memberName);
            if (GUILayout.Button("Invoke"))
            {
                dataTrigger.Invoke();
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}