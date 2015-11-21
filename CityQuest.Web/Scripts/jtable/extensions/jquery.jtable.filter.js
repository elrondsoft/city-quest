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
								.addClass('tools-panel-container padding container-fluid container-xs-height')
								.insertBefore(insertTo);

            self._createFilterControls();
            self._createFilterLoadBtn();
        },

        _createFilterRow: function () {
            var row = $('<div/>')
						.addClass('row row-xs-height')
						.css('padding', '4px');

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
									.addClass('col-xs-3')
									.appendTo(self.$_currentFilterRow);

                var leftContainer = $('<div/>')
									.css('float', 'left')
									.appendTo(inputContainer);

                var rightContainer = $('<div/>')
									.css('float', 'right')
									.appendTo(inputContainer);

                if (inputObject.label) {
                    var label = $('<label/>').addClass('jtable-filter-label').text(inputObject.label).appendTo(leftContainer);
                }

                if (!inputObject.id) {
                    $.extend(inputObject, { id: self._getRandomGuid() });
                }

                var input = $('<input/>').addClass('cq-input').attr('id', inputObject.id).width(inputObject.width || '150px').appendTo(rightContainer);

                leftContainer.css('width', inputContainer.width() - input.width());
            }
        },

        _createFilterLoadBtn: function () {
            var self = this;

            self.$_mainFilterButtonRow = $('<div/>')
								.addClass('row row-xs-height')
								.css('padding', '4px')
								.appendTo(self.$_mainFilterContainer);

            var container = $('<div/>')
                            .addClass('col-xs-3 col-xs-offset-9')
                            .appendTo(self.$_mainFilterButtonRow);

            var inputContainer = $('<div/>')
									.addClass('col-xs-3')
									.appendTo(self.$_currentFilterRow);

            var button = $('<button/>')
				.addClass('btn btn-sm btn-success')
                .css('float', 'right')
                .css('margin', '20px 0px 0px 0px')
				.text(self.options.messages.apply)
				.click(function () {
				    var filter = self._createFilterObject();
				    self.load(filter);
				})
				.appendTo(inputContainer);
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
            }

            return object;
        }
    });

})(jQuery);