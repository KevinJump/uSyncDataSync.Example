(function () {
    'use strict';

    function editorController($scope, exampleDataService) {
        var vm = this;


        exampleDataService.getAll()
            .then(function (result) {
                vm.data = result.data;
            });
    }

    angular.module('umbraco')
        .controller('exampleDataEditController', editorController);
})();