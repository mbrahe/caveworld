using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : MonoBehaviour
{
    public float radius = 0.5f;
    public float height = 0.5f;
    public float acceleration = 5f;
    public float footSpreadTol = 1f;
    public float footSwitchSpeed = 2f;

    public FootController[] feet;

    public Vector2 leftFoot;
    public Vector2 rightFoot;

    Rigidbody2D rb;
    Vector2 inputDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        feet[0].transform.position = rb.position + Vector2.left * radius + Vector2.down * (radius + 2 * height);
        feet[1].transform.position = rb.position + Vector2.right * radius + Vector2.down * (radius + 2 * height);


        //leftFoot = Vector2.zero;
        //rightFoot = Vector2.zero;
    }

    void SwapFeet()
    {
        FootController temp;
        temp = feet[1];
        feet[1] = feet[0];
        feet[0] = feet[1];
    }

    // Update is called once per frame
    private void Update()
    {
        inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (!(feet[0].isStepping || feet[1].isStepping))
        {
            //Move left foot
            if ((leftFoot - (Vector2)feet[0].transform.position).magnitude > footSpreadTol)
            {
                //SwapFeet();
                feet[0].Step(leftFoot, footSwitchSpeed);
            }

            // Move right foot
            if ((rightFoot - (Vector2)feet[1].transform.position).magnitude > footSpreadTol)
            {
                //SwapFeet();
                feet[1].Step(rightFoot, footSwitchSpeed);
            }
        }

        //feet[0].position = (Vector3)leftFoot;
        //feet[1].position = (Vector3)rightFoot;
    }
    void FixedUpdate()
    {
        // Send two raycasts from opposite sides of Walker to find a position for the feet
        // If one fails try again with an x location closer to the center
        // The body is positioned a fixed distance above the line connecting the feet

        Debug.Log("Start frame");
        // Find left foot target position
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.left * radius, Vector2.down, radius + 2 * height);
        if (hit.collider != null && Vector2.Dot(hit.normal, Vector2.up) > .5)
        {
            leftFoot = hit.point;
            Debug.DrawLine(rb.position + Vector2.left * radius, hit.point);
        }
        Debug.Log(leftFoot);
        // Find right foot target position
        hit = Physics2D.Raycast(rb.position + Vector2.right * radius, Vector2.down, radius + height);
        if (hit.collider != null && Vector2.Dot(hit.normal, Vector2.up) > .5)
        {
            rightFoot = hit.point;
            Debug.DrawLine(rb.position + Vector2.right * radius, hit.point);
        }
        Debug.Log(rightFoot);
        Vector2 baseLine = rightFoot - leftFoot;

        Debug.DrawLine(leftFoot, rightFoot, Color.red);


        Vector2 orientation = (Vector2) Vector3.Cross((Vector3) baseLine, Vector3.back).normalized;

        Debug.Log(orientation);

        Vector2 bodyTarget = (((rightFoot + leftFoot) / 2) + (orientation * height));

        if (baseLine.magnitude != 0)
        {
            rb.position = new Vector2(rb.position.x, bodyTarget.y);
        }

        Debug.DrawLine((rightFoot + leftFoot) / 2, rb.position, Color.red);

        rb.AddForce(acceleration * Vector2.right * inputDirection.x);
    }
}
