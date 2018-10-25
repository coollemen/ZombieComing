using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace FlowCanvas.Nodes
{

    #region String

    [Name("stringFormat")]
    [Category("UnityEngine/String")]
    [Description("StringFormat 格式化组合字符, {0} and {1}")]
    [ContextDefinedInputs(typeof(string))]
    public class G_StringFormat : FlowNode
    {   
        [SerializeField]
        private string formatString = "";
        [SerializeField]
        private List<ValueInput<string>> valueList = new List<ValueInput<string>>();
        protected override void RegisterPorts()
        {
            for (int i = 0; i < paraNameList.Count; i++)
            {
                valueList[i] = AddValueInput<string>(paraNameList[i], paraNameList[i]);
            }
            AddValueOutput<string>("value", () =>
            {
                string tempString = formatString;
                for (int i = 0; i < paraNameList.Count; i++)
                {
                    tempString = tempString.Replace(paraNameList[i], valueList[i].value);
                    Debug.Log(tempString);
                }
                return tempString;
            });
        }
        [SerializeField]
        private List<string> paraNameList= new List<string>();
#if UNITY_EDITOR
        protected override void OnNodeGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10f);
            string inputString = GUILayout.TextField(formatString);
            if (formatString!=inputString)
            {
                formatString = inputString;
                paraNameList =new List<string>();
                valueList=new List<ValueInput<string>>();

                string regexFormat = @"\{\d\}";
                MatchCollection mc = Regex.Matches(formatString, regexFormat);
                mc.Cast<Match>().Select(m=>m.Value).ToList<string>().ForEach(x=> { Debug.Log(x);
                    //string y = x.Replace("{", "").Replace("}", "");
                    paraNameList.Add(x);
                    valueList.Add( AddValueInput<string>(x, x));
                });

                GatherPorts();
            }
            GUILayout.EndVertical();
            base.OnNodeGUI();
        }
#endif 
    }

    [Name("stringAppend")]
    [Category("UnityEngine/String")]
    [Description("Append String")]
    [ContextDefinedInputs(typeof(string))]
    public class G_StringAppend : FlowNode
    {

        [SerializeField]
        private int _portCount = 2;
        public int portCount
        {
            get { return _portCount; }
            set
            {
                _portCount = value;
                _portCount = Mathf.Clamp(_portCount, 0, _portCount);
                GatherPorts();
            }
        }

        protected override void RegisterPorts()
        {
            var ins = new List<ValueInput<string>>();
            for (var i = 0; i < portCount; i++)
            {
                ins.Add(AddValueInput<string>(i.ToString()));
            }
            AddValueOutput<string>("Value", () =>
            {
                string finalString=""; ins.ForEach(x=> { finalString = finalString + x.value; });
                return finalString;
            });
        }


#if UNITY_EDITOR
        protected override void OnNodeGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+",GUILayout.Width(30)))
            {
                portCount++;
            }
            GUILayout.Space(10f);
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                portCount--;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            base.OnNodeGUI();
        }

#endif 
    }
    #endregion
}