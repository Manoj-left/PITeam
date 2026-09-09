// this is like namespacing — depends on ngRoute for view switching without full page reloads
var taskManagerApp = angular.module('taskManagerApp', ['ngRoute']);

taskManagerApp.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    // '#/login' instead of the default '#!/login'; works with any static file server, no SPA-fallback needed
    $locationProvider.hashPrefix('');
    $routeProvider
        .when('/login', {
            templateUrl: 'app/views/login.html',
            controller: 'LoginController',
            controllerAs: 'login'
        })
        .when('/register', {
            templateUrl: 'app/views/register.html',
            controller: 'RegisterController',
            controllerAs: 'register'
        })
        .when('/tasks', {
            templateUrl: 'app/views/taskList.html',
            controller: 'TaskListController',
            controllerAs: 'taskList',
            requiresAuth: true
        })
        .when('/tasks/new', {
            templateUrl: 'app/views/taskForm.html',
            controller: 'TaskFormController',
            controllerAs: 'taskForm',
            requiresAuth: true
        })
        .when('/tasks/:id/edit', {
            templateUrl: 'app/views/taskForm.html',
            controller: 'TaskFormController',
            controllerAs: 'taskForm',
            requiresAuth: true
        })
        .when('/admin/users', {
            templateUrl: 'app/views/userList.html',
            controller: 'UserListController',
            controllerAs: 'userList',
            requiresAuth: true,
            requiresAdmin: true
        })
        .otherwise({ redirectTo: '/tasks' });
}]);

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