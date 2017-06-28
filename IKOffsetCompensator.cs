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

using UnityEngine;

namespace RoaringFangs.Animation
{
    /// <summary>
    /// Offsets an IK controller transform by a compensation vector.
    /// The compensation vector shares the bone transform's orientation.
    /// Sort of like an "omnidirectional holdover"
    /// </summary>
    [ExecuteInEditMode]
    public class IKOffsetCompensator : MonoBehaviour
    {
        /// <summary>
        /// The reference bone transform by which to
        /// orient the compensation vector
        /// </summary>
        public Transform BoneTransform;
        /// <summary>
        /// The IK controller transform, usually parented to
        /// this component's GameObject's transform
        /// </summary>
        public Transform IKTransform;

        /// <summary>
        /// Compensation vector, oriented by BoneTransform,
        /// to apply to IKTransform
        /// </summary>
        public Vector3 Compensation;

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                UnityEditor.EditorApplication.update += Update;
#endif
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                UnityEditor.EditorApplication.update -= Update;
#endif
        }

        private void Update()
        {
            if (BoneTransform == null || IKTransform == null)
                return;
            // Effective compensation vector calculated by transforming
            // Compensation vector by BoneTransform
            var compensation = BoneTransform.TransformVector(Compensation);
            // Set the local position of the IK transform since its
            // "handle" is its parent transform
            IKTransform.localPosition = compensation;
        }
    }
}