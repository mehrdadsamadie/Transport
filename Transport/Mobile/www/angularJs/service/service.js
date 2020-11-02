MyApp.factory('path', function () {
  // _path = "http://localhost:64778/";
   _path = "http://sana.entekhabgroup.ir/";
    return { pathName: _path };
});
MyApp.run(['DateService', 'position', 'gPSSaveLocalDb', 'authService', function (DateService, position, gPSSaveLocalDb, authService) {
    DateService.promise;
    authService.isAuthenticat();
    // gPSSaveLocalDb.setConfig();
    //    position.PositionServiceFactory.setPosition();
}]);
MyApp.factory('token', ['$http', '$q', 'path', function ($http, $q, path) {
    var _path = path.pathName;
    return {
        getToken: function () {
            return $http.post(_path + '/Account/GetToken')
                .then(function (response) {
                    if (typeof response.data) {
                        return response.data;
                    } else {
                        // invalid response
                        return $q.reject(response.data);
                    }
                }, function (response) {
                    // something went wrong
                    return $q.reject(response.data);
                });

        }
    };
}]);
MyApp.factory('authService', ['$http', 'localStorageService', 'token', 'path', function ($http, localStorageService, token, path) {
    var path = path.pathName;
    var authServiceFactory = {};
    var _authentication = {
        isAuth: false,
        userName: "",
        token: null,
        imageURL: null,
        role: null,
    };
    var _promis = { value: false };
    var _isAuthenticat = function () {
        $http({
            url: path + '/Account/IsAuthenticat',
            method: 'Post',
        }).success(function (data) {
            if (data.success == true) {
                $http.defaults.headers.common['Auth-Token'] = data.token;
                var authData = localStorageService.get('Authenticat');
                authData.token = data.token;
                localStorageService.set('Authenticat', authData);
                _fillAuthData();
            }
            if (data.success == false) {
                _fillAuthData();
                if (_authentication.userName != "") {
                    var loginViewModel = {
                        Email: _authentication.userName,
                        Password: _authentication.passWord,
                        RememberMe: true
                    };
                    $http({
                        url: path + '/Account/LoginAngular',
                        method: 'Post',
                        data: {
                            model: loginViewModel,
                            returnUrl: "",
                        }
                    }).success(function (data) {
                        if (data.success == true) {
                            $http.defaults.headers.common['Auth-Token'] = data.token;
                            var Authenticat = { token: data.token, passWord: loginViewModel.Password, userName: data.user, isAuth: true, role: data.role };
                            localStorageService.set('Authenticat', Authenticat);
                            _fillAuthData();
                        }
                        if (data.success == false) {
                            localStorageService.remove('Authenticat');
                            _fillAuthData();
                        }
                    }).catch(function (data, status, headers, config) { // <--- catch instead error
                        localStorageService.remove('Authenticat');
                        _fillAuthData();
                        //   alert(data.statusText) //contains the error message

                    });
                }
            }
        });

    }

    var _fillAuthData = function () {
        var authData = localStorageService.get('Authenticat');
        if (authData) {
            _authentication.isAuth = authData.isAuth;
            _authentication.userName = authData.userName;
            _authentication.passWord = authData.passWord;
            _authentication.token = authData.token;
            _authentication.imageURL = authData.imageURL;
            _authentication.role = authData.role;

        }
        else {
            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.passWord = "";
            _authentication.token = null;
            _authentication.imageURL = null;
            _authentication.role = null;

        }
        _promis.value = true;

    }
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.isAuthenticat = _isAuthenticat;
    authServiceFactory.promis = _promis;
    return authServiceFactory;
}]);
MyApp.factory('DateService', ['$http', 'path', function ($http, path) {
    var _path = path.pathName;
    var factoryResult = {
        getOfficeList: function () {
            var promise = $http({
                method: 'GET',
                url: _path + "/Home/GetDate"
            }).success(function (data, status, headers, config) {
                return data;
            });

            return promise;
        }
    };
    factoryResult.getOfficeList()
    return factoryResult;
}]);
MyApp.factory('requestCreateData', ['$http', 'path', function ($http, path) {
    var _path = path.pathName;
    var _data = {
    };

    var _promise = $http.post(_path + "/Request/GetCreateRequestData").success(function (data) {
        _data.value = data;
    });
    return {
        promise: _promise,
        DateFa: _data,
    };

}]);

MyApp.factory('PersonelData', ['$http', 'path', function ($http, path) {
    var _path = path.pathName;
    var factoryResult = {
        getOfficeList: function () {
            var promise = $http({
                method: 'Post',
                url: _path + "/Home/GetPersonelInfo"
            }).success(function (data, status, headers, config) {
                return data;
            });

            return promise;
        }
    };
    factoryResult.getOfficeList();
    return factoryResult;

}]);

