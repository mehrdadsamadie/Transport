MyApp.controller('accountCtrl', ['$scope', '$http', 'path', '$state', 'token', 'authService', 'localStorageService','PersonelData',
    function ($scope, $http, path, $state, token, authService, localStorageService, PersonelData) {
        $scope.path = path.pathName;
        $scope.LoginViewModel = {
            Email: '',
            Password: '',
            RememberMe: true
        };
        $scope.authentication = authService.authentication;
        $scope.promis = authService.promis;
        $scope.errors = [];
        $scope.Authenticat;
        if ($scope.authentication.isAuth == true) {
            //alert($scope.authentication.isAuth);
            $state.go('home');
        }
        $scope.showLogIn = false;
        $scope.$watch('promis.value', function (newval, oldval) {
            if (oldval == false && newval == true) {
                if ($scope.authentication.isAuth == true) {
                    //alert($scope.authentication.isAuth);
                    $state.go('home');
                } else {
                    $scope.showLogIn = true;
                }
            }
            else
            {
                if ($scope.authentication.isAuth != true) {
                    $scope.showLogIn = true;
                }
                else
                {
                    $state.go('home');
                }
            }
        }, true);
        function convertNumbers2English(string) {
            return string.replace(/[\u0660-\u0669\u06f0-\u06f9]/g, function (c) {
                return c.charCodeAt(0) & 0xf;
            });
        }
        $scope.sendFormLogIn = function () {
            $scope.LoginViewModel.Password = convertNumbers2English($scope.LoginViewModel.Password);
            $scope.LoginViewModel.Email = convertNumbers2English($scope.LoginViewModel.Email);
                    $http({
                        url: $scope.path + '/Account/LoginAngular',
                        method: 'Post',
                        data: {
                            model: $scope.LoginViewModel,
                            returnUrl: "",
                        }
                    }).success(function (data) {
                        if (data.success == true) {
                            $http.defaults.headers.common['Auth-Token'] = data.token;
                            $scope.Authenticat = { token: data.token, passWord: $scope.LoginViewModel.Password, userName: data.user, isAuth: data.success, role: data.role };
                            localStorageService.set('Authenticat', $scope.Authenticat);
                            authService.fillAuthData();
                            $state.go('requestList');
                        }
                        if (data.success == false) {
                            $scope.errors = data.errors;
                            //if (!data.ReturnUrl || 0 === data.ReturnUrl.length)
                            //    $location.url(data.ReturnUrl);
                        }
                    }).catch(function (data, status, headers, config) { // <--- catch instead error

                        //   alert(data.statusText) //contains the error message

                    });
                
        }

    }]);
