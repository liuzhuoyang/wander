using UnityEngine;

public class VFXShield : MonoBehaviour
{
    [SerializeField] private ParticleSystem vfx_shieldHit;
    [SerializeField] private ParticleSystem vfx_shield_on;
    [SerializeField] private ParticleSystem vfx_shield_break;

    public bool IsActive{get; private set;}
    private const string SHIELD_ON = "vfx_animation_battle_shield_on";
    private const string SHIELD_OFF = "vfx_animation_battle_shield_off";

    public void OnShieldHit()
    {
        Vector3 hitPoint = transform.position + (Vector3)GeometryUtil.RandomPointInCircle(Vector2.zero, 0.5f, 1.4f);
        OnShieldHit(hitPoint);
    }
    public void OnShieldHit(Vector3 worldHitPoint)
    {
    //播放受攻击particles
        Vector3 localHitpoint = transform.InverseTransformPoint(worldHitPoint);
        var emitParam = new ParticleSystem.EmitParams();
        emitParam.position = localHitpoint;
        vfx_shieldHit.Emit(emitParam, Random.Range(3, 6));
    }
    public void OnShieldBreak()
    {
        IsActive = false;
        vfx_shield_break.Play();
    }
    public void OnShieldBuilt()
    {
        IsActive = true;
        vfx_shield_on.Play();
    }
    public void DeactivateShield()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
    public void ActivateShield()
    {
        gameObject.SetActive(true);
    }
}