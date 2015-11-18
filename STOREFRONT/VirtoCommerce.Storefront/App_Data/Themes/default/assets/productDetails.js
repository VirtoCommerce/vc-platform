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
                    //var properties = _.reject($scope.product.Properties, function (property) { return property.Type !== 'Variation'; });
                    //for (var i = 0; i < properties.length; i++) {
                    //    var values = [_.first(properties[i].Values).Value];
                    //    for (var j = 0; j < data.Variations.length; j++) {
                    //        var prop = _.find(data.Variations[j].Properties, function (property) { return property.Name === properties[i].Name });
                    //        values.push(_.first(prop.Values).Value);
                    //    }
                    //    $scope.properties.push({ name: properties[i].Name, values: _.uniq(angular.copy(values)) });
                    //}
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

        //$scope.selectImage = function (image) {
        //    $scope.selectedImage = image;
        //}

        //$scope.selectProduct = function (product) {
        //    $scope.selectedProduct = product;
        //    $scope.selectImage(_.first(product.Images));
        //}

        //$scope.getProductPropertyByName = function (product, name) {
        //    return _.find(product.Properties, function (prop) { return prop.Name === name });
        //}

        //$scope.comparePropertyValue = function (property, value) {
        //    return _.first(property.Values).Value === value;
        //}

        //$scope.checkSelectedProduct = function (product) {
        //    var retVal = false;
        //    for (var i = 0; i < $scope.properties.length; i++) {
        //        var selectedProperty = $scope.getProductPropertyByName(product, $scope.properties[i].name);
        //        if ($scope.comparePropertyValue(selectedProperty, $scope.properties[i].selectedValue)) {
        //            retVal = true;
        //        }
        //        else {
        //            retVal = false;
        //            break;
        //        }
        //    }
        //    return retVal;
        //}

        //$scope.checkAndSelectProduct = function () {
        //    var productFinded = false;
        //    productFinded = $scope.checkSelectedProduct($scope.product);

        //    if (!productFinded) {
        //        for (var j = 0; j < $scope.product.Variations.length; j++) {
        //            productFinded = $scope.checkSelectedProduct($scope.product.Variations[j]);
        //            if (productFinded) {
        //                $scope.selectProduct($scope.product.Variations[j]);
        //                break;
        //            }
        //        }
        //    }
        //    else {
        //        $scope.selectProduct($scope.product);
        //    }
        //}

        //$scope.rebuildSkuSelector = function (property) {
        //    $scope.checkAndSelectProduct();

        //    for (var i = 0; i < $scope.properties.length; i++) {
        //        if ($scope.properties[i].name === property.name) {
        //            var productProp = $scope.getProductPropertyByName($scope.product, $scope.properties[i].name);
        //            var values = [_.first(productProp.Values).Value];
        //            for (var j = 0; j < $scope.product.Variations.length; j++) {
        //                var prop = $scope.getProductPropertyByName($scope.product.Variations[j], $scope.properties[i].name);
        //                values.push(_.first(prop.Values).Value);
        //            }
        //            $scope.properties[i].values = _.uniq(angular.copy(values));
        //        }
        //        else {
        //            var values = [];
        //            var productProp = $scope.getProductPropertyByName($scope.product, $scope.properties[i].name);
        //            var selectedProp = $scope.getProductPropertyByName($scope.product, property.name);
        //            if (productProp !== null && $scope.comparePropertyValue(selectedProp, property.selectedValue)) {
        //                values.push(_.first(productProp.Values).Value);
        //            }
        //            for (var j = 0; j < $scope.product.Variations.length; j++) {
        //                var prop = $scope.getProductPropertyByName($scope.product.Variations[j], $scope.properties[i].name);
        //                var selectedProp = $scope.getProductPropertyByName($scope.product.Variations[j], property.name);
        //                if (prop !== null && $scope.comparePropertyValue(selectedProp, property.selectedValue)) {
        //                    values.push(_.first(prop.Values).Value);
        //                }
        //            }
        //            $scope.properties[i].values = _.uniq(angular.copy(values));
        //        }
        //    }
        //}

        $scope.initialize();
    }])
    .config(['$interpolateProvider', function ($interpolateProvider) {
        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);