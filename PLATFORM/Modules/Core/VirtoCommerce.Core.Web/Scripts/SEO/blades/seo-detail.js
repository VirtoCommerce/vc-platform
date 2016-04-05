angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.seoDetailController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
   
    function initializeBlade() {
    	var seoInfos = blade.seoContainerObject.seoInfos;
    	if (blade.store) {
    		seoInfos = _.filter(seoInfos, function (x) { return x.storeId === blade.store.id; });
    	}
    	// generate seo for missing languages
    	_.each(blade.seoLanguages, function (lang) {
    		if (_.every(seoInfos, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(lang.toLowerCase()) < 0; })) {
    			seoInfos.push({ isNew: true, languageCode: lang });
    		}
    	});

    	blade.currentEntities = angular.copy(seoInfos);
    	blade.origEntity = seoInfos;
    	blade.isLoading = false;
    };

    $scope.cancelChanges = function () {
        angular.copy(blade.origEntity, blade.currentEntities);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        var seoInfos = _.filter(blade.currentEntities, isValid);

        _.each(seoInfos, function (x) {
            if (x.isNew) {
                x.isNew = undefined;
                x.storeId = blade.store.id;
                blade.seoContainerObject.seoInfos.push(x);
            } else {
                var foundObject = _.find(blade.origEntity, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(x.languageCode.toLowerCase()) === 0; });
                angular.copy(x, foundObject);
            }
        });

        angular.copy(blade.currentEntities, blade.origEntity);
        $scope.bladeClose();

        if (blade.parentRefresh)
            blade.parentRefresh();
    }

    function isValid(data) {
        // check required and valid Url requirements
        return data.semanticUrl && $scope.semanticUrlValidator(data.semanticUrl);
    }

    $scope.semanticUrlValidator = function (value) {
        //var pattern = /^([a-zA-Z0-9\(\)_\-]+)*$/;
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&?:'<>,]/;
        return !pattern.test(value);
    };

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && _.every(_.filter(blade.currentEntities, function (data) { return !data.isNew; }), isValid) && _.some(blade.currentEntities, isValid); // isValid formScope && formScope.$valid;
    }

    $scope.isValid = canSave;

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "core.dialogs.seo-save.title", "core.dialogs.seo-save.message");
    };

    blade.toolbarCommands = [
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntities);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        }
    ];

    blade.headIcon = 'fa-globe';
    blade.title = 'core.blades.seo-detail.title';
    //blade.titleValues = { store: blade.store.id };
    blade.subtitle = 'core.blades.seo-detail.subtitle';

    initializeBlade();
}]);
