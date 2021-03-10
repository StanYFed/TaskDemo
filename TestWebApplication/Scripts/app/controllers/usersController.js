(function () {
    "use strict";

    angular.module('myApp')
        .controller('UsersController', ['$scope', '$interpolate', 'usersResource',
            function ($scope, $interpolate, usersResource) {
                $scope.users = usersResource.query();

                $scope.onDelete = function (user) {
                    var message = $interpolate('Do you want to delete user - {{FirstName}} {{LastName}}?')(user);
                    if (confirm(message)) {
                        usersResource.delete(user, function () {
                            $scope.users = usersResource.query();
                        });
                    }
                };
            }
        ]);
})();