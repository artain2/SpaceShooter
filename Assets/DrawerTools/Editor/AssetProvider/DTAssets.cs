﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace DrawerTools
{
    public static class DTAssets
    {
        public static List<T> FindAssetsByType<T>() where T : Object
        {
            var assets = new List<T>();
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static bool HasAsset<T>(string path, string ext) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>($"{path}.{ext}");
            return asset != null;
        }

        public static T LoadAsset<T>(string path, string ext) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>($"{path}.{ext}");
            return asset;
        }
        
        public static T LoadAsset<T>(string path) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>($"{path}");
            return asset;
        }

        public static List<T> LoadAllAssetsInFolder<T>(string root, bool recursive = false) where T : Object
        {
            var paths = recursive 
                ? DTFolders.GetAllFilePathsInFolderRecursive(root) 
                : DTFolders.GetAllFilePathsInFolder(root);
            var result = new List<T>();
            var reqType = typeof(T);
            foreach (var path in paths)
            {
                var itemType = AssetDatabase.GetMainAssetTypeAtPath(path);
                var possibleComponent = itemType.IsEquivalentTo(typeof(GameObject)) &&
                                         reqType.IsSubclassOf(typeof(MonoBehaviour));
                if (!possibleComponent)
                {
                    if (!(itemType.IsSubclassOf(reqType) || itemType == reqType))
                        continue;
                }

                var loaded = AssetDatabase.LoadAssetAtPath<T>(path);
                if (loaded != null)
                    result.Add(loaded);
            }

            return result;
        }
        
        public static bool TryFindAsset<T>(string name, string ext, out T result, params string[] someRootFolders)
            where T : Object
        {
            result = null;
            var allPaths = AssetDatabase.GetAllAssetPaths();
            var nameMatch = $"/{name}.{ext}";
            foreach (var path in allPaths)
            {
                if (!path.Contains(nameMatch))
                {
                    continue;
                }

                var rootMatch = true;
                foreach (var root in someRootFolders)
                {
                    if (path.Contains(root))
                        continue;

                    rootMatch = false;
                    break;
                }

                if (!rootMatch)
                {
                    continue;
                }

                result = AssetDatabase.LoadAssetAtPath<T>(path);
                return true;
            }

            return false;
        }

        public static bool TryLoadAsset<T>(string path, string ext, out T result) where T : Object
        {
            result = AssetDatabase.LoadAssetAtPath<T>($"{path}.{ext}");
            return result != null;
        }

        public static bool HasConfig<T>(string path) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>($"{path}.asset");
            return asset != null;
        }

        public static T LoadConfig<T>(string path) where T : Object
        {
            path = path.EndsWith(".asset") ? path : path + ".asset";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;
        }

        public static bool TryLoadConfig<T>(string path, out T result) where T : Object
        {
            path = path.EndsWith(".asset") ? path : path + ".asset";
            result = AssetDatabase.LoadAssetAtPath<T>(path);
            return result != null;
        }

        public static bool HasSprite<T>(string path) where T : Object
        {
            path = path.EndsWith(".png") ? path : path + ".png";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset != null;
        }

        public static T LoadSprite<T>(string path) where T : Object
        {
            path = path.EndsWith(".png") ? path : path + ".png";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;
        }

        public static bool TryLoadSprite<T>(string path, out T result) where T : Object
        {
            path = path.EndsWith(".png") ? path : path + ".png";
            result = AssetDatabase.LoadAssetAtPath<T>(path);
            return result != null;
        }

        public static T LoadPrefab<T>(string path) where T : Object
        {
            path = path.EndsWith(".prefab") ? path : path + ".prefab";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;
        }

        public static bool TryLoadPrefab<T>(string path, out T result) where T : Object
        {
            path = path.EndsWith(".prefab") ? path : path + ".prefab";
            result = AssetDatabase.LoadAssetAtPath<T>(path);
            return result != null;
        }

        public static void CreateConfig(string directory, string fileName, ScriptableObject source)
        {
            AssetDatabase.CreateAsset(source, $"{directory}/{fileName}.asset");
        }

        public static void CreateConfig(string path, ScriptableObject source)
        {
            path = path.EndsWith(".asset") ? path : path + ".asset";
            AssetDatabase.CreateAsset(source, path);
        }

        public static void SetDirty(Object obj)
        {
            EditorUtility.SetDirty(obj);
        }

        public static void Refresh()
        {
            AssetDatabase.Refresh();
        }

        public static string GetAssetPath(Object obj)
        {
            return AssetDatabase.GetAssetPath(obj);
        }

        public static string GetAssetFolder(Object obj)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var end = path.LastIndexOf("/");
            return path.Substring(0, end);
        }

        public static bool TryFindClassAsset(Type type, out TextAsset result) =>
            TryFindClassAsset(type.Name, out result);
        public static bool TryFindClassAsset(string typeName, out TextAsset result)
        {
            result = null;
            var classSearchFilter = $"class {typeName}";
            var allScriptPaths = AssetDatabase.GetAllAssetPaths().Where(x => x.EndsWith(".cs")).ToArray();
            
            // Certain match search
            var matchFileName = $"{typeName}.cs";
            var csWithMatchName = allScriptPaths.FirstOrDefault(x => x.EndsWith(matchFileName));
            if (csWithMatchName!=null)
            {
                var code = File.ReadAllText(csWithMatchName);
                if (code.Contains(classSearchFilter))
                {
                    result = AssetDatabase.LoadAssetAtPath<TextAsset>(csWithMatchName);
                    return true;
                }
            }
            
            foreach (var csPath in allScriptPaths)
            {
                var code = File.ReadAllText(csPath);
                if (!code.Contains(classSearchFilter)) 
                    continue;
                result = AssetDatabase.LoadAssetAtPath<TextAsset>(csPath);
                return true;
            }

            return false;
        }
    }
}