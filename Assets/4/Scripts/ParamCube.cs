using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _scaleMultiplier;
    public bool _useBuffer;
    Material _material;
    Color _baseColor;
    public float _minEmission;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
        _baseColor = _material.GetColor("_EmissionColor");
    }
    void FixedUpdate()
    {
        if (_useBuffer)
        {
            AudioPeer._audioBand[_band] = AudioPeer._audioBandBuffer[_band]; 
        }

        float bandValue = AudioPeer._audioBand[_band];
        if (float.IsNaN(bandValue))
        {
            bandValue = 0.1f;
        }

        transform.localScale = new Vector3(transform.localScale.x, (bandValue * _scaleMultiplier) + _startScale, transform.localScale.z);
        Color _color = new Color(bandValue + _minEmission, bandValue + _minEmission, bandValue + _minEmission);
        _material.SetColor("_EmissionColor", _color * _baseColor);
    }
}
