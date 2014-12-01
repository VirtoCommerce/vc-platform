﻿angular.module('catalogModule.blades.import.importJobRun', [
    'angularFileUpload'
])
.controller('importJobRunController', ['$scope', 'FileUploader', 'notificationService', function ($scope, FileUploader, notificationService)
{
    $scope.blade.isLoading = false;
    $scope.job = angular.copy($scope.blade.item);

    $scope.run = function() {
        $scope.job.$run(null, function (notify)
        {
            //TODO show notification
            //notificationService.notificationRefresh();
            $scope.bladeClose();
        });
    }

    function initialize()
    {
        if (!$scope.uploader)
        {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/assets/',
                method: 'PUT',
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // Images only
            uploader.filters.push({
                name: 'csvFilter',
                fn: function (i /*{File|FileLikeObject}*/, options)
                {
                    var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                    return '|csv|vnd.ms-excel|'.indexOf(type) !== -1;
                }
            });


            uploader.onSuccessItem = function (fileItem, asset, status, headers)
            {
                $scope.job.templatePath = asset[0].url;
            };
        }
    };

    initialize();

}]);
