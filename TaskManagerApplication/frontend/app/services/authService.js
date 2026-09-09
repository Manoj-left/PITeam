// keeps the logged-in user in the browser's storage; no server session/token yet (that's a later phase)
taskManagerApp.factory('authService', ['$http', '$window', 'API_BASE_URL', function ($http, $window, API_BASE_URL) {
    var apiUrl = API_BASE_URL + '/auth';
    var storageKey = 'taskManagerCurrentUser';

    return {
        register: function (username, password) {
            return $http.post(apiUrl + '/register', { username: username, password: password });
        },
        login: function (username, password) {
            return $http.post(apiUrl + '/login', { username: username, password: password });
        },
        setCurrentUser: function (user) {
            $window.localStorage.setItem(storageKey, angular.toJson(user));
        },
        getCurrentUser: function () {
            var stored = $window.localStorage.getItem(storageKey);
            return stored ? angular.fromJson(stored) : null;
        },
        // Role is serialized as its backend enum int (0 = User, 1 = Admin), matching how Status/Priority are handled elsewhere
        isAdmin: function (user) {
            return !!user && user.role === 1;
        },
        logout: function () {
            $window.localStorage.removeItem(storageKey);
        }
    };
}]);
