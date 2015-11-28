(function ($) {

    var base = {
        _create: $.hik.jtable.prototype._create
    };

    //extension members
    $.extend(true, $.hik.jtable.prototype, {
        options: {
            filters: [

            ],
            filterLoaded: function () { },
            messages: {}
        },

        $_mainFilterContainer: null,
        $_currentFilterRow: null,
        $_mainFilterButtonRow: null,

        _create: function () {
            var self = this;

            base._create.apply(this, arguments);
            if (self.options.filters && self.options.filters.length > 0) {
                self._createFilterContainer();

                if (self.options.filterLoaded)
                    self.options.filterLoaded();
            }
        },

        _createFilterContainer: function () {
            var self = this;

            var insertTo = self._$table;

            self.$_mainFilterContainer = $('<div/>')
								.attr('id', 'jtable-filter-panel')
                                .css('display', 'none')
								.addClass('cq-filter-panel container-fluid ')
								.insertBefore(insertTo);
            self._createFilterControls();
            self._createFilterLoadBtn();
        },

        _createFilterRow: function () {
            var row = $('<div/>')
						.addClass('row cq-filter-row');

            return row;
        },

        _createFilterControls: function () {
            var self = this;
            var iterator;
            var filters = self.options.filters;
            for (iterator = 0; iterator < filters.length; iterator++) {

                if (iterator % 4 == 0) {
                    self.$_currentFilterRow = self._createFilterRow();

                    self.$_currentFilterRow.appendTo(self.$_mainFilterContainer);
                }

                var currentFilterObject = filters[iterator] || undefined;
                if (currentFilterObject) {
                    if (currentFilterObject.type) {
                        if (currentFilterObject.type == 'input') {
                            self._createInput(currentFilterObject);
                        }
                        else if (currentFilterObject.type == 'combobox') {
                            self._createCombobox(currentFilterObject);
                        }
                        else if (currentFilterObject.type == 'dateTimePicker') {
                            self._createDateTimePicker(currentFilterObject);
                        }
                        else {
                            abp.message.error("Unsupported type.")
                        }
                    }
                    else {
                        abp.message.error("No filter type");
                    }
                }
            }
        },

        _createInput: function (inputObject) {
            var self = this;
            if (inputObject) {
                var inputContainer = $('<div/>')
									.addClass('col-sm-3 cq-filter-container')
									.appendTo(self.$_currentFilterRow);

                var leftContainer = $('<div/>')
                                    .addClass('cq-filter-label-container')
									.appendTo(inputContainer);

                var rightContainer = $('<div/>')
                                    .addClass('cq-filter-input-container')
									.appendTo(inputContainer);

                if (inputObject.label) {
                    var label = $('<label/>').addClass('cq-filter-label').text(inputObject.label).appendTo(leftContainer);
                }

                if (!inputObject.id) {
                    $.extend(inputObject, { id: self._getRandomGuid() });
                }

                var input = $('<input/>').addClass('form-control cq-filter-input').attr('id', inputObject.id).appendTo(rightContainer);
            }
        },

        _createCombobox: function (comboboxObject) {
            var self = this;
            if (comboboxObject) {
                var comboBoxContainer = $('<div/>')
									.addClass('col-md-3')
									.appendTo(self.$_currentFilterRow);

                var leftContainer = $('<div/>')
									.appendTo(comboBoxContainer);

                var rightContainer = $('<div/>')
									.appendTo(comboBoxContainer);

                if (comboboxObject.label) {
                    var label = $('<label/>').addClass('jtable-filter-label').text(comboboxObject.label).appendTo(leftContainer);
                }

                if (!comboboxObject.id) {
                    $.extend(comboboxObject, { id: self._getRandomGuid() });
                }

                var combobox = $('<select/>').attr('id', comboboxObject.id);
                if (comboboxObject.multiple) {
                    combobox = combobox.attr('multiple', true);
                }
                combobox = combobox.addClass('form-control').appendTo(rightContainer);

                if (comboboxObject.method) {

                    var _method = comboboxObject.methodParams
                        ? comboboxObject.method(comboboxObject.methodParams)
                        : comboboxObject.method({});

                    _method.done(function (data) {
                        var options = data;

                        if (comboboxObject.map)
                            options = self._mapOptions(options, comboboxObject.map);

                        self._appendOptionsToComboBox(combobox, options, !comboboxObject.multiple);

                        //combobox.SumoSelect(abp.appConfig.sumoselectConfig);
                    })
						.fail(function (error) {
						    abp.message.error("Can't get options for filter : " + (comboboxObject.label || comboboxObject.assignedField));
						});
                }
            }
        },

        _createDateTimePicker: function (dateTimePickerObject) {
            var self = this;
            if (dateTimePickerObject) {
                var inputContainer = $('<div/>')
									.addClass('col-xs-3')
									.appendTo(self.$_currentFilterRow);

                var leftContainer = $('<div/>')
									.css('float', 'left')
									.appendTo(inputContainer);

                var rightContainer = $('<div/>')
									.css('float', 'right')
									.appendTo(inputContainer);

                if (dateTimePickerObject.label) {
                    var label = $('<label/>')
                        .addClass('jtable-filter-label')
                        .text(dateTimePickerObject.label)
                        .appendTo(leftContainer);
                }

                if (!dateTimePickerObject.id) {
                    $.extend(dateTimePickerObject, { id: self._getRandomGuid() });
                }

                var input = $('<input/>')
                    .addClass('cq-input')
                    .attr('id', dateTimePickerObject.id)
                    .width(dateTimePickerObject.width || '150px')
                    .appendTo(rightContainer);

                leftContainer.css('max-width', inputContainer.width() - input.width() - 6);
                // Using to set DateTimePicker default value 
                var setDateTimePickerDefaultValue = function () {
                    if (dateTimePickerObject.defaultValue) {
                        input.val(dateTimePickerObject.defaultValue);
                    }
                }
                setDateTimePickerDefaultValue();
                // Using to creta DateTimePicker from  input using options
                var createDateTimePicker = function () {
                    var getDateTimePickerSettingsDefaultObject = function () {
                        var dateTimePickerSettingsObject = {
                            timepicker: true,
                            datepicker: true,
                            format: 'm.d.Y H:i',
                            allowBlank: true
                        };
                        return dateTimePickerSettingsObject;
                    }
                    var getDateTimePickerCustomSettingsObject = function (options) {
                        var customSettingsObject = getDateTimePickerSettingsDefaultObject();
                        var keys = Object.keys(options);
                        for (var i = 0; i < keys.length; i++) {
                            customSettingsObject[keys[i]] = options[keys[i]];
                        }
                        return customSettingsObject;
                    }
                    var dateTimePickerSettingsObject;
                    if (dateTimePickerObject.options) {
                        dateTimePickerSettingsObject = getDateTimePickerCustomSettingsObject(dateTimePickerObject.options);
                    }
                    else {
                        dateTimePickerSettingsObject = getDateTimePickerSettingsDefaultObject();
                    }
                    input.datetimepicker(dateTimePickerSettingsObject);
                }
                createDateTimePicker();
            }
        },

        _mapOptions: function (options, mapper) {
            var self = this;
            var result = [];
            var iterator;
            for (iterator = 0; iterator < options.length; iterator++) {
                var currentOption = options[iterator];
                var newOption = {
                    id: currentOption[mapper.id],
                    text: currentOption[mapper.text]
                }
                result.push(newOption);
            }

            return result;
        },

        _appendOptionsToComboBox: function (comboboxJquery, optionsToAppend, addEmptyOption) {
            var self = this;
            var iterator;
            if (addEmptyOption)
                var emptyOption = $('<option/>').appendTo(comboboxJquery);

            for (iterator = 0; iterator < optionsToAppend.length; iterator++) {
                var currentOption = optionsToAppend[iterator];

                var option = $('<option/>').text(currentOption.text).val(currentOption.id).appendTo(comboboxJquery);
            }
        },

        _getRandomGuid: function () {
            var guid = Math.floor((1 + Math.random()) * 0x10000)
			   .toString(16)
			   .substring(1)
			   .toString();

            return guid;
        },

        _createFilterLoadBtn: function () {
            var self = this;

            self.$_mainFilterButtonRow = $('<div/>')
								.addClass('row')
								.appendTo(self.$_mainFilterContainer);

            var container = $('<div/>')
                            .addClass('col-md-3 col-md-offset-9 cq-filter-applybtn-container')
                            .appendTo(self.$_mainFilterButtonRow);

            var button = $('<button/>')
				.addClass('btn btn-success cq-filter-applybtn')
                .css('float', 'right')
				.text(self.options.messages.apply)
				.click(function () {
				    var filter = self._createFilterObject();
				    self.load(filter);
				})
				.appendTo(container);
        },

        _createFilterObject: function () {
            var self = this;
            var object = {};
            var iterator;
            for (iterator = 0; iterator < self.options.filters.length; iterator++) {
                var currentFilter = self.options.filters[iterator];
                if (!currentFilter.assignedField || !currentFilter.id) {
                    continue;
                }

                var controlValue = $('#' + currentFilter.id).val();

                if (!controlValue)
                    continue;

                object[currentFilter.assignedField] = controlValue;

                //Object.defineProperty(object, currentFilter.assignedField, {
                //	value: controlValue
                //});
            }

            return object;
        }
    });

})(jQuery);


// cq-filter-panel
// cq-filter-row
// cq-filter-container
// cq-filter-label-container
// cq-filter-input-container
// cq-filter-label
// cq-filter-input
// cq-filter-applybtn-container
// cq-filter-applybtn 