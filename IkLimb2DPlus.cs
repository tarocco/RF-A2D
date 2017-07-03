using UnityEngine;
using System.Linq;

namespace Anima2D
{
    public class IkLimb2DPlus : IkLimb2D
    {
        struct XFormRotation
        {
            public Transform Transform;
            public Quaternion Rotation;

            public XFormRotation(
                Transform transform,
                Quaternion rotation)
            {
                Transform = transform;
                Rotation = rotation;
            }
        }

        public override void UpdateIK()
        {
            //// Store the original rotations
            //var xforms = solver.solverPoses
            //    .Select(p => new XFormRotation(
            //        p.bone.transform,
            //        p.bone.transform.rotation))
            //    .ToArray();

            // Solve the IK with full weight
            //float original_weight = weight;
            //weight = 1.0f;
            OnIkUpdate();
            solver.Update();
            //weight = original_weight;

            //// Mix in the original rotations
            //foreach (var xform in xforms)
            //{
            //    xform.Transform.rotation =
            //        Quaternion.Slerp(
            //            xform.Rotation,
            //            xform.Transform.rotation,
            //            weight);
            //}

            // Include IK child
            if (orientChild && target.child)
            {
                var dry_rotation = target.child.transform.rotation;
                target.child.transform.rotation =
                    Quaternion.Slerp(
                        dry_rotation,
                        transform.rotation,
                        weight);
            }
        }
    }
}