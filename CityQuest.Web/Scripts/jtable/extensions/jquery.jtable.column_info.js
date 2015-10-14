(function ($) {

	var base = {
		_create: $.hik.jtable.prototype._create
	};

	//extension members
	$.extend(true, $.hik.jtable.prototype, {
		options: {
			
		},

		_create: function () {
			var self = this;

			base._create.apply(this, arguments);
		},

		getColumnInfo: function () {
			return this._fieldList;
		},
	});

})(jQuery);