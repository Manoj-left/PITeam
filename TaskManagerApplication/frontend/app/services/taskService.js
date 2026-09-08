
taskManagerApp.factory('taskService', ['$http', 'API_BASE_URL', function($http, API_BASE_URL){
    var apiUrl= API_BASE_URL + '/tasks';
    return{
        getAll: function(){
            return $http.get(apiUrl);
        }
    }
    
}]);