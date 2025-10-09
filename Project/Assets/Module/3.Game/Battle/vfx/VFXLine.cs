using UnityEngine;

public class VFXLine : BattleBehaviour, IVFX_Behavior
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private float widthMultiplier = 1;
    [SerializeField] private AnimationCurve widthCurve;
    [SerializeField] private float delayTime;

    private float lifeTimer = 0;
    private float lifeTime = 0;
    private bool activated = false;

    private const string PHASE_NAME = "_Phase";
    private const int SEGMENT = 15;
    
    public void OnCreateVFX(Vector2 start, Vector2 target, float lifeTime)
    {
        activated = true;
        lifeTimer = 0;
        this.lifeTime = lifeTime;
        transform.position = start;

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = SEGMENT;
        var mpb = new MaterialPropertyBlock();
        lineRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(PHASE_NAME, Random.Range(0, 1f));
        lineRenderer.SetPropertyBlock(mpb);

        for(int i=0; i<SEGMENT; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)start + (Vector3)(target-start) * i/(SEGMENT-1));
        }
        lineRenderer.textureScale = new Vector2(0.5f, 1);
        lineRenderer.widthMultiplier = widthMultiplier;

        //设置命中效果的延迟（若有效）
        if (hitParticles != null)
        {
            hitParticles.transform.position = target;
            var main = hitParticles.main;
            main.startDelay = delayTime;
            hitParticles.Play();
        }
        //BattleBehaviourManager.Instance.RegisterBehaviour(this);
    }
    public override void BattleUpdate()
    {
        if (activated)
        {
            lifeTimer += Time.deltaTime;
            lineRenderer.widthMultiplier = widthMultiplier * widthCurve.Evaluate(lifeTimer / lifeTime);
            if (lifeTimer / lifeTime >= 1)
            {
                activated = false;
            }
        }
    }
    public void OnCleanUp()
    {
        //BattleBehaviourManager.Instance.UnregisterBehaviour(this);
    }
}
