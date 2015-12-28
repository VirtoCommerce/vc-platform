﻿var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('productController', ['$scope', '$window', 'catalogService', 'marketingService', function ($scope, $window, catalogService, marketingService) {
    //TODO: prevent add to cart not selected variation
    // display validator please select property
    // display price range

    var allVarations = [];
    $scope.selectedVariation = {};
    $scope.allVariationPropsMap = {};
    $scope.productPrice = null;
    $scope.productPriceLoaded = false;

    function Initialize() {
        var productId = $window.products[0].id;
        catalogService.getProduct(productId).then(function (response) {
            var product = response.data;
            //Current product its also variation (titular)
            allVarations = [ product ].concat(product.Variations);
            $scope.allVariationPropsMap = getFlatternDistinctPropertiesMap(allVarations);

            //Auto select initial product as default variation  (its possible because all our products is variations)
            var propertyMap = getVariationPropertyMap(product);
            _.each(_.keys(propertyMap), function (x) {
                $scope.checkProperty(propertyMap[x][0])
            });
            $scope.selectedVariation = product;
            getActualProductPrice($window.products[0]);
        });
    };

    function getFlatternDistinctPropertiesMap(variations) {
        var retVal = {};
        _.each(variations, function (variation) {
            var propertyMap = getVariationPropertyMap(variation);
            //merge
            _.each(_.keys(propertyMap), function (x) {
                retVal[x] = _.uniq(_.union(retVal[x], propertyMap[x]), "Value");
            });
        });
        return retVal;
    };

    function getVariationPropertyMap(variation) {
        retVal = _.groupBy(variation.VariationProperties, function (x) { return x.DisplayName });
        return retVal;
    };

    function getSelectedPropsMap(variationPropsMap) {
        var retVal = {};
        _.each(_.keys(variationPropsMap), function (x) {
            var property = _.find(variationPropsMap[x], function (y) {
                return y.selected;
            });
            if (property) {
                retVal[x] = [property];
            }
        });
        return retVal;
    };

    function comparePropertyMaps(propMap1, propMap2) {
        return _.every(_.keys(propMap1), function (x) {
            var retVal = propMap2.hasOwnProperty(x);
            if (retVal) {
                retVal = propMap1[x][0].Value == propMap2[x][0].Value;
            }
            return retVal;
        });
    };

    function findVariationBySelectedProps(variations, selectedPropMap) {
        var retVal = _.find(variations, function (x) {
            var productPropMap = getVariationPropertyMap(x);
            return comparePropertyMaps(productPropMap, selectedPropMap);
        });
        return retVal;
    };

    function getActualProductPrice(product) {
        marketingService.getActualProductPrices([product]).then(function (response) {
            var price = response.data ? response.data[0] : null;
            if (!price) {
                return;
            }
            $scope.productPrice = price;
            $scope.productPriceLoaded = true;
        });
    }

    //Method called from View when user click to one of properties value
    $scope.checkProperty = function (property) {
        //Select apropriate property and diselect previous selected
        var prevSelected = _.each($scope.allVariationPropsMap[property.DisplayName], function (x) {
            x.selected = x != property ? false : !x.selected;
        });

        var selectedPropsMap = getSelectedPropsMap($scope.allVariationPropsMap);
        //try to find best match variation for already selected properties
        $scope.selectedVariation = findVariationBySelectedProps(allVarations, selectedPropsMap);
    };

    Initialize();
}]);