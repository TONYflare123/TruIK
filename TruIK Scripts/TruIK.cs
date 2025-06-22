using FlaxEngine;

namespace Game
{
    /// <summary>
    /// FootIK Script. Can be used to make a character's feet move with uneven ground.
    /// </summary>
    public class TruIK : Script
    {
		[Serialize, ShowInEditor, EditorOrder(0), EditorDisplay(name: "Left Foot IK")]
        private BoneSocket leftFootIK;
        [Serialize, ShowInEditor, EditorOrder(1), EditorDisplay(name: "Right Foot IK")]
        private BoneSocket rightFootIK;
        [Serialize, ShowInEditor, EditorOrder(2), EditorDisplay(name: "Collision Mask")]
        private LayersMask collisionMask;
        [Header("Fine tuning")]
        [Serialize, ShowInEditor, EditorOrder(2), EditorDisplay(name: "Step Height Offset")]
        private float stepOffset = 10;
        [Serialize, ShowInEditor, EditorOrder(2), EditorDisplay(name: "Rotation Offset")]
        private float rotOffset = 15;

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
            if (Physics.RayCast(new Vector3(leftFootIK.Position.X, Actor.Position.Y + 100, leftFootIK.Position.Z), Vector3.Down, out RayCastHit leftFootHit, 500, collisionMask))
            {
                // Get a forward-facing direction relative to the ground noraml
                Vector3 aimDirection = Vector3.Cross(leftFootHit.Normal, Transform.Left);
                // Create a rotation from the direction
                Quaternion footRot = Quaternion.LookRotation(aimDirection, leftFootHit.Normal);

                //currentLeftIK = Mathf.Lerp(currentLeftIK, leftFootHit.Point.Y - Actor.Position.Y, Time.DeltaTime * 20);

                _leftFootRot.Value = Quaternion.Euler(footRot.EulerAngles.X-rotOffset, 0, -footRot.EulerAngles.Z);
                _leftFootIK.Value = leftFootHit.Point.Y-Actor.Position.Y+stepOffset;
                DebugDraw.DrawLine(leftFootHit.Point,leftFootHit.Point+new Vector3(0,50,0),Color.Red,0,false);
            }
            else
            {
                //currentLeftIK = Mathf.Lerp(currentLeftIK, 0, Time.DeltaTime);

                _leftFootRot.Value = Quaternion.Euler(0, 0, 0);
                _leftFootIK.Value = 0;
            }

            if (Physics.RayCast(new Vector3(rightFootIK.Position.X, Actor.Position.Y + 100, rightFootIK.Position.Z), Vector3.Down, out RayCastHit rightFootHit, 500, collisionMask))
            {
                // Get a forward-facing direction relative to the ground noraml
                Vector3 aimDirection = Vector3.Cross(rightFootHit.Normal, Transform.Left);
                // Create a rotation from the direction
                Quaternion footRot = Quaternion.LookRotation(aimDirection, rightFootHit.Normal);

                //currentRightIK = Mathf.Lerp(currentRightIK, rightFootHit.Point.Y - Actor.Position.Y, Time.DeltaTime * 20);

                _rightFootRot.Value = Quaternion.Euler(footRot.EulerAngles.X-rotOffset, 0, -footRot.EulerAngles.Z);
                _rightFootIK.Value = rightFootHit.Point.Y-Actor.Position.Y+stepOffset;
                DebugDraw.DrawLine(rightFootHit.Point,rightFootHit.Point+new Vector3(0,50,0),Color.Red,0,false);
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
