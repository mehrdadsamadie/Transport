MyApp.controller('requestListCtrl', ['$scope', '$http', 'path', '$state', '$stateParams', '$filter', 'authService', 'gPSFactory', 'localStorageService', 'dateNow',
    function ($scope, $http, path, $state, $stateParams, $filter, authService, gPSFactory, localStorageService, dateNow) {
        $scope.years = [];

        $scope.errors = [];
        $scope.Months = [{
            name: "فروردین",
            number: 1
        }, {
            name: "اردیبهشت",
            number: 2
        }, {
            name: "خرداد",
            number: 3
        }, {
            name: "تیر",
            number: 4
        }, {
            name: "مرداد",
            number: 5
        }, {
            name: "شهریور",
            number: 6
        }, {
            name: "مهر",
            number: 7
        }, {
            name: "ابان",
            number: 8
        }, {
            name: "اذر",
            number: 9
        }, {
            name: "دی",
            number: 10
        }, {
            name: "بهمن",
            number: 11
        }, {
            name: "اسفند",
            number: 12
        }];

        $scope.setDays = function () {
            var days = [];
            if ($scope.selectedMonthStart == null) {
                for (var i = 1; i <= 31; i++) {
                    days.push(i);
                }
            }
            else {
                if ($scope.selectedMonthStart.number == 12 && $scope.selectedYearStart % 4 == 0) {
                    for (var i = 1; i <= 29; i++) {
                        days.push(i);
                    }
                }
                else if ($scope.selectedMonthStart.number >= 7) {
                    for (var i = 1; i <= 30; i++) {
                        days.push(i);
                    }
                }
                else
                    for (var i = 1; i <= 31; i++) {
                        days.push(i);
                    }
            }
            $scope.Days = days;
        };
        $scope.setYears = function () {
            var today = new Date();
            var year = today.getFullYear() - 621;
            for (var i = year - 100; i <= year - 5; i++) {
                $scope.years.push(i);
            }
            $scope.selectedYearStart = $scope.years[$scope.years.length - 1];
        };

        $scope.setYears();

        $scope.Days = [];
        $scope.$watch('selectedMonthStart.number', function (newval, oldval) {

            for (var i = 0; i < $scope.Months.length; i++) {
                if ($scope.Months[i].number == newval) {
                    $scope.selectedMonthStart.name = $scope.Months[i].name;
                }
            }
            $scope.setDays();
        }, true);
        $scope.setDays = function () {
            var days = [];
            if ($scope.selectedMonthEnd == null) {
                for (var i = 1; i <= 31; i++) {
                    days.push(i);
                }
            }
            else {
                if ($scope.selectedMonthEnd.number == 12 && $scope.selectedYearEnd % 4 == 0) {
                    for (var i = 1; i <= 29; i++) {
                        days.push(i);
                    }
                }
                else if ($scope.selectedMonthEnd.number >= 7) {
                    for (var i = 1; i <= 30; i++) {
                        days.push(i);
                    }
                }
                else
                    for (var i = 1; i <= 31; i++) {
                        days.push(i);
                    }
            }
            $scope.Days = days;
        };
        $scope.setYears = function () {
            var today = new Date();
            var year = today.getFullYear() - 621;
            for (var i = year - 100; i <= year - 5; i++) {
                $scope.years.push(i);
            }
            $scope.selectedYearEnd = $scope.years[$scope.years.length - 1];
        };

        $scope.setYears();

        $scope.Days = [];
        $scope.$watch('selectedMonthEnd.number', function (newval, oldval) {

            for (var i = 0; i < $scope.Months.length; i++) {
                if ($scope.Months[i].number == newval) {
                    $scope.selectedMonthEnd.name = $scope.Months[i].name;
                }
            }
            $scope.setDays();
        }, true);



        $scope.path = path.pathName;
        $scope.data = {
            showDelete: false,
            listCanSwipe: true,
            shouldShowReorder: true,
            shouldShowDelete: true,
        };
        $scope.user = authService.authentication;
        $scope.roles = $scope.user.role == null ? null : $scope.user.role.split(",");
        $scope.status = [
            { lable: 0, value: "بررسی نشده" },
            { lable: 1, value: "تایید نشده" },
            { lable: 2, value: "تایید شده" }
        ]
        $scope.changeStatus = function (ismanager, status) {
            if (ismanager) {
                if ($scope.filter.managerStatus != null && $scope.filter.managerStatus.length > 0) {
                    for (var i = 0; i < $scope.filter.managerStatus.length; i++) {
                        if ($scope.filter.managerStatus[i] == status) {
                            $scope.filter.managerStatus.splice(i, 1);
                            $scope.restFilter();
                            return;
                        }
                    }
                }
                $scope.filter.managerStatus.push(parseInt(status));
                $scope.restFilter();
            }
            else {
                if ($scope.filter.transportStatus != null && $scope.filter.transportStatus.length > 0) {
                    for (var i = 0; i < $scope.filter.transportStatus.length; i++) {
                        if ($scope.filter.transportStatus[i] == status) {
                            $scope.filter.transportStatus.splice(i, 1);
                            $scope.restFilter();
                            return;
                        }
                    }

                }
                $scope.filter.transportStatus.push(parseInt(status));
                $scope.restFilter();
            }
        };
        $scope.checkedstatus = function (ismanager, status) {
            if (ismanager) {
                if ($scope.filter.managerStatus != null && $scope.filter.managerStatus.length > 0) {
                    if ($scope.filter.managerStatus.indexOf(parseInt(status)) < 0) {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            else {
                if ($scope.filter.transportStatus != null && $scope.filter.transportStatus.length > 0) {
                    if ($scope.filter.transportStatus.indexOf(parseInt(status)) < 0) {
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }
        $scope.filter = {
            PageNumber: 0,
            PageSize: 5,
            DateNow: dateNow.data,
            endDate: null,
            managerStatus: [0, 2],
            transportStatus: [],
        };
        $scope.selectedDayStart = { day: 1 };
        $scope.selectedDayEnd = { day: 1 };
        function MySpilit() {
            tmp = dateNow.data.split('/');
            $scope.selectedYearStart = parseInt(tmp[0]);
            $scope.selectedDayStart.day = parseInt(tmp[2]);
            $scope.selectedMonthStart = {
                number: parseInt(tmp[1])
            };

            $scope.selectedYearEnd = parseInt(tmp[0]);
            $scope.selectedDayEnd.day = parseInt(tmp[2]);
            $scope.selectedMonthEnd = {
                number: parseInt(tmp[1])
            };
        }

        MySpilit();
        $scope.requests = [];
        $scope.load = false;
        $scope.loadprocess = false;
        $scope.restFilter = function () {
            if ($scope.requests[0] != undefined) {
                $scope.requests.length = 0;
            }
            resetPaging();
            $scope.getRequests();
        }
        var resetPaging = function () {
            $scope.filter.PageNumber = 0;
        };
        $scope.getRequests = function () {
            $scope.load = true;
            $http({
                url: $scope.path + '/Request/List',
                method: 'Post',
                data: {
                    startDate: $scope.selectedYearStart + '/' + $scope.selectedMonthStart.number + '/' + $scope.selectedDayStart.day,
                    endDate: $scope.selectedYearEnd + '/' + $scope.selectedMonthEnd.number + '/' + $scope.selectedDayEnd.day,
                    managerStatus: $scope.filter.managerStatus.length > 0 ? $scope.filter.managerStatus : null,
                    transportStatus: $scope.filter.transportStatus.length > 0 ? $scope.filter.transportStatus : null,
                    pageNumber: $scope.filter.PageNumber,
                    pageSize: $scope.filter.PageSize,
                    RequestPersonel: null,
                    isDelete: false,
                    isCancel: false,
                }
            }).success(function (data) {
                if (data != "") {
                    $scope.pageCount(data.TotalCount);
                    if (data.TotalCount > 0) {
                        if ($scope.requests[0] == undefined) {
                            $scope.requests = data.Request;
                        }
                        else {
                            for (i = 0; data.Request.length > i; i++) {
                                $scope.requests.push(data.Request[i]);

                            }
                        }
                    }
                }
                else {
                    $scope.requests.length = 0;
                    resetPaging();
                }
                $scope.$broadcast('scroll.infiniteScrollComplete');
                $scope.load = false;
            }).error(function (data, status, headers, config) {
                $scope.load = false;
            });
        };
        $scope.deleteRequest = function (data) {
            var r = confirm("ایا مطمئن به حذف این آیتم از لیست می باشید؟");
            if (r == true) {
                $scope.loadprocess = true;
                $http({
                    url: $scope.path + '/Request/Delete',
                    method: 'Post',
                    data: {
                        id: data,
                        isMobile: true
                    }
                }).success(function (data) {
                    $scope.loadprocess = false;
                    if (data == true) {

                        alert("حذف با موفقیت صورت گرفت");
                        $scope.restFilter();
                    }
                    else {
                        alert("خطا در پاک کردن  !")
                    }
                }).error(function (data, status, headers, config) {
                    $scope.loadprocess = false;
                });
            };
        }
        $scope.confirm = function (requestId, status) {
            var arr = [requestId];
            $scope.loadprocess = true;
            $http({
                url: $scope.path + '/Request/Confirm',
                method: 'Post',
                data: {
                    requests: arr,
                    isAccept: status
                }
            }).success(function (data) {
                $scope.loadprocess = false;
                if (data.result == true) {
                    if (status) {
                        alert("تایید با موفقیت صورت گرفت");
                    }
                    else {
                        alert(" عدم تایید با موفقیت صورت گرفت");
                    }
                    $scope.restFilter();
                }
                else {
                    alert(data.Message);
                }
            }).error(function (data, status, headers, config) {
                $scope.loadprocess = false;
            });
        }
        $scope.cancel = function (requestId) {
            var r = confirm("ایا مطمئن به لغو این درخواست می باشید؟");
            if (r == true) {
                $scope.loadprocess = true;
                $http({
                    url: $scope.path + '/Request/Cancel',
                    method: 'Post',
                    data: {
                        id: requestId,
                    }
                }).success(function (data) {
                    $scope.loadprocess = false;
                    if (data.success == true) {
                        alert("درخواست لغو شد");
                        $scope.restFilter();
                    }
                    else {

                        alert(data.message);
                    }
                }).error(function (data, status, headers, config) {
                    $scope.loadprocess = false;
                });
            }
        }


        $scope.pageCount = function (data) {
            $scope.PageCount = Math.ceil(data / $scope.filter.PageSize);
            console.log($scope.PageCount);
        }
        $scope.loadMore = function () {
            if ($scope.filter.PageNumber < $scope.PageCount - 1) {
                var _curentpage = Math.ceil($scope.requests.length / $scope.filter.PageSize);
                $scope.filter.PageNumber = _curentpage;
                console.log(_curentpage);
                $scope.getRequests();

                $scope.loadprocess = true;
            }
            $scope.loadprocess = false;
        }
    }]);