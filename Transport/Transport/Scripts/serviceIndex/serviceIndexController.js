var app = angular.module("transport", [
    'ui.grid',
    'ui.grid.selection',
    'ui.grid.resizeColumns',
    'ui.grid.autoResize',
    'ui.grid.pagination',
    'angucomplete-alt',
    'ui.grid.selection',
    'ui.grid.exporter'
]);
app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode({ enabled: true, requireBase: false });
}])
app.controller("serviceIndexCtrl", function ($scope, $http, uiGridConstants, $window, $location) {
    $scope.Services = [];
    $scope.nowdate;
    $scope.selectedServiceStatu = null;
    $scope.ServiceStatues = [];
    $scope.pagingOptions = {
        pageSizes: [50, 75, 100],
        pageSize: 50,
        currentPage: 0
    };
    $scope.role =
        {
            Admin: false,
            FactorManager: false,
            Transport: false
        };
    $scope.setRole = function (admin, factorManager, transport) {
        $scope.role.Admin = admin == "true" ? true : false;
        $scope.role.FactorManager = factorManager == "true" ? true : false;
        $scope.role.Transport = transport == "true" ? true : false;
        if ($scope.role.Admin == true || $scope.role.Transport == true) {
            $scope.gridOptions.columnDefs[13].visible = true;
        }
        else {
            $scope.gridOptions.columnDefs[13].visible = false;
        };
    }
    $scope.setTime = function (time) {
        $scope.nowdate = time;
    };

    $scope.filter = {
        order: "ServiceDate",
        isAsc: true,
        startDate: null,
        endDate: null,
        startTime: null,
        endTime: null,
        isAcceptTranasportManager: null,
        serviceStatusId: null,
        isDelete: null,
        driverId: null
    };
    $scope.getAllServiceStatu = function () {
        $scope.load = true;
        $http({
            url: '/Manage/Service/GetAllServiceStatu',
            method: 'Post',

        }).success(function (data) {
            $scope.ServiceStatues = data;
        }).error(function (data, status, headers, config) {
        });
    }
    $scope.getAllServiceStatu();

    function getURL() {

        $scope.filter.startDate = $location.search().startDate == undefined ? null : $location.search().startDate;
        $scope.filter.endDate = $location.search().endDate == undefined ? null : $location.search().endDate;
        $scope.filter.startTime = $location.search().startTime == undefined ? null : $location.search().startTime;
        $scope.filter.endTime = $location.search().endTime == undefined ? null : $location.search().endTime;
    }
    getURL();

    $scope.changeURL = function () {
        var url =
            '?startDate=' + ($scope.filter.startDate == (undefined || null) ? '' : $scope.filter.startDate)
            + '&endDate=' + ($scope.filter.endDate == (undefined || null) ? '' : $scope.filter.endDate)
            + '&startTime=' + ($scope.filter.startTime == (undefined || null) ? '' : $scope.filter.startTime)
            + '&endTime=' + ($scope.filter.endTime == (undefined || null) ? '' : $scope.filter.endTime);
        urlsend = url;
        $location.url(url);
        $location.replace();
        $window.history.pushState(null, 'any', $location.absUrl());
    }
    $scope.getServices = function () {
        $scope.load = true;
        $http({
            url: '/Manage/Service/List',
            method: 'Post',
            data: {
                startDate: $scope.filter.startDate,
                endDate: $scope.filter.endDate,
                startTime: $scope.filter.startTime,
                endTime: $scope.filter.endTime,
                serviceStatusId: $scope.filter.serviceStatusId,
                isDelete: $scope.filter.isDelete,
                isAcceptTranasportManager: $scope.filter.isAcceptTranasportManager,
                pageNumber: $scope.pagingOptions.currentPage,
                pageSize: $scope.pagingOptions.pageSize,
                driverId: $scope.filter.driverId,
                order: $scope.filter.order,
                isAsc: $scope.filter.isAsc
            }
        }).success(function (data) {
            if (data != "") {
                $scope.Services = data.Services;
                $scope.gridOptions.totalItems = data.Total;
            }
            else {
                $scope.Services.length = 0;
                $scope.gridOptions.totalItems = 0;
            }
            $scope.load = false;
        }).error(function (data, status, headers, config) {
            $scope.load = false;
        });
    };
    $scope.exportToExcel = function () {
        var url = "/Manage/Service/ExportData?startDate=" + ($scope.filter.startDate == null ? "" : $scope.filter.startDate) + "&endDate=" + ($scope.filter.endDate == null ? "" : $scope.filter.endDate) + "&startTime=" + ($scope.filter.startTime == null ? "" : $scope.filter.startTime) + "&endTime=" + ($scope.filter.endTime == null ? "" : $scope.filter.endTime) +  "&serviceStatusId=" + $scope.filter.serviceStatusId + "&isAcceptTranasportManager=" + $scope.filter.isAcceptTranasportManager + "&isDelete=" + $scope.filter.isDelete + "&driverId=" + $scope.filter.driverId + "&order=" + $scope.filter.order + "&isAsc=" + $scope.filter.isAsc;
        $window.open(url);
    }



    $scope.gridOptions =
        {
            paginationPageSizes: $scope.pagingOptions.pageSizes,
            paginationPageSize: $scope.pagingOptions.pageSize,
            useExternalPagination: true,
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,

            data: 'Services',
            columnDefs: [{
                field: 'Id',
                displayName: 'Id',
                visible: false,

            }, {
                field: 'FullName',
                displayName: 'نام راننده',
                width: 165,
                enableFiltering: false,
                cellClass: 'center',
                cellTemplate: '<a class="ngCellText btn-link" ng-cell-text ng-class="col.colIndex()" ng-click="grid.appScope.editService(row.entity.Id)"  >{{row.entity.FullName}}</a>',

            }, {
                name: 'Date',
                displayName: 'تاریخ سرویس',
                width: 75,
                enableFiltering: false,
                cellClass: 'center',

            }, {
                field: 'DriverType',
                displayName: 'نوع راننده',
                width: 60,
                enableFiltering: false,
                enableSorting: false,

            }, {
                field: 'StartTime',
                displayName: 'زمان شروع',
                width: 75,
                visible: true,
                enableFiltering: false,
                enableSorting: false,
                cellClass: 'center',

            }, {
                field: 'EndTime',
                displayName: 'زمان پایان',
                width: 75,
                visible: true,
                enableFiltering: false,
                enableSorting: false,
                cellClass: 'center',

            }, {
                field: 'Beginning',
                displayName: 'مبدا',
                enableFiltering: false,
                enableSorting: false,
                width: 205,
            }, {
                field: 'Destination',
                displayName: 'مقصد',
                enableFiltering: false,
                enableSorting: false,
                width: 205,
            }, {
                field: 'Name',
                displayName: 'مسیر',
                enableFiltering: false,
                enableSorting: false,
                width: 65,
            }, {
                field: 'IsLocal',
                displayName: 'درون شهرک',
                width: 50,
                cellClass: 'center',
                cellTemplate: '<div class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" class="fa fa-industry text-success"></i><i ng-show="!COL_FIELD" class="fa fa-globe text-warning"></i></div>',
                enableFiltering: false,
                enableSorting: false,
            }, {
                field: 'ServiceStatus',
                displayName: 'وضعیت سرویس',
                enableFiltering: false,
                enableSorting: false,
                width: 65,
                cellClass: 'center',
                enableFiltering: true,
                cellTemplate: '<div ng-if="row.entity.ServiceStatus ==1"><i class="fa fa-stop-circle-o text-danger"></i></div><div ng-if="row.entity.ServiceStatus ==2"><i class="fa fa-car text-info"></i></div><div ng-if="row.entity.ServiceStatus ==3"><i class="fa fa-flag-checkered text-success"></i></div>',
                filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div my-custom-modal></div></div>',
            }, {
                field: 'Requests',
                cellClass: 'center',
                width: 60,
                cellClass: 'center',
                displayName: 'درخواست',
                enableFiltering: false,
                enableSorting: false,
                cellTemplate: '<div class="btn-link " ng-if="row.entity.Requests[0]!=undefined" data-toggle="modal" data-target="#exampleModal" id="openPopUp" ng-click="grid.appScope.setSelectedSevice(row.entity.Requests)">درخواست</div>',
            }, {
                field: 'IsAcceptTranasportManager',
                displayName: 'تایید واحد نقلیه',
                width: 60,
                cellClass: 'center',
                cellTemplate: '<div ng-click="grid.appScope.confirmservice(row.entity.Id)"><div ng-if="row.entity.IsAcceptTranasportManager==false"><i class="fa fa-times text-danger"></i></div><div ng-if="row.entity.IsAcceptTranasportManager ==null"><i class="fa fa-minus"></i></div><div ng-if="row.entity.IsAcceptTranasportManager ==true"><i class="fa fa-check text-success"></i></div></div>',
                enableFiltering: true,
                enableSorting: false,
                filter: {
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ value: true, label: 'تایید شده ها' }, { value: false, label: 'تایید نشده ها' }, { value: null, label: 'بررسی نشده ها' }]
                },

            }, {
                field: 'IsDelete',
                displayName: 'حذف',
                width: 50,
                cellClass: 'center',
                enableFiltering: true,
                cellTemplate: '<div ng-if="row.entity.IsLock==true" class="ngCellText" ng-cell-text ng-class="col.colIndex()" ng-click="grid.appScope.deleteServices(row.entity.Id,row.entity.IsLock)"><i ng-show="COL_FIELD" class="fa fa-check text-muted"></i><i ng-show="!COL_FIELD" class="fa fa-times text-muted"></i></div><a ng-if="row.entity.IsLock!=true" ng-click="grid.appScope.deleteServices(row.entity.Id,row.entity.IsLock)" class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" class="fa fa-check text-success"></i><i ng-show="!COL_FIELD" class="fa fa-times text-danger"></i></a>',
                filter: {
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ value: true, label: 'حذف شده ها' }, { value: false, label: 'حذف نشده ها' }]
                },
            }, {
                field: 'IsLock',
                displayName: 'قفل ',
                enableFiltering: false,
                enableSorting: false,
                width: 50,
                cellClass: 'center',
                cellTemplate: '<div class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" class="fa fa-check text-success"></i><i ng-show="!COL_FIELD" class="fa fa-times text-danger"></i></div>',
            }
            ],
            enableSelectAll: true,
            jqueryUITheme: true,
            showFooter: true,
            width: 1080,
            rowHeight: 30,
            enableColumnMenus: false,
            enableGridMenu: false,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, $scope.sortChanged);
                $scope.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    $scope.pagingOptions.currentPage = newPage - 1;
                    $scope.pagingOptions.pageSize = pageSize;
                    $scope.services();
                });

                $scope.gridApi.core.on.filterChanged($scope, function () {

                    var grid = this.grid;
                    if (grid.columns[10].filters[0].term != undefined && grid.columns[10].filters[0].term != "") {
                        $scope.filter.serviceStatusId = grid.columns[10].filters[0].term.split(',').map(Number);
                    } else if (grid.columns[10].filters[0].term === undefined || grid.columns[10].filters[0].term == "") {
                        $scope.filter.serviceStatusId = null;
                    };
                    if (grid.columns[13].filters[0].term !== undefined) {
                        $scope.filter.isAcceptTranasportManager = grid.columns[13].filters[0].term;
                        // alert(grid.columns[12].filters[0].term);
                    }
                    if (grid.columns[14].filters[0].term !== undefined) {
                        $scope.filter.isDelete = grid.columns[14].filters[0].term;
                    }
                    $scope.reSet();
                });
            }
        };


    $scope.changeServiceStatus = function () {
        var arr = [];
        if ($scope.selectedServiceStatu == null) {
            alert("نوع وضعیت سفر را انتخاب کنید");
            return;
        }
        var temp = $scope.gridApi.selection.getSelectedRows();
        for (var i = 0; i < temp.length; i++) {
            arr.push(parseInt(temp[i].Id));
        }
        if (arr.length < 1) {
            alert("رکوردی انتخاب نشده است");
            return;
        }
        $http({
            url: '/Manage/Service/ChangeServiceStatus',
            method: 'Post',
            data: {
                serviceStatusId: parseInt($scope.selectedServiceStatu),
                services: arr,
            }
        }).success(function (data) {
            if (data.result == true) {
                alert("تغییر وضعیت انجام پذیرفت");
                $scope.getServices();
            }
            else {
                alert(data.Message);
            }
        });

    }
    $scope.confirmservice = function (serviceId, status) {
        var arr = [];
        if (serviceId == null) {
            var temp = $scope.gridApi.selection.getSelectedRows();
            for (var i = 0; i < temp.length; i++) {
                arr.push(temp[i].Id);
            }
        }
        else {
            arr = [serviceId];
        }
        if (arr.length == 0) {
            alert("ایتمی انتخاب نشده است");
        }

        $http({
            url: '/Manage/Service/Confirm',
            method: 'Post',
            data: {
                isAccept: status,
                services: arr
            }
        }).success(function (data) {
            if (data.result == true) {
                if (status) {
                    alert("تایید با موفقیت صورت گرفت");
                }
                else {
                    alert(" عدم تایید با موفقیت صورت گرفت");
                }
                $scope.getServices();
            }
            else {
                alert(data.Message);
            }
        });
    }
    $scope.deleteServices = function (data, islock) {
        if (islock == true) {
            alert("این سرویس قفل شده است");
            return;
        }

        var r = confirm("ایا مطمئن به حذف این آیتم از لیست می باشید؟");
        if (r == true) {
            $http({
                url: '/Manage/Service/Delete',
                method: 'Post',
                data: {
                    id: data,
                }
            }).success(function (data) {
                if (data == true) {
                    alert("حذف با موفقیت صورت گرفت");
                    $scope.getServices();
                }
                else {
                    alert("خطا در پاک کردن  !")
                }
            });
        };


    };
    $scope.editService = function (id) {
        $window.location = "/Manage/Service/Create/" + id;
    }
    $scope.setSort = function (sortColumns, direction) {
        switch (sortColumns) {
            case "ServiceDate":
                {
                    $scope.filter.order = "ServiceDate";
                    $scope.filter.isAsc = direction
                }
                break;
            case "DriverId":
                {
                    $scope.filter.order = "DriverId";
                    $scope.filter.isAsc = direction
                }
                break;
            default:
                {
                    $scope.filter.order = "ServiceDate";
                    $scope.filter.isAsc = direction
                };


        }
        $scope.getServices();
    }

    $scope.reSet = function () {
        $scope.filter.order = 1;
        $scope.filter.isAsc = true;
        $scope.pagingOptions.currentPage = 0;
        $scope.Services.length = 0;
        $scope.TotalCount = 0;
        $scope.gridOptions.totalItems = 0;
        $scope.getServices();
        $scope.changeURL();
    }
    $scope.sortChanged = function (grid, sortColumns) {
        if (sortColumns.length === 0) {
            return;
        } else {
            switch (sortColumns[0].sort.direction) {
                case uiGridConstants.ASC:
                    $scope.setSort(sortColumns[0].name, true);
                    break;
                case uiGridConstants.DESC:
                    $scope.setSort(sortColumns[0].name, false);
                    break;
                case undefined:
                    $scope.setSort(sortColumns[0].name, true);
                    break;
            }
        }
    };
    $scope.setSelectedSevice = function (data) {
        $scope.selectedService = data;
    }

    $scope.$watch('filter.startDate', function (newVal, oldVal) {
        if (newVal === oldVal && oldVal === null) {
            return;
        }
        if (newVal !== undefined) {
            if (newVal == "") {
                $scope.filter.startDate = null;
            }
            $scope.reSet();

        }
    });
    $scope.$watch('filter.endDate', function (newVal, oldVal) {
        if (newVal == oldVal && oldVal === null) {
            return;
        }
        if (newVal !== undefined) {
            if (newVal == "") {
                $scope.filter.endDate = null;
            }
            $scope.reSet();
        }
    });

    $scope.selectedDriver = function (selected) {
        if (selected != undefined) {
            if (selected.originalObject.Id != null) {
                $scope.filter.driverId = selected.originalObject.Id;
            }
            else {
                $scope.filter.driverId = null;
            }
        }
        else {
            $scope.filter.driverId = null;
        }
        $scope.reSet();
    };
    $scope.$watch('nowdate', function (newVal, oldVal) {
        $scope.filter.startDate = $scope.filter.startDate == null ? newVal : $scope.filter.startDate;
        $scope.filter.endDate = $scope.filter.endDate == null ? newVal : $scope.filter.endDate;

    });
});

