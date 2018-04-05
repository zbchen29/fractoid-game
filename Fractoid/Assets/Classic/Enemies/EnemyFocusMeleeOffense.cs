using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocusMeleeOffense : MonoBehaviour
{

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

    public class EnemyMeleeAttack : MonoBehaviour
    {
        //Health script references of current target
        public AllyHealth targetAlly;
        //public WallHealth targetWall;
        //public PlayerHealth targetPlayer;

        //Obtains initial references to health script of closest target
        public void TargetReference(Transform closestTarget)
        {
            //Health script type based on target tag
            if (closestTarget.gameObject.tag == "Ally")
            {
                targetAlly = closestTarget.gameObject.GetComponent<AllyHealth>();
            }
            else if (closestTarget.gameObject.tag == "Player")
            {
                //targetPlayer = closestTarget.gameObject.GetComponent<PlayerHealth>();
            }
            else if (closestTarget.gameObject.tag == "Wall")
            {
                //targetWall = closestTarget.gameObject.GetComponent<WallHealth>();
            }
        }

        //Controlls frequency of melee attacks
        float attackTimerLocal;

        //Executes attack on cached target health script
        public void MeleeAttack(Transform closestTarget, int dmgAmount, float attackInterval)
        {
            //Timer for melee attack
            attackTimerLocal -= Time.deltaTime;

            //Cached health script selected based on current target tag
            if (attackTimerLocal <= 0)
            {
                if (closestTarget.gameObject.tag == "Ally")
                {
                    targetAlly.HitOccur(dmgAmount, targetAlly.hitColor, targetAlly.tintTime);
                    //targetAlly.health -= dmgAmount;
                }
                else if (closestTarget.gameObject.tag == "Player")
                {
                    //targetPlayer.health -= dmgAmount;
                }
                else if (closestTarget.gameObject.tag == "Wall")
                {
                    //targetWall.health -= dmgAmount;
                }

                //Resets timer to attack interval
                attackTimerLocal = attackInterval;
            }
        }

        //Container variable of current melee target; controls frequency of GetComponent() calls
        Transform targetChangeCheck;

        //Manages health reference calls and melee attacks efficiently; avoids redunant GetComponent() calls
        public void EnemyMeleeAttackManager(Transform closestEntity, int dmgAmount, float attackInterval)
        {
            //Runs on target destruction or new closest target
            if (targetChangeCheck == null || targetChangeCheck != closestEntity)
            {
                //Obtains new target health reference
                TargetReference(closestEntity);

                //Damages target
                MeleeAttack(closestEntity, dmgAmount, attackInterval);

                //Updates current melee target
                targetChangeCheck = closestEntity;
            }
            else if (targetChangeCheck == closestEntity)  //Runs if target remains unchanged
            {
                //Damages target
                MeleeAttack(closestEntity, dmgAmount, attackInterval);
            }
        }
    }

    public class EnemyMeleeMovement : MonoBehaviour
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
    public EnemyMeleeAttack EnemyMeleeAttackObject;
    public EnemyMeleeMovement EnemyMeleeMovementObject;

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
    public float enemyMeleeRange;
    public int distractionRange;
    public float attackInterval;
    public int meleeDamage;

    // Use this for initialization
    void Start()
    {

        //Initializes object components
        EnemyAimObject = gameObject.AddComponent<EnemyAim>();
        EnemyMeleeAttackObject = gameObject.AddComponent<EnemyMeleeAttack>();
        EnemyMeleeMovementObject = gameObject.AddComponent<EnemyMeleeMovement>();

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

        //Runs when target is within melee range; stops movement
        if (closestEntity != null && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemyMeleeRange, 2))
        {
            //Executes attack on closest target
            EnemyAimObject.EnemyAimFunction(closestEntity.position);
            EnemyMeleeAttackObject.EnemyMeleeAttackManager(closestEntity, meleeDamage, attackInterval);

            //Cancels movement
            if (motion)
            {
                motion = false;
            }
        }
        else if (motion == false)  //Runs when target is out of melee range and when immobile
        {
            motion = true;
        }
    }

    void FixedUpdate()
    {
        //Runs when motion is true
        if (motion)
        {
            if (closestEntity != null && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(distractionRange, 2))
            {
                //Move when out of melee range; aims towards closest target
                EnemyAimObject.EnemyAimFunction(closestEntity.position);
                EnemyMeleeMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemySpeed);
            }
            else
            {
                //Move when out of melee range; aims towards player
                EnemyAimObject.EnemyAimFunction(playerTransform.position);
                EnemyMeleeMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemySpeed);
            }
        }
    }
}
