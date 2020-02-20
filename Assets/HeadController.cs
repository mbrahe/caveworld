using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public FootController[] feet;
    public float height = 1;
    public float stiffness = 1;
    public float leanForce = 2;
    public float drag = .5f;
    public float balanceTolerance = 1.5f;
    public float stepLength = 1;

    Vector2 inputDirection;

    float stepTimer;
    Rigidbody2D rb;

    Vector2 meanFootPosition {
        get
        {
            Vector2 meanPosition = Vector2.zero;
            foreach (FootController foot in feet)
            {
                meanPosition += (Vector2)foot.gameObject.transform.position;
            }

            return meanPosition / feet.Length;
        }
    }

    Vector2 target
    {
        get
        {
            return meanFootPosition + height * Vector2.up;
        }
    }

    FootController farthestFoot
    {
        get
        {
            FootController farthest = null;
            float farthestDistance = 0;
            foreach (FootController foot in feet)
            {
                float distance = (foot.transform.position - transform.position).magnitude;
                if (distance > farthestDistance)
                {
                    farthest = foot;
                    farthestDistance = distance;
                }
            }

            return farthest;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (FootController foot in feet)
        {
            foot.head = GetComponent<HeadController>();
        }
    }

    private void Update()
    {
        inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        stepTimer -= Time.deltaTime;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce((target - (Vector2) transform.position) * stiffness);
        rb.AddForce(leanForce * inputDirection);
        rb.AddForce(-rb.velocity * drag);

        Vector2 targetSeperation = ((Vector2)transform.position) - target;

        if (targetSeperation.magnitude > balanceTolerance && stepTimer <= 0)
        {
            farthestFoot.Step(inputDirection);
            stepTimer = stepLength;
        }
    }
}
