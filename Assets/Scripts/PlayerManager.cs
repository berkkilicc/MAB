using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlayerManager : MonoBehaviour
{

    private Vector3 direction;
    private Camera Cam;
    [SerializeField] private float PlayerSpeed;

    [SerializeField] private List<Transform> oils = new List<Transform>();
    [SerializeField] private Transform oilsPlace;
    private float Yaxis, delay;

    
    void Start()
    {
        Cam = Camera.main;

        oils.Add(oilsPlace);
    }

    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray,out var distance))
            
                direction = ray.GetPoint(distance);

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x, 0f, direction.z), PlayerSpeed * Time.deltaTime);

            var offset = direction - transform.position;

            if (offset.magnitude > 1f)
                transform.LookAt(direction);

        }

        if (oils.Count > 1)
        {
            for (int i = 1; i < oils.Count; i++)
            {
                var firstOil = oils.ElementAt(i - 1);
                var secondOil = oils.ElementAt(i);

                secondOil.position = new Vector3(Mathf.Lerp(secondOil.position.x, firstOil.position.x, Time.deltaTime * 15f),
                    Mathf.Lerp(secondOil.position.y, firstOil.position.y + 0.05f, Time.deltaTime * 15f), firstOil.position.z);
            }
        }


        if (Physics.Raycast(transform.position,transform.forward,out var hit,1f))
        {
            Debug.DrawRay(transform.position, transform.forward * 1f,Color.green);
            if (hit.collider.CompareTag("oilstable") && oils.Count <5)
            {
                if (hit.collider.transform.childCount > 2)
                {
                    var oil = hit.collider.transform.GetChild(1);
                    oil.rotation = Quaternion.Euler(oil.rotation.x, Random.Range(0f, 180f), oil.rotation.z);
                    oils.Add(oil);
                    oil.parent = null;

                    if (hit.transform.parent.GetComponent<OilsMachine>().CountOils > 1)
                        hit.transform.parent.GetComponent<OilsMachine>().CountOils--;

                    if (hit.transform.parent.GetComponent<OilsMachine>().YAxis > 0)
                        hit.transform.parent.GetComponent<OilsMachine>().YAxis -= 0.05f;
                }
                
            }

            if (hit.collider.CompareTag("op"))
            {
                var oilPlace = hit.collider.transform;

                if (oilPlace.childCount >0)
                {
                    Yaxis = oilPlace.GetChild(oilPlace.childCount - 1).position.y;
                }
                else
                {
                    Yaxis = oilPlace.position.y;
                }

                for(var index = oils.Count-1; index >=1; index--)
                {
                    oils[index].DOJump(new Vector3(oilPlace.position.x, Yaxis, oilPlace.position.z), 2f, 1, 0.5f).SetDelay(delay).SetEase(Ease.Flash);
                    
                    Yaxis += 0.17f;
                    delay += 0.2f;
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f,Color.red);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
