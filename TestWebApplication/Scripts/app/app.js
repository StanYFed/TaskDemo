(function () {
    "use strict";

    var myAppModule = angular.module('myApp', ['ngRoute', 'ngResource']);

    myAppModule.config(['$provide', function ($provide) {
        $provide.decorator('$exceptionHandler', ['$delegate', function ($delegate) {
            return function (exception, cause) {
                $delegate(exception, cause);
                alert(exception.message || 'Something wrong!');
            };
        }]);
    }]);

    myAppModule.config([
        '$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
            $routeProvider
                .when('/users', { templateUrl: '/Users/List', controller: 'UsersController' })
                .when('/users/new', { templateUrl: '/Users/Editor', controller: 'UserController' })
                .when('/users/:userId/edit', { templateUrl: '/Users/Editor', controller: 'UserController' })
                .otherwise({ redirectTo: '/users' });
            $locationProvider.hashPrefix('');
        }
    ]);
}());