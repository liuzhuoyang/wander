using RTSDemo.Zone;
using SimpleVFXSystem;
using UnityEngine;

public class VFXZone : VFXMono
{
    private BuffZone zone;
    private ParticleSystem zoneParticle;

    protected override void VFXBegin()
    {
        zoneParticle = GetComponent<ParticleSystem>();
        var main = zoneParticle.main;
        main.stopAction = ParticleSystemStopAction.Callback;
        zoneParticle.Play();

        if (controlObjects != null && controlObjects.Length > 0)
        {
            zone = controlObjects[0].GetComponent<BuffZone>();
        }
    }
    protected override void VFXUpdate()
    {
        if (zone == null)
        {
            zoneParticle.Stop();
        }
    }
    //Unity Function, called when particle stop playing
    void OnParticleSystemStopped()
    {
        VFXEnd();
    }
}