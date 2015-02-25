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

				_.each(operation.items, function (x) {
					x.total = parseInt(x.quantity) * (parseFloat(x.price) || 0) - (parseFloat(x.discountAmount) || 0) + (parseFloat(x.tax) || 0);
					x.total = Math.round(x.total * 100) / 100;
				});

				
				retVal.subTotal = Math.round(retVal.subTotal * 100) / 100;
				retVal.shippingTotal = Math.round(retVal.shippingTotal * 100) / 100;
				retVal.discountTotal = Math.round(retVal.discountTotal * 100) / 100;
				retVal.taxTotal = Math.round(retVal.taxTotal * 100) / 100;
				retVal.total = retVal.subTotal + retVal.shippingTotal + retVal.taxTotal - retVal.discountTotal;
				retVal.total = Math.round(retVal.total * 100) / 100;
				
			}
			else if (operation.operationType.toLowerCase() == 'shipment') {

				retVal.discountTotal = parseFloat(operation.discountAmount);
				retVal.taxTotal = parseFloat(operation.tax);
				retVal.shippingTotal = parseFloat(operation.sum);
				retVal.total = retVal.shippingTotal + retVal.taxTotal - retVal.discountTotal;
				retVal.total = Math.round(retVal.total * 100) / 100;
			}
		}
		return retVal;
	};

	var retVal = {
		recalculateTotals: recalculateTotals
	};

	return retVal;
}]);