app.controller('myCustomModalCtrl', function ($scope, $compile, $timeout) {
    var $elm;

    $scope.showAgeModal = function () {
        // $scope.listOfAges = [0, 1, 2];
        $scope.listOfAges = [{
            value: "1",
            label: "شروع نشده"
        },
        {
            value: "2",
            label: "در حال انجام"
        },
        {
            value: "3",
            label: "پایان یافته"
        },
        ];


        $scope.listOfAges.sort();

        $scope.gridOptions = {
            data: [],
            enableColumnMenus: false,
            columnDefs: [{
                field: 'age',
                displayName: 'وضعیت',
            }],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                if ($scope.colFilter && $scope.colFilter.listTerm) {
                    $timeout(function () {
                        $scope.colFilter.listTerm.forEach(function (age) {
                            var entities = $scope.gridOptions.data.filter(function (row) {
                                var status;
                                switch (age) {
                                    case "1":
                                        {
                                            status = "شروع نشده";
                                        }
                                        break;
                                    case "2":
                                        {
                                            status = "در حال انجام";
                                        }
                                        break;
                                    case "3":
                                        {
                                            status = "پایان یافته";
                                        }
                                        break;
                                }
                                return row.age === status;
                            });

                            if (entities.length > 0) {
                                $scope.gridApi.selection.selectRow(entities[0]);
                            }
                        });
                    });
                }
            }

        };

        $scope.listOfAges.forEach(function (age) {
            $scope.gridOptions.data.push({ age: age.label });
        });

        var html = '<div class="modal" ng-style="{display: \'block\'}"><div class="modal-dialog"><div class="modal-content"><div class="modal-header">وضعیت تایید</div><div class="modal-body"><div id="grid1" ui-grid="gridOptions" ui-grid-selection class="modalGrid"></div></div><div class="modal-footer"><button id="buttonClose" class="btn btn-primary" ng-click="close()">فیلتر</button></div></div></div></div>';
        $elm = angular.element(html);
        angular.element(document.body).prepend($elm);

        $compile($elm)($scope);

    };

    $scope.close = function () {
        var ages = $scope.gridApi.selection.getSelectedRows();
        $scope.colFilter.listTerm = [];

        ages.forEach(function (age) {
            var status;
            switch (age.age) {
                case "شروع نشده":
                    {
                        status = "1";
                    }
                    break;
                case "در حال انجام":
                    {
                        status = "2";
                    }
                    break;
                case "پایان یافته":
                    {
                        status = "3";
                    }
                    break;
            }
            $scope.colFilter.listTerm.push(status);
        });

        $scope.colFilter.term = $scope.colFilter.listTerm.join(', ');
        $scope.colFilter.condition = new RegExp($scope.colFilter.listTerm.join('|'));

        if ($elm) {
            $elm.remove();
        }
    };
});


app.directive('myCustomModal', function () {
    return {
        template: '<label>{{colFilter.term}}</label><button ng-click="showAgeModal()">...</button>',
        controller: 'myCustomModalCtrl'
    };
});
app.directive('persiandatepicker', function () {
    return {
        restrict: 'AE',
        require: '?ngModel',
        replace: true,
        link: function (scope, element, attrs, ngModel) {
            //if (!ngModel) return;

            element.bind("click", function () {
                PersianDatePicker.Show(this, scope.nowdate);
            });

            element.context.onchange = function () {
                scope.filter[this.name] = this.value;
                scope.$apply();
            };
            element.bind("change", function () {
                scope.filter[this.name] = this.value;
                scope.$apply();
            });
        }
    };
});