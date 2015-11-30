﻿namespace CityQuest
{
    public class CityQuestConsts
    {
        public const string LocalizationSourceName = "CityQuest";
        public const string ConnectionStringName = "CityQuest";
        public const string CityQuestBDContextKey = "CityQuest";
        public const string FakePassword = "~@%PSWRD#~*";
        public const string PermissionKey = "CityQuestPermissionKey";

        public const int MaxCountForKeyGeneration = 1000;
        
        #region Exception consts

        public const string CityQuestUserFriendlyStandartExceptionHeader = "Unexpected error!";
        public const string CityQuestUserFriendlyStandartExceptionBody = "Something went wrong during the last action. Please, try again or contact our system support.";

        #region CityQuest's policy exception consts

        public const string CityQuestPolicyExceptionMessageHeader = "Access denied!";

        public const string CQPolicyExceptionRetrieveDenied = "Your permissions do not allow you to perform this action: retrieve selected entity ({0}).";
        public const string CQPolicyExceptionCreateDenied = "Your permissions do not allow you to perform this action: create selected entity ({0}).";
        public const string CQPolicyExceptionUpdateDenied = "Your permissions do not allow you to perform this action: update selected entity ({0}).";
        public const string CQPolicyExceptionDeleteDenied = "Your permissions do not allow you to perform this action: delete selected entity ({0}).";
        public const string CQPolicyExceptionChangeActivityDenied = "Your permissions do not allow you to perform this action: change activity of selected entity ({0}).";
        
        #endregion

        #region CityQuest's item not found exception consts

        public const string CityQuestItemNotFoundExceptionMessageHeader = "Item not found!";
        public const string CityQuestItemNotFoundExceptionMessageBody = "Can not get access for selected item (entity {0}) due to technical problems. Please, try again or contact your system administrator (system support).";

        #endregion

        #endregion
    }
}