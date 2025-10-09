public class UserEnergy
{
    public int dailyAdEnergy;
    public int dailyGemEnergy;
    public int energyRecoverTimer;

    public UserEnergy()
    {
        dailyAdEnergy = 0;
        dailyGemEnergy = 0;
    }

    public void OnResetDaily()
    {
        dailyAdEnergy = 0;
        dailyGemEnergy = 0;
    }
}
