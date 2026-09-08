taskManagerApp.controller('TaskListController',['$scope',"taskService", function($scope,taskService){
        var vm=this;
        vm.tasks=[];
        taskService.getAll().then(function(response){
            vm.tasks=response.data;
        });
}]);