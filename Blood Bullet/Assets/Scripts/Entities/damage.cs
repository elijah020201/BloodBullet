using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour
{
    enum damageType { moving, stationary, DOT, homing }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] float damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] float speed;
    [SerializeField] float destroyTime;

    bool isDamaging;

    void Start()
    {
        if (type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject, destroyTime);

            if (type == damageType.moving)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }

    }

    void Update()
    {
        if (type == damageType.homing)
        {
            rb.linearVelocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type != damageType.DOT)
        {

            dmg.takeDamage(damageAmount);

        }
        if (type == damageType.homing || type == damageType.moving)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type == damageType.DOT & !isDamaging)
        {
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;

    }
}