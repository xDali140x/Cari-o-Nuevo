using UnityEngine;

public class Vela : MonoBehaviour
{
    [Header("Triggers")]
    public Collider sourceTrigger;      // Trigger del fuego origen
    public Collider targetTrigger;      // Trigger de esta vela

    [Header("Particulas")]
    public ParticleSystem targetParticles;   // Fuego de la vela
    public ParticleSystem targetParticles2;

    [Header("Tiempo de contacto")]
    public float requiredContactTime = 2f;

    [Header("Configuracion")]
    public bool lightOnlyOnce = true;
    public bool isActivated = false;

    private float contactTimer = 0f;

    private void Start()
    {
        if (targetParticles != null)
        {
            targetParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (targetParticles2 != null)
        {
            targetParticles2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void Update()
    {
        if (lightOnlyOnce && isActivated)
            return;

        if (sourceTrigger == null || targetTrigger == null || targetParticles == null)
            return;

        bool triggersTouching = sourceTrigger.enabled &&
                                targetTrigger.enabled &&
                                sourceTrigger.bounds.Intersects(targetTrigger.bounds);

        if (triggersTouching)
        {
            contactTimer += Time.deltaTime;

            if (contactTimer >= requiredContactTime)
            {
                ActivateTargetFire();
            }
        }
        else
        {
            contactTimer = 0f;
        }
    }

    public void ActivateTargetFire()
    {
        if (targetParticles != null && !isActivated)
        {
            targetParticles.Play();
            isActivated = true;
        }
        if (targetParticles2 != null && !isActivated)
        {
            targetParticles2.Play();
            isActivated = true;
        }
    }

    public void ResetActivation()
    {
        contactTimer = 0f;
        isActivated = false;

        if (targetParticles != null)
        {
            targetParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (targetParticles2 != null)
        {
            targetParticles2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}