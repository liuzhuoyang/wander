using System.Collections.Generic;
using UnityEngine;

namespace SimpleVFXSystem
{
    /// <summary>
    /// 用于管理world space下一次性播放的各类particles，
    /// </summary>
    public class VFXParticleManager : MonoBehaviour
    {
        private Dictionary<string, ParticleSystem> particleDict;
        private ParticleSystem.EmitParams tempParams = new ParticleSystem.EmitParams();
        private Transform root;

        void Awake()
        {
            particleDict = new Dictionary<string, ParticleSystem>();
        }
        internal GameObject PlayParticle(string vfxName, Vector2 pos, float scaleMultiplier, float angle)
        {
            if (root == null)
                root = new GameObject("[VFXParticleRoot]").transform;

            ParticleSystem go;
            if (particleDict.ContainsKey(vfxName))
                go = particleDict[vfxName];
            else
            {
                go = Instantiate(VFXManager.Instance.GetVFXPrefab(vfxName), root).GetComponent<ParticleSystem>();
                particleDict.Add(vfxName, go);
            }
            go.name = vfxName;
            go.transform.position = pos;

            tempParams.startSize = scaleMultiplier;
            tempParams.applyShapeToPosition = go.shape.enabled;
            tempParams.rotation = angle;

            go.Emit(tempParams, 1);

            return go.gameObject;
        }
        internal void CleanUpParticles()
        {
            foreach (var particle in particleDict.Values)
            {
                Destroy(particle.gameObject);
            }
            particleDict.Clear();
        }
    }
}