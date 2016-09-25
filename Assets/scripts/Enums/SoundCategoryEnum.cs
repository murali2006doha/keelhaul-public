public enum SoundCategoryEnum
{
    Generic,Chinese,Atlantean,Blackbeard,KrakenStageOne,KrakenStageTwo,KrakenStageThree
}

public class CategoryHelper
{

    public static SoundCategoryEnum convertType(ShipEnum shipType)
    {
        if(shipType == ShipEnum.AtlanteanShip)
        {
            return SoundCategoryEnum.Atlantean;
        }
        if(shipType == ShipEnum.BlackbeardShip)
        {
            return SoundCategoryEnum.Blackbeard;
        }
        if(shipType == ShipEnum.ChineseJunkShip)
        {
            return SoundCategoryEnum.Chinese;
        }
        return SoundCategoryEnum.Generic;
    }
}