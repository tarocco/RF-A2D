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

#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            AssignPrefabProperty(ref _Mesh, Mesh);
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
