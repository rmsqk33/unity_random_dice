namespace FEnum
{
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

    public enum BattlefieldPurchaseResult
    {
        BATTLEFIELD_PURCHASE_RESULT_SUCCESS,
        BATTLEFIELD_PURCHASE_RESULT_NOT_ENOUGH_MONEY,
        BATTLEFIELD_PURCHASE_RESULT_INVALID,
        BATTLEFIELD_PURCHASE_RESULT_ALEADY_ACQUIRED,
    }

    public enum DiceUpgradeResult
    {
        DICE_UPGRADE_RESULT_SUCCESS,
        DICE_UPGRADE_RESULT_INVALID_DICE,
        DICE_UPGRADE_RESULT_NOT_ENOUGH_MONEY,
        DICE_UPGRADE_RESULT_NOT_ENOUGH_DICE,
        DICE_UPGRADE_RESULT_MAX_LEVEL,
    }

    public enum ChangeNameResult
    {
        CHANGE_NAME_RESULT_SUCCESS,
        CHANGE_NAME_RESULT_ALEADY,
        CHANGE_NAME_RESULT_SPECIAL_CHARACTER,
        CHANGE_NAME_RESULT_BLANK,
    }

    public enum DiceUpgradeDisableReason
    {
        NOT_ENOUGH_SP,
        MAX_LEVEL,
    }

    public enum DiceSummonDisableReason
    { 
        NOT_ENOUGH_SP,
        NOT_EMPTY_SLOT,
    }

    public enum IFFType
    {
        Neutral,
        Enemy,
        RemotePlayer,
        LocalPlayer,
    }

    public enum EnemyType
    { 
        Normal,
        MiddleBoss,
        Boss
    }

    public enum SkillType
    {
        Basic,
        Abnormal,
        Summon,
        SummonBasic,
    }

    public enum SkillTargetType
    {
        None,
        Front,
        Myself,
        Path,
        DiceForEnemy,
    }

    public enum SkillEffectType
    {
        Normal,
        Damage,
        Chain,
    }

    public enum SceneType
    {
        None,
        Login,
        Lobby,
        Battle,
        Loading,
    }

    public enum CombatTextType
    {
        None,
        Normal,
        Critical,
        Max
    }

    public enum StorePriceType
    {
        Gold,
        Dia,
        Card,
        Max,
    }

    public enum StatType
    {
        None,
        HP,
        SP,
        MoveSpeed,
        CriticalChance,
        CriticalDamage,
        Level,
        DiceEye,
        AttackSpeed,
    }

    public enum AbnormalityType
    { 
        Stat,
        Damage,
        Lock,
    }

    public enum AbilityType
    {
        None,
        BasicAttackDamage,
        BasicAttackSpeed,
        BasicAttackTarget,
        FireExplosion,
        ChainLightning,
        Poison,
        Ice,
        Thorn,
    }

    public enum AbilitySlotType
    {
        BasicAttackDamage,
        BasicAttackSpeed,
        BasicAttackTarget,
        Max
    }

    public enum ProjectileMoveType
    {
        Normal,
        Curve,
    }

}