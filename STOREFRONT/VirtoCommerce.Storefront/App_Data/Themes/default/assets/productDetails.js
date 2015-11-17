var productApp =
    angular.module('productApp', ['ngResource'])
    .factory('notifications', ['$resource', function ($resource) {
        return $resource('api/product/:id', { id: '@Id' }, {
            get: { method: 'GET', url: 'api/product/:id' }
        });
    }])
    .controller('productController', ['$scope', 'notifications', function ($scope, notifications) {
        $scope.selectedImage = null;
        $scope.properties = [];
        $scope.isSkuSelectorAvailable = false;
        $scope.selectedProduct = null;
        $scope.product = null;

        notifications.get({ id: 'd73b05fc10ad4249b612a2f0ed407dab' }, function (data) {
            $scope.product = data;
            $scope.selectedImage = data.Images[0].Url;
            $scope.isSkuSelectorAvailable = data.Variations.length > 0;
            if ($scope.isSkuSelectorAvailable) {
                var properties = _.reject($scope.product.Properties, function (property) { return property.Type !== 'Variation'; });
                for (var i = 0; i < properties.length; i++) {
                    var values = [ _.first(properties[i].Values).Value ];
                    for (var j = 0; j < data.Variations.length; j++) {
                        var prop = _.find(data.Variations[j].Properties, function (property) { return property.Name === properties[i].Name });
                        values.push(_.first(prop.Values).Value);
                    }
                    $scope.properties.push({ name: properties[i].Name, values: _.uniq(angular.copy(values)) });
                }
            }
        });

        $scope.selectImage = function (image) {
            $scope.selectedImage = image;
        }

        $scope.selectProduct = function (product) {
            $scope.selectedProduct = product;
            $scope.selectImage(_.first(product.Images));
        }

        $scope.getProductPropertyByName = function (product, name) {
            return _.find(product.Properties, function (prop) { return prop.Name === name });
        }

        $scope.comparePropertyValue = function (property, value) {
            return _.first(property.Values).Value === value;
        }

        $scope.checkSelectedProduct = function (product) {
            var retVal = false;
            for (var i = 0; i < $scope.properties.length; i++) {
                var selectedProperty = $scope.getProductPropertyByName(product, $scope.properties[i].name);
                if ($scope.comparePropertyValue(selectedProperty, $scope.properties[i].selectedValue)) {
                    retVal = true;
                }
                else {
                    retVal = false;
                    break;
                }
            }
            return retVal;
        }

        $scope.checkAndSelectProduct = function () {
            var productFinded = false;
            productFinded = $scope.checkSelectedProduct($scope.product);

            if (!productFinded) {
                for (var j = 0; j < $scope.product.Variations.length; j++) {
                    productFinded = $scope.checkSelectedProduct($scope.product.Variations[j]);
                    if (productFinded) {
                        $scope.selectProduct($scope.product.Variations[j]);
                        break;
                    }
                }
            }
            else {
                $scope.selectProduct($scope.product);
            }
        }

        $scope.rebuildSkuSelector = function (property) {
            $scope.checkAndSelectProduct();

            for (var i = 0; i < $scope.properties.length; i++) {
                if ($scope.properties[i].name === property.name) {
                    var productProp = $scope.getProductPropertyByName($scope.product, $scope.properties[i].name);
                    var values = [_.first(productProp.Values).Value];
                    for (var j = 0; j < $scope.product.Variations.length; j++) {
                        var prop = $scope.getProductPropertyByName($scope.product.Variations[j], $scope.properties[i].name);
                        values.push(_.first(prop.Values).Value);
                    }
                    $scope.properties[i].values = _.uniq(angular.copy(values));
                }
                else {
                    var values = [];
                    var productProp = $scope.getProductPropertyByName($scope.product, $scope.properties[i].name);
                    var selectedProp = $scope.getProductPropertyByName($scope.product, property.name);
                    if (productProp !== null && $scope.comparePropertyValue(selectedProp, property.selectedValue)) {
                        values.push(_.first(productProp.Values).Value);
                    }
                    for (var j = 0; j < $scope.product.Variations.length; j++) {
                        var prop = $scope.getProductPropertyByName($scope.product.Variations[j], $scope.properties[i].name);
                        var selectedProp = $scope.getProductPropertyByName($scope.product.Variations[j], property.name);
                        if (prop !== null && $scope.comparePropertyValue(selectedProp, property.selectedValue)) {
                            values.push(_.first(prop.Values).Value);
                        }
                    }
                    $scope.properties[i].values = _.uniq(angular.copy(values));
                }
            }
        }

    }])
    .config(['$interpolateProvider', function ($interpolateProvider) {
        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);