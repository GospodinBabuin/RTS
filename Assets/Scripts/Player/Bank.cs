using UnityEngine;

public class Bank : MonoBehaviour
{
    public delegate void BankHandler(object sender, int oldValue, int newValue);
    public event BankHandler OnMassChangedEvent;
    
    public int maxMass { get; private set; } 
    public int maxEnergy { get; private set; }
    public int currentMass { get; private set; }
    public int currentEnergy { get; private set; }

    public void AddMass(object sender, int amount)
    {
        int oldMassValue = this.currentMass;
        this.currentMass += amount;
        
        this.OnMassChangedEvent?.Invoke(sender, oldMassValue, this.currentMass);
    }

    public void SpendMass(object sender, int amount)
    {
        int oldMassValue = this.currentMass;
        this.currentMass -= amount;
        
        this.OnMassChangedEvent?.Invoke(sender, oldMassValue, this.currentMass);
    }

    public bool IsEnoughMass(int amount)
    {
        return amount <= currentMass;
    }
}
