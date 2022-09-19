using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class OilsMachine : MonoBehaviour
{
    [SerializeField] private Transform[] OilsPlace = new Transform[5];
    [SerializeField] private GameObject oil;
    public float oilsDeliveryTime,YAxis;
    public int CountOils;

    void Start()
    {
        for (int i = 0; i < OilsPlace.Length; i++)
        {
            OilsPlace[i] = transform.GetChild(0).GetChild(i);

        }

        StartCoroutine(MachineOil(oilsDeliveryTime));
    }

    public IEnumerator MachineOil(float Time)
    {
        
        var OP_index = 0;

        while (CountOils < 50)
        {
            GameObject newOils = Instantiate(oil, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, transform.GetChild(1));

            newOils.transform.DOJump(new Vector3(OilsPlace[OP_index].position.x, OilsPlace[OP_index].position.y + YAxis, OilsPlace[OP_index].position.z), 2f, 1, 0.5f).SetEase(Ease.OutQuad);

            if (OP_index <4)
            {
                OP_index++;
            }
            else
            {
                OP_index = 0;
                YAxis += 0.008f;
            }

            yield return new WaitForSecondsRealtime(Time);

        }
    }
}
