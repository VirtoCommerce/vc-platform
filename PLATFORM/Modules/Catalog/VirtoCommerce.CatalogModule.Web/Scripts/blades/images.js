angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.imagesController', ['$scope', '$filter', 'FileUploader', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService', 'platformWebApp.assets.api', function ($scope, $filter, FileUploader, dialogService, bladeNavigationService, authService, assets) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        blade.currentResource.get({ id: blade.currentEntityId }, function (data) {
            $scope.origItem = data;
            blade.currentEntity = angular.copy(data);
            blade.isLoading = false;
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    $scope.isDirty = function () {
        return !angular.equals(blade.currentEntity, $scope.origItem);
    };

    $scope.reset = function () {
        angular.copy($scope.origItem, blade.currentEntity);
    };

    $scope.addImageFromUrl = function () {
    	if (blade.newExternalImageUrl) {
    		assets.uploadFromUrl({ folderUrl: 'catalog', url: blade.newExternalImageUrl }, function (data) {
    			blade.currentEntity.images.push(data);
    			blade.newExternalImageUrl = undefined;
    		});       
        }
    };

    blade.onClose = function (closeCallback) {
        if ($scope.isDirty() && authService.checkPermission(blade.permission)) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The images has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };


    $scope.saveChanges = function () {
        blade.isLoading = true;
        blade.currentResource.update({}, blade.currentEntity, function () {
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    function initialize() {
        if (!$scope.uploader && authService.checkPermission(blade.permission)) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/platform/assets?folderUrl=catalog',
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
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
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
            name: "Save", icon: 'fa fa-save',
            executeMethod: $scope.saveChanges,
            canExecuteMethod: $scope.isDirty,
            permission: blade.permission
        },
		{
		    name: "Remove", icon: 'fa fa-trash-o', executeMethod: function () { $scope.removeAction(); },
		    canExecuteMethod: function () {
		        var retVal = false;
		        if (blade.currentEntity && blade.currentEntity.images) {
		            var selectedImages = $filter('filter')(blade.currentEntity.images, { $selected: true });
		            retVal = selectedImages.length > 0;
		        }
		        return retVal;
		    },
		    permission: blade.permission
		},
        {
            name: "Gallery", icon: 'fa fa-image',
            executeMethod: function () {
                var dialog = {
                    images: blade.currentEntity.images,
                    currentImage: blade.currentEntity.images[0]
                };
                dialogService.showGalleryDialog(dialog);
            },
            canExecuteMethod: function () {
                var canExecute = false;
                if (blade.currentEntity && blade.currentEntity.images && blade.currentEntity.images.length > 0) {
                    canExecute = true;
                }
                return canExecute;
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
