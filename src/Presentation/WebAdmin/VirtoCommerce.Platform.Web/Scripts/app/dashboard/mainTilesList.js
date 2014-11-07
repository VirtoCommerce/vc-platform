angular.module('platformWebApp.dashboard', [
    'googlechart',
    'platformWebApp.security.auth'
])
    .directive('vaMainTilesList', ['$document', 'authService', function ($document, authService) {
        return {
            templateUrl: 'Scripts/app/dashboard/mainTilesList.tpl.html',
            restrict: 'E',
            replace: true,
            // scope: true,
            link: function ($scope, $element, $attrs, $controller) {
                $scope.auth = authService;

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
                              "p": {}
                          },
                          {
                              "id": "this-year",
                              "label": "This Year",
                              "type": "number",
                              "p": {}
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
                        "legend": { position: 'top', maxLines: 3 },
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
            }
        }
    }
    ]);