angular.module('platformWebApp.module1.resources.module1resources', [])
.factory('module1resource', ['$resource', function ($resource) {
	return $resource('api/module1/', { }, {
		get: { method: 'GET', url: 'api/module1/get/', isArray:true },
	});

    
}]);

