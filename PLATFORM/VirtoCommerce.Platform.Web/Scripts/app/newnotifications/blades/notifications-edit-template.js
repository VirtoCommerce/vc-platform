angular.module('platformWebApp')
.controller('platformWebApp.editTemplateController', ['$rootScope', '$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, $timeout, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;
	var codemirrorEditor;
	blade.parametersForTemplate = [];

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.getTemplate({ type: blade.notificationType, objectId: blade.objectId, objectTypeId: blade.objectTypeId, language: blade.language }, function (data) {
			data.type = blade.notificationType;
			data.objectId = blade.objectId;
			data.objectTypeId = blade.objectTypeId;
			if (blade.language === 'undefined' && _.indexOf(blade.usedLanguages, data.language) !== -1) {
				data.language = _.first(blade.languages);
			}
			blade.origEntity = _.clone(data);
			blade.currentEntity = data;

			notifications.prepareTestData({ type: blade.notificationType }, function (data) {
				blade.parametersForTemplate = data;

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
			if (!blade.isNew) {
				blade.isLoading = false;
				blade.origEntity = _.clone(blade.currentEntity);
				blade.parentBlade.initialize();
			}
			else {
				if (!blade.isFirst) {
					blade.parentBlade.initialize();
					bladeNavigationService.closeBlade(blade);
				}
				else {
					blade.parentBlade.openList(blade.notificationType, blade.objectId, blade.objectTypeId);
				}
			}
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.testResolve = function () {
		var newBlade = {
			id: 'testResolve',
			title: 'Preview notification',
			subtitle: 'Enter test data for ' + blade.notificationType,
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: blade.currentEntity.language,
			controller: 'platformWebApp.testResolveController',
			template: 'Scripts/app/newnotifications/blades/notifications-test-resolve.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.testSend = function () {
		var newBlade = {
			id: 'testSend',
			title: 'Send notification',
			subtitle: 'Enter test data for ' + blade.notificationType,
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: blade.currentEntity.language,
			controller: 'platformWebApp.testSendController',
			template: 'Scripts/app/newnotifications/blades/notifications-test-send.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.setActive = function () {
		blade.currentEntity.isDefault = true;
		blade.updateTemplate();
	}

	blade.delete = function () {
		notifications.deleteTemplate({ id: blade.currentEntity.id }, function (data) {
			blade.parentBlade.initialize();
			bladeNavigationService.closeBlade(blade);
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	}

	if (!blade.isNew) {
		$scope.blade.toolbarCommands = [
			{
				name: "Save", icon: 'fa fa-save',
				executeMethod: function () {
					blade.updateTemplate();
				},
				canExecuteMethod: function () {
					return blade.canSave();
				}
			},
			{
				name: "Undo", icon: 'fa fa-undo',
				executeMethod: function () {
					blade.currentEntity = _.clone(blade.origEntity);
				},
				canExecuteMethod: function () {
					return blade.canSave();
				}
			},
			{
				name: "Preview", icon: 'fa fa-eye',
				executeMethod: function () {
					blade.testResolve();
				},
				canExecuteMethod: function () {
					return !blade.canSave();
				}
			},
			{
				name: "Send", icon: 'fa fa-envelope',
				executeMethod: function () {
					blade.testSend();
				},
				canExecuteMethod: function () {
					return !blade.canSave();
				}
			},
			{
				name: "Set Active", icon: 'fa fa-pencil-square-o',
				executeMethod: function () {
					blade.setActive();
				},
				canExecuteMethod: function () {
					if (angular.isUndefined(blade.currentEntity)) {
						return false;
					}
					return !blade.currentEntity.isDefault;
				}
			},
			{
				name: "Delete", icon: 'fa fa-trash',
				executeMethod: function () {
					blade.delete();
				},
				canExecuteMethod: function () {
					return !blade.canSave();
				}
			}
		];
	}
	else {
		$scope.blade.toolbarCommands = [
			{
				name: "Create", icon: 'fa fa-save',
				executeMethod: function () {
					blade.updateTemplate();
				},
				canExecuteMethod: function () {
					return blade.canSave();
				}
			}
		];
	}

	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

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

	blade.canSave = function () {
		return !angular.equals(blade.origEntity, blade.currentEntity) && !$scope.formScope.$invalid;
	}

	blade.getLanguages = function () {
		blade.languages = ['ru-RU', 'en-US', 'fr-FR', 'de-DE'];
		blade.languages = _.difference(blade.languages, blade.usedLanguages);
	}

	blade.getFlag = function (x) {
		switch (x) {
			case 'ru-RU':
				return 'ru';

			case 'en-US':
				return 'us';

			case 'fr-FR':
				return 'fr';

			case 'zh-CN':
				return 'ch';

			case 'ru-RU':
				return 'ru';

			case 'ja-JP':
				return 'jp';

			case 'de-DE':
				return 'de';
		}
	}

	blade.headIcon = 'fa-envelope';

	blade.getLanguages();
	blade.initialize();
}]);