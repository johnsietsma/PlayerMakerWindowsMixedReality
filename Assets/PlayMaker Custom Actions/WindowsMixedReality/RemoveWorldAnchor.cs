using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("HoloLens")]
    [Tooltip("Remove a world anchor")]
    public class RemoveWorldAnchor : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject to remove the anchor from")]
        public FsmOwnerDefault worldAnchorObject;

        [Tooltip("Leave blank if the anchor name is the same as the component name")]
        public FsmString worldAnchorName;

        public FsmBool removeFromWorldAnchorStore = true;

        public override void OnEnter()
        {
            GameObject gameObject = Fsm.GetOwnerDefaultTarget(worldAnchorObject);

            // Check to see if there already is a WorldAnchor Component attached
            WorldAnchor worldAnchor = gameObject.GetComponent<WorldAnchor>(); 
            if (worldAnchor==null)
            {
                Debug.LogWarning("WorldAnchor component is not found on GameObject: " + gameObject.name);
                Finish();
                return;
            }

            string anchorName = string.IsNullOrEmpty(worldAnchorName.Value) ? worldAnchor.name : worldAnchorName.Value; ;

            if (removeFromWorldAnchorStore.Value)
            {
                WorldAnchorStore.GetAsync(
                    (WorldAnchorStore worldAnchorStore)=>RemoveAnchor(worldAnchorStore, anchorName) );
            }

            GameObject.Destroy(worldAnchor);

            Finish();
        }

        public override void OnExit()
        {
            //TODO: Check for outstanding async ops and hooked events.
        }

        public override void Reset()
        {
            worldAnchorObject = null;
            worldAnchorName = "";
            removeFromWorldAnchorStore = true;
        }

        private void RemoveAnchor(WorldAnchorStore worldAnchorStore, string worldAnchorName)
        {
            if( !worldAnchorStore.Delete(worldAnchorName) )
            {
                Debug.Log("Couldn't remove world anchor: " + worldAnchorName);
            }
        }
    }
}

