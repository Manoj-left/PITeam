// Module is the Namespace/container for functionality.
// it has a dependecy array we can pass other module names to include them as dependencies.
// this returned object is used to register controllers, services, directives etc.
// the root module is bootstrapped by ng-app="taskManagerApp"(not the var the actual module) and becomes the app's entry point.

var taskManagerApp = angular.module('taskManagerApp', ['ngRoute']);
