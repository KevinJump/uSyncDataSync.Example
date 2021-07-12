(function () {
    'use strict';

    function exampleDataService($http) {

        var serviceRoot = '/umbraco/backoffice/ExampleData/ExampleDataApi';

        return {
            getAll: getAll

        };

        /////////////

        function getAll() {
            return $http.get(serviceRoot + '/GetAll');
        }
    }

    angular.module('umbraco.services')
        .factory('exampleDataService', exampleDataService);
})();