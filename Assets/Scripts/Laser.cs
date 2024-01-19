using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        //translate laser up
        // Move the laser upwards.
        // 'Vector3.up' is a shorthand for new Vector3(0, 1, 0), which represents upward direction in Unity's world space.
        // '_speed' is the speed at which the laser moves, and 'Time.deltaTime' is the time that has passed since the last frame.
        // This calculation makes the movement smooth and consistent regardless of the frame rate.
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //if laser position is greater than 8 on the y destroy the object
        // Check if the laser's position on the y-axis is greater than 7.10.
        // This y position limit may vary based on the size of your game screen or camera setup.
        // It essentially checks whether the laser has gone off the top of the screen.
        if (transform.position.y > 7.10)
        {
            // If the laser has moved past the top edge of the screen, destroy it.
            // Destroying the game object frees up resources, as it's no longer needed.

            //Check if this object has a parent - destroy the parent too!
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            // 'this.gameObject' refers to the game object this script is attached to.
            Destroy(this.gameObject);
        }
    }
}
