//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, ['virtoCommerce.catalogModule', 'virtoCommerce.pricingModule'])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.orderModule', {
              url: '/orders',
              templateUrl: 'Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'orders',
                          title: 'Customer orders',
                          //subtitle: 'Manage Orders',
                          controller: 'virtoCommerce.orderModule.customerOrderListController',
                          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', '$http', '$compile', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', '$localStorage', 'virtoCommerce.orderModule.order_res_customerOrders', function ($rootScope, $http, $compile, mainMenuService, widgetService, $state, $localStorage, customerOrders) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/orders',
          icon: 'fa fa-file-text',
          title: 'Orders',
          priority: 99,
          action: function () { $state.go('workspace.orderModule'); },
          permission: 'order:query'
      };
      mainMenuService.addMenuItem(menuItem);


      //Register widgets
      var operationItemsWidget = {
          controller: 'virtoCommerce.orderModule.operationItemsWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-items-widget.tpl.html',
      };
      widgetService.registerWidget(operationItemsWidget, 'customerOrderDetailWidgets');
      widgetService.registerWidget(operationItemsWidget, 'shipmentDetailWidgets');


      var customerOrderAddressWidget = {
          controller: 'virtoCommerce.orderModule.customerOrderAddressWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-address-widget.tpl.html',
      };
      widgetService.registerWidget(customerOrderAddressWidget, 'customerOrderDetailWidgets');

      var customerOrderTotalsWidget = {
          controller: 'virtoCommerce.orderModule.customerOrderTotalsWidgetController',
          size: [2, 1],
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-totals-widget.tpl.html',
      };
      widgetService.registerWidget(customerOrderTotalsWidget, 'customerOrderDetailWidgets');


      var operationCommentWidget = {
          controller: 'virtoCommerce.orderModule.operationCommentWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-comment-widget.tpl.html',
      };
      widgetService.registerWidget(operationCommentWidget, 'customerOrderDetailWidgets');


      var shipmentAddressWidget = {
          controller: 'virtoCommerce.orderModule.shipmentAddressWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-address-widget.tpl.html',
      };
      widgetService.registerWidget(shipmentAddressWidget, 'shipmentDetailWidgets');


      var shipmentTotalWidget = {
          controller: 'virtoCommerce.orderModule.shipmentTotalsWidgetController',
          size: [2, 1],
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-totals-widget.tpl.html',
      };
      widgetService.registerWidget(shipmentTotalWidget, 'shipmentDetailWidgets');


      var operationsTreeWidget = {
          controller: 'virtoCommerce.orderModule.operationTreeWidgetController',
          size: [4, 4],
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-tree-widget.tpl.html',
      };
      widgetService.registerWidget(operationsTreeWidget, 'customerOrderDetailWidgets');
      widgetService.registerWidget(operationsTreeWidget, 'shipmentDetailWidgets');
      widgetService.registerWidget(operationsTreeWidget, 'paymentDetailWidgets');

      // register dashboard widgets
      var statisticsController = 'virtoCommerce.orderModule.dashboard.statisticsWidgetController';
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-revenue.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-customersCount.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-revenuePerCustomer.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-orderValue.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-itemsPurchased.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-lineitemsPerOrder.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [3, 2],
          template: 'order-statistics-revenueByQuarter.html',
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [3, 2],
          template: 'order-statistics-orderValueByQuarter.html',
      }, 'mainDashboard');

      $http.get('Modules/$(VirtoCommerce.Orders)/Scripts/widgets/dashboard/statistics-templates.html').then(function (response) {
          // compile the response, which will put stuff into the cache
          $compile(response.data);
      });

      $rootScope.$on('loginStatusChanged', function (event, authContext) {
          if (authContext.isAuthenticated) {
              var now = new Date();
              var startDate = new Date();
              startDate.setFullYear(now.getFullYear() - 1);

              customerOrders.getDashboardStatistics({ start: startDate, end: now }, function (data) {
                  // prepare statistics
                  var dataByQuarters = _.groupBy(data.revenuePeriodDetails, function (x) { return x.year + ' Q' + x.quarter; });
                  var amountByQuarter = _.map(dataByQuarters, function (stats, quarter) {
                      var sum = _.reduce(stats, function (memo, x) { return memo + x.amount; }, 0);
                      return { c: [{ v: quarter }, { v: sum }] };
                  });

                  data.chartRevenueByQuarter = {
                      "type": "LineChart",
                      "data": {
                          "cols": [
                              { id: "quarter", label: "Quarter", type: "string" },
                              //{ id: "avg-orderValue", label: "Average Order value", type: "number" }
                              { id: "avg-orderValue", label: "Revenue", type: "number" }
                          ],
                          rows: amountByQuarter
                      },
                      "options": {
                          //"title": "Average Order value by quarter",
                          "title": "Revenue by quarter",
                          "legend": { position: 'top' },
                          "vAxis": {
                              // "title": "Sales unit",
                              "gridlines": { "count": 8 }
                          },
                          "hAxis": {
                              // "title": "Date"
                          }
                      },
                      "formatters": {},
                  };

                  data.demoChartObject = {
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
                          "subtitle": 'Sales: 2014-2015',
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
                  };

                  $localStorage.ordersDashboardStatistics = data;
              },
              function (error) {
                  console.log(error);
              });
          }
      });
  }]);
