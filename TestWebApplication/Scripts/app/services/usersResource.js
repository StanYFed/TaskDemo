(function () {
    "use strict";

    angular.module('myApp')
        .factory('usersResource', ['$resource', 'webServiceRoot', function ($resource, webServiceRoot) {
            if (!webServiceRoot) {
                throw "Invalid initialization. Set 'webServiceRoot' constant.";
            }

            var UserResource = $resource(webServiceRoot + 'users/:userId', null,
                {
                    'post': { method: 'POST' },
                    'put': { method: 'PUT' }
                }
            );

            UserResource.buildNew = function () {
                return new UserResource({
                    FirstName: null,
                    LastName: null,
                    Email: null
                });
            }

            return UserResource;
        }]);
})();