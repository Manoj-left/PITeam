
taskManagerApp.factory('taskService', ['$http', 'API_BASE_URL', function($http, API_BASE_URL){
    var apiUrl= API_BASE_URL + '/tasks';
    return{
        getAll: function(){
            return $http.get(apiUrl);
        },

        getByFilters: function(filters){
            return $http.get(apiUrl, {params: filters});
        },

        getById: function(id){
            return $http.get(apiUrl + '/' + id);
        },

        create: function(task){
            return $http.post(apiUrl,task);

        },
        update: function(id, task){
            return $http.patch(apiUrl + '/' + id, task);
        },
        delete: function(id){
            return $http.delete(apiUrl + '/' + id);
        }
        
    }
    
}]);