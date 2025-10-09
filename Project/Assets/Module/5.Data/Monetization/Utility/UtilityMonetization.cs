public static class UtilityMonetization
{
    public static float GetPriceFromTag(PriceTag tag)
    {
        switch (tag)
        {
            case PriceTag.US_0_99: return 0.99f;
            case PriceTag.US_1_99: return 1.99f;
            case PriceTag.US_2_99: return 2.99f;
            case PriceTag.US_4_99: return 4.99f;
            case PriceTag.US_7_99: return 7.99f;
            case PriceTag.US_9_99: return 9.99f;
            case PriceTag.US_14_99: return 14.99f;
            case PriceTag.US_19_99: return 19.99f;
            case PriceTag.US_29_99: return 29.99f;
            case PriceTag.US_39_99: return 39.99f;
            case PriceTag.US_49_99: return 49.99f;
            case PriceTag.US_69_99: return 69.99f;
            case PriceTag.US_79_99: return 79.99f;
            case PriceTag.US_99_99: return 99.99f;
            default: return 0;
        }
    }
}
