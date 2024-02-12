using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI damageText;


    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDamageText()
    {
        damageText.text = "Olá";
    }


}
