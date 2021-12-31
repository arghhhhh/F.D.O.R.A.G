using System.Collections.Generic;
using UnityEngine;

public class TangentCircles : CircleTangent
{
    [Header("Setup")]
    public GameObject circlePrefab;
    private GameObject innerCircleGO, outerCircleGO;
    private Vector4 innerCircle, outerCircle;
    public float innerCircleRadius, outerCircleRadius;
    private List<Vector4> tangentCircle;
    private List<GameObject> tangentObject;
    [Range(1, 64)]
    public int circleAmount;
    private int ogCircleAmount;
    //private float ogInnerCircleX, ogInnerCircleZ, ogOuterCircleX, ogOuterCircleZ;

    private Vector3 mousePos = new Vector3(0, 0, 0);
    private Vector3 smoothPos = new Vector3(0, 0, 0);
    public float moveSpeed = 0.1f;

    private GameObject boundaryObj;
    private RaycastHit hit;
    private float drawPointX, drawPointZ;

    [Header("Color")]
    public Material materialBase;
    private List<Material> material;
    public Gradient gradient;
    public float emissionMultiplier;

    void Start()
    {
        Cursor.visible = false; //makes mouse cursor invisible
        innerCircle = new Vector4(0, 0, 0, innerCircleRadius);
        outerCircle = new Vector4(0, 0, 0, outerCircleRadius);

        boundaryObj = new GameObject("Boundary");
        boundaryObj.AddComponent<SphereCollider>();
        boundaryObj.GetComponent<SphereCollider>().radius = outerCircle.w - innerCircle.w;

        //innerCircleGO = (GameObject)Instantiate(circlePrefab);
        //outerCircleGO = (GameObject)Instantiate(circlePrefab);
        ogCircleAmount = circleAmount;
        InstantiateCircles();
    }

    void InstantiateCircles()
    {
        tangentCircle = new List<Vector4>();
        tangentObject = new List<GameObject>();
        material = new List<Material>();
        for (int i = 0; i<circleAmount; i++)
        {
            GameObject tangentInstance = (GameObject)Instantiate(circlePrefab);
            tangentCircle.Add(Vector4.zero); //fill list with empty data
            tangentObject.Add(tangentInstance);
            tangentObject[i].transform.parent = this.transform;
            material.Add(new Material(materialBase));
            tangentObject[i].GetComponent<MeshRenderer>().material = material[i];
            material[i].SetColor("_EmissionColor", gradient.Evaluate((1f / circleAmount) * i) * emissionMultiplier);
            material[i].SetColor("_Color", new Color(0, 0, 0));
        }
        ogCircleAmount = circleAmount;
    }

