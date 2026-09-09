taskManagerApp.controller('RegisterController', ['$location', 'authService', function ($location, authService) {
    var register = this;
    register.credentials = { username: '', password: '' };
    register.errorMessage = '';

    register.submit = function () {
        authService.register(register.credentials.username, register.credentials.password).then(function (response) {
            authService.setCurrentUser(response.data);
            $location.path('/tasks');
        }).catch(function (error) {
            register.errorMessage = error.status === 409 ? 'That username is already taken.' : 'Error registering: ' + error.status;
        });
    };
}]);
