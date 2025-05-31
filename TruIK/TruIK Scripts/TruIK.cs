using FlaxEngine;

namespace Game
{
    /// <summary>
    /// FootIK Script. Can be used to make a character's feet move with uneven ground.
    /// </summary>
    public class IK : Script
    {
		[Serialize, ShowInEditor, EditorOrder(0), EditorDisplay(name: "Left Foot IK")]
        private BoneSocket leftFootIK;
        [Serialize, ShowInEditor, EditorOrder(1), EditorDisplay(name: "Right Foot IK")]
        private BoneSocket rightFootIK;
        [Serialize, ShowInEditor, EditorOrder(2), EditorDisplay(name: "Collision Mask")]
        private LayersMask collisionMask;

        private AnimGraphParameter _leftFootRot;
        private AnimGraphParameter _rightFootRot;
        private AnimGraphParameter _leftFootIK;
        private AnimGraphParameter _rightFootIK;

        public override void OnStart()
        {
            // Cache parameters
            _leftFootRot = Actor.As<AnimatedModel>().GetParameter("LeftFootRot");
            _rightFootRot = Actor.As<AnimatedModel>().GetParameter("RightFootRot");
            _leftFootIK = Actor.As<AnimatedModel>().GetParameter("LeftFootIK");
            _rightFootIK = Actor.As<AnimatedModel>().GetParameter("RightFootIK");
        }

        public override void OnFixedUpdate()
        {
            if (Physics.RayCast(new Vector3(leftFootIK.Position.X, Actor.Position.Y + 10, leftFootIK.Position.Z), Vector3.Down, out RayCastHit leftFootHit, 50, collisionMask))
            {
                // Get a forward-facing direction relative to the ground noraml
                Vector3 aimDirection = Vector3.Cross(leftFootHit.Normal, Transform.Left);
                // Create a rotation from the direction
                Quaternion footRot = Quaternion.LookRotation(aimDirection, leftFootHit.Normal);

                //currentLeftIK = Mathf.Lerp(currentLeftIK, leftFootHit.Point.Y - Actor.Position.Y, Time.DeltaTime * 20);

                _leftFootRot.Value = Quaternion.Euler(footRot.EulerAngles.X, 0, footRot.EulerAngles.Z);
                _leftFootIK.Value = leftFootHit.Point.Y-Actor.Position.Y+10;
                DebugDraw.DrawSphere(new BoundingSphere(leftFootHit.Point, 10), Color.Red);
            }
            else
            {
                //currentLeftIK = Mathf.Lerp(currentLeftIK, 0, Time.DeltaTime);

                _leftFootRot.Value = Quaternion.Euler(0, 0, 0);
                _leftFootIK.Value = 0;
            }

            if (Physics.RayCast(new Vector3(rightFootIK.Position.X, Actor.Position.Y + 10, rightFootIK.Position.Z), Vector3.Down, out RayCastHit rightFootHit, 50, collisionMask))
            {
                // Get a forward-facing direction relative to the ground noraml
                Vector3 aimDirection = Vector3.Cross(rightFootHit.Normal, Transform.Left);
                // Create a rotation from the direction
                Quaternion footRot = Quaternion.LookRotation(aimDirection, rightFootHit.Normal);

                //currentRightIK = Mathf.Lerp(currentRightIK, rightFootHit.Point.Y - Actor.Position.Y, Time.DeltaTime * 20);

                _rightFootRot.Value = Quaternion.Euler(footRot.EulerAngles.X, 0, footRot.EulerAngles.Z);
                _rightFootIK.Value = rightFootHit.Point.Y-Actor.Position.Y+10;
                DebugDraw.DrawSphere(new BoundingSphere(rightFootHit.Point, 10), Color.Red);
            }
            else
            {
                //currentRightIK = Mathf.Lerp(currentRightIK, 0, Time.DeltaTime * 10);

                _rightFootRot.Value = Quaternion.Euler(0, 0, 0);
                _rightFootIK.Value = 0;
            }
        }
    }
}
