angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemImageController', ['$scope', 'virtoCommerce.catalogModule.items', '$filter', 'FileUploader', 'dialogService', function ($scope, items, $filter, FileUploader, dialogService) {
    $scope.currentBlade = $scope.blade;
	$scope.item = {};
	$scope.origItem = {};

	$scope.currentBlade.refresh = function (parentRefresh) {
		items.get({ id: $scope.currentBlade.itemId }, function (data) {
			$scope.origItem = data;
			$scope.item = angular.copy(data);
			$scope.currentBlade.isLoading = false;
			if (parentRefresh) {
				$scope.currentBlade.parentBlade.refresh();
			}
		});
	}

	$scope.isDirty = function () {
		return !angular.equals($scope.item, $scope.origItem);
	};

	$scope.reset = function () {
		angular.copy($scope.origItem, $scope.item);
	};

	$scope.currentBlade.onClose = function (closeCallback) {
		if ($scope.isDirty()) {
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
		$scope.currentBlade.isLoading = true;
		items.updateitem({}, $scope.item, function (data, headers)
		{
			$scope.currentBlade.refresh(true);
		});
	};

    function initialize()
    {
        if (!$scope.uploader)
        {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/assets',
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
                	image.itemId = $scope.item.id;
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

    $scope.bladeToolbarCommands = [

        {
            name: "Save", icon: 'fa fa-save',
        	executeMethod: function ()
        	{
        	    $scope.saveChanges();
        	},
        	canExecuteMethod: function ()
        	{
        	    return $scope.isDirty();
        	},
        	permission: 'catalog:items:manage'
        },
		{
		    name: "Remove", icon: 'fa fa-trash-o', executeMethod: function () { $scope.removeAction(); },
			canExecuteMethod: function () {
				var retVal = false;
				if (angular.isDefined($scope.item.images)) {
					var selectedImages = $filter('filter')($scope.item.images, { selected: true });
					retVal = selectedImages.length > 0;
				}
				return retVal;
			},
			permission: 'catalog:items:manage'
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
    $scope.currentBlade.refresh();

}]);
