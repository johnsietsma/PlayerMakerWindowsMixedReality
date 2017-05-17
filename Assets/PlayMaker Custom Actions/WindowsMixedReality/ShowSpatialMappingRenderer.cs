using UnityEngine;
using UnityEngine.VR.WSA;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("HoloLens")]
    [Tooltip("Show or hide the Spatial Mapping Renderer")]
    public class ShowSpatialMappingRenderer : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject that has or should have the Spatial Mapping Renderer")]
        public FsmOwnerDefault worldSpatialMappingObject;

        public FsmBool showOcclusion;
        public FsmBool showVisualisation;
		public FsmMaterial occlusionMaterial;
		public FsmMaterial visualMaterial;
        public FsmBool freezeUpdates;

        public override void OnEnter()
        {
            GameObject gameObject = Fsm.GetOwnerDefaultTarget(worldSpatialMappingObject);
            var spatialMappingRenderer = gameObject.GetComponent<SpatialMappingRenderer>();
            if (spatialMappingRenderer == null)
            {
                spatialMappingRenderer = this.Owner.AddComponent<SpatialMappingRenderer>();
            }

            spatialMappingRenderer.enabled = showOcclusion.Value | showVisualisation.Value;

            if( spatialMappingRenderer.enabled ) {
                if( showOcclusion.Value ) {
                    spatialMappingRenderer.renderState = SpatialMappingRenderer.RenderState.Occlusion;
                }
                else if( showVisualisation.Value ) {
                    spatialMappingRenderer.renderState = SpatialMappingRenderer.RenderState.Visualization;
                }

                spatialMappingRenderer.occlusionMaterial = occlusionMaterial.Value;
                spatialMappingRenderer.visualMaterial = visualMaterial.Value;
                spatialMappingRenderer.freezeUpdates = freezeUpdates.Value;
            }

            Finish();
        }

        public override void Reset()
        {
            showOcclusion = true;
            showVisualisation = false;
            occlusionMaterial = null;
            visualMaterial = null;
            freezeUpdates = false;
        }
    }
}

