using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down 4 meters per second - if bottom of screen respawn at top
        //with a new random x position
        // Move the GameObject this script is attached to down the screen.
        // Vector3.down is a shorthand for a Vector3 (0, -1, 0), representing downward direction.
        // _speed is the speed at which the object moves and Time.deltaTime is the time passed since the last frame,
        // which makes the movement smooth and frame rate independent.
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Check if the GameObject has moved past the bottom edge of the screen (y position is less than -5).
        // This y position limit may vary based on the size of your game screen or camera setup.
        if (transform.position.y < -1f)
        {
            // If the GameObject is below the bottom edge of the screen, 
            // generate a random x position between -8 and 8.
            // This x position limit may vary based on the size of your game screen or camera setup.
            float randomX = UnityEngine.Random.Range(-8f, 8f);

            // Reposition the GameObject at the new random x position and just above the top edge of the screen (y position is 7).
            // The z position remains the same (0 in this case).
            transform.position = new Vector3(randomX, 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " +  other.transform.name);

        //if other is player, damage the player, destroy us
        if (other.tag == "Player")
        {
            //_speed = _speed * -1;
            //Calling method Damage from Player by using GetComponent<????>()
            //other.transform.GetComponent<Player>().Damage();

            Player player = other.transform.GetComponent<Player>();

            //null checking to avoid null errors
            if (player != null )
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
        //if other is laser, laser destroy us
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
