using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : MonoBehaviour
{
    public class PlayerAim : MonoBehaviour
    {
        /*
        //Alternative, mouse position parameter
        Vector3 mouseLocation;
        */

        //Aim position coordinate
        public Vector3 targetAim;

        //Declare rotation coordinates that orients player towards aim
        Quaternion aimRotation;

        //Declare angle between world right and aim direction
        float aimAngle;

        //Player turn speed
        float turnSpeed = 10f;

        //Runs aim processes
        public void AimFunction()
        {
            /*
            //Alternative, sets targetAim a constant distance in front of camera

            mouseLocation = Input.mousePosition;
            mouseLocation.z = 10f;
            targetAim = Camera.main.ScreenToWorldPoint(mouseLocation);

            //Simple instant tracking
            transform.right = targetAim - transform.position;
            */

            //Obtains coordinates of mouse
            targetAim = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Ensures targetAim coincides with game plane
            targetAim.z = 0f;

            //Angle between global right and aim direction
            aimAngle = Vector3.Angle(Vector3.right, targetAim - transform.position);

            //Workaround replacing Vector3.SignedAngle() for negative angles
            if (targetAim.y < transform.position.y)
            {
                //Negative angle if aiming below player position
                aimAngle = aimAngle * -1;
            }

            //Creates rotation coordinates that orients player towards aim
            aimRotation = Quaternion.Euler(0f, 0f, aimAngle);

            //Smoothed turn from current rotation to aimRotation
            transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, turnSpeed * Time.deltaTime);
        }
    }

    public class PlayerAttack : MonoBehaviour
    {

        //Declares local variable for controlling auto attack
        float attackTimerLocal = 0;

        //Direction vector for firing directly towards mouse
        Vector2 bulletDirection;

        public void FireBullet(Vector3 aimVector, BulletScript bulletPrefab, int bulletSpeed, string customTag, int bulletDamage, float decayTime, bool buffStatus, float attackInterval)
        {
            //Increments timer
            attackTimerLocal += Time.deltaTime;

            //Auto fire mode controlled by timer
            if (attackTimerLocal >= attackInterval)
            {
                //Instantiates bullet prefab argument
                BulletScript bulletClone = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletScript>();

                //Calls initialization method in BulletScript; fires directly right; use transfrom.right as aimVector argument
                //bulletClone.SetBullet(aimVector, bulletSpeed, customTag, bulletDamage, buffStatus);

                //Direction vector for firing directly towards mouse; use PlayerAimObject.targetAim as aimVector argument
                bulletDirection = aimVector - transform.position;
                bulletClone.SetBullet(bulletDirection / bulletDirection.magnitude, bulletSpeed, customTag, bulletDamage, decayTime, buffStatus);

                //Resets attack timer
                attackTimerLocal = 0;
            }
        }

        public void FireBulletSingle(Vector3 aimVector, BulletScript bulletPrefab, int bulletSpeed, string customTag, int bulletDamage, float decayTime, bool buffStatus)
        {
            //Instantiates bullet prefab argument
            BulletScript bulletClone = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletScript>();

            //Calls initialization method in BulletScript; fires directly right; use transfrom.right as aimVector argument
            //bulletClone.SetBullet(aimVector, bulletSpeed, customTag, bulletDamage, decayTime, buffStatus);

            //Direction vector for firing directly towards mouse; use PlayerAimObject.targetAim as aimVector argument
            Vector2 bulletDirection = aimVector - transform.position;
            bulletClone.SetBullet(bulletDirection / bulletDirection.magnitude, bulletSpeed, customTag, bulletDamage, decayTime, buffStatus);
        }
    }

    //Declares object components
    public PlayerAim PlayerAimObject;
    public PlayerAttack PlayerAttackObject;

    //Linked bullet prefab
    public BulletScript bullet;

    //Auto attack state
    bool autoAttack = false;

    //Public variables
    public float attackInterval;
    public int bulletDamage;  //Currency inconsequential due to code structure
    public int bulletSpeed;
    public float bulletDecayTime;
    public string bulletTag;

    // Use this for initialization
    void Start()
    {
        //Initializes object components
        PlayerAimObject = gameObject.AddComponent<PlayerAim>();
        PlayerAttackObject = gameObject.AddComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        //Calls aim function in PlayerAimObject
        PlayerAimObject.AimFunction();

        if (Input.GetKeyDown("t"))
        {
            if (autoAttack == false)
            {
                autoAttack = true;
            }
            else if (autoAttack == true)
            {
                autoAttack = false;
            }
        }

        //Automatic fire without clicking
        if (autoAttack == true)
        {
            //Calls fire function using linked bullet prefab as instantiate arugment; fires towards mouse
            PlayerAttackObject.FireBullet(PlayerAimObject.targetAim, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false, attackInterval);
        }
        //Autofire attack mode controlled by timer
        else if (Input.GetMouseButton(0))
        {
            //Calls fire function using linked bullet prefab as instantiate arugment; fires local right
            //PlayerAttackObject.FireBullet(transform.right, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false, attackInterval);

            //Calls fire function using linked bullet prefab as instantiate arugment; fires towards mouse
            PlayerAttackObject.FireBullet(PlayerAimObject.targetAim, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false, attackInterval);
        }

        /*
        //Single click attack mode
        if (Input.GetMouseButtonDown(0))
        {
            //Calls fire function using linked bullet prefab as instantiate argument
            PlayerAttackObject.FireBulletSingle(PlayerAimObject.targetAim, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false);
        }
        */
    }
}
