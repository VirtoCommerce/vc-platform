angular.module('platformWebApp').factory('platformWebApp.login', ['$resource', '$log', function ($resource, $log) {
    $log.warn('VC0016: platformWebApp.login is obsolete. Use platformWebApp.signInMethodService instead.');

    return $resource('', {}, {
        getLoginTypes: { url: 'api/platform/security/logintypes', method: 'GET', isArray: true }
    });
}]);
