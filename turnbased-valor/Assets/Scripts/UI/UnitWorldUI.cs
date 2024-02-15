using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ShootAction shootAction;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void DamagePopup()
    {


        // Obter o dano atualizado da classe ShootAction
        //int damage = shootAction.CalculateDamage();

        // Atualizar o texto do dano
        //damageText.text = damage.ToString();

        // Obter o transform do GameObject pai desejado
        Transform parentTransform = transform; // Por exemplo, o transform do GameObject atual

        // Instanciar o prefab com o transform pai
        GameObject damageTextObject = Instantiate(damageTextPrefab, parentTransform.position, parentTransform.rotation, parentTransform);

        // Configura o texto no prefab instanciado
        TextMeshProUGUI damageTextComponent = damageTextObject.GetComponent<TextMeshProUGUI>();
        if (damageTextComponent != null)
        {
            damageTextComponent.text = damageText.text;
        }

        Debug.Log("New Total Damage: " + damageText.text);
        //UptadeDamageText();


        //Após 1s
        Destroy(damageTextObject, 1f);
        

    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
        DamagePopup();
    }
}
