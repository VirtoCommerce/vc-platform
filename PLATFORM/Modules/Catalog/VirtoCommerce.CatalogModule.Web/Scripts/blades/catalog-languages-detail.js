angular.module('virtoCommerce.catalogModule')
.controller('catalogLanguagesController', ['$rootScope', '$scope', 'bladeNavigationService', '$injector', 'catalogs', 'dialogService', '$filter',
    function ($rootScope, $scope, bladeNavigationService, $injector, catalogs, dialogService, $filter)
    {
        $scope.currentBlade = $scope.blade;

        $scope.currentBlade.refresh = function (parentRefresh)
        {
            catalogs.getCatalogLanguages({ id: $scope.currentBlade.currentEntityId }, function (data)
            {
                initializeBlade(data);
                if (parentRefresh)
                {
                    $scope.currentBlade.parentBlade.refresh();
                }
            });
        }

        function initializeBlade(data)
        {
            $scope.currentBlade.availableLanguages = angular.copy(data);
            $scope.currentBlade.isLoading = false;
        };

        $scope.selectLanguage = function (lang)
        {
            if (lang != undefined && lang.isDefault != true)
            {
                //language has to be removed if it belongs to catalog or added else
                if (lang.catalogId != undefined) {
                    $scope.removeLanguage([lang]);
                } else {
                    $scope.addLanguage(lang);
                }
            }
        }

        $scope.addLanguage = function (lang)
        {
            if (lang != undefined)
            {

                lang.catalogId = $scope.currentBlade.currentEntityId;
                //$scope.currentBlade.availableLanguages.push(lang);
                $scope.saveChanges();
            }
        }


        $scope.setAsDefault = function(defaultLang) {
            angular.forEach($scope.currentBlade.availableLanguages, function (lang)
            {
                lang.isDefault = false;
            });
            defaultLang.isDefault = true;
            $scope.saveChanges();
        }

        $scope.removeLanguage = function (selectedLanguages)
        {

            if (selectedLanguages == undefined)
            {
                selectedLanguages = $filter('filter')($scope.currentBlade.availableLanguages, { selected: true });
            }

            angular.forEach(selectedLanguages, function (lang)
            {
                var idx = $scope.currentBlade.availableLanguages.indexOf(lang);
                if (idx >= 0)
                {
                    $scope.currentBlade.availableLanguages.splice(idx, 1);
                }
            });

            $scope.saveChanges();
        };

        $scope.saveChanges = function() {
            $scope.currentBlade.isLoading = true;
            catalogs.updateCatalogLanguages({ catalogId: $scope.currentBlade.currentEntityId }, $scope.currentBlade.availableLanguages, function (data, headers)
            {
                $scope.currentBlade.refresh(true);
            });
        }

        $scope.currentBlade.refresh(false);
        

    }]);
