using System;
using UnityEngine;
using Joss.Helpers;
using System.Collections.Generic;

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

    [Range(0,7)]
    public int sidesBand;
    [Range(0, 7)]
    public int scaleXBand;
    [Range(0, 7)]
    public int scaleYBand;
    [Range(0, 7)]
    public int offsetXBand;
    [Range(0, 7)]
    public int offsetYBand;

    private List<float> sidesTrack = new List<float>();
    private List<float> offXTrack = new List<float>();
    private List<float> offYTrack = new List<float>();

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
        //sidesTrack.Add(sides);
        float maxSides = AdaptScaleMax(sides, sidesTrack, 10);

        sides = Toolbox.Remap(sides, 0f, maxSides, 4f, 9f);
        Debug.Log("Now sides is " + sides);
        sides = (float)Math.Round(sides);


        _quadShader.material.SetFloat("_Sidez", sides);

        int random = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int random2 = UnityEngine.Random.Range(0, 2) * 2 - 1;

        float offsetX = AudioPeer._audioBandBuffer[offsetXBand];
        float offsetY = AudioPeer._audioBandBuffer[offsetYBand];
        float maxOffsetX = AdaptScaleMax(offsetX, offXTrack, 10);
        float maxOffsetY = AdaptScaleMax(offsetY, offYTrack, 10);
        offsetX = Toolbox.Remap(AudioPeer._audioBandBuffer[offsetXBand], 0f, maxOffsetX, 0f, 0.2f);
        offsetY = Toolbox.Remap(AudioPeer._audioBandBuffer[offsetYBand], 0f, maxOffsetY, 0f, 0.2f);

        Vector2 offsetXY = new Vector2(offsetX * random, offsetY * random2);
        _quadShader.material.SetVector("_OffsetSmall", offsetXY);
    }

    float AdaptScaleMax(float f, List<float> l, int t) {
        //change the scale of a lerp function based on the avg of the last t number of calls
        l.Add(f);
        if (l.Count > t)
            l.RemoveAt(0);
        float total = 0;
        foreach(float item in l)
        {
            total += item;
        }
        total = total / l.Count;
        float max = total * 2;
        return max;
    }
}
