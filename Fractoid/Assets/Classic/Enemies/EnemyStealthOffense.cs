using UnityEngine;

public class EnemyStealthOffense : MonoBehaviour {

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

    public class EnemyStealthAttack : MonoBehaviour
    {
        //Health script references of current target
        public AllyHealth targetAlly;
        //public WallHealth targetWall;
        //public PlayerHealth targetPlayer;

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

        //Executes attack on cached target health script
        public void MeleeAttack(Transform closestTarget, int dmgAmount, float attackInterval)
        {
            //Timer for melee attack
            attackTimerLocal += Time.deltaTime;

            //Cached health script selected based on current target tag
            if (attackTimerLocal >= attackInterval)
            {
                if (closestTarget.gameObject.tag == "Ally")
                {
                    targetAlly.health -= dmgAmount;
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
                attackTimerLocal = 0;
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

    public class EnemyStealthMovement : MonoBehaviour
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
    public EnemyStealthAttack EnemyStealthAttackObject;
    public EnemyStealthMovement EnemyStealthMovementObject;
    public EntityGraphics EntityGraphicsObject;
    public SpriteRenderer SpriteRendererObject;

    //Associated bullet prefab script component; instantiation also clones entire prefab
    public BulletScript bullet;

    //Ally Parent and Player transforms
    Transform allyController;
    Transform hiddenController;
    Transform enemyController;
    Transform playerTransform;

    //Container for current closest target
    Transform closestEntity;

    //Enemy rigidbody
    Rigidbody2D enemyRB;

    //Controls frequency of closest target check
    float checkTimer = 0f;

    //Indicates movement status; controls FixedUpate motion
    bool motion = false;

    ///Cloak variables
    bool cloaked = true;
    Color normalColor = new Color(1, 1, 1, 1);
    Color cloakedColor = new Color(1, 1, 1, 0.05f);

    //Public variables
    public int enemySpeed;
    public int enemyCloakSpeed;
    public float enemyRange;
    public float enemyMeleeRange;
    public float enemySwitchRange;
    public int distractionRange;
    public float attackInterval;
    public float meleeAttackInterval;
    public int meleeDamage;
    public int bulletDamage;  //Currently inconsequential due to code organization
    public int bulletSpeed;
    public float bulletDecayTime;
    public string bulletTag;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "LoyalBullet" || other.gameObject.tag == "ResoluteBullet" || other.gameObject.tag == "PlayerBullet")
        {
            cloaked = false;
            EntityGraphicsObject.cloaked = false;
            SpriteRendererObject.color = normalColor;
            gameObject.transform.SetParent(enemyController, true);
        }
    }

    // Use this for initialization
    void Start () {

        //Initializes object components
        EnemyAimObject = gameObject.AddComponent<EnemyAim>();
        EnemyStealthAttackObject = gameObject.AddComponent<EnemyStealthAttack>();
        EnemyStealthMovementObject = gameObject.AddComponent<EnemyStealthMovement>();

        //Gets SpriteRenderer component
        SpriteRendererObject = gameObject.GetComponent<SpriteRenderer>();

        //Cached reference to Ally Parent and Player transforms
        allyController = GameObject.FindGameObjectWithTag("AllyParent").GetComponent<Transform>();
        hiddenController = GameObject.FindGameObjectWithTag("HiddenParent").GetComponent<Transform>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        //Reference to gameObject rigidbody
        enemyRB = gameObject.GetComponent<Rigidbody2D>();

        //Sets parent to hidden controller
        gameObject.transform.SetParent(hiddenController, true);
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

        //Runs when cloaked
        if (cloaked)
        {
            //Ensures object is invisible
            if (EntityGraphicsObject.cloaked == false || SpriteRendererObject.color != cloakedColor)
            {
                EntityGraphicsObject.cloaked = true;
                SpriteRendererObject.color = cloakedColor;
            }

            //Runs when target is within melee range; stops movement
            if (closestEntity != null && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemyMeleeRange, 2))
            {
                //Executes attack on closest target
                EnemyStealthAttackObject.EnemyMeleeAttackManager(closestEntity, meleeDamage, meleeAttackInterval);

                //Exits cloak
                cloaked = false;
                EntityGraphicsObject.cloaked = false;
                SpriteRendererObject.color = normalColor;
                gameObject.transform.SetParent(enemyController, true);

                //Stops movement
                motion = false;
            }
            else if (motion == false)  //Runs when target is out of range and when immobile
            {
                motion = true;
            }
        }
        else
        {
            //Runs when within melee range
            if (closestEntity != null && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemyMeleeRange, 2))
            {
                //Executes attack on closest target
                EnemyAimObject.EnemyAimFunction(closestEntity.position);
                EnemyStealthAttackObject.EnemyMeleeAttackManager(closestEntity, meleeDamage, meleeAttackInterval);

                if (motion)
                {
                    motion = false;
                }
            }
            //Runs when within distance to attempt melee attack
            else if (closestEntity != null && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemySwitchRange, 2))
            {
                EnemyAimObject.EnemyAimFunction(closestEntity.position);

                if (motion == false)
                {
                    motion = true;
                }
            }
            //Runs when within ranged attack distance, yet outside of range for attempting melee attack
            else if (closestEntity != null && EnemyStealthMovementObject.OnScreenTest() && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemyRange, 2))
            {
                //Aim and attack when in range
                EnemyAimObject.EnemyAimFunction(closestEntity.position);
                EnemyStealthAttackObject.FireBullet(transform.right, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false, attackInterval);

                //Stops movement
                if (motion)
                {
                    motion = false;
                }
            }
            else if (motion == false)  //Runs when target is out of range and when immobile
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
            if (cloaked)
            {
                if (closestEntity != null && EnemyStealthMovementObject.OnScreenTest() && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(distractionRange, 2))
                {
                    //Aim and move when out of range
                    EnemyAimObject.EnemyAimFunction(closestEntity.position);
                    EnemyStealthMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemyCloakSpeed);
                }
                else
                {
                    //Aim and move when out of range
                    EnemyAimObject.EnemyAimFunction(playerTransform.position);
                    EnemyStealthMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemyCloakSpeed);
                }
            }
            else
            {
                if (closestEntity != null && EnemyStealthMovementObject.OnScreenTest() && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(enemySwitchRange, 2))
                {
                    //Aim and move when out of range
                    EnemyAimObject.EnemyAimFunction(closestEntity.position);
                    EnemyStealthMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemySpeed);
                }
                else
                {
                    //Aim and move when out of range
                    EnemyAimObject.EnemyAimFunction(playerTransform.position);
                    EnemyStealthMovementObject.EnemyMovement(closestEntity, playerTransform, enemyRB, distractionRange, enemySpeed);
                }
            }
        }
    }
}
