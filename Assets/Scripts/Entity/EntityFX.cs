using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private Material flashMat;
    [SerializeField] private float flashDuration;
    private SpriteRenderer entitySR;
    private Material srcMat;

    void Start()
    {
        entitySR = GetComponentInChildren<SpriteRenderer>();
        srcMat = entitySR.material;
    }

    public IEnumerator FlashFXCoroutine()
    {
        entitySR.material = flashMat;
        yield return new WaitForSeconds(flashDuration);
        entitySR.material = srcMat;
    }
    public void StartWhiteToYellowCoroutine()
    {
        StartCoroutine(WhiteToYellowCoroutine());
    }
    public IEnumerator WhiteToYellowCoroutine()
    {
        Debug.Log("STUN COROUTINE !!!");
        entitySR.color = new Color(0.66f, 0.66f, 0.66f);
        yield return new WaitForSeconds(.1f);
        entitySR.color = Color.white;
    }

    // ±ù¶³
    public IEnumerator FreezeFXCoroutine(float duration)
    {
        entitySR.color = new Color(0.5f, 0.9f, 0.9f);
        yield return new WaitForSeconds(duration);
        entitySR.color = Color.white;
    }
    // È¼ÉÕ
    public IEnumerator BurnFXCoroutine(float duration)
    {
        entitySR.color = new Color(0.9f, 0.5f, 0.5f);
        yield return new WaitForSeconds(duration);
        entitySR.color = Color.white;
    }
    // Âé±Ô
    public IEnumerator ShockFXCoroutine(float duration)
    {
        entitySR.color = new Color(0.9f, 0.9f, 0.6f);
        yield return new WaitForSeconds(duration);
        entitySR.color = Color.white;
    }
}
