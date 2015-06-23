angular.module('platformWebApp')
.controller('platformWebApp.editTemplateController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	$scope.selectedEntityId = null;
	var blade = $scope.blade;
	var codemirrorEditor;

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.getTemplate({ type: blade.currentEntityParent.type, objectId: blade.currentEntityParent.objectId }, function (data) {
			blade.origEntity = _.clone(data);
			blade.isLoading = false;
			blade.currentEntity = data;

			$timeout(function () {
				if (codemirrorEditor) {
					codemirrorEditor.refresh();
					codemirrorEditor.focus();
				}
				blade.origEntity = angular.copy(blade.currentEntity);
			}, 1);
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.updateTemplate = function () {
		blade.isLoading = true;
		notifications.updateTemplate({}, blade.currentEntity, function () {
			blade.isLoading = false;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.testResolve = function () {
		var newBlade = {
			id: 'testResolve',
			title: 'Test resolving notification template',
			notificationType: blade.currentEntityParent.type,
			controller: 'platformWebApp.testResolveController',
			template: 'Scripts/app/newnotifications/blades/notifications-test-resolve.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.testSend = function () {

	}

	$scope.blade.toolbarCommands = [
        {
        	name: "Save template", icon: 'fa fa-save',
        	executeMethod: function () {
        		blade.updateTemplate();
        	},
        	canExecuteMethod: function () {
        		return !angular.equals(blade.origEntity, blade.currentEntity);
        	}
        },
		{
			name: "Undo changes", icon: 'fa fa-undo',
			executeMethod: function () {
				blade.currentEntity = _.clone(blade.origEntity);
			},
			canExecuteMethod: function () {
				return !angular.equals(blade.origEntity, blade.currentEntity);
			}
		},
		{
			name: "Test template", icon: 'fa fa-plus-square',
			executeMethod: function () {
				blade.testResolve();
			},
			canExecuteMethod: function () {
				return true;
			}
		}
	];

	$scope.editorOptions = {
		lineWrapping: true,
		lineNumbers: true,
		parserfile: "liquid.js",
		extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
		foldGutter: true,
		gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
		onLoad: function (_editor) {
			codemirrorEditor = _editor;
		},
		mode: "liquid-html"
	};

	blade.initialize();
}]);