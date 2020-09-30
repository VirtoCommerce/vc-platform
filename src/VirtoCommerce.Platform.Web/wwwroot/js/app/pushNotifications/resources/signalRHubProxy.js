import * as signalR from '@aspnet/signalr';

angular.module('platformWebApp').factory('platformWebApp.signalRHubProxy', ['$rootScope', function ($rootScope) {
    function signalRHubProxyFactory() {
        var reconnectionIntervals = [2000, 5000, 10000, 30000];
        var reconnectionIndex = 0;

        var connection = new signalR.HubConnectionBuilder()
            .withUrl('/pushNotificationHub')
            .configureLogging(signalR.LogLevel.Error)
            .build();

        connection.start();

        async function start() {
            try {
                await connection.start();
                reconnectionIndex = 0;
            } catch (err) {
                if (reconnectionIndex > reconnectionIntervals.length - 1) {
                    reconnectionIndex = reconnectionIntervals.length - 1;
                }

                setTimeout(() => {
                    reconnectionIndex++;
                    start();
                }, reconnectionIntervals[reconnectionIndex]);
            }
        }

        connection.onclose(async () => await start());

        return {
            on: function (eventName, callback) {
                connection.on(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            off: function (eventName, callback) {
                connection.off(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            invoke: function (methodName, callback) {
                connection.invoke(methodName)
                    .done(function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
            },
            connection: connection
        }
    };

    return signalRHubProxyFactory;
}]);
