angular.module('virtoCommerce.orderModule.services', [
])
.factory('calculateTotalsService', [function () {

	function recalculateTotals(operation)
	{
		if (angular.isDefined(operation.operationType)) {
			if (operation.operationType.toLowerCase() == 'customerorder') {
				operation.subTotal = _.reduce(operation.items, function (memo, x) { return memo + x.price * x.quantity; }, 0);
				operation.shippingTotal = _.reduce(operation.shipments, function (memo, x) { return memo + x.sum; }, 0);

				operation.discountTotal = operation.discountAmount ? parseFloat(operation.discountAmount) : 0;
				operation.discountTotal += _.reduce(operation.items, function (memo, x) { return memo + parseFloat(x.discountAmount) }, 0);
				operation.discountTotal += _.reduce(operation.shipments, function (memo, x) { return memo + parseFloat(x.discountAmount) }, 0);

				operation.taxTotal = parseFloat(operation.tax);
				operation.taxTotal += _.reduce(operation.items, function (memo, x) { return memo + parseFloat(x.tax); }, 0);
				operation.taxTotal += _.reduce(operation.shipments, function (memo, x) { return memo + parseFloat(x.tax); }, 0);

				operation.total = operation.subTotal + operation.shippingTotal + operation.taxTotal - operation.discountTotal;
			}
		}
	};

	var retVal = {
		recalculateTotals: recalculateTotals
	};

	return retVal;
}]);

