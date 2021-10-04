

public class SaveState 
{
    public int currency = 0; //Amount of currency per save 
    public int completedLevels = 0; //Levels completed per save 

    public int collectedCurrency = 0; //Collected currency for each level played

    public int cpCollected = 0; //Total number of checkpoints collected

    //Using binary to check skins/trails owned, starts with 0 colours owned
    public int colourOwned = 0; 

    public int activeColour = 0;

    public bool usingAccelerometer = true; //For users without accelerometer within their device

}
