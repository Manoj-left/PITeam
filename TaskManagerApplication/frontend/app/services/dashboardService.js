taskManagerApp.factory('dashboardService', ['$http', 'API_BASE_URL', function ($http, API_BASE_URL) {
    var apiUrl = API_BASE_URL + '/dashboard';

    return {
        getStats: function (userId, isAdmin) {
            return $http.get(apiUrl, { params: { userId: userId, isAdmin: isAdmin } });
        }
    };
}]);
