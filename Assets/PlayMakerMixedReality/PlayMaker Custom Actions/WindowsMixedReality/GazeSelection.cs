using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("HoloLens")]
    [Tooltip("Use the gaze direction of the HoloLens to select GameObjects.")]
    public class GazeSelection : FsmStateAction
    {
        [Tooltip("The distance the ray will travel.")]
        public FsmFloat distance;

        [Tooltip("Store the GameObject selected by the Gaze in a variable.")]
        [UIHint(UIHint.Variable)]
        public FsmGameObject storeSelectedGazeObject;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the world position of the gaze on the GameObject in a variable.")]
        public FsmVector3 storeGazePoint;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the normal of the GameObject at the Gaze point in a variable.")]
        public FsmVector3 storeGazePointNormal;

        [Tooltip("Set how often to cast a ray. 0 = once, don't repeat; 1 = everyFrame; 2 = every other frame... \nSince raycasts can get expensive use the highest repeat interval you can get away with.")]
        public FsmInt repeatInterval;

        [UIHint(UIHint.Layer)]
        [Tooltip("Pick only from these layers.")]
        public FsmInt[] layerMask;

        [Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
        public FsmBool invertMask;

        [Tooltip("Event for when a GameObject is first gazed at.")]
        [UIHint(UIHint.Variable)]
        public FsmEvent gazeEnterEvent;

        [Tooltip("Event for when a GameObject being gazed at.")]
        [UIHint(UIHint.Variable)]
        public FsmEvent gazeStayEvent;

        [Tooltip("Event for when a GameObject is no longer being gazed at.")]
        [UIHint(UIHint.Variable)]
        public FsmEvent gazeExitEvent;

        int repeat;
        private GameObject selectedObject;

        public override void Reset()
        {
            distance = -1;
            storeSelectedGazeObject = null;
            storeGazePoint = new FsmVector3 { UseVariable = true };
            storeGazePointNormal = new FsmVector3 { UseVariable = true };
            repeatInterval = 1;
            layerMask = new FsmInt[0];
            invertMask = false;
            gazeEnterEvent = null;
            gazeStayEvent = null;
            gazeExitEvent = null;
        }

        public override void OnEnter()
        {
            DoRaycast();

            if (repeatInterval.Value <= 0)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            repeat--;

            if (repeat == 0)
            {
                DoRaycast();
            }
        }

        void DoRaycast()
        {
            if (distance.Value == 0)
            {
                Debug.LogWarning("Please give the GazeSelection a distance.");
                return;
            }

            repeat = repeatInterval.Value;

            Transform cameraTransform = Camera.main.transform;

            var rayPos = cameraTransform.position;
            var rayDirection = cameraTransform.forward;
            var rayLength = distance.Value < 0 ? Mathf.Infinity : distance.Value;
            GameObject newSelectedObject = null;

            RaycastHit hitInfo;
            if (Physics.Raycast(rayPos, rayDirection, out hitInfo, rayLength, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value)))
            {
                // We hit something, store the result
                newSelectedObject = hitInfo.collider.gameObject;
                storeGazePoint.Value = hitInfo.point;
                storeGazePointNormal.Value = hitInfo.normal;
            }
            else
            {
                // We didn't hit anything, wipe the variables
                storeGazePoint.Value = Vector3.zero;
                storeGazePointNormal.Value = Vector3.zero;
            }

            if(newSelectedObject != storeSelectedGazeObject.Value)
            {
                if( storeSelectedGazeObject.Value!=null && gazeExitEvent!=null )
                {
                    Fsm.Event(gazeExitEvent);
                    storeSelectedGazeObject.Value = null;
                }
                // Let the exit/enter events happen over two frames so the next state can hook the enter event.
                else if( newSelectedObject!=null && gazeEnterEvent!=null )
                {
                    Fsm.Event(gazeEnterEvent);
                    storeSelectedGazeObject.Value = newSelectedObject;
                }
                
            }
            else if( storeSelectedGazeObject.Value!=null && gazeStayEvent!=null )
            {
                Fsm.Event(gazeStayEvent);
            }
        }
    }
}

