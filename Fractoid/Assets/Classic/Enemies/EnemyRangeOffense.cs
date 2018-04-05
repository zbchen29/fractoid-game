using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeOffense : MonoBehaviour {

    public class EnemyAim : MonoBehaviour
    {
        //Initial closest target distance set to infinity
        float closestTargetSqrDist = Mathf.Infinity;

        //LocateTarget variables
        Vector3 currentTargetVector;
        float currentTargetSqrDist;
        Transform closestEntityTransform;

        //Function returns Transform of nearest child of Enemy Parent
        public Transform LocateTarget(Transform allyParent, Transform player)
        {
            //Obtains transform of closest ally target
            foreach (Transform child in allyParent)
            {
                //Obtains distance to tested target
                currentTargetVector = child.position - transform.position;
                currentTargetSqrDist = currentTargetVector.sqrMagnitude;

                //Updates closest ally target and current closest distance
                if (currentTargetSqrDist < closestTargetSqrDist)
                {
                    closestTargetSqrDist = currentTargetSqrDist;
                    closestEntityTransform = child;
                }
            }

            //Checks closest ally distance against player distance
            if (Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2) <= closestTargetSqrDist)
            {
                //Updates closest target to player
                closestEntityTransform = player;
            }

            //Resets initial target distance to infinity for next iteration
            closestTargetSqrDist = Mathf.Infinity;

            //Returns closest target transform
            return closestEntityTransform;
        }

        //AllyAimFunction variables
        Vector3 targetAim;
        float aimAngle;
        Quaternion aimRotation;
        float turnSpeed = 8f;

        //Function aims towards vector3 position arugment
        public void EnemyAimFunction(Vector3 targetAim)
        {
            //Calculates positive angle between target direction and global right
            aimAngle = Vector3.Angle(Vector3.right, targetAim - transform.position);

            //Accounts for negative angles
            if (targetAim.y < transform.position.y)
            {
                aimAngle = aimAngle * -1;
            }

            //Creates quaternion from designed angle rotation
            aimRotation = Quaternion.Euler(0, 0, aimAngle);

            //Smoothed rotation towards target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, turnSpeed * Time.deltaTime);
        }
    }

    public class EnemyRangeAttack : MonoBehaviour
    {
        //Declares local variable for controlling auto attack
        float attackTimerLocal = 0;

        //Instantiates bullet based on parameters
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
                bulletClone.SetBullet(aimVector, bulletSpeed, customTag, bulletDamage, decayTime, buffStatus);

                //Direction vector for firing directly towards mouse; use PlayerAimObject.targetAim as aimVector argument
                //Vector2 bulletDirection = aimVector - transform.position;
                //bulletClone.SetBullet(bulletDirection / bulletDirection.magnitude, bulletSpeed, customTag, bulletDamage, buffStatus);

                //Resets attack timer
                attackTimerLocal = 0;
            }
        }
    }

    public class EnemyRangeMovement : MonoBehaviour
    {
        //Movement direction vector
        Vector2 direction2D;

        //Movement function
        public void EnemyMovement(Rigidbody2D enemyRigidbody2D, Vector3 targetPosition, int speed)
        {
            direction2D.x = targetPosition.x - transform.position.x;
            direction2D.y = targetPosition.y - transform.position.y;

            enemyRigidbody2D.AddForce(direction2D / direction2D.magnitude * speed);
        }

        //Onscreen confirmation function
        public bool OnScreenTest()
        {
            if (transform.position.x <= 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //Declares object components
    public EnemyAim EnemyAimObject;
    public EnemyRangeAttack EnemyRangedAttackObject;
    public EnemyRangeMovement EnemyRangeMovementObject;

    //Associated bullet prefab script component; instantiation also clones entire prefab
    public BulletScript bullet;

    //Ally Parent and Player transforms
    Transform allyController;
    Transform playerTransform;

    //Container for current closest target
    Transform closestEntity;
    
    //Enemy rigidbody
    Rigidbody2D enemyRB;

    //Controls frequency of closest target check
    float checkTimer = 0f;

    //Indicates movement status; controls FixedUpate motion
    bool motion = false;

    //Public variables
    public int enemySpeed;
    public float enemyRange;
    public float attackInterval;
    public int bulletDamage;  //Currently inconsequential due to code organization
    public int bulletSpeed;
    public float bulletDecayTime;
    public string bulletTag;

    // Use this for initialization
    void Start () {

        //Initializes object components
        EnemyAimObject = gameObject.AddComponent<EnemyAim>();
        EnemyRangedAttackObject = gameObject.AddComponent<EnemyRangeAttack>();
        EnemyRangeMovementObject = gameObject.AddComponent<EnemyRangeMovement>();

        //Cached reference to Ally Parent and Player transforms
        allyController = GameObject.FindGameObjectWithTag("AllyParent").GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Reference to gameObject rigidbody
        enemyRB = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //Timer for closest target check
        checkTimer -= Time.deltaTime;

        //Calls target check function on timer completion or target destruction
        if (checkTimer <= 0 || closestEntity == null)
        {
            closestEntity = EnemyAimObject.LocateTarget(allyController, playerTransform);
            checkTimer = 0.5f;
        }

        //Called when enemy is in active zone and target is within range
        if (closestEntity != null && EnemyRangeMovementObject.OnScreenTest() && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemyRange, 2))
        {
            //Aim and attack when in range
            EnemyAimObject.EnemyAimFunction(closestEntity.position);
            EnemyRangedAttackObject.FireBullet(transform.right, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false, attackInterval);

            //Cancels movement
            if (motion)
            {
                motion = false;
            }
        }
        else if (motion == false) //Called when target is outside active zone or target is out of range and when immobile
        {
            //Activates motion
            motion = true;
        }
    }

    void FixedUpdate()
    {
        //Runs when motion is true
        if (motion)
        {
            //Aim and move when out of range
            EnemyAimObject.EnemyAimFunction(playerTransform.position);
            EnemyRangeMovementObject.EnemyMovement(enemyRB, playerTransform.position, enemySpeed);
        }
    }
}
