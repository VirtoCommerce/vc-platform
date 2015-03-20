angular.module('virtoCommerce.catalogModule')
.controller('newProductSeoDetailController', ['$scope', '$http', function ($scope, $http)
{
    $scope.wizardBlade = $scope.blade.parentBlade;

    function initializeBlade(data)
    {

        if (angular.isUndefined(data) || data.length < $scope.wizardBlade.parentBlade.catalog.languages.length)
        {
            data = [];
            _.each($scope.wizardBlade.parentBlade.catalog.languages, function (lang)
            {
                if (_.every(data, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(lang.languageCode.toLowerCase()) < 0; }))
                {
                    data.push({ languageCode: lang.languageCode });
                }
            });

            if (angular.isDefined($scope.wizardBlade.item.name)) {

                $http.get('api/catalog/getslug?text=' + $scope.wizardBlade.item.name).
                    success(function (slug)
                    {
                        _.each($scope.seoInfos, function (seo)
                        {
                            if (angular.isUndefined(seo.semanticUrl))
                            {
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

    $scope.saveChanges = function() {
        $scope.wizardBlade.item.seoInfos = $scope.seoInfos;
        $scope.bladeClose();
    };

    $scope.semanticUrlValidator = function (value) {
        // var pattern = /^\w*$/; // alphanumeric and underscores
        var pattern = /^([a-zA-Z0-9\(\)_\-]+)*$/;
        return pattern.test(value);
    }


    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    initializeBlade($scope.blade.seoInfos);
}]);
