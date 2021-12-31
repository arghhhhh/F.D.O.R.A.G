using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class WarpSpeed : MonoBehaviour
{
    private VisualEffect warpSpeedVFX;
    public MeshRenderer cone;
    public float rate = 0.02f;
    private bool isSpacePressed;
    private float amount = 0f;

    void Start()
    {
        warpSpeedVFX = gameObject.GetComponent<VisualEffect>();
        warpSpeedVFX.Stop();
        SetAmount(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpacePressed = false;
        }
        if (isSpacePressed)
        {
            warpSpeedVFX.Play();
            ActivateParticles();
        }
        else
        {
            DeactivateParticles();
        }
    }

    void SetAmount(float a)
    {
        warpSpeedVFX.SetFloat("WarpAmount", a);
        cone.material.SetFloat("_Noise_Slider", a);
    }

    void ActivateParticles()
    {
        if (amount <= (1 - rate))
        {
            amount += rate;
            SetAmount(amount);
        }
        else SetAmount(1);
    }

    private void DeactivateParticles()
    {
        if (amount >= (4*rate))
        {
            amount -= (4*rate);
            SetAmount(amount);
        }
        else
        {
            SetAmount(0);
            warpSpeedVFX.Stop();
        }
    }
}
