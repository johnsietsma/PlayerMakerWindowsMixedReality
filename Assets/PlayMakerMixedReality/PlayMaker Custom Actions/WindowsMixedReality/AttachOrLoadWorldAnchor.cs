using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("HoloLens")]
    [Tooltip("Add or load a world anchor to the World Anchor Store")]
    public class AttachorLoadWorldAnchor : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject to attach the anchor to")]
        public FsmOwnerDefault worldAnchorObject;

        [RequiredField]
        public FsmString worldAnchorName;

        public FsmBool saveToWorldAnchorStore = true;

        public override void OnEnter()
        {
            GameObject gameObject = Fsm.GetOwnerDefaultTarget(worldAnchorObject);
            string anchorName = worldAnchorName.Value;

            // Check to see if there already is a WorldAnchor Component attached
            WorldAnchor worldAnchor = gameObject.GetComponent<WorldAnchor>(); 
            if (worldAnchor==null)
            {
                // Add the WorldAnchor Component
                worldAnchor = gameObject.AddComponent<WorldAnchor>();
            }

            // Change the WorldAnchor Component's name to match the name of the anchor
            worldAnchor.name = anchorName;

            // Store the anchor
            if (saveToWorldAnchorStore.Value)
            {
                WorldAnchorStore.GetAsync(
                    (WorldAnchorStore worldAnchorStore)=>AddAnchor(worldAnchorStore, worldAnchor) );
            }

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
            saveToWorldAnchorStore = true;
        }

        private void AddAnchor(WorldAnchorStore worldAnchorStore, WorldAnchor worldAnchor)
        {
            GameObject gameObject = Fsm.GetOwnerDefaultTarget(worldAnchorObject);

            // Check to see if it already exists
            WorldAnchor savedAnchor = worldAnchorStore.Load(worldAnchor.name, gameObject);
            if(savedAnchor!=null)
            {
                savedAnchor.name = worldAnchor.name;
            }
            else
            {
                if (worldAnchor.isLocated)
                {
                    SaveAnchor(worldAnchorStore, worldAnchor);
                }
                else
                {
                    // TODO: Unregister anon event?
                    worldAnchor.OnTrackingChanged += (WorldAnchor wa, bool located) => SaveAnchor(worldAnchorStore, wa);
                }
            }
        }

        private void SaveAnchor(WorldAnchorStore worldAnchorStore, WorldAnchor worldAnchor)
        {
            if( !worldAnchorStore.Save(worldAnchor.name, worldAnchor) )
            {
                Debug.Log("Failed to save world anchor: " + worldAnchor.name);
            }
        }
    }
}

