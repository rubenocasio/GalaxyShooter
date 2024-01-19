using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;

    //Id for powerups, utilize one script
    [SerializeField] //0 = triple shot, 1 = speed, 2 = shields
    private int _powerupID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at a speed of 3 (adjust in the inspector) (don't forget variable)
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        //when we leave the screen destroy this object
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }
    //check for collisions
    //only be collectible by the player - use tags
    //on collected, destroy
    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("PowerID: " + other.transform.name);

        //if other is player, collect game object
        if (other.tag == "Player")
        {
            Player _player = other.transform.GetComponent<Player>();
            //
            if(_player != null)
            {
                //Efficient way
                switch(_powerupID)
                {
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        UnityEngine.Debug.Log("Collected Shields");
                        break;
                    default:
                        UnityEngine.Debug.Log("Default Value");
                        break;

                }

            }
            Destroy(this.gameObject);
        }
    }
}
