'use strict';

angular.module('virtoCommerce.coreModule.common')
    .directive('vaNavListBlade', function () {
    	return {
    		restrict: 'E',
    		scope: {
    			min: '=',
    			max: '=',
    			step: '='
    		},
    		link: function (scope, el, attrs, ctrl) { }
    	}
    });