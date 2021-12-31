using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    [HideInInspector]
    public static float[] _samplesLeft = new float[512];
    [HideInInspector]
    public static float[] _samplesRight = new float[512];

    private float[] _freqBand;
    private float[] _bandBuffer;
    private float[] _bufferDecrease;
    private float[] _freqBandHighest; // make this public to calculate audio profile for specific track
    [HideInInspector]
    public static float[] _audioBand, _audioBandBuffer;

    [HideInInspector]
    public static float _amplitude, _amplitudeBuffer;
    private float _amplitudeHighest;
    public float _audioProfile;

    public enum _channel { Stereo, Left, Right};
    public _channel channel = new _channel();

    public enum _size { Eight, SixtyFour};
    public _size size = new _size();
    private int bandCount;

    void Start()
    {
        if (size == _size.SixtyFour) bandCount = 64;
        else bandCount = 8;
        _freqBand = new float[bandCount];
        _bandBuffer = new float[bandCount];
        _bufferDecrease = new float[bandCount];
        _freqBandHighest = new float[bandCount];
        _audioBand = new float[bandCount];
        _audioBandBuffer = new float[bandCount];
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < bandCount; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < bandCount; i++)
        {
            currentAmplitude += _audioBand[i];
            currentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (currentAmplitude >= _amplitudeHighest)
        {
            _amplitudeHighest = currentAmplitude;
        }
        _amplitude = currentAmplitude / _amplitudeHighest;
        _amplitudeBuffer = currentAmplitudeBuffer / _amplitudeHighest;
    }

    void CreateAudioBands() // gets a range between 0 and 1 for the height of each band
    {
        for (int i = 0; i < bandCount; i++)
        {
            if (_freqBand[i] >= _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < bandCount; i++)
        {
            if (_freqBand[i] >  _bandBuffer[i]) { 
                  _bandBuffer[i] = _freqBand[i];
                _bufferDecrease[i] = 0.005f;
            }
            else if (_freqBand[i] < _bandBuffer[i])
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= 1.2f;

                if (_bandBuffer[i] < 0) _bandBuffer[i] = 0;
            }
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        if (size == _size.SixtyFour)
        {
         /* 22050 / 512 = 43Hz per sample
         * 0-15 = 1 * 16 = 16 samples
         * 15-31 = 2 * 16 = 32 samples
         * 32-39 = 4 * 8 = 32 samples
         * 40-47 = 6 * 8 = 48 samples
         * 48-55 = 16 * 8 = 128 samples
         * 56-63 = 32 * 8 = 256 samples
         * 512 total samples
         */
            int count = 0;
            int sampleCount = 1;
            int power = 0;

            for (int i = 0; i < 64; i++)
            {
                float average = 0;
                if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
                {
                    power++;
                    sampleCount = (int)Mathf.Pow(2, power);
                    if (power == 3)
                    {
                        sampleCount -= 2;
                    }
                }

                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == _channel.Left)
                    {
                        average += _samplesLeft[count] * (count + 1);
                    }
                    else if (channel == _channel.Right)
                    {
                        average += _samplesRight[count] * (count + 1);
                    }
                    else // Stereo audio by default
                    {
                        average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                    }
                    count++;
                }

                average /= count;

                _freqBand[i] = average * 80;
            }
        }
        else
        {
         /* 22050 / 512 = 43Hz per sample
         * 20 - 60 Hz
         * 250 - 500 Hz
         * 500 - 2000 Hz
         * 2000 - 4000 Hz
         * 4000 - 6000 Hz
         * 6000 - 20000 Hz
         * 
         * b0 - 2 samples => 2*43 = 86 Hz
         * b1 - 4 samples => 86 + 4*43 = 258 Hz
         * b2 - 8 samples => 258 + 8*43 = 602Hz
         * b3 - 16 samples => 602 + 16*43 = 1290 Hz
         * b4 - 32 samples => 1290 + 32*43 = 2666 Hz
         * b5 - 64 samples => 2666 + 64*43 = 5418 Hz
         * b6 - 128 samples => 5418 + 128*43 = 10922 Hz
         * b7 - 256 samples => 10922 + 256*43 = 21930 Hz
         */
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i) * 2;

                if (i == 0)
                {
                    sampleCount += 2; // 510 total samples, need two more added to a freq band to reach full 512
                }
                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == _channel.Left)
                    {
                        average += _samplesLeft[count] * (count + 1);
                    }
                    else if (channel == _channel.Right)
                    {
                        average += _samplesRight[count] * (count + 1);
                    }
                    else // Stereo audio by default
                    {
                        average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                    }
                    count++;
                }

                average /= count;

                _freqBand[i] = average * 10;
            }
        }
    }
}
