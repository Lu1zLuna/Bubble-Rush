using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool canShoot;
    public float speed = 25f;
    
    public Transform nextBubblePosition;
    public GameObject currentBubble;
    public GameObject nextBubble;
    public GameObject bottomShootPoint;

    public Vector2 lookDirection;
    public float lookAngle;
    public GameObject line;
    public GameObject limit;
    public LineRenderer lineRenderer;
    public Vector3 mousePosition;
    
    public void Awake()
    {
        if (line == null)
            line = GameObject.FindGameObjectWithTag("Line");

        if (limit == null)
            limit = GameObject.FindGameObjectWithTag("Limit");

        if (line == null) Debug.LogError("🚨 Objeto 'Line' não encontrado!");
        if (limit == null) Debug.LogError("🚨 Objeto 'Limit' não encontrado!");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Update rodando");
        // Debug.Log("Camera.main: " + Camera.main);
        // Debug.Log("bottomShootPoint: " + bottomShootPoint);
        // Debug.Log("limit: " + limit);

        if (GameManager.instance.gameState == "play")
        {
            // Pega a posição do mouse na tela
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lookDirection = mousePosition - transform.position;
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            if (Input.GetMouseButton(0)
                && mousePosition.y > bottomShootPoint.transform.position.y
                && mousePosition.y < limit.transform.position.y)
            {
                line.transform.position = transform.position;
                line.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90);

                if (LevelManager.instance != null
                    && LevelManager.instance.GetBubbleAreaChildCount() > 0) // implementar
                {
                    line.SetActive(true);
                }
                else
                {
                    line.SetActive(false);
                }

                if (canShoot
                    && Input.GetMouseButtonUp(0)
                    && mousePosition.y > bottomShootPoint.transform.position.y
                    && mousePosition.y < limit.transform.position.y)
                {
                    canShoot = false;
                    Shoot();
                }

            }

        }
    }
    

    public void Shoot()
    {
        if (currentBubble == null)
        {
            CreateNextBubble();
        }

        ScoreManager.GetInstance().AddThrows();
        AudioManager.instance.PlaySound("shoot");
        
        // Aplica a rotação calculada para a bolha atual
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle + 90);
        currentBubble.transform.rotation = transform.rotation;
        
        // Ativa e aplica colisão
        currentBubble.GetComponent<CircleCollider2D>().enabled = true;
        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        rb.gravityScale = 0f;
        
        // Impulso da bolha
        rb.AddForce(currentBubble.transform.up * speed, ForceMode2D.Impulse);
        
        currentBubble = null;
        
    }
    
    public void SwapBubble()
    {
        List<GameObject> bubbleInScene = LevelManager.instance.bubblesInScene;
        if (bubbleInScene.Count < 1) return;
        
        currentBubble.transform.position = nextBubblePosition.position;
        nextBubble.transform.position = transform.position;
        GameObject temp = currentBubble;
        currentBubble = nextBubble;
        nextBubble = temp;
    }

    public void CreateNewBubbles()
    {
        if (nextBubble != null)
        {
            Destroy(nextBubble);
        }

        if (currentBubble != null)
        {
            Destroy(currentBubble);
        }

        nextBubble = null;
        currentBubble = null;
        CreateNextBubble();
        canShoot = true;
    }


    public void CreateNextBubble()
    {
        List<GameObject> bubblesInScene = LevelManager.instance.bubblesInScene;
        List<string> colors = LevelManager.instance.colorsInScene;
        
        if (bubblesInScene.Count < 1) return;

        if (nextBubble == null)
        {
            nextBubble = InstantiateNewBubble(bubblesInScene);
        }
    }

    private GameObject InstantiateNewBubble(List<GameObject> bubblesInScene)
    {
        if (bubblesInScene.Count > 0)
        {
            GameObject newBubble = Instantiate(bubblesInScene[Random.Range(0, bubblesInScene.Count)]);
            newBubble.transform.position = nextBubblePosition.position;
            newBubble.GetComponent<Bubble>().isFixed = false;
            newBubble.GetComponent<CircleCollider2D>().enabled = false;
            Rigidbody2D rb2d = newBubble.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
            rb2d.gravityScale = 0f;
            return newBubble;
        }
        else
        {
            return null;
        }

    }
}
