angular.module('virtoCommerce.customerModule')
.factory('virtoCommerce.customerModule.members', ['$resource', function ($resource) {
	return $resource('api/members/:_id', { _id: '@_id' }, {
    	search: { method: 'POST', url: 'api/members/search' },
    	update: { method: 'PUT' }
    });
}])
.factory('virtoCommerce.customerModule.organizations', ['$resource', function ($resource) {
	return $resource('api/members/organizations', {});
}])
