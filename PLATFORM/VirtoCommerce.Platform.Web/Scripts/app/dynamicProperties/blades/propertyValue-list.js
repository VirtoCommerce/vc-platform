angular.module('platformWebApp')
.controller('platformWebApp.propertyValueListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', function ($scope, bladeNavigationService, dialogService, settings) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';
    blade.title = "Properties values";
    blade.subtitle = "Edit properties values";
    $scope.languages = [];

    $scope.blade.refresh = function () {
    	settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (data) {
    		$scope.languages = data;
    	});
    	blade.origEntity = blade.currentEntity;
    	blade.currentEntity = angular.copy(blade.currentEntity);
    	blade.isLoading = false;
    };
  
    function isDirty() {
    	return !angular.equals(blade.currentEntity, blade.origEntity);
    }

    $scope.cancelChanges = function () {
    	angular.copy(blade.origEntity.dynamicProperties, blade.currentEntity.dynamicProperties);
    	$scope.bladeClose();
    };

    $scope.saveChanges = function () {
    	if (isDirty()) {
    		angular.copy(blade.currentEntity.dynamicProperties, blade.origEntity.dynamicProperties);
    	}
    	$scope.bladeClose();
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.editDictionary = function (property) {
    	var editDictionaryBlade = {
    		id: "propertyDictionary",
    		controller: 'platformWebApp.propertyDictionaryController',
    		template: 'Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html',
    		currentEntity: property
    	};
		bladeNavigationService.showBlade(editDictionaryBlade, blade);
    };

    blade.toolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
            	angular.copy(blade.origEntity.dynamicProperties, blade.currentEntity.dynamicProperties);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
		{
		 	name: "Manage type properties", icon: 'fa fa-edit',
		 	executeMethod: function () {
		 		var newBlade = {
		 			id: 'dynamicPropertyList',
		 			objectType: blade.currentEntity.objectType,
		 			controller: 'platformWebApp.dynamicPropertyListController',
		 			template: 'Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
		 		};
		 		bladeNavigationService.showBlade(newBlade, blade);
		 	},
		 	canExecuteMethod: function () {
		 		return angular.isDefined(blade.currentEntity.objectType);
		 	}
		 }
    ];

    $scope.blade.refresh();
}]);
