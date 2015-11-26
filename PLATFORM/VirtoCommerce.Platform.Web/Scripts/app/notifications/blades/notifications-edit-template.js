﻿angular.module('platformWebApp')
.controller('platformWebApp.editTemplateController', ['$rootScope', '$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.notifications', function ($rootScope, $scope, $timeout, bladeNavigationService, dialogService, notifications) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

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
		    blade.isLoading = false;
		    blade.origEntity = _.clone(blade.currentEntity);
			if (!blade.isNew) {
				blade.parentBlade.initialize();
			}
			else {
			    blade.isNew = false;
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
			title: 'platform.blades.notifications-test-resolve.title',
			subtitle: 'platform.blades.notifications-test-resolve.subtitle',
			subtitleValues: { type: blade.notificationType },
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: blade.currentEntity.language,
			controller: 'platformWebApp.testResolveController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-test-resolve.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.testSend = function () {
		var newBlade = {
			id: 'testSend',
			title: 'platform.blades.notifications-test-send.title',
			subtitle: 'platform.blades.notifications-test-send.subtitle',
			subtitleValues: { type: blade.notificationType },
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: blade.currentEntity.language,
			controller: 'platformWebApp.testSendController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-test-send.tpl.html'
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
			    name: "platform.commands.save", icon: 'fa fa-save',
				executeMethod: function () {
					blade.updateTemplate();
				},
				canExecuteMethod: function () {
					return blade.canSave();
				}
			},
			{
			    name: "platform.commands.undo", icon: 'fa fa-undo',
				executeMethod: function () {
					blade.currentEntity = _.clone(blade.origEntity);
				},
				canExecuteMethod: function () {
					return blade.canSave();
				}
			},
			{
			    name: "platform.commands.preview", icon: 'fa fa-eye',
				executeMethod: function () {
					blade.testResolve();
				},
				canExecuteMethod: function () {
					return !blade.canSave();
				}
			},
			{
			    name: "platform.commands.send", icon: 'fa fa-envelope',
				executeMethod: function () {
					blade.testSend();
				},
				canExecuteMethod: function () {
					return !blade.canSave();
				}
			},
			{
			    name: "platform.commands.set-active", icon: 'fa fa-pencil-square-o',
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
			    name: "platform.commands.delete", icon: 'fa fa-trash',
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
			    name: "platform.commands.create", icon: 'fa fa-save',
				executeMethod: function () {
					blade.updateTemplate();
				},
				canExecuteMethod: function () {
					return blade.canSave();
				}
			}
		];
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
		var retVal = (!angular.equals(blade.origEntity, blade.currentEntity) || blade.isNew);
		if (!angular.isUndefined($scope.formScope)) {
			retVal = retVal && !$scope.formScope.$invalid;
		}
		return retVal;
	}

	blade.onClose = function (closeCallback) {
	    if (blade.canSave()) {
	        var dialog = {
	            id: "confirmCurrentBladeClose",
	            title: "platform.dialogs.notification-template-save.title",
	            message: "platform.dialogs.notification-template-save.message",
	            callback: function (needSave) {
	                if (needSave) {
	                    blade.updateTemplate();
	                }
	                closeCallback();
	            }
	        }
	        dialogService.showConfirmationDialog(dialog);
	    }
	    else {
	        closeCallback();
	    }
	};

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