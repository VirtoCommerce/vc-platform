angular.module('platformWebApp')
    .factory('platformWebApp.signInMethodService', ['$q', 'platformWebApp.signInMethodsApi', function ($q, signInMethodsApi) {
        const extensions = {};
        const signInMethodsLoading = signInMethodsApi.getSignInMethods().$promise;

        function applyExtension(loadedSignInMethod) {
            const signInMethod = angular.extend({}, loadedSignInMethod, extensions[loadedSignInMethod.name]);

            signInMethod.canSignIn = angular.isFunction(signInMethod.signIn);
            signInMethod.canCreateKey = signInMethod.canCreateKey && angular.isFunction(signInMethod.createKey);

            return signInMethod;
        }

        return {
            extendSignInMethod: function (name, extension) {
                extensions[name] = extension;
            },
            onSignInMethodsLoaded: function (callback) {
                return signInMethodsLoading.then(callback);
            },
            getSignInMethods: function () {
                return signInMethodsLoading.then(function (loadedSignInMethods) {
                    return _.map(loadedSignInMethods, applyExtension);
                });
            },
            getUserSignInMethods: function () {
                return signInMethodsApi.getCurrentUserSignInMethods().$promise.then(function (loadedSignInMethods) {
                    return _.map(loadedSignInMethods, applyExtension);
                });
            },
            signInWithParameters: function (parameters) {
                return signInMethodsApi.signIn(parameters).$promise;
            },
            signIn: function (signInMethod) {
                return $q.when(signInMethod.signIn());
            },
            createKey: function (signInMethod) {
                return $q.when(signInMethod.createKey());
            },
            renameKey: function (signInMethod, key, name) {
                return signInMethodsApi.renameKey({ name: signInMethod.name, id: key.id }, { name: name }).$promise;
            },
            deleteKey: function (signInMethod, key) {
                return signInMethodsApi.deleteKey({ name: signInMethod.name, id: key.id }).$promise;
            },
        };
    }]);
