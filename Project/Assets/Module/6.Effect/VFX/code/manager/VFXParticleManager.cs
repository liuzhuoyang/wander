using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于管理world space下一次性播放的各类particles，
/// 避免频繁创建重复的particles
/// </summary>
public class VFXParticleManager : MonoBehaviour
{
    public Dictionary<string, ParticleSystem> particleDict;
    private ParticleSystem.EmitParams tempParams;
    void Awake()
    {
        tempParams = new ParticleSystem.EmitParams();
        particleDict = new Dictionary<string, ParticleSystem>();
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void OnDestroy()
    {
        EventManager.StopListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    public GameObject GetParticle(string vfxName)
    {
        if (particleDict.ContainsKey(vfxName))
            return particleDict[vfxName].gameObject;
        else
        {
            var go = Instantiate(GameAssetGenericManager.Instance.GetVFXPrefab(vfxName), transform);
            particleDict.Add(vfxName, go.GetComponent<ParticleSystem>());
            return go;
        }
    }
    public GameObject PlayParticle(string vfxName, Vector2 pos, float scaleMultiplier, float angle)
    {
        ParticleSystem go;
        if (particleDict.ContainsKey(vfxName))
            go = particleDict[vfxName];
        else
        {
            go = Instantiate(GameAssetGenericManager.Instance.GetVFXPrefab(vfxName), transform).GetComponent<ParticleSystem>();
            particleDict.Add(vfxName, go);
        }
        go.name = vfxName;
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(0, 0, angle);
        tempParams.startSize = scaleMultiplier;
        tempParams.applyShapeToPosition = go.shape.enabled;
        tempParams.rotation = angle;

        go.Emit(tempParams, 1);

        return go.gameObject;
    }
    void OnAction(ActionArgs args)
    {
        if (args.action == ActionType.ArenaEnd
         || args.action == ActionType.TowerEnd
         || args.action == ActionType.DungeonEnd
         || args.action == ActionType.LevelEnd)
        {
            CleanUpParticles();
        }
    }
    void CleanUpParticles()
    {
        foreach (var particle in particleDict.Values)
        {
            Destroy(particle.gameObject);
        }
        particleDict.Clear();
    }
}
