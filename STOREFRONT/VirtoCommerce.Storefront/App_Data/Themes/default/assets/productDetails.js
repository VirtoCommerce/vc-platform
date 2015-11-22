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
        $scope.productsPropertiesCombinations = [];
        $scope.selectedProduct = null;
        $scope.product = null;
        $scope.productId = $window.productId;

        $scope.initialize = function () {
            products.get({ id: $scope.productId }, function (data) {
                $scope.product = data;
                $scope.selectedImage = $scope.product.Images[0];
                $scope.trustedDescription = $sce.trustAsHtml($scope.product.Description);
                if (data.Variations.length > 0) {
                    $scope.createPropertiesCombinations();
                    $scope.createProductPropertiesCombinations();
                }
            });
        }

        //create product properties combinations for checking them with selected values
        $scope.createProductPropertiesCombinations = function () {
            var combinations = _.map(_.where($scope.product.Properties, { Type: 'Variation' }), function (prop) { return { name: _.first(prop.Values).PropertyName, value: _.first(prop.Values).Value }; });
            $scope.productsPropertiesCombinations.push({ product: $scope.product, properties: combinations });
            for (var i = 0; i < $scope.product.Variations.length; i++) {
                combinations = _.map(_.where($scope.product.Variations[i].Properties, { Type: 'Variation' }), function (prop) { return { name: _.first(prop.Values).PropertyName, value: _.first(prop.Values).Value }; });
                $scope.productsPropertiesCombinations.push({ product: $scope.product.Variations[i], properties: combinations });
            }
        }

        $scope.propertyValueIsAvailable = function (propertyName, value) {
            var retVal = false;
            for(var i = 0; i < $scope.productsPropertiesCombinations.length; i++){

            }

            return retVal;
        }

        $scope.setSelectedProduct = function () {
            //check that all properties selected
            for (key in $scope.properties) {
                if (!_.has($scope.properties[key], 'selectedValue')) {
                    $scope.selectedProduct = undefined;
                    return;
                }
            }

            //product list checking for property values
            for (var i = 0; i < $scope.productsPropertiesCombinations.length; i++) {
                //check that all product property values equals to selected, if true, make product selected
                if (_.reject($scope.productsPropertiesCombinations[i].properties, function (prop) { return prop.value == $scope.properties[prop.name].selectedValue }).length === 0) {
                    $scope.selectedProduct = $scope.productsPropertiesCombinations[i].product;
                    $scope.selectedImage = $scope.selectedProduct.Images[0];
                    return;
                }
            }
        }

        //create sku selector properties
        $scope.createPropertiesCombinations = function (propertyName) {
            $scope.setSelectedProduct();
            var props = _.where($scope.product.Properties, { Type: 'Variation' });
            for (var i = 0; i < $scope.product.Variations.length; i++) {
                props = props.concat(_.where($scope.product.Variations[i].Properties, { Type: 'Variation' }));
            }
            var properties = _.groupBy(_.map(props, function (prop) { return _.first(prop.Values) }), function (val) { return val.PropertyName });;

            for (key in properties) {
                properties[key] = { values: _.map(_.uniq(_.map(properties[key], function (value) { return value.Value; })), function (val) { return { value: val, isAvailable: true }; }) };

                //if (propertyName === Object.keys(properties)[i] || propertyName === '')
                //    properties[Object.keys(properties)[i]] = { values: _.map(_.uniq(_.map(properties[Object.keys(properties)[i]], function (value) { return value.Value; })), function (val) { return { value: val, isAvailable: true }; }) };
                //else
                //    properties[Object.keys(properties)[i]] = { values: _.map(_.uniq(_.map(properties[Object.keys(properties)[i]], function (value) { return value.Value; })), function (val) { return { value: val, isAvailable: $scope.propertyValueIsAvailable(propertyName, value) }; }) };
            }

            $scope.properties = properties;
        }

        $scope.initialize();
    }])
    .config(['$interpolateProvider', function ($interpolateProvider) {
        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);