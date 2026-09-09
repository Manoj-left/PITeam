taskManagerApp.controller('TaskFormController', ['$routeParams','$location','taskService','authService', function($routeParams, $location, taskService, authService){

    var taskForm=this;
    taskForm.task={};
    taskForm.isEditMode= !!$routeParams.id;
    taskForm.errorMessage = '';
    var currentUser = authService.getCurrentUser();

    if(taskForm.isEditMode){
        taskService.getById($routeParams.id).then(function(response){
            taskForm.task = response.data;
            taskForm.task.dueAt = new Date(taskForm.task.dueAt); // date input requires a Date object, not a string
        }).catch(function(error){
            taskForm.errorMessage = 'Error fetching task: ' + error.status;
        });
    }

    // the backend expects a plain "yyyy-MM-dd" string (DateOnly), not the full ISO timestamp a JS Date serializes to
    function toDateOnlyString(date) {
        var year = date.getUTCFullYear();
        var month = ('0' + (date.getUTCMonth() + 1)).slice(-2);
        var day = ('0' + date.getUTCDate()).slice(-2);
        return year + '-' + month + '-' + day;
    }

    taskForm.save= function(){
        var payload = angular.copy(taskForm.task);
        payload.dueAt = toDateOnlyString(taskForm.task.dueAt);
        if (!taskForm.isEditMode) {
            payload.userId = currentUser.id; // ownership is set once at creation; edits don't change it
        }
        var request=taskForm.isEditMode? taskService.update($routeParams.id, payload) : taskService.create(payload);
        request.then(function(response){
        $location.path('/tasks');
    }).catch(function(error){
        taskForm.errorMessage = 'Error saving task: ' + error.status;
    })
    };
}]);