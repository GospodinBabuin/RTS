
public class Factory : Building
{
    public override void ActivateMenu()
    {
        MenuManager.OpenMenu(MenuManager.FactoryMenu, this);
    }
}
