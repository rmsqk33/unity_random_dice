#pragma once

enum DiceGrade
{
	DICE_GRADE_NONE,
	DICE_GRADE_NORMAL,
	DICE_GRADE_RARE,
	DICE_GRADE_EPIC,
	DICE_GRADE_LEGEND,
	DICE_GRADE_MAX,
};

enum StorePurchaseResult
{
	STORE_PURCHASE_RESULT_SUCCESS,
	STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY,
	STORE_PURCHASE_RESULT_SOLDOUT,
	STORE_PURCHASE_RESULT_INVALID_GOODS,
};

enum BattlefieldPurchaseResult
{
	BATTLEFIELD_PURCHASE_RESULT_SUCCESS,
	BATTLEFIELD_PURCHASE_RESULT_NOT_ENOUGH_MONEY,
	BATTLEFIELD_PURCHASE_RESULT_INVALID,
	BATTLEFIELD_PURCHASE_RESULT_ALEADY_ACQUIRED,
};

enum DiceUpgradeResult
{
	DICE_UPGRADE_RESULT_SUCCESS,
	DICE_UPGRADE_RESULT_INVALID_DICE,
	DICE_UPGRADE_RESULT_NOT_ENOUGH_MONEY,
	DICE_UPGRADE_RESULT_NOT_ENOUGH_DICE,
	DICE_UPGRADE_RESULT_MAX_LEVEL,
};

enum ChangeNameResult
{
	CHANGE_NAME_RESULT_SUCCESS,
	CHANGE_NAME_RESULT_ALEADY,
	CHANGE_NAME_RESULT_SPECIAL_CHARACTER,
	CHANGE_NAME_RESULT_BLANK,
};

enum ControllerType
{
	CONTROLLER_TYPE_NONE,
	CONTROLLER_TYPE_INVENTORY,
	CONTROLLER_TYPE_STORE,
	CONTROLLER_TYPE_DICE,
	CONTROLLER_TYPE_BATTLEFIELD,
	CONTROLLER_TYPE_STAT,
};

enum StorePriceType
{
	STORE_PRICE_TYPE_GOLD,
	STORE_PRICE_TYPE_DIA,
	STORE_PRICE_TYPE_CARD,
};