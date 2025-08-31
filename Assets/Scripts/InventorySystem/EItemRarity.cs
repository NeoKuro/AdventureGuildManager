public enum EItemRarity
{
    Common    = 0,
    Uncommon  = 1,
    Rare      = 2,
    Epic      = 3,
    Legendary = 4,

}

public static class EItemRarityExtensions
{
    public static float GetRarityScoringValue(this EItemRarity rarity)
    {
        switch (rarity)
        {
            default:
            case EItemRarity.Common:
                return 0.8f;
            case EItemRarity.Uncommon:
                return 1f;
            case EItemRarity.Rare:
                return 1.1f;
            case EItemRarity.Epic:
                return 1.25f;
            case EItemRarity.Legendary:
                return 1.5f;
        }
    }
}