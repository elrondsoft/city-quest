(function () {
    var app = angular.module('app');
    app.directive('jtable', function ($window) {
        return {
            restrict: 'EA',
            scope: {
                title: '=title',
                paging: '=paging',
                actions: '=actions',
                filters: '=filters',
                fields: '=fields',
                recordsLoaded: '&',
                rowInserted: '&',
                toolbar: '=toolbar',
                loaded: '&',
                messages: '=messages',
                filterLoaded: '&'
            },
            link: function (scope, elm, attrs, ctrl) {
                var rowInsertedHandler = scope.rowInserted();
                var recordsLoadedHandler = scope.recordsLoaded();
                var filterLoadedHandler = scope.filterLoaded();

                $(elm).jtable({
                    title: scope.title,
                    paging: scope.paging,
                    actions: scope.actions,
                    filters: scope.filters,
                    fields: scope.fields,
                    messages: scope.messages,
                    recordsLoaded: function (event, data) {
                        if (recordsLoadedHandler)
                            recordsLoadedHandler(event, data);
                    },
                    rowInserted: function (event, data) {
                        if (rowInsertedHandler)
                            rowInsertedHandler(event, data);
                    },
                    toolbar: scope.toolbar,
                    filterLoaded: function () {
                        if (filterLoadedHandler)
                            filterLoadedHandler();
                    }
                });
                $(elm).jtable('load');
                scope.loaded({});
            }
        };
    });
})();