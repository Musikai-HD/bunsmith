using System;
using TMPro;
using UnityEngine;

public class 

DamageIndicator : MonoBehaviour
{
    public Vector3 startingPos;
    [SerializeField] TextMeshPro damageText;
    bool fadingOut;
    [SerializeField] Vector2 curSpeed;
    public float maxHSpeed, maxVSpeed, grav, maxGrav, maxFallSpeed;

    public void Awake()
    {
        startingPos = transform.position;
    }

    void Start()
    {
        Invoke("FadeOut", 1f);
        Invoke("Die", 2f);
    }

    void Update()
    {
        if (fadingOut)
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, damageText.color.a - Time.deltaTime * 1.5f);
        }
        transform.position += new Vector3(curSpeed.x, curSpeed.y, 0f) * Time.deltaTime;
        curSpeed.y -= grav * Time.deltaTime;
        curSpeed.x = Mathf.Lerp(curSpeed.x, 0f, Time.deltaTime);
        if (curSpeed.y < -maxFallSpeed) {curSpeed.y = -maxFallSpeed;}
        if (transform.position.y < startingPos.y - 0.5f && curSpeed.y < 0f)
        {
            curSpeed.y *= -0.5f;
        }
    }

    public void SetIndicator(float damage, Vector2 startVariance)
    {
        curSpeed = new Vector2(UnityEngine.Random.Range(-maxHSpeed, maxHSpeed), UnityEngine.Random.Range(maxVSpeed * 0.5f, maxVSpeed));
        damageText.text = damage % 1f == 0f ? damage.ToString("0") : damage.ToString("0.0");
        transform.position += new Vector3(UnityEngine.Random.Range(-startVariance.x * 0.5f, startVariance.x * 0.5f), startVariance.y + UnityEngine.Random.Range(-startVariance.y * 0.5f, startVariance.y * 0.5f), 0f);
    }

    void FadeOut()
    
    {
        fadingOut = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
