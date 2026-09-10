taskManagerApp.factory('userService', ['$http', 'API_BASE_URL', function ($http, API_BASE_URL) {
    var apiUrl = API_BASE_URL + '/users';

    return {
        search: function (search) {
            return $http.get(apiUrl, { params: { search: search } });
        },
        delete: function (id) {
            return $http.delete(apiUrl + '/' + id);
        }
    };
}]);
