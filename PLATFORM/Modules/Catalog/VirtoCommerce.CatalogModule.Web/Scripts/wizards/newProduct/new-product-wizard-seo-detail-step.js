angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductSeoDetailController', ['$scope', '$http', function ($scope, $http) {
    $scope.wizardBlade = $scope.blade.parentBlade;

    function initializeBlade(item) {
        var data = item.seoInfos;
        if (angular.isUndefined(data) || data.length < $scope.wizardBlade.parentBlade.catalog.languages.length) {
            data = [];
            _.each($scope.wizardBlade.parentBlade.catalog.languages, function (lang) {
                if (_.every(data, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(lang.languageCode.toLowerCase()) < 0; })) {
                    data.push({ languageCode: lang.languageCode });
                }
            });

            var stringForSlug = item.name;
            _.each(item.properties, function (prop) {
                _.each(prop.values, function (val) {
                    stringForSlug += ' ' + val.value;
                });
            });

            if (stringForSlug) {
                $http.get('api/catalog/getslug?text=' + stringForSlug)
                    .success(function (slug) {
                        _.each($scope.seoInfos, function (seo) {
                            if (angular.isUndefined(seo.semanticUrl)) {
                                seo.semanticUrl = slug;
                            }
                        });
                    });
            }
        }

        $scope.seoInfos = angular.copy(data);
        $scope.blade.origItem = data;
        $scope.blade.isLoading = false;
    };

    $scope.saveChanges = function () {
        $scope.blade.item.seoInfos = $scope.seoInfos;
        $scope.bladeClose();
    };

    $scope.semanticUrlValidator = function (value) {
        // var pattern = /^([a-zA-Z0-9\(\)_\-]+)*$/;
        var pattern = /[$+;=%{}[\]|\/@ ~#!^*&()?:'<>,]/;
        return !pattern.test(value);
    }


    initializeBlade($scope.blade.item);
}]);
