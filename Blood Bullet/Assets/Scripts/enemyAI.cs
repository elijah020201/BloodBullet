using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] float HP;

    Material defaultMaterial;
    Color defaultColor;
    Color flashColor = new Color(1f, 0f, 0f, 1f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultMaterial = model.material;
        defaultColor = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Destroy(gameObject);
        } else
        {
            StartCoroutine(damageFlash());
        }
    }

    IEnumerator damageFlash()
    {
        model.material.color = flashColor;
        yield return new WaitForSeconds(0.05f);
        if (gameObject != null)
            model.material.color = defaultColor;
    }
}
