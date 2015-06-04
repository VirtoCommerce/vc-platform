angular.module('virtoCommerce.orderModule')
.factory('virtoCommerce.orderModule.calculateTotalsService', [function () {

	function recalculateTotals(operation)
	{
		if (angular.isDefined(operation.operationType)) {
			if (operation.operationType.toLowerCase() == 'customerorder') {

				operation._subTotal = _.reduce(operation.items, function (memo, x) { return memo + x.price * x.quantity; }, 0);
				operation._shippingTotal = _.reduce(operation.shipments, function (memo, x) { return memo + x.sum; }, 0);

				operation._discountTotal = operation.discountAmount ? parseFloat(operation.discountAmount) : 0;
				operation._discountTotal += _.reduce(operation.items, function (memo, x) { return memo + parseFloat(x.discountAmount) }, 0);
				operation._discountTotal += _.reduce(operation.shipments, function (memo, x) { return memo + parseFloat(x.discountAmount) }, 0);

				operation._taxTotal = parseFloat(operation.tax);
				operation._taxTotal += _.reduce(operation.items, function (memo, x) { return memo + parseFloat(x.tax); }, 0);
				operation._taxTotal += _.reduce(operation.shipments, function (memo, x) { return memo + parseFloat(x.tax); }, 0);

				_.each(operation.items, function (x) {
					x._total = parseInt(x.quantity) * (parseFloat(x.price) || 0) - (parseFloat(x.discountAmount) || 0) + (parseFloat(x.tax) || 0);
					x._total = Math.round(x._total * 100) / 100;
				});

				
				operation._subTotal = Math.round(operation._subTotal * 100) / 100;
				operation._shippingTotal = Math.round(operation._shippingTotal * 100) / 100;
				operation._discountTotal = Math.round(operation._discountTotal * 100) / 100;
				operation._taxTotal = Math.round(operation._taxTotal * 100) / 100;
				operation._total = operation._subTotal + operation._shippingTotal + operation._taxTotal - operation._discountTotal;
				operation._total = Math.round(operation._total * 100) / 100;
				
			}
			else if (operation.operationType.toLowerCase() == 'shipment') {

				operation._discountTotal = parseFloat(operation.discountAmount);
				_.each(operation.items, function (x) {
					x._total = parseInt(x.quantity) * (parseFloat(x.price) || 0) - (parseFloat(x.discountAmount) || 0) + (parseFloat(x.tax) || 0);
					x._total = Math.round(x._total * 100) / 100;
				});
				operation._subTotal = _.reduce(operation.items, function (memo, x) { return memo + x.price * x.quantity; }, 0);
				operation._taxTotal = parseFloat(operation.tax);
				operation._shippingTotal = parseFloat(operation.sum);
				operation._total = operation._subTotal + operation._shippingTotal + operation._taxTotal - operation._discountTotal;
				operation._total = Math.round(operation._total * 100) / 100;
			}
		}
		return operation;
	};

	var operation = {
		recalculateTotals: recalculateTotals
	};

	return operation;
}]);

