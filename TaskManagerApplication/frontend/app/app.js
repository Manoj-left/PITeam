// this is like namespacing — depends on ngRoute for view switching without full page reloads
var taskManagerApp = angular.module('taskManagerApp', ['ngRoute']);

taskManagerApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/tasks', {
            templateUrl: 'app/views/taskList.html',
            controller: 'TaskListController',
            controllerAs: 'vm'
        })
        .when('/tasks/new', {
            templateUrl: 'app/views/taskForm.html',
            controller: 'TaskFormController',
            controllerAs: 'vm'
        })
        .when('/tasks/:id/edit', {
            templateUrl: 'app/views/taskForm.html',
            controller: 'TaskFormController',
            controllerAs: 'vm'
        })
        .otherwise({ redirectTo: '/tasks' });
}]);