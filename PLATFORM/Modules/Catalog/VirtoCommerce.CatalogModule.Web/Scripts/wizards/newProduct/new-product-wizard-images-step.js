angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardImagesController', ['$scope', '$filter', 'FileUploader', function ($scope, $filter, FileUploader)
{
    $scope.blade.isLoading = false;
    $scope.item = angular.copy($scope.blade.item);

	$scope.saveChanges = function () {
	    $scope.blade.parentBlade.item.images = $scope.item.images;
	    $scope.bladeClose();
	};

    function initialize()
    {
        if (!$scope.uploader)
        {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/assets/',
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // Images only
            uploader.filters.push({
                name: 'imageFilter',
                fn: function (i /*{File|FileLikeObject}*/, options)
                {
                    var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            });


            uploader.onSuccessItem = function (fileItem, images, status, headers)
            {
                angular.forEach(images, function (image)
                {
                    if (!angular.isDefined($scope.item.images)) {
                        $scope.item.images = [];
                    }
                    //ADD uploaded image to the item
                    $scope.item.images.push(image);
                });
            };
        }
    };

    $scope.toggleImageSelect = function (e, image)
    {
        if (e.ctrlKey == 1)
        {
            image.selected = !image.selected;
        } else
        {
            angular.forEach($scope.item.images, function (i)
            {
                i.selected = false;
            });
            image.selected = true;
        }
    }

    $scope.removeAction = function (selectedImages)
    {
        if (selectedImages == undefined)
        {
        	selectedImages = $filter('filter')($scope.item.images, { selected: true });
        }

        angular.forEach(selectedImages, function (image)
        {
        	var idx = $scope.item.images.indexOf(image);
            if (idx >= 0)
            {
            	$scope.item.images.splice(idx, 1);
            }
        });
    };

    $scope.blade.toolbarCommands = [
		{
		    name: "Remove", icon: 'fa fa-trash-o', executeMethod: function () { $scope.removeAction(); },
			canExecuteMethod: function () {
				var retVal = false;
				if (angular.isDefined($scope.item.images)) {
					var selectedImages = $filter('filter')($scope.item.images, { selected: true });
					retVal = selectedImages.length > 0;
				}
				return retVal;
			}
		}
    ];

    $scope.sortableOptions = {
        update: function (e, ui)
        {
            if (ui.item.sortable.dropindex == 0) {
                angular.forEach($scope.item.images, function (i)
                {
                    if (i.id == ui.item.sortable.model.id)
                    {
                        i.group = "primaryimage";
                    } else {
                        i.group = "images";
                    }
                });
            }
        },
        stop: function (e, ui)
        {
        }
    };

    initialize();

}]);
