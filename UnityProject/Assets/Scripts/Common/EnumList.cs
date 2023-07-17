namespace RandomDice
{
    public enum FIELD_NUMBER
    {
        Normal = 0,
        FieldNumber_End
    }

    public enum DiceGrade
    {
        DICE_GRADE_NONE,
        DICE_GRADE_NORMAL,
        DICE_GRADE_RARE,
        DICE_GRADE_EPIC,
        DICE_GRADE_LEGEND,
    }

    public enum StorePurchaseResult
    {
        STORE_PURCHASE_RESULT_SUCCESS,
        STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY,
        STORE_PURCHASE_RESULT_SOLDOUT,
        STORE_PURCHASE_RESULT_INVALID_GOODS,
    }

    public enum DiceUpgradeResult
    {
        DICE_UPGRADE_RESULT_SUCCESS,
        DICE_UPGRADE_RESULT_INVALID_DICE,
        DICE_UPGRADE_RESULT_NOT_ENOUGH_MONEY,
        DICE_UPGRADE_RESULT_NOT_ENOUGH_DICE,
        DICE_UPGRADE_RESULT_MAX_LEVEL,
    };
}