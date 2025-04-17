using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float yPosition;
    private float camXPosition;
    private float camYPosition;

    private float length;

    private float distanceMoved;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        
        camXPosition = cam.transform.position.x;
        camYPosition = cam.transform.position.y;

        length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        distanceMoved = (cam.transform.position.x - xPosition) * (1 - parallaxEffect);

        float xDistanceToMove = (cam.transform.position.x - xPosition) * parallaxEffect;
        float yDistanceToMove = (cam.transform.position.y - yPosition) * parallaxEffect;

        transform.position = new Vector3(xPosition + xDistanceToMove, yPosition + yDistanceToMove);

        if (distanceMoved > length)
        {
            /* 这就是我说的那个傻逼多少差一段的，逼得我硬编码的部分 */
            xPosition = xPosition + length + 1.7f;
        } else if (distanceMoved < -length)
        {
            xPosition = xPosition - length - 1.7f;
        }
    }
    
}
