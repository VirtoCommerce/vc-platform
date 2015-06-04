var app = angular.module('app', ['ui.bootstrap', 'gridster']);

app.controller('TestController', ['$scope', function ($scope){
    $scope.testValue   = 5;
    $scope.testValue_2 = 10;

    $scope.brandDefault = 'Apple';

    $scope.brands = [
        {
            "name": 'Apple'
        },
        {
            "name": 'Samsung'
        },
        {
            "name": 'Sony'
        }
    ];
}]);

app.controller('DatepickerDemoCtrl', function ($scope) {
    // datepicker
    $scope.datepickers = {
        bd: false
    }

    $scope.showWeeks = true;

    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

    $scope.clear = function () {
        $scope.blade.currentEntity.birthDate = null;
    };

    $scope.today = new Date();

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
    };

    $scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
    $scope.format = $scope.formats[0];
});

app.controller('gridsterController', ['$scope', function ($scope) {
    $scope.chartObject = {
        "type": "ColumnChart",
        "displayed": true,
        "data": {
            "cols": [
            {
                "id": "month",
                "label": "Month",
                "type": "string",
                "p": {}
            },
            {
                "id": "last-year",
                "label": "Last Year",
                "type": "number",
                "p": {
                }
            },
            {
                "id": "this-year",
                "label": "This Year",
                "type": "number",
                "p": {
                }
            }
            ],
            "rows": [
            {
                "c": [
                    {
                        "v": "January"
                    },
                    {
                        "v": 1020.12,
                        "f": "$1020.12"
                    },
                    {
                        "v": 1100.10,
                        "f": "$1500.10"
                    }
                ]
            },
            {
                "c": [
                {
                    "v": "February"
                },
                {
                    "v": 1210.01,
                    "f": "$1210.01"

                },
                {
                    "v": 1410.61,
                    "f": "$1410.61"
                }
                ]
            },
            {
                "c": [
                {
                    "v": "March"
                },
                {
                    "v": 1251.82,
                    "f": "$1251.82"
                },
                {
                    "v": 1300.91,
                    "f": "$1300.91"
                }
                ]
            }
            ]
        },
        "options": {
            "title": "Sales per month",
            "subtitle": 'Sales: 2013-2014',
            "isStacked": "false",
            "fill": 20,
            "legend": {
                position: 'top', maxLines: 3
            },
            "displayExactValues": true,
            "vAxis": {
                "title": "Sales unit",
                "gridlines": {
                    "count": 10
                }
            },
            "hAxis": {
                "title": "Date"
            }
        },
        "formatters": {},
        "view": {}
    }

    $scope.standardItems = [
        { sizeX: 2, sizeY: 1, template: 'tmp1.html', data: { name: 'Modules', count: 30 } },
        { sizeX: 1, sizeY: 1, row: 2, col: 1, template: 'tmp1.html', data: { name: 'Catalogs', count: 5 } },
        { sizeX: 1, sizeY: 1, row: 2, col: 0, template: 'tmp1.html', data: { name: 'Products', count: 456 } },
        { sizeX: 1, sizeY: 1, row: 0, col: 2, template: 'tmp2.html', data: { name: 'Graphic', ico: 'fa-signal' } },
    ];

    $scope.gridsterOpts = {
        minRows: 2, // the minimum height of the grid, in rows
        maxRows: 100,
        columns: 6, // the width of the grid, in columns
        colWidth: '130', // can be an integer or 'auto'.  'auto' uses the pixel width of the element divided by 'columns'
        rowHeight: 'match', // can be an integer or 'match'.  Match uses the colWidth, giving you square widgets.
        margins: [10, 10], // the pixel distance between each widget
        outerMargin: false,
        defaultSizeX: 2, // the default width of a gridster item, if not specifed
        defaultSizeY: 1, // the default height of a gridster item, if not specified
        mobileBreakPoint: 600, // if the screen is not wider that this, remove the grid layout and stack the items
        resizable: {
            enabled: false
        },
        draggable: {
            enabled: true, // whether dragging items is supported
            handle: '.my-class', // optional selector for resize handle
            //start: function (event, uiWidget, $element) {
            //}, // optional callback fired when drag is started,
            //drag: function (event, uiWidget, $element) {
            //}, // optional callback fired when item is moved,
            //stop: function (event, uiWidget, $element) {
            //} // optional callback fired when item is finished dragging
        }
    };
}])