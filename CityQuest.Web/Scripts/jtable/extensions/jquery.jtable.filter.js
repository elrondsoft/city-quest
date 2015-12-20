(function ($) {

    var base = {
        _create: $.hik.jtable.prototype._create
    };

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
                                // .css('display', 'none')
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
                        else if (currentFilterObject.type == 'datetime') {
                            self._createDateTime(currentFilterObject);
                        }
                        else if (currentFilterObject.type == 'select') {
                            self._createSelect(currentFilterObject);
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
                    var label = $('<label/>')
                        .addClass('cq-filter-label')
                        .text(inputObject.label)
                        .appendTo(leftContainer);
                }

                if (!inputObject.id) {
                    $.extend(inputObject, { id: self._getRandomGuid() });
                }

                var input = $('<input/>')
                    .addClass('form-control cq-filter-input')
                    .attr('id', inputObject.id)
                    .appendTo(rightContainer);
            }
        },

        _createSelect: function (selectObject) {
            var self = this;
            if (selectObject) {
                var filterContainer = $('<div/>')
                                    .addClass('col-md-3 cq-filter-container')
                                    .appendTo(self.$_currentFilterRow);

                var labelContainer = $('<div/>')
                                    .addClass('cq-filter-label-container')
                                    .appendTo(filterContainer);

                var selectContainer = $('<div/>')
                                    .addClass('cq-filter-select-container')
                                    .appendTo(filterContainer);

                if (selectObject.label) {
                    var label = $('<label/>')
                                    .addClass('cq-filter-label')
                                    .text(selectObject.label)
                                    .appendTo(labelContainer);
                }

                if (!selectObject.id) {
                    $.extend(selectObject, { id: self._getRandomGuid() });
                }

                var select = $('<select/>')
                                    .attr('id', selectObject.id)
                                    .addClass('selectpicker form-control')
                                    .attr('multiple', true)
                                    .css('height', '44px')
                                    .appendTo(selectContainer);

                if (selectObject.options) {
                    var data = [];

                    if (Object.prototype.toString.call(selectObject.options) === '[object Array]') {
                        data = selectObject.options;
                    }

                    if (Object.prototype.toString.call(selectObject.options) === '[object Function]') {
                        var params = selectObject.optionsParams || {};
                        selectObject.options(params).success(function (outputData) {
                            if (Object.prototype.toString.call(outputData.items) === '[object Array]') {
                                data = outputData.items;
                                var selectEntry = $('#' + selectObject.id);

                                for (key in data) {
                                    selectEntry
                                        .append($("<option></option>")
                                        .attr("value", data[key].value)
                                        .text(data[key].displayText));
                                }

                            }
                        });
                    }
                }
            }
        },

        _createDateTime: function (inputObject) {
            var self = this;
            if (inputObject) {
                var filterContainer = $('<div/>')
                                    .addClass('col-md-3 cq-filter-container')
                                    .appendTo(self.$_currentFilterRow);

                var labelContainer = $('<div/>')
                                    .addClass('cq-filter-label-container')
                                    .appendTo(filterContainer);

                var selectContainer = $('<div/>')
                                    .addClass('cq-filter-datetime-container')
                                    .appendTo(filterContainer);

                if (inputObject.label) {
                    var label = $('<label/>')
                                    .addClass('cq-filter-label')
                                    .text(inputObject.label)
                                    .appendTo(labelContainer);
                }

                if (!inputObject.id) {
                    $.extend(inputObject, { id: self._getRandomGuid() });
                }

                var datetime = $('<input/>')
                    .addClass('form-control cq-filter-datetime')
                    .attr('id', inputObject.id)
                    .appendTo(filterContainer)
                    .datetimepicker({
                        timepicker: true,
                        format: 'Y-m-d H:i'
                    });

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

                var controlValue;

                if (currentFilter.assignedField == "DateStart") {
                    var val = $('#' + currentFilter.id).val();
                    
                    controlValue = moment(val, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DDTHH:mm:ss');
                } else {
                    controlValue = $('#' + currentFilter.id).val();
                }

                if (!controlValue)
                    continue;

                object[currentFilter.assignedField] = controlValue;
                
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
// cq-filter-applybtn // custom