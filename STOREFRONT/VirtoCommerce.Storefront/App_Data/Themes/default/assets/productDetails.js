var productApp =
    angular.module('productApp', ['ngResource'])
    .factory('products', ['$resource', function ($resource) {
        return $resource('product/:id', { id: '@Id' }, {
            get: { method: 'GET', url: 'product/:id' }
        });
    }])
    .controller('productController', ['$scope', '$window', 'products', '$sce', function ($scope, $window, products, $sce) {
        $scope.selectedImage = null;
        $scope.properties = [];
        $scope.selectedProduct = null;
        $scope.product = null;
        $scope.productId = $window.productId;

        $scope.initialize = function () {
            products.get({ id: $scope.productId }, function (data) {
                $scope.product = data;
                $scope.selectedImage = data.Images[0].Url;
                $scope.selectedProduct = data;
                $scope.trustedDescription = $sce.trustAsHtml($scope.selectedProduct.Description);
                if (data.Variations.length > 0) {
                    $scope.createPropertiesCombinations();
                }
            });
        }

        $scope.createPropertiesCombinations = function () {
            var properties = _.groupBy(_.map(_.where($scope.product.Properties, { Type: 'Variation' }), function (prop) { return _first(prop.Values) }), function (val) { return val.PropertyName });;

            for (var i = 0; i < Object.keys(properties) ; i++) {
                properties[Object.keys(properties)[i]] = _.map(_.uniq(_.map(properties[Object.keys(properties)[i]], function (value) { return value.Value; })), function (val) { return { value: val, isAvailable: false }; });
            }

            $scope.properties = properties;

            //properties = _.where($scope.product.Properties, { Type: 'Variation' });
            //var propertyNames = _.map(properties, function (property) { return property.Name; });
            //for (var i = 0; i < $scope.product.Variations.length; i++) {
            //    properties = properties.concat(_.filter($scope.product.Variations[i].Properties, function (property) { return _.contains(propertyNames, property.Name); }));
            //}
            //var values = [];
            //for (var i = 0; i < properties.length; i++) {
            //    values.push(_.first(properties[i].Values));
            //}
            //var groupedValues = _.groupBy(values, function (val) { return val.PropertyName });
            //for (var i = 0; i < propertyNames.length; i++) {
            //    groupedValues[propertyNames[i]] = _.map(_.uniq(_.map(groupedValues[propertyNames[i]], function (value) { return value.Value; })), function (val) { return { value: val, isAvailable: false }; });
            //}
            //$scope.properties = groupedValues;
        }

        $scope.initialize();
    }])
    .config(['$interpolateProvider', function ($interpolateProvider) {
        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);