using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyOffense : MonoBehaviour {

    public class AllyAim : MonoBehaviour
    {
        float closestTargetSqrDist = Mathf.Infinity;

        //LocateTarget variables
        Vector3 currentTargetVector;
        float currentTargetSqrDist;
        Transform closestEntityTransform;

        //Function returns Transform of nearest child of Enemy Parent
        public Transform LocateTarget(Transform enemyParent)
        {
            if (enemyParent.childCount == 0)
            {
                return null;
            }
            else
            {
                foreach (Transform child in enemyParent)
                {
                    currentTargetVector = child.position - transform.position;
                    currentTargetSqrDist = currentTargetVector.sqrMagnitude;

                    if (currentTargetSqrDist < closestTargetSqrDist)
                    {
                        closestTargetSqrDist = currentTargetSqrDist;
                        closestEntityTransform = child;
                    }
                }

                closestTargetSqrDist = Mathf.Infinity;

                return closestEntityTransform;
            }
        }

        //AllyAimFunction variables
        Vector3 targetAim;
        float aimAngle;
        Quaternion aimRotation;
        float turnSpeed = 8f;

        //Function aims towards vector3 arugment
        public void AllyAimFunction(Vector3 targetAim)
        {
            aimAngle = Vector3.Angle(Vector3.right, targetAim - transform.position);

            if (targetAim.y < transform.position.y)
            {
                aimAngle = aimAngle * -1;
            }

            aimRotation = Quaternion.Euler(0, 0, aimAngle);

            transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, turnSpeed * Time.deltaTime);
        }
    }

    public class AllyAttack : MonoBehaviour
    {
        //Declares local variable for controlling auto attack
        float attackTimerLocal = 0;

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

    public AllyAim AllyAimObject;

    public AllyAttack AllyAttackObject;

    public BulletScript bullet;

    //Searched Enemy Controller empty
    Transform enemyController;

    //Transform variable filled by LocateTarget method
    Transform closestEntity;

    //Timer for controlling target updating
    float checkTimer = 0f;

    //Public variables
    public float allyRange;
    public float attackInterval;
    public int bulletDamage;        //Currently inconsequential due to code organization
    public int bulletSpeed;
    public float bulletDecayTime;
    public string bulletTag;

    // Use this for initialization
    void Start () {

        AllyAimObject = gameObject.AddComponent<AllyAim>();

        AllyAttackObject = gameObject.AddComponent<AllyAttack>();

        enemyController = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<Transform>();

	}
	
	// Update is called once per frame
	void Update () {

        checkTimer -= Time.deltaTime;

        if (checkTimer <= 0 || closestEntity == null)
        {
            closestEntity = AllyAimObject.LocateTarget(enemyController);
            checkTimer = 0.5f;
        }


        if (closestEntity != null && Mathf.Pow(closestEntity.position.x - transform.position.x, 2) + Mathf.Pow(closestEntity.position.y - transform.position.y, 2) <= Mathf.Pow(allyRange, 2))
        {
            AllyAimObject.AllyAimFunction(closestEntity.position);
            AllyAttackObject.FireBullet(transform.right, bullet, bulletSpeed, bulletTag, bulletDamage, bulletDecayTime, false, attackInterval);
        }
	}
}
