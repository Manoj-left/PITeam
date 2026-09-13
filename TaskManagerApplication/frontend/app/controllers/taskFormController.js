taskManagerApp.controller('TaskFormController', ['$routeParams','$location','taskService','authService', function($routeParams, $location, taskService, authService){

    var taskForm=this;
    taskForm.task={};
    taskForm.isEditMode= !!$routeParams.id;
    taskForm.errorMessage = '';
    var currentUser = authService.getCurrentUser();

    if(taskForm.isEditMode){
        taskService.getById($routeParams.id).then(function(response){
            taskForm.task = response.data;
            taskForm.task.dueAt = parseDateOnlyToLocalDate(taskForm.task.dueAt); // date input requires a Date object, not a string
        }).catch(function(error){
            taskForm.errorMessage = 'Error fetching task: ' + error.status;
        });
    }

    // `new Date("yyyy-MM-dd")` is parsed as UTC midnight per the JS spec, which would silently
    // shift the calendar day once converted to local time - build a local-midnight Date directly instead.
    function parseDateOnlyToLocalDate(dateOnlyString) {
        var parts = dateOnlyString.split('-');
        return new Date(parseInt(parts[0], 10), parseInt(parts[1], 10) - 1, parseInt(parts[2], 10));
    }

    // the backend expects a plain "yyyy-MM-dd" string, not the full ISO timestamp a JS Date serializes to
    // uses local getters (not UTC) since the Date is always local midnight (from the date picker or parseDateOnlyToLocalDate above)
    function toDateOnlyString(date) {
        var year = date.getFullYear();
        var month = ('0' + (date.getMonth() + 1)).slice(-2);
        var day = ('0' + date.getDate()).slice(-2);
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