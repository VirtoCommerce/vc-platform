angular.module('virtoCommerce.orderModule.services', [
])
.factory('calculateTotalsService', [function () {

	function recalculateTotals(operation)
	{
		var retVal = {};
		if (angular.isDefined(operation.operationType)) {
			if (operation.operationType.toLowerCase() == 'customerorder') {

				retVal.subTotal = _.reduce(operation.items, function (memo, x) { return memo + x.price * x.quantity; }, 0);
				retVal.shippingTotal = _.reduce(operation.shipments, function (memo, x) { return memo + x.sum; }, 0);

				retVal.discountTotal = operation.discountAmount ? parseFloat(operation.discountAmount) : 0;
				retVal.discountTotal += _.reduce(operation.items, function (memo, x) { return memo + parseFloat(x.discountAmount) }, 0);
				retVal.discountTotal += _.reduce(operation.shipments, function (memo, x) { return memo + parseFloat(x.discountAmount) }, 0);

				retVal.taxTotal = parseFloat(operation.tax);
				retVal.taxTotal += _.reduce(operation.items, function (memo, x) { return memo + parseFloat(x.tax); }, 0);
				retVal.taxTotal += _.reduce(operation.shipments, function (memo, x) { return memo + parseFloat(x.tax); }, 0);

				retVal.total = retVal.subTotal + retVal.shippingTotal + retVal.taxTotal - retVal.discountTotal;
			}
			else if (operation.operationType.toLowerCase() == 'shipment') {

				retVal.discountTotal = parseFloat(operation.discountAmount);
				retVal.taxTotal = parseFloat(operation.tax);
				retVal.shippingTotal = parseFloat(operation.sum);
				retVal.total = retVal.shippingTotal + retVal.taxTotal - retVal.discountTotal;
			}
		}
		return retVal;
	};

	var retVal = {
		recalculateTotals: recalculateTotals
	};

	return retVal;
}]);

