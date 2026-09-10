// shows/hides nav links based on login state; refreshed on every route change since nav lives outside ng-view
taskManagerApp.controller('NavController', ['$rootScope', '$location', 'authService', function ($rootScope, $location, authService) {
    var nav = this;

    function refresh() {
        nav.currentUser = authService.getCurrentUser();
        nav.isAdmin = authService.isAdmin(nav.currentUser);
    }

    refresh();
    $rootScope.$on('$routeChangeSuccess', refresh);

    // startsWith so /tasks/:id/edit still marks the Tasks link active
    nav.isActive = function (path) {
        var current = $location.path();
        return current === path || current.indexOf(path + '/') === 0;
    };

    nav.logout = function () {
        authService.logout();
        refresh();
        $location.path('/login');
    };
}]);
