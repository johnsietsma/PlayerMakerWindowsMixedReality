// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Ammended by John Sietsma

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Sets the up vector of a Game Object.")]
	public class SetUpVector : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that will have its up vector set.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Use a stored Vector3 up vector, and/or set individual axis below.")]
		public FsmVector3 vector;
		
		public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;	

		public override void Reset()
		{
			gameObject = null;
			vector = null;
			// default axis to variable dropdown with None selected.
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			everyFrame = false;
			lateUpdate = false;
		}

		public override void OnEnter()
		{
			if (!everyFrame && !lateUpdate)
			{
				DoSetForward();
				Finish();
			}		
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
                DoSetForward();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
                DoSetForward();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}

		void DoSetForward()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			// init normal
			
			Vector3 upVector;

			if (vector.IsNone)
			{
                upVector = go.transform.up;
			}
			else
			{
				upVector = vector.Value;
			}
			
			// override any axis

			if (!x.IsNone) upVector.x = x.Value;
			if (!y.IsNone) upVector.y = y.Value;
			if (!z.IsNone) upVector.z = z.Value;

			// apply
			go.transform.up = upVector;
		}


	}
}