MyApp.factory('gPSFactory', ['position', '$http', 'path', '$interval', 'gPSSaveLocalDb', function (position, $http, path, $interval, gPSSaveLocalDb) {
    var gPSServiceFactory = {};
    var stop;
    var send = false;
    function sendData() {
        if (send)
            return;
        var _currentPosition = {
            Latitude: position.PositionServiceFactory.currentPosition.Latitude,
            Longitude: position.PositionServiceFactory.currentPosition.Longitude,
            MomentSpeed: position.PositionServiceFactory.currentPosition.Speed,
        };
        send = true;
        $http({
            url: path.pathName + 'Worksheet/AddLocation',
            method: 'post',
            data: {
                model: _currentPosition,
            }
        }).success(function (data) {
            send = false;
            if (data.success == true) {
                if (gPSSaveLocalDb.count.Number > 0) {
                    gPSSaveLocalDb.sendData();
                }
            }
            if (data.success == false) {
                gPSSaveLocalDb.insert();
            }
        }).error(function (data, status) {
            send = false;
            gPSSaveLocalDb.insert();
        });
    }
    var _setStrat = function () {
        stop = $interval(function () {
            sendData();
        }, 5000);
    }
    var _setEnd = function () {
        $interval.cancel(stop);
    }

    gPSServiceFactory.setStrat = _setStrat;
    gPSServiceFactory.setEnd = _setEnd;
    return gPSServiceFactory;

}]);

MyApp.factory('position', function ($rootScope) {
    var PositionServiceFactory = {};
    var watchID;
    var _currentPosition = {
        Latitude: null,
        Longitude: null,
        Altitude: null,
        Accuracy: null,
        AltitudeAccuracy: null,
        Heading: null,
        Speed: null,
        Timestamp: null,
        Count: 0
    };
    var onSuccess = function (position) {
        $rootScope.$apply(function () {
            _currentPosition.Count++;
            _currentPosition.Latitude = position.coords.latitude;
            _currentPosition.Longitude = position.coords.longitude;
            _currentPosition.Altitude = position.coords.altitude;
            _currentPosition.Accuracy = position.coords.accuracy;
            _currentPosition.AltitudeAccuracy = position.coords.altitudeAccuracy;
            _currentPosition.Heading = position.coords.heading;
            _currentPosition.Speed = position.coords.speed;
            _currentPosition.Timestamp = position.timestamp;
        });
        navigator.geolocation.clearWatch(watchID);

    };

    function onError(error) {
        //alert('code: ' + error.code + '\n' + 'message: ' + error.message + '\n');
    }
    var _setPosition = function () {
        watchID = navigator.geolocation.watchPosition(onSuccess, onError, { maximumAge: 1000, timeout: 10000, enableHighAccuracy: true });
    }
    _setPosition();
    PositionServiceFactory.currentPosition = _currentPosition;
    PositionServiceFactory.setPosition = _setPosition;

    return { PositionServiceFactory: PositionServiceFactory, promise: onSuccess, };
});
MyApp.factory('gPSSaveLocalDb', ['position', '$http', 'path', '$interval', function (position, $http, path, $interval) {
    var gPSSaveLocalDbFactory = {};
    var db;
    var _count = { Number: 0 };
    var send = false
    var _setStrat = function () {
        return null;
    }
    ///////////////////////////
    var _setConfig = function () {
        db = window.openDatabase("Transport", "1.0", "Transport", 1000000);
        db.transaction(populateDB, errorCB, successCB);
    }
    function populateDB(tx) {
        tx.executeSql('CREATE TABLE IF NOT EXISTS Location (id, Latitude, Longitude, MomentSpeed, Date DATETIME)');
        tx.executeSql('SELECT * FROM Location', [], function (tx, results) {
            // this function is called when the executeSql is ended
            _count.Number = results.rows.length;
        });
    }
    var _insert = function () {
        db.transaction(insertDB, errorCB, successCB);
    }
    var _delete = function () {
        db.transaction(deleteDB, errorCB, successCB);
    }
    function insertDB(tx) {
        tx.executeSql('INSERT INTO Location (Latitude, Longitude,MomentSpeed, Date) VALUES ('
            + position.PositionServiceFactory.currentPosition.Latitude + ','
            + position.PositionServiceFactory.currentPosition.Longitude + ','
            + position.PositionServiceFactory.currentPosition.Speed + ','
            + Date.now()
            + ')');
        _count.Number = _count.Number++;
    }


    function deleteDB(tx) {
        tx.executeSql('DELETE FROM Location');
        _count.Number = 0
    }

    // Transaction error callback
    function errorCB(err) {
        // alert("Error processing SQL: " + err);
        console.log("Error processing SQL: " + err)
    }

    // Transaction success callback
    function successCB() {
        // alert("success!");
        console.log("success!");
    }


    var _sendData = function () {
        if (send || _count > 0)
            return;
        var _positions;
        var query = "SELECT * FROM Location"
        db.transaction(function (tx) {
            tx.executeSql(query, [], function (tx, _positions) {
                var positions = [];
                for (i = 0; i < _positions.rows.length; i++) {
                    positions.push(_positions.rows.item(i));
                }
                if (send == false) {
                    send = true;
                    $http({
                        url: path.pathName + '/Worksheet/AddListLocations',
                        method: 'post',
                        data: {
                            model: positions,
                        }
                    }).success(function (data) {
                        send = false;
                        if (data.success == true) {
                            _delete();
                        }
                        if (data.success == false) {
                        }
                    }).error(function (data, status) {
                        send = false;
                    });
                }
            }, function (a, b) {
                console.warn(a);
                console.warn(b);
            })
        })

    }


    ///////////////////////////
    gPSSaveLocalDbFactory.setConfig = _setConfig;
    gPSSaveLocalDbFactory.insert = _insert;
    gPSSaveLocalDbFactory.count = _count;
    gPSSaveLocalDbFactory.delete = _delete;
    gPSSaveLocalDbFactory.sendData = _sendData;
    return gPSSaveLocalDbFactory;

}]);
