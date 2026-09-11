using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public float speed = 2f;
    public Transform[] points;

    public float chainNuggetsDistance = 2f;
    [SerializeField] Transform chainNuggetsSpawnerTransform;

    public GameObject chainNugget;

    private int i;

    public bool isMovingPlatform = false;

    void Start()
    {
        transform.position = points[0].position;




        chainNuggetsSpawnerTransform.position = points[0].position;

        Instantiate(chainNugget, chainNuggetsSpawnerTransform.position, Quaternion.identity, transform.parent.Find("ChainNuggets"));


        foreach (Transform point in points)
        {

            

            while (chainNuggetsSpawnerTransform.position != point.position)
            {
                

               
                chainNuggetsSpawnerTransform.position = Vector2.MoveTowards(chainNuggetsSpawnerTransform.position, point.position, chainNuggetsDistance);

                Instantiate(chainNugget, chainNuggetsSpawnerTransform.position, Quaternion.identity, transform.parent.Find("ChainNuggets"));

            }

            
        }

        while (chainNuggetsSpawnerTransform.position != points[0].position && points.Length > 2)
        {



            chainNuggetsSpawnerTransform.position = Vector2.MoveTowards(chainNuggetsSpawnerTransform.position, points[0].position, chainNuggetsDistance);

            chainNugget = Instantiate(chainNugget, chainNuggetsSpawnerTransform.position, Quaternion.identity, transform.parent.Find("ChainNuggets"));

            chainNugget.GetComponent<SpriteRenderer>().sortingOrder = -1;

        }


    }


    void Update()
    {


        if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
        {
            i++;

            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isMovingPlatform)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isMovingPlatform)
        {
            collision.transform.SetParent(null);
        }
    }
}
