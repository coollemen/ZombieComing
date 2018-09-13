using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TDTK{
	
	[CustomEditor(typeof(RscManager))]
	public class I_RscManagerEditor : _TDInspector {

		private RscManager instance;
		public override void Awake(){
			base.Awake();
			instance = (RscManager)target;
		}
		
		
		public override void OnInspectorGUI(){
			base.OnInspectorGUI();
			
			if(instance==null){ Awake(); return; }
			
			Undo.RecordObject(instance, "RscManager");
			GUI.changed = false;
			
			EditorGUILayout.Space();
			
			
				cont=new GUIContent("Carry Over:", "Check to have the resource starting value taken from the last level and save the ending resource value for the next level.\n\nIf this is the first level, the specified value is used instead");
				instance.carryOver=EditorGUILayout.Toggle(cont, instance.carryOver);
				
				cont=new GUIContent("Sell Multiplier:", "The multiplier apply to tower value when calculating the tower sell value.");
				instance.sellValueMultiplier=EditorGUILayout.FloatField(cont, instance.sellValueMultiplier);
				
				cont=new GUIContent("Regenerate Resource:", "Check to have the resource regenerate overtime");
				instance.regenerateRsc=EditorGUILayout.Toggle(cont, instance.regenerateRsc);
			
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
				string text="The following list applies project wide.\n";
				text+="'Value' is the only local attribute applicable this scene.";
				EditorGUILayout.HelpBox(text, MessageType.Info);
			
			
				List<RscItem> dbList=RscDB.GetList();
				
				GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Resource List", TDE.headerS, GUILayout.MaxWidth(120));
					if(GUILayout.Button("Add New Resource", GUILayout.MaxWidth(150))) dbList.Add(new RscItem());
				GUILayout.EndHorizontal();
				
				while(instance.rscList.Count<dbList.Count) instance.rscList.Add(0);
				while(instance.rscList.Count>dbList.Count) instance.rscList.RemoveAt(instance.rscList.Count-1);
				
				float defaultWidth=EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth=50;
			
			EditorGUILayout.Space();
			
				for(int i=0; i<dbList.Count; i++){
					RscItem item=dbList[i];
					
					GUILayout.BeginHorizontal();
						item.icon=(Sprite)EditorGUILayout.ObjectField(item.icon, typeof(Sprite), true, GUILayout.Width(50), GUILayout.Height(50));
						
						GUILayout.BeginVertical();
							
							item.name=EditorGUILayout.TextField("Name:", item.name, GUILayout.ExpandWidth(true));
							instance.rscList[i]=EditorGUILayout.IntField("Value:", instance.rscList[i], GUILayout.ExpandWidth(true));
							
							GUILayout.BeginHorizontal();
								EditorGUIUtility.labelWidth=defaultWidth;
								item.enableCarry=EditorGUILayout.Toggle("Enable Carry Over:", item.enableCarry, GUILayout.ExpandWidth(true));
								EditorGUIUtility.labelWidth=50;
					
								if(GUILayout.Button("Remove", GUILayout.MaxWidth(60), GUILayout.MaxHeight(14))){ 
									dbList.RemoveAt(i);
									instance.rscList.RemoveAt(i);
									i-=1;
								}
							GUILayout.EndHorizontal();
						
						GUILayout.EndVertical();
					
					GUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
				}
				
				EditorGUIUtility.labelWidth=defaultWidth;
			
			EditorGUILayout.Space();
			
			DefaultInspector();
			
			if(GUI.changed) EditorUtility.SetDirty(instance);
		}
		
		
	}

}