    void PositionRaycast()
    {
        //create vector pointing from mousPos to 0,0
        //Vector3 direction = toPosition - fromPosition;
        if (Physics.Raycast(mousePos, -mousePos, out hit, Mathf.Infinity))
        {
            //Debug.Log("hit");
            drawPointX = hit.point.x;
            drawPointZ = hit.point.z;
        }
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        smoothPos = Vector3.Lerp(transform.position, mousePos, moveSpeed); // smooth cursor movements

        if (ogCircleAmount != circleAmount)
        {
            //clean out both lists before updating their data and instantiating new prefabs
            tangentCircle.Clear();
            for (int i=0; i<ogCircleAmount; i++)
            {
                Destroy(tangentObject[i]);
            }
            InstantiateCircles();
        }

        #region
        //control innerCircle.x and innerCircle.z based on mousePos.x and mousePos.z
        //restrict innerCircle.x and innerCircle.z to max values inside edges of outerCircle
        //look at mousPos.x and mousPos.z, set each to value <= (outerCircle.w - innerCircle.w)
        //outerCircle.w = innerCircle.w + Mathf.Sqrt((innerCircle.x * innerCircle.x) + (innerCircle.z * innerCircle.z));
        //innerCircleMaxX = Mathf.Sqrt((outerCircle.w * outerCircle.w) - (2 * outerCircle.w * innerCircle.w) + (innerCircle.w * innerCircle.w) - (innerCircle.z * innerCircle.z));
        //innerCircleMaxZ = Mathf.Sqrt((outerCircle.w * outerCircle.w) - (2 * outerCircle.w * innerCircle.w) + (innerCircle.w * innerCircle.w) - (innerCircle.x * innerCircle.x));

        //float innerCircleMaxX = Mathf.Sqrt((outerCircle.w * outerCircle.w) - (2 * outerCircle.w * innerCircle.w) + (innerCircle.w * innerCircle.w) - (innerCircle.z * innerCircle.z));
        //float innerCircleMaxZ = Mathf.Sqrt((outerCircle.w * outerCircle.w) - (2 * outerCircle.w * innerCircle.w) + (innerCircle.w * innerCircle.w) - (innerCircle.x * innerCircle.x));
        //Debug.Log(innerCircleMaxX + " " + innerCircleMaxZ);
        //if (mousePos.x > 0)
        //{
        //    if (mousePos.x > innerCircleMaxX) mousePos.x = innerCircleMaxX;
        //}
        //else if (mousePos.x < 0)
        //{
        //    if (mousePos.x < -innerCircleMaxX) mousePos.x = -innerCircleMaxX;
        //}

        //if (mousePos.z > 0)
        //{
        //    if (mousePos.z > innerCircleMaxZ) mousePos.z = innerCircleMaxZ;
        //}
        //else if (mousePos.z < 0)
        //{
        //    if (mousePos.z < -innerCircleMaxZ) mousePos.z = -innerCircleMaxZ;
        //}
        #endregion

        float innerCircleMaxX = Mathf.Sqrt(Mathf.Abs((outerCircle.w * outerCircle.w) - (2 * outerCircle.w * innerCircle.w) + (innerCircle.w * innerCircle.w) - (innerCircle.z * innerCircle.z)));
        float innerCircleMaxZ = Mathf.Sqrt(Mathf.Abs((outerCircle.w * outerCircle.w) - (2 * outerCircle.w * innerCircle.w) + (innerCircle.w * innerCircle.w) - (innerCircle.x * innerCircle.x)));
        drawPointX = smoothPos.x;
        drawPointZ = smoothPos.z;
        if (Mathf.Abs(smoothPos.x) > innerCircleMaxX || Mathf.Abs(smoothPos.z) > innerCircleMaxZ)
        {
            PositionRaycast();
        }
        innerCircle = new Vector4(drawPointX, innerCircle.y, drawPointZ, innerCircle.w); //update innerCircle vector based on mousePos
        //innerCircleGO.transform.position = new Vector3(innerCircle.x, innerCircle.y, innerCircle.z);
        //innerCircleGO.transform.localScale = new Vector3(innerCircle.w, innerCircle.w, innerCircle.w) * 2;
        //outerCircleGO.transform.position = new Vector3(outerCircle.x, outerCircle.y, outerCircle.z);
        //outerCircleGO.transform.localScale = new Vector3(outerCircle.w, outerCircle.w, outerCircle.w) * 2;
        for (int i = 0; i<circleAmount; i++)
        {
            tangentCircle[i] = findTangentCircle(outerCircle, innerCircle, (360f / circleAmount) * i);
            tangentObject[i].transform.position = new Vector3(tangentCircle[i].x, tangentCircle[i].y, tangentCircle[i].z);
            tangentObject[i].transform.localScale = new Vector3(tangentCircle[i].w, tangentCircle[i].w, tangentCircle[i].w) * 2;
        }
    }
}

//OG CLASS WITH ARRAYS INSTEAD OF LISTS
//public class TangentCircles : CircleTangent
//{
//    public GameObject circlePrefab;
//    private GameObject innerCircleGO, outerCircleGO;
//    public Vector4 innerCircle, outerCircle;
//    public Vector4[] tangentCircle;
//    public GameObject[] tangentObject;
//    [Range(1, 64)]
//    public int circleAmount;
//    void Start()
//    {
//        innerCircleGO = (GameObject)Instantiate(circlePrefab);
//        outerCircleGO = (GameObject)Instantiate(circlePrefab);
//        tangentCircle = new Vector4[circleAmount];
//        tangentObject = new GameObject[circleAmount];
//        for (int i = 0; i < circleAmount; i++)
//        {
//            GameObject tangentInstance = (GameObject)Instantiate(circlePrefab);
//            tangentObject[i] = tangentInstance;
//            tangentObject[i].transform.parent = this.transform;
//        }
//    }

//    void Update()
//    {
//        innerCircleGO.transform.position = new Vector3(innerCircle.x, innerCircle.y, innerCircle.z);
//        innerCircleGO.transform.localScale = new Vector3(innerCircle.w, innerCircle.w, innerCircle.w) * 2;
//        outerCircleGO.transform.position = new Vector3(outerCircle.x, outerCircle.y, outerCircle.z);
//        outerCircleGO.transform.localScale = new Vector3(outerCircle.w, outerCircle.w, outerCircle.w) * 2;
//        for (int i = 0; i < circleAmount; i++)
//        {
//            tangentCircle[i] = findTangentCircle(outerCircle, innerCircle, (360f / circleAmount) * i);
//            tangentObject[i].transform.position = new Vector3(tangentCircle[i].x, tangentCircle[i].y, tangentCircle[i].z);
//            tangentObject[i].transform.localScale = new Vector3(tangentCircle[i].w, tangentCircle[i].w, tangentCircle[i].w) * 2;
//        }
//    }
//}
