angular.module('platformWebApp')
.factory('signalRHubProxy', ['$rootScope', 'signalRServer', function ($rootScope, signalRServer) {
    function signalRHubProxyFactory(serverUrl, hubName, startOptions) {
        var connection = $.hubConnection(signalRServer);
        var proxy = connection.createHubProxy(hubName);
        connection.start(startOptions).done(function () { });

        return {
            on: function (eventName, callback) {
                proxy.on(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            off: function (eventName, callback) {
                proxy.off(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            invoke: function (methodName, callback) {
                proxy.invoke(methodName)
                    .done(function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
            },
            connection: connection
        };
    };

    return signalRHubProxyFactory;
}]);
