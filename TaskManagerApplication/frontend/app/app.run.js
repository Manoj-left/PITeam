// frontend-only access gate: real server-side enforcement is a later phase
taskManagerApp.run(['$rootScope', '$location', 'authService', function ($rootScope, $location, authService) {
    $rootScope.$on('$routeChangeStart', function (event, next) {
        if (!next) {
            return;
        }

        var currentUser = authService.getCurrentUser();

        if (next.requiresAuth && !currentUser) {
            $location.path('/login');
            return;
        }

        if (next.requiresAdmin && !authService.isAdmin(currentUser)) {
            $location.path('/tasks');
        }
    });
}]);
