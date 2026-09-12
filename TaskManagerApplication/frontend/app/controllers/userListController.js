taskManagerApp.controller('UserListController', ['userService', 'taskHubService', function (userService, taskHubService) {
    var userList = this;
    userList.search = '';
    userList.users = [];
    userList.errorMessage = '';
    userList.roleLabels = ['User', 'Admin'];

    userList.runSearch = function () {
        userService.search(userList.search).then(function (response) {
            userList.users = response.data;
            userList.errorMessage = '';
        }).catch(function (error) {
            userList.users = [];
            userList.errorMessage = 'Error fetching users: ' + error.status;
        });
    };

    userList.runSearch();

    userList.deleteUser = function (id) {
        userService.delete(id).then(function () {
            userList.runSearch();
        }).catch(function (error) {
            userList.errorMessage = 'Error deleting user: ' + error.status;
        });
    };

    // keep each user's task count live as tasks are created/edited/deleted elsewhere
    taskHubService.on('TaskCreated', userList.runSearch);
    taskHubService.on('TaskUpdated', userList.runSearch);
    taskHubService.on('TaskDeleted', userList.runSearch);
}]);
