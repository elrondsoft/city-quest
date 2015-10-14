var abp = abp || {};
(function () {
    abp.appConfig = abp.appConfig || {};

    var localize = function (key) {
        return abp.localization.localize(key, 'CityQuest');
    };

    abp.appConfig.sumoselectConfig = {
        placeholder: localize('SelectHere'),   // Dont change it here.
        csvDispCount: 1,              // display no. of items in multiselect. 0 to display all.
        captionFormat: '{0} Selected', // format of caption text. you can set your locale.
        floatWidth: 400,              // Screen width of device at which the list is rendered in floating popup fashion.
        forceCustomRendering: false,  // force the custom modal on all devices below floatWidth resolution.
        nativeOnDevice: ['Android', 'BlackBerry', 'iPhone', 'iPad', 'iPod', 'Opera Mini', 'IEMobile', 'Silk'], //
        outputAsCSV: false,           // true to POST data as csv ( false for Html control array ie. deafault select )
        csvSepChar: ',',              // seperation char in csv mode
        okCancelInMulti: false,       //display ok cancel buttons in desktop mode multiselect also.
        triggerChangeCombined: true,  // im multi select mode wether to trigger change event on individual selection or combined selection.
        selectAll: false,             // to display select all button in multiselect mode.|| also select all will not be available on mobile devices.
        selectAlltext: localize('SelectAll')  // the text to display for select all.

    };
})();