taskManagerApp.controller('TaskListController', ['taskService', 'taskHubService', 'authService', function (taskService, taskHubService, authService) {
    var taskList = this;
    taskList.tasks = [];
    taskList.errorMessage = '';
    taskList.currentUser = authService.getCurrentUser();
    taskList.isAdmin = authService.isAdmin(taskList.currentUser);

    // display labels, indexed to match the backend enum's int values
    taskList.statusLabels = ['Not Started', 'In Progress', 'Completed', 'On Hold'];
    taskList.priorityLabels = ['Low', 'Medium', 'High'];

    // values sent to the API and this array must match the backend enum member names exactly
    taskList.statusOptions = ['NotStarted', 'InProgress', 'Completed', 'OnHold'];
    taskList.priorityOptions = ['Low', 'Medium', 'High'];

    taskList.filters = {
        status: '',
        priority: ''
    };

    taskList.sort = {
        SortBy: ''
    };

    taskList.applyFilters = function () {
        var params = {};
        // admins see every user's tasks; a regular user only ever sees their own
        if (!taskList.isAdmin && taskList.currentUser) {
            params.userId = taskList.currentUser.id;
        }
        if (taskList.filters.status) {
            params.status = taskList.filters.status;
        }
        if (taskList.filters.priority) {
            params.priority = taskList.filters.priority;
        }
        if (taskList.sort.SortBy) {
            params.sortBy = taskList.sort.SortBy;
            // priority is stored Low=0..High=2, so descending shows High first; due date ascending shows the soonest date first
            params.isAscending = taskList.sort.SortBy !== 'priority';
        }

        taskService.getByFilters(params).then(function (response) {
            taskList.tasks = response.data;
            taskList.errorMessage = '';
        }).catch(function (error) {
            taskList.tasks = [];
            taskList.errorMessage = 'Error fetching tasks: ' + error.status;
        });
    };

    taskList.applyFilters();

    taskList.deleteTask = function (id) {
    taskService.delete(id).then(function () {
        taskList.applyFilters(); // refresh the list
    }).catch(function (error) {
        taskList.errorMessage = 'Error deleting task: ' + error.status;
    });
};

    // live updates: any other tab creating/editing/deleting a task refreshes this list too
    taskHubService.on('TaskCreated', taskList.applyFilters);
    taskHubService.on('TaskUpdated', taskList.applyFilters);
    taskHubService.on('TaskDeleted', taskList.applyFilters);
}]);