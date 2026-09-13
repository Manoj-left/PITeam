taskManagerApp.controller('LoginController', ['$location', 'authService', function ($location, authService) {
    var login = this;
    login.credentials = { username: '', password: '' };
    login.errorMessage = '';

    login.submit = function () {
        authService.login(login.credentials.username, login.credentials.password).then(function (response) {
            authService.setCurrentUser(response.data);
            $location.path('/dashboard');
        }).catch(function (error) {
            login.errorMessage = error.status === 401 ? 'Invalid username or password.' : 'Error logging in: ' + error.status;
        });
    };
}]);
