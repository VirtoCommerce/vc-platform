angular.module('platformWebApp')
    .controller('platformWebApp.assets.assetUploadController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'FileUploader', 'platformWebApp.dialogService', function ($scope, assets, bladeNavigationService, FileUploader, dialogService) {
        var blade = $scope.blade;
        var currentEntities;
        if (!blade.fileUploadOptions) {
            blade.fileUploadOptions = {};
        }
        var folderUrl = blade.currentEntityId || "";

        function initialize() {
            if (!$scope.uploader) {
                // Create the uploader
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    url: 'api/platform/assets?folderUrl=' + folderUrl,
                    method: 'POST',
                    //autoUpload: true,
                    removeAfterUpload: true
                });

                if (blade.fileUploadOptions.filterCallback && angular.isFunction(blade.fileUploadOptions.filterCallback)) {
                    uploader.filters.push({
                        name: 'customFileFilter',
                        fn: blade.fileUploadOptions.filterCallback
                    });
                } else if (blade.fileUploadOptions.accept && blade.fileUploadOptions.accept.contains('image')) {
                    uploader.filters.push({
                        name: 'imageFilter',
                        fn: function (item) {
                            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                            return '|jpg|png|jpeg|bmp|gif|svg+xml|'.indexOf(type) !== -1;
                        }
                    });
                }

                uploader.onAfterAddingAll = function (addedItems) {
                    if (folderUrl) {
                        blade.isLoading = true;
                        blade.uploadCompleted = false;
                        bladeNavigationService.setError(null, blade);

                        // check for asset duplicates
                        assets.search({ folderUrl: folderUrl },
                            function (data) {
                                blade.isLoading = false;
                                currentEntities = data.results;

                                _.each(addedItems, promptUserDecision);
                                uploader.uploadAll();
                            }, function (error) {
                                bladeNavigationService.setError('Error ' + error.status, blade);
                            });
                        
                    } else {
                        dialogService.showNotificationDialog({
                            id: "error",
                            title: "platform.dialogs.asset-upload-error.title",
                            message: "platform.dialogs.asset-upload-error.message"
                        });
                    }
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };

                uploader.onCompleteAll = function () {
                    refreshParentBlade();
                    blade.uploadCompleted = true;
                };

                uploader.onSuccessItem = function (fileItem, images) {
                    if (blade.onUploadComplete) {
                        blade.onUploadComplete(images);
                    }
                };
            }
        }

        function refreshParentBlade() {
            if (blade.parentBlade.refresh && !blade.fileUploadOptions.suppressParentRefresh) {
                blade.parentBlade.refresh();
            }
        }

        function promptUserDecision(item) {
            if (_.any(currentEntities, function (x) { return x.type === 'blob' && x.name.toLowerCase() === item.file.name.toLowerCase() })) {
                var result = prompt("File \"" + item.file.name + "\" already exists!\n- Change name / press [OK] to overwrite.\n- Press [Cancel] to skip this file.\nFile name:", item.file.name);
                if (result == null) {
                    item.remove();
                } else if (!result) {
                    promptUserDecision(item);
                } else if (result !== item.file.name) {
                    item.file.name = result;
                    promptUserDecision(item);
                } else {
                    item.url += "&forceFileOverwrite=true";
                }
            }
        }

        $scope.addImageFromUrl = function () {
            if (blade.newExternalImageUrl) {
                blade.uploadCompleted = false;
                if (folderUrl) {                    
                    assets.uploadFromUrl({ folderUrl: folderUrl, url: blade.newExternalImageUrl }, function (data) {
                        refreshParentBlade();
                        if (blade.onUploadComplete) {
                            blade.onUploadComplete(data);
                        }
                        blade.newExternalImageUrl = undefined;
                        blade.uploadCompleted = true;
                    });
                } else {
                    dialogService.showNotificationDialog({
                        id: "error",
                        title: "platform.dialogs.asset-upload-error.title",
                        message: "platform.dialogs.asset-upload-error.message"
                    });
                }
            }
        };

        blade.headIcon = 'fa-file-o';

        initialize();
        blade.isLoading = false;
    }]);
