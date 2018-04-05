using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplashOffense : MonoBehaviour {

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

    public class EnemySplashAttack : MonoBehaviour
    {
        //Current position difference vector being tested
        Vector3 currentTargetVector;
        
        //Splash attack function
        public void SplashAttack(Transform allyParent, Transform player, float splashRange, int dmgAmount)
        {
            //Cycles through each ally child of Ally Controller
            foreach (Transform child in allyParent)
            {
                //Vector between current position and each ally
                currentTargetVector = child.position - transform.position;

                //Applies damage if ally is within splash range
                if (currentTargetVector.sqrMagnitude <= Mathf.Pow(splashRange, 2))
                {
                    child.gameObject.GetComponent<AllyHealth>().health -= dmgAmount;
                }
            }

            /*
            foreach (Transform child in wallParent)
            {
                currentTargetVector = child.position - transform.position;

                if (currentTargetVector.sqrMagnitude <= Mathf.Pow(splashRange, 2))
                {
                    target.gameObject.GetComponent<WallHealth>().health -= dmgAmount;
                }
            }
            */

            /*
            if (Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2) <= Mathf.Pow(splashRange, 2))
            {
                player.gameObject.GetComponent<PlayerHealth>().health -= dmgAmount;
            }
            */

            //Destroys enemy after attack
            Destroy(gameObject, 0.2f);
        }
    }

    public class EnemySplashMovement : MonoBehaviour
    {
        //Movement direction vector
        Vector2 direction2D;

        //Controls enemy movement
        public void EnemyMovement(Transform closestEntity, Transform playerTransform, Rigidbody2D enemyRigidbody2D, int distraction, int speed)
        {
            //Runs if the closest target is within distraction range
            if (Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(distraction, 2))
            {
                //Sets movement direction towards closest target
                direction2D.x = closestEntity.position.x - transform.position.x;
                direction2D.y = closestEntity.position.y - transform.position.y;
            }
            else //Runs if closest target is outside distraction range
            {
                //Sets movement direction towards player
                direction2D.x = playerTransform.position.x - transform.position.x;
                direction2D.y = playerTransform.position.y - transform.position.y;
            }

            //Adds constant force towards direction
            enemyRigidbody2D.AddForce(direction2D / direction2D.magnitude * speed);
        }
    }

    //Declares object components
    public EnemyAim EnemyAimObject;
    public EnemySplashAttack EnemySplashAttackObject;
    public EnemySplashMovement EnemySplashMovementObject;

    //Ally Parent and Player transforms
    Transform allyController;
    Transform playerTransform;

    //Container for current closest target
    Transform closestEntity;

    //Enemy rigidbody
    Rigidbody2D enemyRB;

    //Ensures single attack
    bool attackTriggered = false;

    //Controls frequency of closest target check
    float checkTimer = 0f;

    //Indicates movement status; controls FixedUpate motion
    bool motion = false;

    //Public variables
    public int enemySpeed;
    public float splashRange;
    public float enemyTriggerRange;
    public int distractionRange;
    public int splashDamage;

    // Use this for initialization
    void Start()
    {
        //Initializes object components
        EnemyAimObject = gameObject.AddComponent<EnemyAim>();
        EnemySplashAttackObject = gameObject.AddComponent<EnemySplashAttack>();
        EnemySplashMovementObject = gameObject.AddComponent<EnemySplashMovement>();

        //Cached reference to Ally Parent and Player transforms
        allyController = GameObject.FindGameObjectWithTag("AllyParent").GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Reference to gameObject rigidbody
        enemyRB = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Timer for closest target check
        checkTimer -= Time.deltaTime;

        //Calls target check function on timer completion or target destruction
        if (checkTimer <= 0 || closestEntity == null)
        {
            closestEntity = EnemyAimObject.LocateTarget(allyController, playerTransform);
            checkTimer = 0.5f;
        }

        //Runs only if attack has yet to be triggered
        if (attackTriggered == false)
        {
            //Runs if target is within trigger range
            if (Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemyTriggerRange, 2))
            {
                //Executes splash attack
                EnemySplashAttackObject.SplashAttack(allyController, playerTransform, splashRange, splashDamage);

                //Sets triggered status to prevent successive attacks
                attackTriggered = true;

                //Cancels movement
                if (motion)
                {
                    motion = false;
                }
            }
            else if (motion == false) //Runs when target is outside trigger range and when immobile
            {
                motion = true;
            }
        }
    }

    void FixedUpdate()
    {
        //Runs when motion is true
        if (motion)
        {
            //Move when out of range
            EnemySplashMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemySpeed);
        }
    }
}
