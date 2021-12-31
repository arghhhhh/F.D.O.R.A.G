using System;
using UnityEngine;


public class ShapeChanger : MonoBehaviour
{
    Renderer _quadShader;
    private float updateInterval = 0.01f;
    public float songBPM;
    private float beatInterval;
    private float sides = 0;
    private float rotation;
    public float rotationAmt;
    private float timer1;
    private float timer2;

    [Range(0,63)]
    public int sidesBand;
    [Range(0, 63)]
    public int scaleXBand;
    [Range(0, 63)]
    public int scaleYBand;
    [Range(0, 63)]
    public int offsetXBand;
    [Range(0, 63)]
    public int offsetYBand;

    void Start()
    {
        _quadShader = GetComponent<Renderer>();
        beatInterval = 60f / songBPM;
    }

    void Update()
    {
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (timer1 >= beatInterval)
        {
            UpdateEveryBeat();
        }
        
        if (timer2 >= updateInterval)
        {
            _quadShader.material.SetFloat("_Rotation", rotation);
            timer2 -= updateInterval;
            rotation += rotationAmt;
            if (Mathf.Abs(rotation) >= 360f)
                rotation = rotationAmt;
        }
        if (!float.IsNaN(AudioPeer._audioBandBuffer[scaleXBand]) || !float.IsNaN(AudioPeer._audioBandBuffer[scaleYBand]))
            transform.localScale = Vector3.one + new Vector3(AudioPeer._audioBandBuffer[scaleXBand], AudioPeer._audioBandBuffer[scaleYBand], 0);
    }

    void UpdateEveryBeat()
    {
        timer1 -= beatInterval;
        sides = AudioPeer._audioBandBuffer[sidesBand];
        sides = Mathf.Lerp(4f, 9f, sides);
        sides = (float)Math.Round(sides);


        _quadShader.material.SetFloat("_Sidez", sides);

        int random = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int random2 = UnityEngine.Random.Range(0, 2) * 2 - 1;
        float offsetX = Mathf.Lerp(0, 0.25f, AudioPeer._audioBandBuffer[offsetXBand]);
        float offsetY = Mathf.Lerp(0, 0.25f, AudioPeer._audioBandBuffer[offsetYBand]);
        Vector2 offsetXY = new Vector2(offsetX * random, offsetY * random2);
        _quadShader.material.SetVector("_OffsetSmall", offsetXY);
    }
}
