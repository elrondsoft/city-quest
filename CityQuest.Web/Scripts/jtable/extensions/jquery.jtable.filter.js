(function ($) {

    var base = {
        _create: $.hik.jtable.prototype._create
    };

    /* refactor that shit */

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
                        else if (currentFilterObject.type == 'multiselect') {
                            self._createMultiselect(currentFilterObject);
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
									.addClass('col-md-3 col-sm-12 cq-filter-container form-group')
									.appendTo(self.$_currentFilterRow);

                var leftContainer = $('<label/>')
                                    .addClass('cq-filter-label-container')
                                    .attr('value', 'test')
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

        _createMultiselect: function (selectObject) {
            var self = this;
            if (selectObject) {
                var filterContainer = $('<div/>')
                                    .addClass('col-md-3 col-sm-12 cq-filter-container form-group')
                                    .appendTo(self.$_currentFilterRow);

                var labelContainer = $('<label/>')
                                    .addClass('cq-filter-label-container')
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


                var select = $('<div />')
                                    .attr('id', selectObject.id)
                                    .addClass('ms-container')
                                    .append($('<div />').addClass('ms-input')
                                            .append($('<div />').addClass('ms-selected-list'))
                                            .append($('<div />').addClass('ms-action').html('&#9660;')))
                                    .append($('<div />').addClass('ms-list'))
                                    .appendTo(filterContainer);


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
                                var selectEntry = $('#' + selectObject.id + ' .ms-list');

                                for (key in data) {
                                    selectEntry.append($('<div />')
                                    .addClass('ms-list-item')
                                    .attr('value', data[key].value)
                                    .text(data[key].displayText));
                                }
                            }
                        });
                    }
                }


                /* multiselect_logic(start) */

                var base_id = selectObject.id;

                $('body').on('click', '.ms-action', function () {
                    $(".ms-list").slideToggle('fast');
                });

                $(document).bind('click', function (e) {
                    var $clicked = $(e.target);
                    if (!$clicked.parents().hasClass("ms-container")) {
                        $(".ms-list").hide();
                    }
                });

                $('body').on('click', '.ms-list-item', function () {
                    var value = $(this).attr('value');
                    var text = $(this).text();
                    var output = $('<div />')
                            .addClass('ms-selected')
                            .append($('<span />').addClass('ms-remove').html('&#x274C;'))
                            .append($('<span />').addClass('text').text(text))
                            .attr('value', value);

                    $('#' + base_id + ' .ms-selected-list').prepend(output);
                    $(this).remove();
                });

                $('body').on('click', '.ms-remove', function () {
                    var value = $(this).parent().attr('value');
                    var text = $(this).parent().find('.text').text();
                    var output = $('<div />')
                            .addClass('ms-list-item')
                            .attr('value', value)
                            .text(text);

                    $('#' + base_id + ' .ms-list').prepend(output);
                    $(this).parent().remove();
                });
                /* multiselect_logic(end) */

            }
        },

        _createSelect: function (selectObject) {
            var self = this;
            if (selectObject) {
                var filterContainer = $('<div/>')
                                    .addClass('col-md-3 col-sm-12 cq-filter-container form-group')
                                    .appendTo(self.$_currentFilterRow);

                var labelContainer = $('<label/>')
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
                                    .addClass('col-md-3 col-sm-12 cq-filter-container form-group ')
                                    .appendTo(self.$_currentFilterRow);

                var labelContainer = $('<label/>')
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
                            .addClass('col-md-offset-9 col-sm-offset-1 col-md-3 col-sm-10')
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

                if (currentFilter.type == 'input') {
                    var val = $('#' + currentFilter.id).val();
                    controlValue = val;
                }

                if (currentFilter.type == 'select') {
                    var values = [];
                    var val = $('#' + currentFilter.id).val();
                    values.push(val);
                    controlValue = values;
                }

                if (currentFilter.type == 'multiselect') {
                    var multiselectId = currentFilter.id;
                    var values = [];

                    $('#' + multiselectId + ' .ms-selected').each(function () {
                        var val = $(this).attr('value');
                        values.push(val);
                    });

                    controlValue = values;
                }

                if (currentFilter.type == 'datetime') {
                    var val = $('#' + currentFilter.id).val();
                    controlValue = moment(val, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DDTHH:mm:ss');
                }


                if (!controlValue)
                    continue;

                object[currentFilter.assignedField] = controlValue;
            }

            return object;
        }
    });

})(jQuery);

