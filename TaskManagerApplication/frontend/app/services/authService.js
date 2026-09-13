// keeps the logged-in user in the browser's storage; no server session/token yet 
// sessionStorage (not localStorage) is scoped per browser tab, so logging in as different users in
// different tabs doesn't overwrite each other's session when one of the tabs refreshes
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
            $window.sessionStorage.setItem(storageKey, angular.toJson(user));
        },
        getCurrentUser: function () {
            var stored = $window.sessionStorage.getItem(storageKey);
            return stored ? angular.fromJson(stored) : null;
        },
        // Role is serialized as its backend enum int (0 = User, 1 = Admin).
        isAdmin: function (user) {
            return !!user && user.role === 1;
        },
        logout: function () {
            $window.sessionStorage.removeItem(storageKey);
        }
    };
}]);
