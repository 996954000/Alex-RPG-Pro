using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthBarSlider;
    private EntityStat entityStat;
    // Start is called before the first frame update
    void Start()
    {
        healthBarSlider = GetComponentInChildren<Slider>();
        entityStat = GetComponentInParent<EntityStat>();

        /* ¶©ÔÄÊÂ¼þ */
        entityStat.OnUpdateHealthUI += HandleUpdateHealthBar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleUpdateHealthBar(Stat maxHealth, Stat currentHealth)
    {
        healthBarSlider.value = currentHealth.getValue() / maxHealth.getValue();
    }

    private void OnDestroy()
    {
        entityStat.OnUpdateHealthUI -= HandleUpdateHealthBar;
    }
}
