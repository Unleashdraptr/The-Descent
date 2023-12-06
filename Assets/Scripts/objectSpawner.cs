using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSpawner : MonoBehaviour
{
    [SerializeField] public GameObject objectToSpawn;
    [SerializeField] private bool grapplerExtended;
    [SerializeField] private bool hasGrapplingHook;
    [SerializeField] private float extend;
    [SerializeField] private GameObject hook;

    

    // Start is called before the first frame update
    void Start()
    {
        grapplerExtended = false;
        hasGrapplingHook = false;
    }

    // Update is called once per frame
    void Update()
    {
        extend = Input.GetAxisRaw("Launch Grapple");

        if (extend > 0 && grapplerExtended == false)
        {
            grapplerExtended = true;
            hook = Instantiate(objectToSpawn, transform.position+(calculateSpawnVector()*2), objectToSpawn.transform.rotation);
            Rigidbody2D ghrb = hook.GetComponent<Rigidbody2D>();
            grappling ghscript = hook.GetComponent<grappling>();
            ghrb.AddForce(calculateSpawnVector()*10, ForceMode2D.Impulse);
            hasGrapplingHook = true;
        }

        if (hasGrapplingHook == true)
        {
            if (extend < 0 && distance() < 1f)
            {
                Destroy(hook);
                hasGrapplingHook = false;
                grapplerExtended = false;
                
            }
        }
    }
    // Calculate where the grappling hook will be spawned and the direction to launch it in
    Vector3 calculateSpawnVector()
    {
        // The grappling hook will be launched towards the mouse pointer
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 forceVector = mousePos-transform.position;
        float forceSize = vector3ToScalar(forceVector);
        forceVector = forceVector/forceSize;
        Vector2 forceVector2 = new Vector2(forceVector.x,forceVector.y);
        return forceVector;
    }

    // Calculate the distance between the grappling hook and the player
    public float distance()
    {
        Vector3 position1 = transform.position;
        Vector3 position2 = hook.transform.position;
        return Mathf.Sqrt(Mathf.Pow(position2.x-position1.x,2)+Mathf.Pow(position2.y-position1.y,2));
    }

    // Does pythagoras' theorem for you
    public float vector3ToScalar(Vector3 input)
    {
        return Mathf.Sqrt(Mathf.Pow(input.x,2)+Mathf.Pow(input.y,2)+Mathf.Pow(input.z,2));
    }
}
