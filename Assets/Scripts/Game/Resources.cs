using System;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    [SerializeField] private Text massCountText;
    [SerializeField] private Text energyCountText;

    private Bank _bank;
    private void Awake()
    {
        _bank = GameObject.FindWithTag("Player").GetComponent<Player>().Bank;

        _bank.OnMassChangedEvent += OnChangeMassTextVariable;
    }

    public void OnChangeMassTextVariable(object sender, int oldValue, int newValue)
    {
        massCountText.text = newValue.ToString();
    }
    
    public void OnChangeEnergyTextVariable(object sender, int oldValue, int newValue)
    {
        energyCountText.text = newValue.ToString();
    }
}
