using UnityEngine;
using System.Linq;

namespace Anima2D
{
    public class IkLimb2DPlus : IkLimb2D
    {
        // Requires hacking A2D (just make it virtual!)
        public override void UpdateIK()
        {
            OnIkUpdate();
            solver.Update();

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