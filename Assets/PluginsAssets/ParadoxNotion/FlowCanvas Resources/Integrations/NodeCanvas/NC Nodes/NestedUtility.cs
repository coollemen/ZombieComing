using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Object = UnityEngine.Object;

public static class NestedUtility
{

    //-------------------------------------------------
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null)
        {
            throw new ArgumentException("Argument cannot be null.", "source");
        }

        foreach (T value in source)
        {
            action(value);
        }
    }

#if UNITY_EDITOR

    public static bool CheckHasThisAsset(Object _asset, Object prefab)
    {
        if (AssetDatabase.IsMainAsset(prefab))
        {
            string path = AssetDatabase.GetAssetPath(_asset);

            //  any sub assets inside the prefab.
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

            int length = assets.Length;
            for (int i = 0; i < length; i++)
            {
                Object asset = assets[i];
                if (asset is GameObject || asset is Component)
                {
                    continue;
                }
                else if (asset == _asset)
                {
                    return true;
                }

            }
        }
        return false;
    }

    public static bool SaveToAssetCheckHasThisAsset(Object _asset, Object targetMainAsset)
    {
        if (AssetDatabase.IsMainAsset(targetMainAsset))
        {
            string path = AssetDatabase.GetAssetPath(targetMainAsset);

            //  any sub assets inside the prefab.
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

            int length = assets.Length;
            for (int i = 0; i < length; i++)
            {
                Object asset = assets[i];
                if (asset is GameObject || asset is Component)
                {
                    continue;
                }
                else if (asset == _asset)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void DeleteAllUselessBoundAsset(Graph currentGraph)
    { 
        //Debug.Log("delete clear root graph :"+currentGraph.name);
        string path = AssetDatabase.GetAssetPath(currentGraph);
        //Debug.Log("script path :" + path);
        //all subAsset and mainAsset.
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        //assets.ForEach(x=>Debug.Log(x.name));
        List<Graph> allNestedGraphs = new List<Graph>();

        if (!AssetDatabase.IsMainAsset(currentGraph) &&currentGraph.agent != null)
        {
            var prefabType = PrefabUtility.GetPrefabType(currentGraph.agent.gameObject); //如果是prefab
            if (prefabType != PrefabType.Prefab || Application.isPlaying)
            {
                Debug.Log("currentGraph is not prefab :" + currentGraph.name);
                return;
            }
            GraphOwner[] gos = currentGraph.agent.transform.root.GetComponentsInChildren<GraphOwner>();
            gos.ForEach(a =>
            {
                if (a.graph != null) allNestedGraphs.AddRange(a.graph.GetAllNestedGraphs<Graph>(true));
            });

            int length = assets.Length;
            for (int i = 0; i < length; i++)
            {
                Object asset = assets[i];
                if (UnityEditor.AssetDatabase.IsMainAsset(asset) || asset is GameObject || asset is Component)
                    //忽视主资源和实体物体或组件
                {
                    continue;
                }
                else if (!CheckAssetGraph(asset, gos)) //如果不是图表主资源asset
                {
                    bool assetUsed = false;
                    for (int j = 0; j < allNestedGraphs.Count; j++)
                    {
                        if (allNestedGraphs[j] == asset) //如果图表中Asse资源仍然被引用, 就不删除该资源
                        {
                            assetUsed = true;
                            break;
                        }
                    }
                    if (!assetUsed)
                        Object.DestroyImmediate(asset, true);
                }
            }
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UnityEditor.AssetDatabase.ImportAsset(path);
            };
        }
        else //如果是Asset
        {
            if (AssetDatabase.IsSubAsset(currentGraph))
                return;
            allNestedGraphs = currentGraph.GetAllNestedGraphs<Graph>(true);
            int length = assets.Length;
            for (int i = 0; i < length; i++)
            {
                Object asset = assets[i];
                if (UnityEditor.AssetDatabase.IsMainAsset(asset))
                {
                    continue;
                }
                else
                {
                    bool assetUsed = false;
                    for (int j = 0; j < allNestedGraphs.Count; j++)
                    {
                        if (allNestedGraphs[j] == asset) //如果图表中资源仍然被引用, 就不删除该资源
                        {
                            assetUsed = true;
                            break;
                        }
                    }
                    if (!assetUsed)
                        Object.DestroyImmediate(asset, true);
                }
            }
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UnityEditor.AssetDatabase.ImportAsset(path);
            };
        }


    }

    static bool CheckAssetGraph(Object asset, GraphOwner[] go)
    {
        foreach (var owner in go)
        {
            if (owner.graph == asset)
                return true;
        }
        return false;
    }

    public static T CreateBoundNested<T>(IGraphAssignable parent, Graph owner) where T : Graph
    {
        T newGraph;

        newGraph = ScriptableObject.CreateInstance<T>();
        (newGraph as UnityEngine.Object).name = owner.name;
        newGraph.Validate();

        if (newGraph != null)
        {
            //newGraph.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            Undo.RegisterCreatedObjectUndo(newGraph, "CreateBoundNested");
            parent.nestedGraph = newGraph;
            //设置graph归属
            if(parent.GetType().IsSubclassOf(typeof(Graph)))
                newGraph.agent = ((Graph) parent).agent;
            if (AssetDatabase.IsMainAsset(owner))
            {
                //Debug.Log("Create main aseet: "+owner);
                AssetDatabase.AddObjectToAsset(newGraph, owner);

                EditorApplication.delayCall += () => { AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(owner)); };
                owner.Validate();
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(owner);
            }
            else if (AssetDatabase.IsSubAsset(owner))
            {
                //Debug.Log("Create sub aseet: " + owner);

                AssetDatabase.AddObjectToAsset(newGraph, GetMainAssetBySubAsset(owner));

                EditorApplication.delayCall += () => { AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(owner)); };
                owner.Validate();
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(owner);

            }
        }
        return newGraph;
    }

    public static Object GetMainAssetBySubAsset(Object sub)
    {

        string path = AssetDatabase.GetAssetPath(sub);
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (var asset in assets)
        {
            if (AssetDatabase.IsMainAsset(asset))
            {
                return asset;
            }
        }
        return null;
    }

    public static Object GetMainAssetByPath(string path)
    {
        //Debug.Log("path:"+path);
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        //Debug.Log("mainasset count:" + assets.Length);
        foreach (var asset in assets)
        {
            //Debug.Log("asset:" + asset);
            if (AssetDatabase.IsMainAsset(asset))
            {
                //Debug.Log("mainasset:" + asset);
                return asset;
            }
        }
        //Debug.Log("asset: null");
        return null;
    }

    [InitializeOnLoadMethod]
    public static void StartInitializeOnLoadMethod()
    {
        int time = System.Environment.TickCount;
        PrefabUtility.prefabInstanceUpdated = delegate(GameObject instance)
        {
            if (Selection.activeGameObject == null)
                return;

            if (
                !Selection.activeGameObject.transform.root.GetComponentsInChildren<Transform>()
                    .ToList()
                    .Contains(instance.transform)) //prefab 变更 是该物体
                return;

            if (System.Environment.TickCount - time < 1000)
            {
                return;
            }


            time = System.Environment.TickCount;
            GraphOwner[] gos = instance.GetComponentsInChildren<GraphOwner>();
            if (gos == null && gos.Length < 1)
                return;

            //var prefabType = UnityEditor.PrefabUtility.GetPrefabType(instance.gameObject);
            ////Debug.Log("prefabType:" + prefabType);
            //if (prefabType == UnityEditor.PrefabType.Prefab)
            //{
            //Debug.Log("Change:"+instance.name);
            gos.ForEach(a => ApplyGraphChange(a, instance));
            //}
        };
    }

    public static bool CheckAssetRef(Object target, List<Graph> allNestedGraph)
    {
        string path = AssetDatabase.GetAssetPath(target);
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        for (int i = 0; i < assets.Length; i++)
        {
            if (allNestedGraph.Contains((Graph) assets[i]))
            {
                Debug.Log("无法覆盖已经被引用的资源:" + assets[i].name);
                return true;
            }
        }
        return false;
    }

    public static void SaveToAsset(Graph graph, Object targetMainAsset)
    {
        if (Application.isPlaying)
            return;
        string copyPath;

        //if (CheckAssetRef(targetMainAsset, graph.GetAllNestedGraphs<Graph>(true)))
        //{
        //    EditorUtility.DisplayDialog("无法创建资源", "无法覆盖已经被引用的资源", "OK");
        //    return;
        //}
        //DeleteAllUselessBoundAsset(graph);

        if (graph.agent != null)
        {
            Debug.Log("Save To Asset:"+ graph.agent);
            List<Graph> allNestGraphs = new List<Graph>();
            allNestGraphs.AddRange(graph.GetAllNestedGraphs<Graph>(true));
            var prefabType = UnityEditor.PrefabUtility.GetPrefabType(graph.agent.gameObject);
            //Debug.Log("prefabType:" + prefabType);

            if (prefabType == UnityEditor.PrefabType.PrefabInstance || prefabType == UnityEditor.PrefabType.Prefab)
            {
                string targetMainAssetPath = AssetDatabase.GetAssetPath(targetMainAsset);               
                string prefbPath;
                if (prefabType == UnityEditor.PrefabType.PrefabInstance)
                {
                    //AssetDatabase.DeleteAsset(targetMainAssetPath);

                    Object prefab = PrefabUtility.GetCorrespondingObjectFromSource(graph.agent.gameObject);
                    // Debug.Log("Instance prefab name:" + prefab.name);
                    prefbPath = AssetDatabase.GetAssetPath(prefab);
                    copyPath = targetMainAssetPath;
                    //Debug.Log("Instance prefab path:" + prefbPath);
                    //Debug.Log("currentPrefab is not MainAsset, try to create new ");

                    Object[] _assets = AssetDatabase.LoadAllAssetsAtPath(prefbPath);
                    //Object MainPrefabAsset = targetMainAsset;
                    //foreach (var asset in _assets)
                    //{
                    //    if (AssetDatabase.IsMainAsset(asset))
                    //    {
                    //        Debug.Log("获得主Prefab资源物体：" + asset.name);
                    //        MainPrefabAsset = asset;
                    //        break;
                    //    }
                    //}
                    List<Object> assetList = _assets.ToList();
                    bool temp = false;
                    foreach (var nestGraph in allNestGraphs)
                    {
                        if (!assetList.Contains(nestGraph))
                        {
                            //Debug.Log("当前子PrefabInstance，在prefab下没有graph资源，生成它");
                            if (AssetDatabase.IsMainAsset(nestGraph) || AssetDatabase.IsSubAsset(nestGraph))
                            {
                                //Debug.Log("当前资源已经保存到磁盘");
                                var newGraph = ScriptableObject.CreateInstance(nestGraph.GetType());
                                (newGraph as UnityEngine.Object).name = nestGraph.name;
                                EditorUtility.CopySerialized(nestGraph, newGraph);
                                ((Graph) newGraph).Validate();
                                AssetDatabase.AddObjectToAsset(newGraph, targetMainAsset);
                                Debug.Log(string.Format("请手动调整嵌套资源: {0} 的引用", newGraph.name));
                                temp = true;
                            }
                            else
                            {
                                AssetDatabase.AddObjectToAsset(nestGraph, targetMainAsset);
                            }
                        }
                        else
                        {
                            temp = true;
                            Debug.Log(string.Format("请手动调整嵌套资源: {0} 的引用", nestGraph.name));
                        }
                    }
                    if (temp)
                    {
                        //Debug.Log(string.Format("手动调整恢复嵌套资源的引用后,建议使用clearUselessNestedAsset按钮,清理一次无用嵌套资源"));
                    }

                    UnityEditor.EditorApplication.delayCall +=
                        () => { AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(targetMainAsset)); };

                    //Debug.Log("prefab path:" + copyPath);
                    //AssetDatabase.CopyAsset(prefbPath, copyPath);
                }
                else
                {                   
                    prefbPath = AssetDatabase.GetAssetPath(graph);
                    //Debug.Log("Realprefab path:" + prefbPath);
                    copyPath = targetMainAssetPath;
                    //Debug.Log("prefab path:" + copyPath);
                    AssetDatabase.CopyAsset(prefbPath, copyPath);

                    Object[] assets = AssetDatabase.LoadAllAssetsAtPath(copyPath);
                    if (assets == null || assets.Length == 0)
                    {
                        //Debug.Log("no duplicate");
                        return;
                    }
                    Object MainAsset = assets[0];

                    //List<Graph> newCopyNestGraphList = new List<Graph>();
                    int length = assets.Length;
                    for (int i = 0; i < length; i++)
                    {
                        Object asset = assets[i];
                        if (AssetDatabase.IsMainAsset(asset))
                        {
                            MainAsset = asset;
                            //Debug.Log(MainAsset.name);
                            continue;
                        }

                        if (asset is GameObject || asset is Component)
                        {
                            if (asset is Transform)
                                continue;
                            Object.DestroyImmediate(asset, true);
                            continue;
                        }

                        //Debug.Log("asset name:" + asset.name + "  graph name:" + graph.name);
                        if (asset.name == graph.name)
                        {
                            // Debug.Log("Main Scirpt:"+asset.name);
                            ((Graph)asset).Validate();
                            //newCopyNestGraphList = ((Graph) asset).GetAllNestedGraphs<Graph>(true);
                            AssetDatabase.SetMainObject(asset, copyPath);
                        }
                    }


                    UnityEditor.EditorApplication.delayCall +=
                    () => { AssetDatabase.ImportAsset(copyPath); };

                    Object.DestroyImmediate(MainAsset, true);
                }


                UnityEditor.EditorApplication.delayCall +=
                    () => { AssetDatabase.ImportAsset(copyPath); };

                AssetDatabase.Refresh();
            }
            else //prefab parent物体模式 或者资源模式 
            {
                foreach (var v in allNestGraphs)
                {
                    if (v != graph && !SaveToAssetCheckHasThisAsset(v, graph))
                    {                               
                        if (AssetDatabase.IsMainAsset(v) || AssetDatabase.IsSubAsset(v))
                        {
                            //Debug.Log("当前资源已经保存到磁盘");
                            var newGraph = ScriptableObject.CreateInstance(v.GetType());
                            (newGraph as UnityEngine.Object).name = v.name;
                            EditorUtility.CopySerialized(v, newGraph);
                            ((Graph) newGraph).Validate();
                            AssetDatabase.AddObjectToAsset(newGraph, targetMainAsset);
                        }
                        else
                        {
                            //Debug.Log("prefabNone add to mainAsset:" + v.name);
                            AssetDatabase.AddObjectToAsset(v, targetMainAsset);
                            ((Graph)v).Validate();
                        }
                    }
                }
                UnityEditor.EditorApplication.delayCall +=
                    () => { AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(targetMainAsset)); };
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(targetMainAsset);
            }


        }

    }

    public static void ApplyGraphChange(GraphOwner go,GameObject instance)
    {
        //--------------------- 同步更新prefab资源
        bool isBound = go.graphIsBound;

        if (!isBound)
            return;

        var prefabType = UnityEditor.PrefabUtility.GetPrefabType(go.gameObject);

        if (prefabType == UnityEditor.PrefabType.PrefabInstance)
        {
            if (!Application.isPlaying)
            {
                GameObject prefab = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(instance);

                List<Graph> allNestGraphs = go.graph.GetAllNestedGraphs<Graph>(true);

                foreach (var v in allNestGraphs)
                {
                    if (!NestedUtility.CheckHasThisAsset(v, prefab))
                    {
                        UnityEditor.AssetDatabase.AddObjectToAsset(v, prefab);
                    }
                }

                UnityEditor.EditorApplication.delayCall +=
    () => { AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(prefab)); };
                AssetDatabase.Refresh();
            }
        }
    }
#endif
}
