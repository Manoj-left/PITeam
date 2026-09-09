// wraps the raw SignalR connection so controllers don't touch signalR globals directly
taskManagerApp.factory('taskHubService', ['$timeout', 'HUB_BASE_URL', function ($timeout, HUB_BASE_URL) {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl(HUB_BASE_URL)
        .withAutomaticReconnect()
        .build();

    connection.start().catch(function (err) {
        console.error('SignalR connection failed: ', err);
    });

    return {
        // SignalR callbacks run outside Angular's digest cycle, so $timeout tells Angular to re-check the view
        on: function (eventName, callback) {
            connection.on(eventName, function () {
                var args = arguments;
                $timeout(function () {
                    callback.apply(null, args);
                });
            });
        }
    };
}]);
