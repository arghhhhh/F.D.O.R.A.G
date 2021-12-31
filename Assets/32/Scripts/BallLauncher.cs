using System.Collections;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    GameObject ballInstance;
    Transform ballRotator;
    Rigidbody ballRigidbody;
    public Transform hoop;
    Transform actualBall;
    bool hasLaunched;
    bool madeBasket;
    [Range(0,180f)]
    public float rotationAngle = 180;
    public float rotationSpeed = 6;

    public float h = 25;
    public float gravity = -18;

    LineRenderer lineVisual;
    public int lineResolution = 10;

    private Vector3 mousePos = new Vector3(0, 0, 0);
    private Vector3 smoothPos = new Vector3(0, 0, 0);
    public float moveSpeed = 0.1f;

    void Start()
    {
        InstantiateBall();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasLaunched = true;
            lineVisual.enabled = false;
            Launch();
        }

        mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.x; //distance from camera to ball
        mousePos = Camera.main.ScreenToWorldPoint(mousePos + Vector3.forward);
        smoothPos = Vector3.Lerp(ballRigidbody.transform.position, mousePos, moveSpeed); // smooth cursor movements

        if (!hasLaunched)
        {
            ballRigidbody.transform.position = new Vector3(0, smoothPos.y, smoothPos.z);
            DrawPath();
        }
        else
        {
            if (!madeBasket)
            {
                rotationAngle -= Time.deltaTime * rotationSpeed;
                ballRotator.localRotation = Quaternion.Euler(rotationAngle, 90, -90).normalized;
                if (ballRigidbody.position.x >= hoop.position.x)
                {
                    StartCoroutine(ExecuteAfterTime(2f));
                    madeBasket = true;
                }
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        DestroyImmediate(ballInstance);
        InstantiateBall();
    }

    void InstantiateBall()
    {
        ballInstance = Instantiate(ballPrefab);
        ballRigidbody = ballInstance.GetComponent<Rigidbody>();
        ballRigidbody.useGravity = false;
        ballRigidbody.transform.position = new Vector3(0, 6f, 0);
        lineVisual = ballInstance.GetComponent<LineRenderer>();
        lineVisual.positionCount = lineResolution;
        ballRotator = ballInstance.transform.Find("Rotation").transform;
        actualBall = ballInstance.transform.Find("Rotation/Ball").transform;
        actualBall.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f))); 
        hasLaunched = false;
        madeBasket = false;
    }
    void Launch()
    {
        Physics.gravity = Vector3.up * gravity;
        ballRigidbody.useGravity = true;
        ballRigidbody.velocity = CalculateLaunchData().initialVelocity;
    }

    LaunchData CalculateLaunchData()
    {
        float displacementY = hoop.position.y - ballRigidbody.position.y;
        Vector3 displacementX = new Vector3(hoop.position.x - ballRigidbody.position.x +1, 0, 0);
        Vector3 displacementZ = new Vector3(0f, 0f, hoop.position.z - ballRigidbody.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityX = displacementX / time;
        Vector3 velocityZ = displacementZ / time;

        return new LaunchData((velocityX + velocityY * -Mathf.Sign(gravity) + velocityZ), time);
    }

    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = ballRigidbody.position;
        for (int i = 0; i < lineResolution; i++)
        {
            float simulationTime = i / (float)lineResolution * launchData.timeToTarget + 0.15f;
            Vector3 displacement = launchData.initialVelocity * simulationTime + new Vector3(0, gravity * simulationTime * simulationTime / 2f, 0);
            Vector3 drawPoint = ballRigidbody.position + displacement;
            lineVisual.SetPosition(i, previousDrawPoint);
            //Debug.DrawLine(previousDrawPoint, drawPoint, Color.green); //draws line in Debug view
            previousDrawPoint = drawPoint;
        }
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData (Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
