taskManagerApp.controller('UserListController', ['userService', function (userService) {
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
}]);
