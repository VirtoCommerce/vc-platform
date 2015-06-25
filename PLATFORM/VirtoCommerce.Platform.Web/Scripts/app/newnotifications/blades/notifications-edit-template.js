angular.module('platformWebApp')
.controller('platformWebApp.editTemplateController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	$scope.selectedEntityId = null;
	var blade = $scope.blade;
	var codemirrorEditor;
	blade.parametersForTemplate = [];

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.getTemplate({ type: blade.currentEntityParent.type, objectId: blade.currentEntityParent.objectId }, function (data) {
			blade.origEntity = _.clone(data);
			blade.currentEntity = data;

			notifications.prepareTestData({ type: blade.currentEntityParent.type }, function (data) {
				for (var i = 0; i < data.length; i++) {
					var words = data[i].match(/([A-Z]?[^A-Z]*)/g).slice(0, -1);
					var preparedWords = [];
					for (var j = 0; j < words.length; j++) {
						preparedWords.push(words[j].toLowerCase());
					}

					var addedParam = "{{ context." + preparedWords.join("_") + " }}";

					blade.parametersForTemplate.push({ key: data[i], value: addedParam });
				}
				blade.isLoading = false;

			}, function (error) {
				bladeNavigationService.setError('Error ' + error.status, blade);
			});

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
			blade.origEntity = _.clone(blade.currentEntity);
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.testResolve = function () {
		var newBlade = {
			id: 'testResolve',
			title: 'Preview notification',
			subtitle: 'Enter test data for ' + blade.currentEntityParent.type,
			notificationType: blade.currentEntityParent.type,
			controller: 'platformWebApp.testResolveController',
			template: 'Scripts/app/newnotifications/blades/notifications-test-resolve.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.testSend = function () {
		var newBlade = {
			id: 'testSend',
			title: 'Send notification',
			subtitle: 'Enter test data for ' + blade.currentEntityParent.type,
			notificationType: blade.currentEntityParent.type,
			controller: 'platformWebApp.testSendController',
			template: 'Scripts/app/newnotifications/blades/notifications-test-send.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	$scope.blade.toolbarCommands = [
        {
        	name: "Save", icon: 'fa fa-save',
        	executeMethod: function () {
        		blade.updateTemplate();
        	},
        	canExecuteMethod: function () {
        		return !angular.equals(blade.origEntity, blade.currentEntity);
        	}
        },
		{
			name: "Undo", icon: 'fa fa-undo',
			executeMethod: function () {
				blade.currentEntity = _.clone(blade.origEntity);
			},
			canExecuteMethod: function () {
				return !angular.equals(blade.origEntity, blade.currentEntity);
			}
		},
		{
			name: "Preview", icon: 'fa fa-eye',
			executeMethod: function () {
				blade.testResolve();
			},
			canExecuteMethod: function () {
				return angular.equals(blade.origEntity, blade.currentEntity);
			}
		},
		{
			name: "Send", icon: 'fa fa-envelope',
			executeMethod: function () {
				blade.testSend();
			},
			canExecuteMethod: function () {
				return angular.equals(blade.origEntity, blade.currentEntity);
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

	blade.headIcon = 'fa-envelope';

	blade.initialize();
}]);