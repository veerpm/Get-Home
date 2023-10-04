using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public Transform player;
    private float speed = 1;
    public float lineofSight = 5.0f;
    public float throwingRange;
    private float nextThrowTime;
    public float throwingCD = 1f;
    public GameObject throwable;
    public GameObject throwableParentLeft;
    public GameObject throwableParentRight;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < lineofSight && distanceFromPlayer > throwingRange)
        {
            Debug.Log("This works!");
            transform.position = Vector2.MoveTowards(this.transform.position, player.position + new Vector3(0.7f,-0.5f, 0.0f), speed * Time.deltaTime);
        }
        else if(distanceFromPlayer <= throwingRange && nextThrowTime < Time.time)
        {
            if (player.position.x < transform.position.x)
            {
                Debug.Log("This exists!");
                Instantiate(throwable, throwableParentLeft.transform.position, Quaternion.identity);
                nextThrowTime = Time.time + throwingCD;
            }
            else
            {
                Debug.Log("This exists2!");
                Instantiate(throwable, throwableParentRight.transform.position, Quaternion.identity);
                nextThrowTime = Time.time + throwingCD;
            }
            
        }
       
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineofSight);
        Gizmos.DrawWireSphere(transform.position, throwingRange);
    }

}
