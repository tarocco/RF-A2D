/*
The MIT License (MIT)

Copyright (c) 2017 Roaring Fangs LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using Anima2D;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using System;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace RoaringFangs.Animation
{
    [CustomEditor(typeof(SpriteMeshSelector))]
    public class SpriteMeshSelectorEditor : UnityEditor.Editor
    {
        public const string BindingAssetPathParent = "Assets";
        public const string BindingAssetPathFolder = "SpriteMeshBindings";

        public static string CreateManagedBindingAssetFolder()
        {
            var folder_path = BindingAssetPathParent + "/" + BindingAssetPathFolder;
            var folder_is_valid =
                    AssetDatabase.IsValidFolder(folder_path);
            if (!folder_is_valid)
                AssetDatabase.CreateFolder(
                    BindingAssetPathParent,
                    BindingAssetPathFolder);
            return folder_path;
        }

        private static string GetSha256String(string str)
        {
            var algorithm = new System.Security.Cryptography.SHA256Managed();
            var hash_builder = new System.Text.StringBuilder();
            var result = algorithm.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(str), 0,
                System.Text.Encoding.UTF8.GetByteCount(str));
            foreach (byte @byte in result)
                hash_builder.Append(@byte.ToString("x2"));
            var hash_string = hash_builder.ToString();
            var hash_string_lower = hash_string.ToLower();
            return hash_string_lower;
        }

        private static string GetObjectGUID(UnityObject @object)
        {
            var asset_path = AssetDatabase.GetAssetPath(@object);
            return AssetDatabase.AssetPathToGUID(asset_path);
        }

        private static string MultiHashObjectUUID(params UnityObject[] objects)
        {
            var concatenated_guid_builder = new System.Text.StringBuilder();
            foreach (var @object in objects)
                concatenated_guid_builder.Append(GetObjectGUID(@object));
            var concatenated_guid = concatenated_guid_builder.ToString();
            var hash_string = GetSha256String(concatenated_guid);
            return hash_string.Substring(0, 16);
        }

        private static string GetManagedBindingName(params UnityObject[] parameters)
        {
            if (parameters.Any(p => p != null))
                return MultiHashObjectUUID(parameters);
            return "Blank";
        }

        public static GameObject CreateManagedBindingAsset(
            string name,
            SpriteMesh mesh,
            Material material)
        {
            var game_object = new GameObject(name);
            var binding = game_object.AddComponent<SpriteMeshBinding>();
            binding.Mesh = mesh;
            // TODO
            //binding.Material = material;
            var path =
                CreateManagedBindingAssetFolder() +
                "/" + name + ".prefab";
            var binding_asset = AssetDatabase.LoadAssetAtPath<GameObject>(path) ??
                                PrefabUtility.CreatePrefab(path, game_object);
            DestroyImmediate(game_object);
            return binding_asset;
        }

        public static GameObject CreateManagedBindingAsset2(
            SpriteMesh mesh)
        {
            var name = GetManagedBindingName(mesh);
            var folder = CreateManagedBindingAssetFolder();
            var path = folder + "/" + name + ".prefab";
            var binding_asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            // If the asset already exists with the same name, return it
            if (binding_asset)
                return binding_asset;
            // Otherwise create a new asset by copying the blank prefab and
            // setting its properties
            var empty_path = folder + "/Blank.prefab";
            // Unity's documentation is useless
            // What does the returned value of AssetDatabase.CopyAsset even mean?
            AssetDatabase.CopyAsset(empty_path, path);
            // Try loading the copied asset
            binding_asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (binding_asset == null)
                throw new Exception("Could not copy empty SpriteMeshBinding prefab asset.");
            // Update the copy's properties
            var binding = binding_asset.GetComponent<SpriteMeshBinding>();
            binding.Mesh = mesh;
            // TODO
            //binding.Material = material;
            return binding_asset;
        }

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script", "_BindingObject");
            var binding_object_sp = serializedObject.FindProperty("_BindingObject");

            var self = (SpriteMeshSelector)target;
            try
            {
                var binding_object =
                    binding_object_sp.objectReferenceValue as GameObject;
                if (binding_object == null)
                {
                    binding_object = CreateManagedBindingAsset2(
                        null);
                    binding_object_sp.objectReferenceValue = binding_object;
                }

                var binding = binding_object.GetComponent<SpriteMeshBinding>();

                if (binding == null)
                    throw new ArgumentException(
                        "Binding object set but is missing " +
                        "SpriteMeshSelector component.");

                EditorGUI.BeginChangeCheck();

                var mesh = (SpriteMesh)EditorGUILayout.ObjectField("Sprite Mesh", binding.Mesh, typeof(SpriteMesh), false);
                // TODO
                //var material = (Material)EditorGUILayout.ObjectField("Material", binding.Material, typeof(Material), false);

                bool changes = EditorGUI.EndChangeCheck();

                if (changes)
                {
                    binding_object = CreateManagedBindingAsset2(
                        mesh);
                    binding_object_sp.objectReferenceValue = binding_object;
                }

                using (new EditorGUI.DisabledGroupScope(true))
                    EditorGUILayout.PropertyField(binding_object_sp);
            }
            catch (Exception ex)
            {
                EditorGUILayout.PropertyField(binding_object_sp);
                EditorGUILayout.HelpBox(ex.Message, MessageType.Error);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}