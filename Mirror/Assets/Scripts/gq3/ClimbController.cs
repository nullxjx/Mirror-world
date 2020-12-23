using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    public float ClimbSpeed;
    public Collider2D tree_collider2D;
    public Collider2D trunk_collider2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string player_tag = collision.tag;
        if (player_tag == "level3player" || player_tag == "level3player_mirror")
        {
            //ContactPoint2D[] contactPoint = new ContactPoint2D[1];
            //collision.GetContacts(contactPoint);
            //Debug.Log("contact: " + contactPoint[0].normal.y);
            //if (contactPoint[0].normal.y > 0 && contactPoint[0].normal.x == 0)
            //{   
            //}

            float height = 0;
            if(player_tag == "level3player")
                height = GameObject.Find("player").GetComponent<Rigidbody2D>().transform.position.y;
            else
                height = GameObject.Find("player_mirror").GetComponent<Rigidbody2D>().transform.position.y;

            //Debug.Log(height);
            if (height < 0) {//保证是从下面跳到树上去
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                collision.SendMessage("SetTrunk", true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "level3player" || collision.tag == "level3player_mirror") {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, GlobalHelper.instance.ClimbSpeed);
                collision.SendMessage("SetTrunk", true);

                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), tree_collider2D);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -GlobalHelper.instance.ClimbSpeed);
                collision.SendMessage("SetTrunk", true);

                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), tree_collider2D);
            }
            else if (Input.GetKey(KeyCode.Space)) 
            {
                //解决在树干顶部无法往上跳导致动画切换有问题
                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), trunk_collider2D);
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "level3player" || collision.tag == "level3player_mirror")
        {
            Debug.Log("player leave trunk");
            collision.GetComponent<Rigidbody2D>().gravityScale = 1;
            collision.SendMessage("SetTrunk", false);

            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), tree_collider2D, false);
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), trunk_collider2D, false);
        }
    }
}
