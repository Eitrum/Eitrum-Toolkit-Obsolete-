using System;
using UnityEngine;

namespace Eitrum.Rendering {
    [Serializable]
    public struct RenderingBlock {

        #region Variables

        public Mesh mesh;
        public Material material;
        public Matrix4x4 matrix;

        private static Matrix4x4[] instanceRenderering = new Matrix4x4[8];

        #endregion

        #region Constructor

        public RenderingBlock(Mesh mesh) {
            this.mesh = mesh;
            this.material = null;
            this.matrix = Matrix4x4.identity;
        }

        public RenderingBlock(Mesh mesh, Material material) {
            this.mesh = mesh;
            this.material = material;
            this.matrix = Matrix4x4.identity;
        }

        public RenderingBlock(Mesh mesh, Material material, Matrix4x4 matrix) {
            this.mesh = mesh;
            this.material = material;
            this.matrix = matrix;
        }

        #endregion

        #region Static Create

        public static RenderingBlock CreateInstance(GameObject target) {
            var rend = target.GetComponentInChildren<Renderer>();
            if (rend == null)
                return default(RenderingBlock);
            if (rend is SkinnedMeshRenderer) {
                var smr = rend as SkinnedMeshRenderer;
                return new RenderingBlock(smr.sharedMesh, smr.sharedMaterial, smr.transform.localToWorldMatrix);
            }
            var mf = rend.GetComponent<MeshFilter>();
            if (mf == null)
                return default(RenderingBlock);

            return new RenderingBlock(mf.sharedMesh, (rend as MeshRenderer).sharedMaterial, rend.transform.localToWorldMatrix);
        }

        public static RenderingBlock CreateInstance(Renderer renderer) {
            if (renderer == null)
                return default(RenderingBlock);
            if (renderer is SkinnedMeshRenderer) {
                var smr = renderer as SkinnedMeshRenderer;
                return new RenderingBlock(smr.sharedMesh, smr.sharedMaterial, smr.transform.localToWorldMatrix);
            }
            var mf = renderer.GetComponent<MeshFilter>();
            if (mf == null)
                return default(RenderingBlock);

            return new RenderingBlock(mf.sharedMesh, (renderer as MeshRenderer).sharedMaterial, renderer.transform.localToWorldMatrix);
        }

        #endregion

        #region Render

        public void Render() {
            Graphics.DrawMesh(mesh, matrix, material, 1);
        }

        public void Render(Material overrideMaterial) {
            Graphics.DrawMesh(mesh, matrix, overrideMaterial, 1);
        }

        public void Render(Matrix4x4 matrix) {
            Graphics.DrawMesh(mesh, matrix * this.matrix, material, 1);
        }

        public void Render(Matrix4x4 matrix, Material overrideMaterial) {
            Graphics.DrawMesh(mesh, matrix * this.matrix, overrideMaterial, 1);
        }

        public void Render(Matrix4x4[] matrices) {
            var mLength = matrices.Length;
            if (instanceRenderering.Length < mLength) {
                var tLenght = instanceRenderering.Length;
                do {
                    tLenght *= 2;
                } while (tLenght < mLength);
                instanceRenderering = new Matrix4x4[tLenght];
            }
            for (int i = 0; i < mLength; i++) {
                instanceRenderering[i] = matrices[i] * this.matrix;
            }
            Graphics.DrawMeshInstanced(mesh, 0, material, matrices, mLength);
        }

        #endregion
    }
}
