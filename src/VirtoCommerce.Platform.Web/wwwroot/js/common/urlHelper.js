angular.module('platformWebApp').factory('platformWebApp.urlHelper', [function () {
    function getSafeReturnUrl() {
        const returnUrl = getQueryValue('ReturnUrl');
        if (returnUrl && isLocalUrl(returnUrl)) {
            return returnUrl;
        }

        return undefined;
    }

    function getQueryValue(name) {
        const query = new URLSearchParams(window.location.search);
        return query.get(name);
    }

    function isLocalUrl(value) {
        try {
            const url = new URL(value, window.location.origin);
            return url.origin === window.location.origin;
        } catch (e) {
            return false;
        }
    }

    return {
        getSafeReturnUrl,
        getQueryValue,
        isLocalUrl
    };
}]);
