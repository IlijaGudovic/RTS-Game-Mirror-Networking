using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthDispley : MonoBehaviour
{

    [SerializeField] private Helth hp = null;

    [SerializeField] private GameObject helthBar = null;
    [SerializeField] private Image helthBarImage = null;


    private void Awake()
    {

        hp.clientOnHpUpdate += hendleHelthUpdate;

    }

    private void OnDestroy()
    {

        hp.clientOnHpUpdate -= hendleHelthUpdate;

    }


    private void OnMouseEnter()
    {
        helthBar.SetActive(true);
    }

    private void OnMouseExit()
    {
        helthBar.SetActive(false);
    }


    private void hendleHelthUpdate(int currentHp , int maxHp)
    {

        helthBarImage.fillAmount = (float)currentHp / maxHp;

    }


}
