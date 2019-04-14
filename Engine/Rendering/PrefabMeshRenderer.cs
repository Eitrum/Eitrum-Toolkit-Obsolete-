using Eitrum.Engine.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Rendering {
    public class PrefabMeshRenderer : EiComponent {

        #region Variables

        [SerializeField] private GameObject renderTarget;

        private RenderingBlock[] renderingBlocks;

        #endregion

        #region Properties

        public GameObject RenderTarget {
            get {
                return renderTarget;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake() {
            Assign(renderTarget);
        }

        private void OnEnable() {
            SubscribeLateUpdate();
        }

        private void OnDisable() {
            UnsubscribeLateUpdate();
        }

        #endregion

        #region Assign

        public void Assign(GameObject target) {
            this.renderTarget = target;
            var renderers = target.GetComponentsInChildren<Renderer>();
            renderingBlocks = new RenderingBlock[renderers.Length];
            for (int i = 0, length = renderers.Length; i < length; i++) {
                renderingBlocks[i] = RenderingBlock.CreateInstance(renderers[i]);
            }
        }

        #endregion

        #region Rendering

        public override void LateUpdateComponent(float time) {
            Render(transform);
        }

        public void Render() {
            for (int i = renderingBlocks.Length - 1; i >= 0; i--) {
                renderingBlocks[i].Render();
            }
        }

        public void Render(Transform transform) {
            for (int i = renderingBlocks.Length - 1; i >= 0; i--) {
                renderingBlocks[i].Render(transform.localToWorldMatrix);
            }
        }

        #endregion
    }
}