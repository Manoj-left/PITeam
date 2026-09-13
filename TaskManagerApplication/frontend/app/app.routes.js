taskManagerApp.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    // '/login' instead of the default '#!/login'; works with any static file server, no SPA-fallback needed
    $locationProvider.hashPrefix('');
    // $routeProvider.when(path, routeDefinition)
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
        .when('/dashboard', {
            templateUrl: 'app/views/dashboard.html',
            controller: 'DashboardController',
            controllerAs: 'dashboard',
            requiresAuth: true
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
        // we have a route parameter named id here and is accessible in TaskFormController.js
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
