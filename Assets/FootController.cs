using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootController : MonoBehaviour
{
    public bool isStepping;
    float speed;
    Vector2 target;
    Vector2 origin;

    float t;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Step(Vector2 destination, float stepSpeed)
    {
        if (!isStepping)
        {
            isStepping = true;
        }
        origin = (Vector2) transform.position;
        target = destination;
        speed = stepSpeed;

        t = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (isStepping)
        {
            t += speed * Time.deltaTime;
            transform.position = Vector2.Lerp(origin, target, t);
        }

        if (t >= 1)
        {
            transform.position = (Vector3)target;
            isStepping = false;
        }
    }
}
