using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootController : MonoBehaviour
{
    public HeadController head;
    public float stepRadius = 2;
    public int raycastFineness = 4;

    public void Step(Vector2 direction)
    {
        Vector2 target = direction.normalized * stepRadius * head.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        Vector2 origin = target + Vector2.up * .5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 10);
        Debug.Log(hit.collider);

        if (hit.collider != null && Mathf.Abs(Vector2.Dot(hit.normal.normalized, Vector2.up)) > .6)
        {
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y, transform.position.z);
            transform.position = newPos;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
