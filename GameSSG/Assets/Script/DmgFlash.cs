using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private Coroutine _dmgFlashCoroutine;
    void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Init();
    }
    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        for(int i =0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }
    public void CallDmgFlash()
    {
        _dmgFlashCoroutine = StartCoroutine(DmgFlasher());
    }
    private IEnumerator DmgFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(0.5f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }
    private void SetFlashColor()
    {
        for(int i = 0; i < _materials.Length;i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }
    private void SetFlashAmount(float amount)
    {
        for(int i = 0; i < _materials.Length;i++)
        {
            _materials[i].SetFloat("_ColorRange", amount);
        }
    }
    
   
}
