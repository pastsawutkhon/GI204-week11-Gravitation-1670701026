using UnityEngine;
using System.Collections.Generic;

public class Gravitation : MonoBehaviour
{
    public static List<Gravitation> otherObj;
    private Rigidbody _rb;
    private Transform sun;
    const float G = 0.667f;

    [SerializeField] bool isSun = false;
    [SerializeField] float orbitSpeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (otherObj == null)
        {
            otherObj = new List<Gravitation>(); 
        }
        otherObj.Add(this);

        GameObject sunObj = GameObject.Find("Sun");
        if (sunObj != null)
        {
            sun = sunObj.transform;
        }

        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
    }

    // Update is called once per frame
    void Start()
    {
        if (!isSun && sun != null)
        {
            Vector3 toSun = (sun.position - transform.position).normalized;
            Vector3 orbitDir = Vector3.Cross(toSun, Vector3.up).normalized;

            _rb.linearVelocity = orbitDir * orbitSpeed;
        }
    }
    void Update()
    {
        if (!isSun && sun != null)
        {
            transform.LookAt(sun);
        }
    }
    void FixedUpdate()
    {
        foreach (Gravitation obj in otherObj)
        {
            if (obj != this)
            {
                Attract(obj);
            }
        }

    }

    void Attract(Gravitation other)
    {
        Rigidbody otherRb = other._rb;
        Vector3 direction = _rb.position - otherRb.position;
        float distance = direction.magnitude;
        if (distance == 0f) return;

        // F = G(m1 * m2) / r^2 
        float forceMagnitude = G * (_rb.mass * otherRb.mass) / Mathf.Pow(distance, 2);
        Vector3 gravitationForce = forceMagnitude * direction.normalized;
        otherRb.AddForce(gravitationForce);
    }
}
