(function () {
    "use strict";

    angular.module('myApp')
        .controller('UserController', ['$scope', '$location', '$routeParams', 'usersResource',
            function ($scope, $location, $routeParams, usersResource) {
                $scope.isNew = !$routeParams.userId;

                if ($scope.isNew) {
                    $scope.title = "Add new user";
                    $scope.user = usersResource.buildNew();
                } else {
                    $scope.title = "Update user";
                    $scope.user = usersResource.get({ userId: $routeParams.userId });
                }

                $scope.gotoUsers = function () {
                    $location.path('/users');
                }

                $scope.onSubmit = function () {
                    if ($scope.isNew) {
                        usersResource.post($scope.user, function () {
                            $scope.gotoUsers();
                        });
                    } else {
                        usersResource.put({ userId: $routeParams.userId }, $scope.user, function () {
                            $scope.gotoUsers();
                        });
                    }
                };

                $scope.onCancel = function () {
                    $scope.gotoUsers();
                };
            }
        ]);
})();