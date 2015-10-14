var abp = abp || {};
(function () {
    abp.appConfig = abp.appConfig || {};

    var localize = function (key) {
        return abp.localization.localize(key, 'CityQuest');
    };

    abp.appConfig.jtableMessages = {
        loadingMessage: localize('LoadingMessage'),
        serverCommunicationError: localize('ServerCommunicationError'),
        noDataAvailable: localize('NoDataAvailable'),
        addNewRecord: localize('AddNewRecord'),
        editRecord: localize('EditRecord'),
        areYouSure: localize('AreYouSure'),
        deleteConfirmation: localize('DeleteConfirmation'),
        save: localize('Save'),
        saving: localize('Saving'),
        cancel: localize('Cancel'),
        deleteText: localize('DeleteText'),
        deleting: localize('Deleting'),
        error: localize('Error'),
        close: localize('Close'),
        cannotLoadOptionsFor: localize('CannotLoadOptionsFor'),
        pagingInfo: localize('PagingInfo'),
        canNotDeletedRecords: localize('CanNotDeletedRecords'),
        deleteProggress: localize('DeleteProggress'),
        pageSizeChangeLabel: localize('PageSizeChangeLabel'),
        gotoPageLabel: localize('GotoPageLabel'),
        apply: localize('Apply')
    };
})();