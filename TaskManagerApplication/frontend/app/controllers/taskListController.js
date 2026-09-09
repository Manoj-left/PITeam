taskManagerApp.controller('TaskListController', ['taskService', 'taskHubService', 'authService', 'userService', '$location', function (taskService, taskHubService, authService, userService, $location) {
    var taskList = this;
    taskList.tasks = [];
    taskList.errorMessage = '';
    taskList.currentUser = authService.getCurrentUser();
    taskList.isAdmin = authService.isAdmin(taskList.currentUser);
    taskList.userMap = {}; // userId -> username, admin-only, used to show each task's owner
    // admins can drill into one user's tasks via a ?userId= query param (set from the Users page); omitted, they see everyone's
    taskList.viewingUserId = taskList.isAdmin && $location.search().userId ? Number($location.search().userId) : null;

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
        // a regular user only ever sees their own tasks; an admin sees everyone's unless drilled into one user
        if (!taskList.isAdmin) {
            params.userId = taskList.currentUser.id;
        } else if (taskList.viewingUserId) {
            params.userId = taskList.viewingUserId;
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
            // admin isn't a task-owning role, so the "everyone's tasks" aggregate excludes the admin's own leftover tasks
            taskList.tasks = (taskList.isAdmin && !taskList.viewingUserId)
                ? response.data.filter(function (task) { return task.userId !== taskList.currentUser.id; })
                : response.data;
            taskList.errorMessage = '';
        }).catch(function (error) {
            taskList.tasks = [];
            taskList.errorMessage = 'Error fetching tasks: ' + error.status;
        });
    };

    taskList.applyFilters();

    // admin-only: build a userId -> username lookup so the Owner column reads names instead of raw ids
    if (taskList.isAdmin) {
        userService.search('').then(function (response) {
            response.data.forEach(function (user) {
                taskList.userMap[user.id] = user.username;
            });
        });
    }

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