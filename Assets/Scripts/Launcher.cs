using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform firePoint;
    public float bubbleSpeed = 10f;

    void Update()
    {
        Aim();

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void Shoot()
    {
        GameObject bubble = Instantiate(bubblePrefab, firePoint.position, firePoint.rotation);
        bubble.GetComponent<Rigidbody2D>().linearVelocity = firePoint.up * bubbleSpeed;
    }
}
