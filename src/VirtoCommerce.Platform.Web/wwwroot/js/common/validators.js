angular.module('platformWebApp').factory('platformWebApp.validators', [function () {
    function webSafeFileNameValidator(value) {
        var pattern = /^[\w.-]+$/;
        return pattern.test(value);
    }

    function uriWithoutQuery(value) {
        try {
            new URL(value);
            // url without query path and special chars
            var pattern = /[ !@#$%^&*()+=\[\]{}\\;'"|,<>?]/;
            return !pattern.test(value);
        } catch (_) {
            return false;
        }
    }

    function none(value) {
        return true;
    }

    return {
        webSafeFileNameValidator: webSafeFileNameValidator,
        uriWithoutQuery: uriWithoutQuery,
        none: none
    };
}]);
