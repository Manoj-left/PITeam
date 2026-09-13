taskManagerApp.controller('TaskListController', ['taskService', 'taskHubService', 'authService', 'userService', '$location', '$interval', '$scope', function (taskService, taskHubService, authService, userService, $location, $interval, $scope) {
    var taskList = this;
    taskList.tasks = [];
    taskList.errorMessage = '';
    taskList.now = Date.now();
    taskList.currentUser = authService.getCurrentUser();
    taskList.isAdmin = authService.isAdmin(taskList.currentUser);
    taskList.userMap = {}; // userId -> username, admin-only, used to show each task's owner
    // admins can drill into one user's tasks via a ?userId= query param (set from the Users page); omitted, they see everyone's
    taskList.viewingUserId = taskList.isAdmin && $location.search().userId ? Number($location.search().userId) : null;

    // display labels, indexed to match the backend enum's int values
    taskList.statusLabels = ['Not Started', 'In Progress', 'Completed', 'On Hold'];
    taskList.priorityLabels = ['Low', 'Medium', 'High'];

    // CSS modifier suffixes for badge/card coloring, indexed the same way as the labels above
    taskList.statusClasses = ['status-not-started', 'status-in-progress', 'status-completed', 'status-on-hold'];
    taskList.priorityClasses = ['priority-low', 'priority-medium', 'priority-high'];

    // values sent to the API and this array must match the backend enum member names exactly
    taskList.statusOptions = ['NotStarted', 'InProgress', 'Completed', 'OnHold'];
    taskList.priorityOptions = ['Low', 'Medium', 'High'];

    taskList.filters = {
        status: '',
        priority: '',
        search: '',
        overdue: ''
    };

    taskList.sort = {
        SortBy: ''
    };

    taskList.timeInStatus = function (statusChangedAt) {
        if (!statusChangedAt) {
            return 'Status time unavailable';
        }

        var hasTimezone = /(?:Z|[+-]\d{2}:\d{2})$/i.test(statusChangedAt);
        var timestamp = new Date(hasTimezone ? statusChangedAt : statusChangedAt + 'Z').getTime();
        var elapsedMinutes = Math.max(0, Math.floor((taskList.now - timestamp) / 60000));
        var days = Math.floor(elapsedMinutes / 1440);
        var hours = Math.floor((elapsedMinutes % 1440) / 60);
        var minutes = elapsedMinutes % 60;

        if (elapsedMinutes === 0) {
            return 'Status changed just now';
        }
        if (days > 0) {
            return 'In status for ' + days + 'd ' + hours + 'h';
        }
        if (hours > 0) {
            return 'In status for ' + hours + 'h ' + minutes + 'm';
        }
        return 'In status for ' + minutes + 'm';
    };

    var statusClock = $interval(function () {
        taskList.now = Date.now();
    }, 60000);

    $scope.$on('$destroy', function () {
        $interval.cancel(statusClock);
    });

    // dueAt is a plain "yyyy-MM-dd" DateOnly string with no time-of-day, so compare against local midnight today
    taskList.isOverdue = function (task) {
        if (!task.dueAt || task.status === 2) {
            return false;
        }
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        var dueDate = new Date(task.dueAt);
        return dueDate.getTime() < today.getTime();
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
        if (taskList.filters.search) {
            params.search = taskList.filters.search;
        }
        if (taskList.filters.overdue === 'overdue') {
            params.isOverdue = true;
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

    // live updates- any other tab creating/editing/deleting a task refreshes this list too
    taskHubService.on('TaskCreated', taskList.applyFilters);
    taskHubService.on('TaskUpdated', taskList.applyFilters);
    taskHubService.on('TaskDeleted', taskList.applyFilters);
}]);