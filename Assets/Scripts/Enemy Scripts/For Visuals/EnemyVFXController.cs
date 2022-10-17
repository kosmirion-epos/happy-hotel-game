using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyVFXController : MonoBehaviour
{
    EnemyBaseVariation enemy;
    [SerializeField] Transform head;
    [SerializeField] Transform body;
    [SerializeField] bool headshot;
    [SerializeField] bool bodyshot;
    [SerializeField] GameObject headParts;
    [SerializeField] GameObject bodyParts;



    public void TriggerHeadshot() {
        StartCoroutine(PlayHeadDestroy());

    }



    public void TriggerBodyshot()
    {
        StartCoroutine(PlayBodyDestroy());
    }


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyBaseVariation>();
    }

    // Debugg
    void Update()
    {
        if (headshot)
        {
            headshot = false;
            StartCoroutine(PlayHeadDestroy());
        }

        if (bodyshot)
        {
            bodyshot = false;
            StartCoroutine(PlayBodyDestroy());
        }

    }


    IEnumerator PlayHeadDestroy()
    {
        //Disable the head mesh
        head.gameObject.SetActive(false);

        //Instantiate the destroyed head prefab 
        GameObject h = Instantiate(headParts, head.transform.position, head.transform.rotation);

        yield return new WaitForSeconds(.2f);

        //Disable the body mesh
        body.gameObject.SetActive(false);

        //Instantiate the destroyed body prefab 
        GameObject b = Instantiate(bodyParts, body.parent);
        b.transform.localPosition = body.localPosition;
        b.transform.localRotation = body.localRotation;

        //Deisable head object as it's already spawned
        b.transform.GetChild(0).gameObject.SetActive(false);

        //Reduce Impactforce
        EnemyPart[] parts = b.GetComponentsInChildren<EnemyPart>();
        foreach(EnemyPart p in parts)
        {
            p.initalForce = .1f;
        }

        yield return new WaitForSeconds(2f);

        //Destroy all spwaned parts
        Destroy(h);
        Destroy(b);

        //Despawn Enemy after VFX
        if (enemy)
            enemy.Despawn(false);



    }


    IEnumerator PlayBodyDestroy()
    {
        //Disable the head and body mesh
        body.gameObject.SetActive(false);
        head.gameObject.SetActive(false);

        //Instantiate the destroyed body prefab 
        GameObject b = Instantiate(bodyParts, body.parent);
        b.transform.localPosition = body.localPosition;
        b.transform.localRotation = body.localRotation;

        yield return new WaitForSeconds(2f);

        //Destroy all spwaned parts
        Destroy(b);

        //Despawn Enemy after VFX
        if (enemy)
            enemy.Despawn(false);

    }




    /*
        if (rotateshot)
        {
            rotateshot = false;
            StartCoroutine(PlayVFX());
        }


        if (!canBeShot)
            return; 



        [SerializeField] float duration = 1f;
        [SerializeField] float speed = 55f;
        IEnumerator PlayVFX()
        {
            canBeShot = false;

            for (float i = 0; i < duration; i += 0.1f)
            {

                head.transform.RotateAround(head.transform.position, transform.right, rotationSpeed.Evaluate(i / duration) * speed);
                //head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x + rotationSpeed.Evaluate(i/5) * 15f, head.transform.eulerAngles.y, head.transform.eulerAngles.z);
                yield return new WaitForSeconds(.1f);
            }

            canBeShot = true; 

        }
    */

}
