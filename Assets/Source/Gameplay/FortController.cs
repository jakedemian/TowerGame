using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortController : MonoBehaviour {
    public GameObject uiHealthBarPanel;
    public GameObject uiShieldPanel;

    private Vector2 uiHealthBarPanelMaxSize;

    private float fortHealth;
    private float currentShield;
    private float fortMaxHealth = 100f;

    void Start () {
        fortHealth = fortMaxHealth;
        currentShield = 0f;
        uiHealthBarPanelMaxSize = uiHealthBarPanel.GetComponent<RectTransform> ().rect.size;
        UpdateHealthBar ();
    }

    public void DoDamage (float dmg) {
        float damageLeftToDo = dmg;
        if (currentShield > 0f) {
            damageLeftToDo -= currentShield;
            if (damageLeftToDo > 0f) {
                currentShield = 0;
            } else {
                currentShield = currentShield - dmg;
            }
        }

        fortHealth -= damageLeftToDo;
        UpdateHealthBar ();

        if (fortHealth <= 0f) {
            fortHealth = 0f;
            // you lose!!
        }
    }

    public void SetShield (float shield) {
        if (currentShield > shield) {
            return;
        }
        currentShield = shield;
        UpdateHealthBar ();
    }

    private void UpdateHealthBar () {
        float newHealthBarWidth = (fortHealth / fortMaxHealth) * uiHealthBarPanelMaxSize.x;
        uiHealthBarPanel.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (newHealthBarWidth, uiHealthBarPanelMaxSize.y);

        float newShieldWidth = (currentShield / fortMaxHealth) * uiHealthBarPanelMaxSize.x;
        uiShieldPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2 (newShieldWidth, uiHealthBarPanelMaxSize.y);
    }

}

public static class ShieldHelper {
    private static FortController GetController () {
        return GameObject.FindGameObjectWithTag ("Fort").GetComponent<FortController> ();
    }

    public static void SetShield (float shield) {
        GetController ().SetShield (shield);
    }
}