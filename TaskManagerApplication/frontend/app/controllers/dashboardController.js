taskManagerApp.controller('DashboardController', ['dashboardService', 'authService', 'taskHubService', function (dashboardService, authService, taskHubService) {
    var dashboard = this;
    dashboard.errorMessage = '';
    dashboard.stats = null;
    dashboard.statusBreakdown = [];
    dashboard.donutGradient = '';

    var currentUser = authService.getCurrentUser();
    dashboard.isAdmin = authService.isAdmin(currentUser);

    // display labels/CSS classes/colors/icons, indexed to match the backend TaskStatusType enum's int values
    var statusLabels = ['Not Started', 'In Progress', 'Completed', 'On Hold'];
    var statusClasses = ['kpi-card-not-started', 'kpi-card-in-progress', 'kpi-card-completed', 'kpi-card-on-hold'];
    var statusColors = ['var(--color-status-not-started)', 'var(--color-status-in-progress)', 'var(--color-status-completed)', 'var(--color-status-on-hold)'];
    var statusIconPaths = [
        'M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z',
        'M12 20c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm.5-13H11v6l5.25 3.15.75-1.23-4.5-2.67V7z',
        'M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z',
        'M6 19h4V5H6v14zm8-14v14h4V5h-4z'
    ];

    // builds a CSS conic-gradient string so the breakdown doubles as a donut chart - no charting library needed
    function buildDonutGradient(breakdown, total) {
        if (!total) {
            return 'conic-gradient(var(--color-border) 0% 100%)';
        }
        var cumulativePercent = 0;
        var stops = breakdown.map(function (status) {
            var start = cumulativePercent;
            cumulativePercent += (status.count / total) * 100;
            return status.color + ' ' + start + '% ' + cumulativePercent + '%';
        });
        return 'conic-gradient(' + stops.join(', ') + ')';
    }

    dashboard.loadStats = function () {
        dashboardService.getStats(currentUser.id, dashboard.isAdmin).then(function (response) {
            dashboard.stats = response.data;
            var total = dashboard.stats.taskStats.totalTasks;

            // StatusCounts dictionary keys are the enum's raw int serialized as a string ("0","1",...) - same
            // raw-int-over-the-wire convention used everywhere else in this app (no JsonStringEnumConverter)
            var counts = dashboard.stats.taskStats.statusCounts || {};
            dashboard.statusBreakdown = statusLabels.map(function (label, index) {
                var count = counts[index] || 0;
                return {
                    label: label,
                    cssClass: statusClasses[index],
                    color: statusColors[index],
                    iconPath: statusIconPaths[index],
                    count: count,
                    percentage: total ? Math.round((count / total) * 100) : 0
                };
            });

            dashboard.donutGradient = buildDonutGradient(dashboard.statusBreakdown, total);
            dashboard.errorMessage = '';
        }).catch(function (error) {
            dashboard.errorMessage = 'Error fetching dashboard stats: ' + error.status;
        });
    };

    dashboard.loadStats();

    // live updates - any task created/updated/deleted, or user registered/deleted, recalculates these KPIs in realtime
    taskHubService.on('TaskCreated', dashboard.loadStats);
    taskHubService.on('TaskUpdated', dashboard.loadStats);
    taskHubService.on('TaskDeleted', dashboard.loadStats);
    taskHubService.on('UserRegistered', dashboard.loadStats);
    taskHubService.on('UserDeleted', dashboard.loadStats);
}]);

