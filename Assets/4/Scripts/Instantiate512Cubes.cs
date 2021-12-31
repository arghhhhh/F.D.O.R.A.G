using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[512];
    //List<GameObject> _sampleCubes = new List<GameObject>();
    public float _maxScale;
    [Range(1,512)]
    public int samplesShown;
    private int _samplesShown;
    [Range(0,20)]
    public float radius;
    [Range(1, 512)]
    public float spacing;
    private bool play;

    void Awake()
    {
        _samplesShown = samplesShown;
        play = true;
        InstanstiateCubes();
    }

    void Update()
    {
        //list version
        {
            //if (_sampleCubes != null)
            //{
            //    int i = 0;
            //    foreach (var cube in _sampleCubes)
            //    {
            //        cube.transform.position = transform.position; //initialize cube at (0,0,0)
            //        cube.transform.position += Vector3.forward * radius; //move cube outward from origin along z-axis
            //                                                             //rotate cube around origin, placing along point on circle or arc (determined by spacing)
            //                                                             //rotate cube so the cube array appears perfectly centered by offsetting the starting position of i in the circle
            //                                                             //offset the starting position of i so the median cube(s) is directly in front of the camera
            //                                                             //add 1/2 the width of the cube (0.5f) to its offset to perfectly center everything
            //        cube.transform.position = Quaternion.Euler(0, 360f / spacing * (i - ((float)samplesShown / 2f) + 0.5f), 0) * cube.transform.position;
            //        if (i < samplesShown / 2)
            //            cube.transform.localScale = new Vector3(1, ((AudioPeer._samplesLeft[i] + AudioPeer._samplesRight[i]) * _maxScale) + 2, 1);
            //        else
            //            cube.transform.localScale = new Vector3(1, ((AudioPeer._samplesLeft[samplesShown - i] + AudioPeer._samplesRight[samplesShown - i]) * _maxScale) + 2, 1);

            //        i++;
            //    }
            //}
        }

        for (int i = 0; i < samplesShown; i++)
        {
            if (_sampleCube != null)
            {
                _sampleCube[i].transform.localPosition = transform.position; //initialize cube at (0,0,0)
                _sampleCube[i].transform.localPosition += Vector3.forward * radius; //move cube outward from origin along z-axis
                                                                               //rotate cube around origin, placing along point on circle or arc (determined by spacing)
                                                                               //rotate cube so the cube array appears perfectly centered by offsetting the starting position of i in the circle
                                                                               //offset the starting position of i so the median cube(s) is directly in front of the camera
                                                                               //add 1/2 the width of the cube (0.5f) to its offset to perfectly center everything
                _sampleCube[i].transform.localPosition = Quaternion.Euler(0, 360f / spacing * (i - ((float)samplesShown / 2f) + 0.5f), 0) * _sampleCube[i].transform.localPosition;

                if (i < samplesShown / 2)
                    _sampleCube[i].transform.localScale = new Vector3(1, ((AudioPeer._samplesLeft[i] + AudioPeer._samplesRight[i]) * _maxScale) + 2, 1);
                else
                    _sampleCube[i].transform.localScale = new Vector3(1, ((AudioPeer._samplesLeft[samplesShown - i] + AudioPeer._samplesRight[samplesShown - i]) * _maxScale) + 2, 1);
            }
        }
    }

    void OnValidate()
    {
        if (spacing < samplesShown) spacing = samplesShown;
        if (samplesShown != _samplesShown && play) //only allow samplesShown to be modified before runtime
            samplesShown = _samplesShown;
    }
    void InstanstiateCubes()
    {
        //list version
        {
            //if (_sampleCubes.Count > 0) 
            //{
            //    foreach (var cube in _sampleCubes)
            //    {
            //        Destroy(cube);
            //    }
            //    _sampleCubes.Clear();
            //}
        }

        for (int i = 0; i < samplesShown; i++)
        {
            GameObject _instanceSampleCube = Instantiate(_sampleCubePrefab, transform);
            _sampleCube[i] = _instanceSampleCube;
            //_sampleCubes.Add(_instanceSampleCube);
            _instanceSampleCube.name = "SampleCube" + i;
        }
    }
}
