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

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace RoaringFangs.Animation
{
    public class SpriteMeshBinding :
        MonoBehaviour,
        ISerializationCallbackReceiver
    {
        [SerializeField]
        private SpriteMesh _Mesh;

        public SpriteMesh Mesh
        {
            get { return _Mesh; }
            set { _Mesh = value; }
        }

        // TODO
        /*
        [SerializeField]
        private Material _Material;

        public Material Material
        {
            get { return _Material; }
            set { _Material = value; }
        }
        */

#if UNITY_EDITOR

        private static void AssignPrefabProperty<T>(
            ref T backing_field,
            T value)
            where T : UnityObject
        {
            if (backing_field != value)
            {
                if (value != null)
                {
                    value = (T)PrefabUtility.GetPrefabParent(value);
                    if (value == null)
                        throw new System.ArgumentNullException(
                            "Property requires a prefab parent but prefab " +
                            "is disconnected or missing.");
                }
                backing_field = value;
            }
        }

        // TODO
        /*
        public static IEnumerable<T> FindAssetsByType<T>()
            where T : UnityObject
        {
            var filter = string.Format("t:{0}", typeof(T));
            var guids = AssetDatabase.FindAssets(filter);
            foreach (var guid in guids)
            {
                var asset_path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(asset_path);
                if (asset != null)
                    yield return asset;
            }
        }

        public static IEnumerable<string> FindAssetPathsByType(Type type)
        {
            var filter = string.Format("t:{0}", type);
            return AssetDatabase.FindAssets(filter)
                .Select(AssetDatabase.GUIDToAssetPath);
        }

        private static readonly Type[] DependentTypes =
        {
            typeof(SpriteMeshSelector)
        };

        private IEnumerable<T> GetRequiredAssets<T>(
            IEnumerable<Type> dependent_types) where T : UnityObject
        {
            // Find all the dependent assets of the matching types
            // and then get their dependencies
            return dependent_types
                .SelectMany(FindAssetPathsByType)
                .SelectMany(AssetDatabase.GetDependencies)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(a => a != null);
        }
        */

#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            AssignPrefabProperty(ref _Mesh, Mesh);

            // TODO
            /*
            AssignPrefabProperty(ref _Material, Material);

            var this_path = AssetDatabase.GetAssetPath(this);
            UUID = AssetDatabase.GetAssetDependencyHash(this_path).ToString();

            var required_bindings =
                GetRequiredAssets<SpriteMeshBinding>(DependentTypes)
                    .ToArray();
            var all_bindings =
                FindAssetsByType<SpriteMeshBinding>()
                    .ToArray();
            var orphaned_bindings =
                all_bindings.Except(required_bindings)
                    .ToArray();
            var number_of_orphaned_bindings = orphaned_bindings.Count();
            if (number_of_orphaned_bindings > 0)
            {
                Debug.LogWarning(
                    "Orphaned bindings: " +
                    number_of_orphaned_bindings);
            }
            */
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}