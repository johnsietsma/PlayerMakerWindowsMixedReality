using HutongGames.PlayMaker;
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

        [ObjectType(typeof(SpatialMappingRenderer.RenderState))]
        public FsmEnum renderState;

		public FsmMaterial occlusionMaterial;

		public FsmMaterial visualMaterial;

        public FsmBool freezeUpdates;

        [ObjectType(typeof(SpatialMappingRenderer.LODType))]
        public FsmEnum levelOfDetail;

        public override void OnEnter()
        {
            GameObject gameObject = Fsm.GetOwnerDefaultTarget(worldSpatialMappingObject);
            var spatialMappingRenderer = gameObject.GetComponent<SpatialMappingRenderer>();
            if (spatialMappingRenderer == null)
            {
                spatialMappingRenderer = this.Owner.AddComponent<SpatialMappingRenderer>();
            }

            var renderStateValue = (SpatialMappingRenderer.RenderState)renderState.Value;
            spatialMappingRenderer.enabled = renderStateValue != SpatialMappingRenderer.RenderState.None;

            spatialMappingRenderer.renderState = renderStateValue;
            spatialMappingRenderer.occlusionMaterial = occlusionMaterial.Value;
            spatialMappingRenderer.visualMaterial = visualMaterial.Value;
            spatialMappingRenderer.freezeUpdates = freezeUpdates.Value;
            spatialMappingRenderer.lodType = (SpatialMappingBase.LODType)levelOfDetail.Value;
            Finish();
        }

        public override void Reset()
        {
            renderState.Value = SpatialMappingRenderer.RenderState.None;
            occlusionMaterial = null;
            visualMaterial = null;
            freezeUpdates = false;
        }
    }
}

