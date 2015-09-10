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
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
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
          controller: 'virtoCommerce.orderModule.customerOrderItemsWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-items-widget.tpl.html'
      };
      widgetService.registerWidget(operationItemsWidget, 'customerOrderDetailWidgets');

      var shipmentItemsWidget = {
          controller: 'virtoCommerce.orderModule.shipmentItemsWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-items-widget.tpl.html'
      };
      widgetService.registerWidget(shipmentItemsWidget, 'shipmentDetailWidgets');


      var customerOrderAddressWidget = {
          controller: 'virtoCommerce.orderModule.customerOrderAddressWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-address-widget.tpl.html'
      };
      widgetService.registerWidget(customerOrderAddressWidget, 'customerOrderDetailWidgets');

      var customerOrderTotalsWidget = {
          controller: 'virtoCommerce.orderModule.customerOrderTotalsWidgetController',
          size: [2, 1],
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-totals-widget.tpl.html'
      };
      widgetService.registerWidget(customerOrderTotalsWidget, 'customerOrderDetailWidgets');


      var operationCommentWidget = {
          controller: 'virtoCommerce.orderModule.operationCommentWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-comment-widget.tpl.html'
      };
      widgetService.registerWidget(operationCommentWidget, 'customerOrderDetailWidgets');

      var shipmentAddressWidget = {
          controller: 'virtoCommerce.orderModule.shipmentAddressWidgetController',
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-address-widget.tpl.html'
      };
      widgetService.registerWidget(shipmentAddressWidget, 'shipmentDetailWidgets');


      var shipmentTotalWidget = {
          controller: 'virtoCommerce.orderModule.shipmentTotalsWidgetController',
          size: [2, 1],
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-totals-widget.tpl.html'
      };
      widgetService.registerWidget(shipmentTotalWidget, 'shipmentDetailWidgets');

      var dynamicPropertyWidget = {
          controller: 'platformWebApp.dynamicPropertyWidgetController',
          template: '$(Platform)/Scripts/app/dynamicProperties/widgets/dynamicPropertyWidget.tpl.html'
      };
      widgetService.registerWidget(dynamicPropertyWidget, 'shipmentDetailWidgets');
      widgetService.registerWidget(dynamicPropertyWidget, 'customerOrderDetailWidgets');
      widgetService.registerWidget(dynamicPropertyWidget, 'paymentDetailWidgets');



      var operationsTreeWidget = {
          controller: 'virtoCommerce.orderModule.operationTreeWidgetController',
          size: [4, 4],
          template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-tree-widget.tpl.html'
      };
      widgetService.registerWidget(operationsTreeWidget, 'customerOrderDetailWidgets');
      widgetService.registerWidget(operationsTreeWidget, 'shipmentDetailWidgets');
      widgetService.registerWidget(operationsTreeWidget, 'paymentDetailWidgets');

      // register dashboard widgets
      var statisticsController = 'virtoCommerce.orderModule.dashboard.statisticsWidgetController';
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-revenue.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-customersCount.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-revenuePerCustomer.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-orderValue.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-itemsPurchased.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [2, 1],
          template: 'order-statistics-lineitemsPerOrder.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [3, 2],
          template: 'order-statistics-revenueByQuarter.html'
      }, 'mainDashboard');
      widgetService.registerWidget({
          controller: statisticsController,
          size: [3, 2],
          template: 'order-statistics-orderValueByQuarter.html'
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
                  var statisticsToChartRows = function (statsList, allCurrencies) {
                      var groupedQuarters = _.groupBy(statsList, function (stats) { return stats.year + ' Q' + stats.quarter; });
                      return _.map(groupedQuarters, function (stats, key) {
                          var values = [{ v: key }];
                          _.each(allCurrencies, function (x) {
                              var stat = _.findWhere(stats, { currency: x });
                              values.push({ v: stat ? stat.amount : 0 });
                          });
                          return {
                              c: values
                          };
                      });
                  }

                  var allCurrencies = _.unique(_.pluck(data.avgOrderValuePeriodDetails, 'currency').sort());

                  var cols = [{ id: "quarter", label: "Quarter", type: "string" }];
                  _.each(allCurrencies, function (x) {
                      cols.push({ id: "revenue" + x, label: x, type: "number" });
                  });

                  data.chartRevenueByQuarter = {
                      "type": "LineChart",
                      "data": {
                          cols: cols,
                          rows: statisticsToChartRows(data.revenuePeriodDetails, allCurrencies)
                      },
                      "options": {
                          "title": "Revenue by quarter",
                          "legend": { position: 'top' },
                          "vAxis": {
                              // "title": "Sales unit",
                              gridlines: { count: 8 }
                          },
                          "hAxis": {
                              // "title": "Date"
                              slantedText: true,
                              slantedTextAngle: 20
                          }
                      },
                      "formatters": {}
                  };

                  cols = [{ id: "quarter", label: "Quarter", type: "string" }];
                  _.each(allCurrencies, function (x) {
                      cols.push({ id: "avg-orderValue" + x, label: x, type: "number" });
                  });

                  data.chartOrderValueByQuarter = {
                      "type": "ColumnChart",
                      "data": {
                          cols: cols,
                          rows: statisticsToChartRows(data.avgOrderValuePeriodDetails, allCurrencies)
                      },
                      "options": {
                          "title": "Average Order value by quarter",
                          "legend": { position: 'top' },
                          "vAxis": {
                              gridlines: { count: 8 }
                          },
                          "hAxis": {
                              slantedText: true,
                              slantedTextAngle: 20
                          }
                      },
                      "formatters": {}
                  };

                  $localStorage.ordersDashboardStatistics = data;
              },
              function (error) {
                  console.log(error);
              });
          }
      });
  }]);
