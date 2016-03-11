angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.imagesController', ['$scope', '$filter', '$translate', 'FileUploader', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService', 'platformWebApp.assets.api', function ($scope, $filter, $translate, FileUploader, dialogService, bladeNavigationService, authService, assets) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';

    blade.refresh = function (parentRefresh) {
        blade.currentResource.get({ id: blade.currentEntityId }, function (data) {
            if ($scope.uploader)
                $scope.uploader.url = 'api/platform/assets?folderUrl=catalog/' + data.code;
            $scope.origItem = data;
            blade.currentEntity = angular.copy(data);
            blade.isLoading = false;
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, $scope.origItem) && blade.hasUpdatePermission();
    }

    $scope.reset = function () {
        angular.copy($scope.origItem, blade.currentEntity);
    };

    $scope.addImageFromUrl = function () {
        if (blade.newExternalImageUrl) {
            assets.uploadFromUrl({ folderUrl: 'catalog/' + $scope.origItem.code, url: blade.newExternalImageUrl }, function (data) {
                blade.currentEntity.images.push(data);
                blade.newExternalImageUrl = undefined;
            });
        }
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, $scope.saveChanges, closeCallback, "catalog.dialogs.image-save.title", "catalog.dialogs.image-save.message");
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;
        blade.currentResource.update({}, { id: blade.currentEntityId, images: blade.currentEntity.images }, function () {
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    function initialize() {
        if (!$scope.uploader && blade.hasUpdatePermission()) {
            // create the uploader            
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // Images only
            uploader.filters.push({
                name: 'imageFilter',
                fn: function (i /*{File|FileLikeObject}*/, options) {
                    var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            });

            uploader.onSuccessItem = function (fileItem, images, status, headers) {
                angular.forEach(images, function (image) {
                    //ADD uploaded image
                    blade.currentEntity.images.push(image);
                });
            };

            uploader.onAfterAddingAll = function (addedItems) {
                bladeNavigationService.setError(null, blade);
            };

            uploader.onErrorItem = function (item, response, status, headers) {
                bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
            };
        }
    };

    $scope.toggleImageSelect = function (e, image) {
        if (e.ctrlKey == 1) {
            image.$selected = !image.$selected;
        } else {
            if (image.$selected) {
                image.$selected = false;
            } else {
                image.$selected = true;
            }
        }
    }

    $scope.removeItem = function (image) {
        var idx = blade.currentEntity.images.indexOf(image);
        if (idx >= 0) {
            blade.currentEntity.images.splice(idx, 1);
        }
    };

    $scope.copyUrl = function (data) {
        $translate('catalog.blades.images.labels.copy-url-prompt').then(function (promptMessage) {
            window.prompt(promptMessage, data.url);
        });
    }

    $scope.removeAction = function (selectedImages) {
        if (selectedImages == undefined) {
            selectedImages = $filter('filter')(blade.currentEntity.images, { $selected: true });
        }

        angular.forEach(selectedImages, function (image) {
            var idx = blade.currentEntity.images.indexOf(image);
            if (idx >= 0) {
                blade.currentEntity.images.splice(idx, 1);
            }
        });
    };

    blade.headIcon = 'fa-image';

    blade.toolbarCommands = [
        {
            name: 'platform.commands.save', icon: 'fa fa-save',
            executeMethod: $scope.saveChanges,
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
		{
		    name: 'platform.commands.remove', icon: 'fa fa-trash-o', executeMethod: function () { $scope.removeAction(); },
		    canExecuteMethod: function () {
		        var retVal = false;
		        if (blade.currentEntity && blade.currentEntity.images) {
		            var selectedImages = $filter('filter')(blade.currentEntity.images, { $selected: true });
		            retVal = selectedImages.length > 0;
		        }
		        return retVal;
		    },
		    permission: blade.updatePermission
		},
        {
            name: 'catalog.commands.gallery', icon: 'fa fa-image',
            executeMethod: function () {
                var dialog = {
                    images: blade.currentEntity.images,
                    currentImage: blade.currentEntity.images[0]
                };
                dialogService.showGalleryDialog(dialog);
            },
            canExecuteMethod: function () {
                return blade.currentEntity && _.any(blade.currentEntity.images);
            }
        }
    ];

    $scope.sortableOptions = {
        update: function (e, ui) {

        },
        stop: function (e, ui) {
        }
    };

    initialize();
    blade.refresh();

}]);
