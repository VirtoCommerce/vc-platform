angular.module('platformWebApp')
    .factory('platformWebApp.userProfileIconService', ['$rootScope', function ($rootScope) {

    var currentMemberId = null;
    var userIconUrl = null;

    $rootScope.$on("loginStatusChanged", function (event, authContext) {
        if (!authContext.memberId) {
            setUserIconUrl(currentMemberId, null);
        }
        currentMemberId = authContext.memberId;
    });

    var service = {
        iconUrlChanging: false,
        setUserIconUrl: setUserIconUrl,
        getUserIconUrl: function () {
            return userIconUrl;
        }
    };

    function setUserIconUrl (memberId, url) {
        if (currentMemberId === memberId) {
            userIconUrl = url;
            service.iconUrlChanging = !service.iconUrlChanging;
        }
    }

    return service;
}]);
