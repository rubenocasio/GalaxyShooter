// This line imports the System.Collections namespace, which contains interfaces and classes
// that define various collections of objects, such as lists, queues, bit arrays, hash tables and dictionaries.
using System;
using System.Collections;
// This line imports the System.Collections.Generic namespace, which contains interfaces and classes 
// that define generic collections, which allow users to create strongly typed collections 
// that provide better type safety and performance than non-generic strongly typed collections.
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

// This line imports the UnityEngine namespace, which provides access to every Unity engine 
// class and allows scripts to interact with them. 
using UnityEngine;

// This is the declaration of a new class called myScript that inherits from the MonoBehaviour class. 
// MonoBehaviour is the base class from which every Unity script derives.
public class Player : MonoBehaviour
{
    //public or private reference (variable)
    //datatype every variable has a data type (ints, float, bool, string)
    // public float speed = 3.5f; //f = float (need to end with suffix with float)

    //private variable - if we need another script to modify this variable then we'll use public
    //Serialize the data to be read in the inspector and overwrite it from inspector
    [SerializeField]
    private float _speed = 5.0f;//f = float (need to end with suffix with float)

    [SerializeField]
    private float _speedMultiplier = 2;

    // public float horizontalInput;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    // The SerializeField attribute allows the _fireRate field to show up in the Unity editor while keeping it private in code.
    // This is a good practice as it allows for tuning game parameters in the editor while maintaining encapsulation.
    [SerializeField]

    // _fireRate determines how often the player can fire a laser.
    // Here it's set to 0.5f, which means the player can fire a laser every half second.
    private float _fireRate = 0.1f;

    // _nextFire is used to keep track of when the player is allowed to fire the laser again.
    // It's initialized to -1f to ensure that the player can fire a laser immediately when the game starts.
    // This is because Time.time (which is used to determine when next to fire) starts at 0 and is always increasing.
    // So any negative initial value will allow the player to fire immediately.
    private float _nextFire = -1.0f;

    //Life of our player
    [SerializeField]
    private int _life = 3;

    private SpawnManager _spawnManager;

    //variable for triple shot active = what do we do with it?
    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    // Start() is a Unity-specific function that's called just before any of the Update methods is called the first time. 
    // It's often used to set initial parameters, fetch references, or to run a setup process for a game object.
    // This is where you’d put all the initialization code that needs to happen once.
    void Start()
    {
        //take the current position = a new position (x,y,z)
        transform.position = new Vector3(0, 0, 0);


        //Find gameobject then get access to spawn manager script
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            UnityEngine.Debug.Log("The spawn manager is NULL");
        }
    }

    // Update() is a Unity-specific function that gets automatically called every frame by the engine. 
    // This is where you'll want to put things that need to update frequently like moving non-physics objects, simple timers, etc.
    // Any instructions in here will be executed in each frame of the game, as long as the script is enabled.
    void Update()
    {
        CalculateMovement();

        if (Input.GetMouseButton(0) && Time.time >= _nextFire)
        {
            Stun();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Direction of travel
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //if speed boost is active is false
        //else speed boost multiplier
        if(_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);

        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-12.4f, transform.position.y, 0);
        }
        else if (transform.position.x < -12.4f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }


    }

    void Stun()
    {
        //if i hit space key = spawn game objects
        //spawn game objects
        // If left mouse button is held down and it's time to fire next laser
        //the condition was moved above to update method
        /*
        if (Input.GetMouseButton(0) && Time.time >= _nextFire)
        {
            UnityEngine.Debug.Log("Mouse button is being held down.");

            // Update the time when next laser can be fired
            _nextFire = Time.time + _fireRate;

            // Instantiate new laser prefab at the desired position
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        }
        */
        UnityEngine.Debug.Log("Mouse button is being held down.");

        // Update the time when next laser can be fired
        _nextFire = Time.time + _fireRate;


        //if mouse button is pressed, if TS acticve fire 3 lasers
        //else fire one laser
        //Instantiate 3 lasers (triple shot prefab)
        if (_isTripleShotActive == true)
        {
            //instantiate for the triple shot
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Instantiate laser prefab at the desired position (Single Shot)
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.005f, 0), Quaternion.identity);

        }

    }

    public void Damage()
    {
        //_life-- or _life = _life - 1
        //if shields is active
        //do nothing
        //deacticvate shields
        //return;
        //When you use this return nothing below it gets called

        _life--;

        //Check if dead if so destroy us - have enemy call this method through script communication
        if (_life < 1)
        {
            //communicate with spawn manager to let them know to stop spawning
            //script communication GetComponent
            _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);
        }

    }
    /* Triple Shot Logic */
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    /* Speed Boost Logic */
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }
}
