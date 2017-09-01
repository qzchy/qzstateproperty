/// <reference path="../vendor/echarts/echarts.js" />
/// <reference path="../vendor/libs/webuploader/webuploader.js" />
/// <reference path="../vendor/modules/angular-amap/angular-amap.min.js" />
/// <reference path="../vendor/modules/angular-amap/angular-amap.js" />
/// <reference path="../vendor/modules/angular-amap/angular-amap.js" />
'use strict';

/* Config for the router */
angular.module('app').run(['$rootScope', '$state', '$stateParams',  'authService', function ($rootScope, $state, $stateParams, authService) {
    //只有实例对象和常量能够被注入到 运行(run )块当中。这样做的目的是为了防止在应用程序运行期间，增加，修改了系统配置。
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams, fromState, fromParams) {
        //console.error(authService);
        if (toState.name != 'access.signin') {
            if (!authService.authentication.isAuth) {
                event.preventDefault();
                $state.go('access.signin');
            }
            else {

                //是否需要管理员
                var needAdmin = toState.needAdmin;
                var isAdmin = authService.isAdmin();
                if (needAdmin && !isAdmin) {
                    event.preventDefault();
                    $state.go('app.home');
                }
                else return;
            }
        }
       
    });
}]).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/app/home');

    $stateProvider
            .state('app', {
                abstract: true,
                url: '/app',
                templateUrl: 'app/views/app.html'
            })
            .state('app.home', {
                url: '/home',
                templateUrl: 'app/home/views/index.html',
                resolve: {
                    deps: ['$ocLazyLoad','uiLoad',
                    function ($ocLazyLoad,uiLoad) {
                        return uiLoad.load([ 
                        ]).then(function () {
                            return $ocLazyLoad.load(['angular-nicescroll',
                            'app/home/Controllers/HomeCtrl.js',
                          'app/property/Services/PropertyService.js'
                            ]);
                        });
                    }
                    ]
                }
            })
            .state('app.overview', {
                url: '/overview',
                templateUrl: 'app/dashboard/views/index.html',
                resolve: {
                    deps: ['$ocLazyLoad', 'uiLoad',
                    function ($ocLazyLoad, uiLoad) {
                        return uiLoad.load([
                        ]).then(function () {
                            return $ocLazyLoad.load([
                                'vendor/echarts/echarts.js',
                                'app/property/Services/PropertyService.js',
                                'app/statistics/Services/StatisticsService.js',
                                //'app/statistics/Controllers/OverviewStatisticsCtrl.js',
                                'app/dashboard/Controllers/DashboardCtrl.js']);
                        });
                    }
                    ]
                }
            })
        //#region 目录管理路由配置
        .state('app.property', {
            abstract: true,
            url: '',
            template: '<div ui-view></div>',
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js']);
                }]
            }
        })
        .state('app.property.list', {
            url: '/properties',
            templateUrl: 'app/property/views/list.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['angular-nicescroll', 'vr.directives.slider', 'ui.select',
                           'app/systemmanage/governments/Services/GovernmentService.js',
                          'app/property/Services/PropertyService.js',
                         
                       //  'app/map/services/MapService.js',
                        //  'app/property/Controllers/PropertyCreateOrEditCtrl.js',
                          'app/property/Controllers/PropertyListCtrl.js'])
                  }
                ]
            }
        })
        .state('app.property.create', {
            url: '/properties/create',
            templateUrl: 'app/property/views/create.html',
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {

                          return $ocLazyLoad.load([
                              'vendor/libs/webuploader/webuploader.js',
                              'vendor/libs/webuploader/webuploader.css',
                              'vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                              'vendor/leaflet/wicket/wicket-leaflet.js',
                             'https://cdn.bootcss.com/leaflet.draw/0.4.9/leaflet.draw.css',
                             'https://cdn.bootcss.com/leaflet.draw/0.4.9/leaflet.draw-src.js',
                          ]).then(function () {
                              return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus']).then(function () {
                                  return $ocLazyLoad.load([
                                      'app/common/controller/UploadModalDialogCtrl.js', 'app/property/Services/PropertyService.js',
                                      'app/systemmanage/governments/Services/GovernmentService.js',
                                      'app/map/services/MapService.js',
                                      'app/property/Controllers/PropertyCreateOrEditCtrl.js']);
                              });
                          });

                      });
                  }
                ]
            }
        })
        .state('app.property.edit', {
            url: '/properties/:id/edit',
            templateUrl: 'app/property/views/create.html',
            controller: function ($scope, $stateParams) {
                $scope.propertyId = $stateParams.id;               
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {

                          return $ocLazyLoad.load([
                              'vendor/libs/webuploader/webuploader.js',
                              'vendor/libs/webuploader/webuploader.css',
                              'vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                              'vendor/leaflet/wicket/wicket-leaflet.js',
                             'https://cdn.bootcss.com/leaflet.draw/0.4.9/leaflet.draw.css',
                             'https://cdn.bootcss.com/leaflet.draw/0.4.9/leaflet.draw-src.js',
                          ]).then(function () {
                              return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus']).then(function () {
                                  return $ocLazyLoad.load([
                                      'app/common/controller/UploadModalDialogCtrl.js', 'app/property/Services/PropertyService.js',
                                      'app/systemmanage/governments/Services/GovernmentService.js',
                                      'app/map/services/MapService.js',
                                      'app/property/Controllers/PropertyCreateOrEditCtrl.js']);
                              });
                          });

                      });
                  }
                ]
            }
        })
        .state('app.property.detail', {
            url: '/properties/:id',
            templateUrl: 'app/property/views/detail.html',
            controller: function ($scope, $stateParams) {
                $scope.propertyId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {
                          return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                            'vendor/leaflet/wicket/wicket-leaflet.js',
                              'vendor/echarts/echarts.js'
                          ]).then(function () {
                              return $ocLazyLoad.load(['fancyboxplus','app/property/Services/PropertyService.js', 'app/property/Controllers/PropertyCtrl.js', 'app/map/services/MapService.js']);
                          })

                      });
                  }
                ]
            }
        })
        .state('app.property.government', {
            url: '/properties/governments/:id',
            templateUrl: 'app/property/views/government_detail.html',
            controller: function ($scope, $stateParams) {
                $scope.governmentId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                 //'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                 //   //'vendor/leaflet/leaflet.css',
                 //   'vendor/leaflet/leaflet.js',

                      ]).then(function () {

                          return $ocLazyLoad.load([
                              //'vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                              //'vendor/leaflet/wicket/wicket.js',
                              //                              'vendor/leaflet/wicket/wicket-leaflet.js',
                              //'vendor/echarts/echarts.js'
                          ]).then(function () {
                              return $ocLazyLoad.load(['app/property/Services/PropertyService.js', 'app/systemmanage/governments/Services/GovernmentService.js', 'app/property/controllers/GovernmentCtrl.js',
                                  'app/map/services/MapService.js']);
                          })

                      });
                  }
                ]
            }
        })

        //.state('app.property,process', {          
        //    url: 'properties/process',
        //    template: '<div ui-view></div>',
        //    resolve: {
        //        deps: ['$ocLazyLoad', function ($ocLazyLoad) {
        //            return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js']);
        //        }]
        //    }
        //})
        .state('app.property.process', {
            url: 'properties/process',
            templateUrl: 'app/property/views/process.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {

                      return $ocLazyLoad.load([
                      'vendor/libs/webuploader/webuploader.js',
                      'vendor/libs/webuploader/webuploader.css']).then(function () {
                          return $ocLazyLoad.load(['toaster', 'infinite-scroll', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select']).then(function () {
                              return $ocLazyLoad.load([
                             'app/systemmanage/governments/Services/GovernmentService.js',
                            'app/property/Services/PropertyService.js',
                            'app/property/Controllers/PropertyProcessCtrl.js'])
                          });
                      });
                  }
                ]
            }
        })
        .state('app.property.process_approve', {
            url: 'properties/approve?:approveType',
            templateUrl: 'app/property/views/process_approve.html',
            controller: function ($scope, $stateParams) {
                $scope.approveType = $stateParams.approveType;
            },
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['toaster','angular-nicescroll', 'ui.select',
                           'app/systemmanage/governments/Services/GovernmentService.js',
                          'app/property/Services/PropertyService.js',
                          'app/property/Controllers/PropertyProcessApproveCtrl.js'])
                  }
                ]
            }
        })

        .state('app.property.process_newCreatedetail', {
            url: '/properties/process/:id/newCreatedetail',
            templateUrl: 'app/property/views/newCreatedetail.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
                $scope.approveType = 'newCreate';
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {
                          return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                            'vendor/leaflet/wicket/wicket-leaflet.js',
                          ]).then(function () {

                              return $ocLazyLoad.load(['toaster', 'fancyboxplus']).then(function () {
                                  return $ocLazyLoad.load(['app/property/Services/PropertyService.js',
                                      'app/property/Controllers/PropertyApproveDetailCtrl.js',
                                      'app/map/services/MapService.js']);
                              });
                          })

                      });
                  }
                ]
            }
        })
         .state('app.property.process_editdetail', {
             url: '/properties/process/:id/editdetail',
             templateUrl: 'app/property/views/editdetail.html',
             controller: function ($scope, $stateParams) {
                 $scope.approveId = $stateParams.id;
                 $scope.approveType = 'edit';
             },
             resolve: {
                 deps: ['$ocLazyLoad', 'uiLoad',
                   function ($ocLazyLoad, uiLoad) {
                       return uiLoad.load([
                               'vendor/leaflet/wicket/wicket.js',
                  'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                     //'vendor/leaflet/leaflet.css',
                     'vendor/leaflet/leaflet.js'
                       ]).then(function () {
                           return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                             'vendor/leaflet/wicket/wicket-leaflet.js',
                           ]).then(function () {

                               return $ocLazyLoad.load(['toaster', 'fancyboxplus', ]).then(function () {
                                   return $ocLazyLoad.load(['app/property/Services/PropertyService.js',
                                       'app/property/Controllers/PropertyApproveDetailCtrl.js',
                                       'app/map/services/MapService.js']);
                               });
                           })

                       });
                   }
                 ]
             }
         })
        .state('app.property.process_lenddetail', {
            url: '/properties/process/:id/lenddetail',
            templateUrl: 'app/property/views/lenddetail.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
                $scope.approveType = 'lend';
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {
                          return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                            'vendor/leaflet/wicket/wicket-leaflet.js',
                          ]).then(function () {

                              return $ocLazyLoad.load(['toaster', 'fancyboxplus' ]).then(function () {
                                  return $ocLazyLoad.load(['app/property/Services/PropertyService.js',
                                      'app/property/Controllers/PropertyApproveDetailCtrl.js',
                                      'app/map/services/MapService.js']);
                              });
                          })

                      });
                  }
                ]
            }
        })
        .state('app.property.process_rentdetail', {
            url: '/properties/process/:id/rentdetail',
            templateUrl: 'app/property/views/rentdetail.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
                $scope.approveType = 'rent';
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {
                          return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                            'vendor/leaflet/wicket/wicket-leaflet.js',
                          ]).then(function () {

                              return $ocLazyLoad.load(['toaster', 'fancyboxplus', ]).then(function () {
                                  return $ocLazyLoad.load(['app/property/Services/PropertyService.js',
                                      'app/property/Controllers/PropertyApproveDetailCtrl.js',
                                      'app/map/services/MapService.js']);
                              });
                          })

                      });
                  }
                ]
            }
        })
        .state('app.property.process_allotdetail', {
            url: '/properties/process/:id/allotdetail',
            templateUrl: 'app/property/views/allotdetail.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
                $scope.approveType = 'allot';
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {
                          return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                            'vendor/leaflet/wicket/wicket-leaflet.js',
                          ]).then(function () {

                              return $ocLazyLoad.load(['toaster', 'fancyboxplus', ]).then(function () {
                                  return $ocLazyLoad.load(['app/property/Services/PropertyService.js',
                                      'app/property/Controllers/PropertyApproveDetailCtrl.js',
                                      'app/map/services/MapService.js']);
                              });
                          })

                      });
                  }
                ]
            }
        })
        .state('app.property.process_offdetail', {
            url: '/properties/process/:id/offdetail',
            templateUrl: 'app/property/views/offdetail.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
                $scope.approveType = 'off';
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                              'vendor/leaflet/wicket/wicket.js',
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    //'vendor/leaflet/leaflet.css',
                    'vendor/leaflet/leaflet.js'
                      ]).then(function () {
                          return $ocLazyLoad.load(['vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                                                            'vendor/leaflet/wicket/wicket-leaflet.js',
                          ]).then(function () {

                              return $ocLazyLoad.load(['toaster', 'fancyboxplus', ]).then(function () {
                                  return $ocLazyLoad.load(['app/property/Services/PropertyService.js',
                                      'app/property/Controllers/PropertyApproveDetailCtrl.js',
                                      'app/map/services/MapService.js']);
                              });
                          })

                      });
                  }
                ]
            }
        })

        .state('app.property.process_lendedit', {
            url: '/properties/process/:id/lendedit',
            templateUrl: 'app/property/views/lendedit.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {

                      return $ocLazyLoad.load([
                        'vendor/libs/webuploader/webuploader.js',
                        'vendor/libs/webuploader/webuploader.css'
                      ]).then(function () {
                          return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select']).then(function () {
                              return $ocLazyLoad.load([
                             'app/systemmanage/governments/Services/GovernmentService.js',
                            'app/property/Services/PropertyService.js',
                            'app/property/Controllers/PropertyLendEditCtrl.js'])
                          });
                      });
                  }
                ]
            }
        })
        .state('app.property.process_rentedit', {
            url: '/properties/process/:id/rentedit',
            templateUrl: 'app/property/views/rentedit.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad',
                function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
'vendor/libs/webuploader/webuploader.js',
'vendor/libs/webuploader/webuploader.css'
                    ]).then(function () {

                        return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select',
'app/systemmanage/governments/Services/GovernmentService.js',
'app/property/Services/PropertyService.js',
'app/property/Controllers/PropertyRentEditCtrl.js']);

                    });
                }
                ]
            }
        })
        .state('app.property.process_allotedit', {
            url: '/properties/process/:id/allotedit',
            templateUrl: 'app/property/views/allotedit.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad',
                function ($ocLazyLoad) {

                    return $ocLazyLoad.load([
'vendor/libs/webuploader/webuploader.js',
'vendor/libs/webuploader/webuploader.css'
                    ]).then(function () {

                        return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select', ]).then(function () {
                            return $ocLazyLoad.load([
                            'app/systemmanage/governments/Services/GovernmentService.js',
                            'app/property/Services/PropertyService.js',

                            'app/property/Controllers/PropertyAllotEditCtrl.js'])
                        })
                    });


                }
                ]
            }
        })
        .state('app.property.process_offedit', {
            url: '/properties/process/:id/offedit',
            templateUrl: 'app/property/views/offedit.html',
            controller: function ($scope, $stateParams) {
                $scope.approveId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad',
                    function ($ocLazyLoad) {

                        return $ocLazyLoad.load([
'vendor/libs/webuploader/webuploader.js',
'vendor/libs/webuploader/webuploader.css'
                        ]).then(function () {

                            return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select', ]).then(function () {
                                return $ocLazyLoad.load([
                            'app/systemmanage/governments/Services/GovernmentService.js',
                            'app/property/Services/PropertyService.js',

                            'app/property/Controllers/PropertyOffEditCtrl.js'])
                            });

                        });
                    }
                ]
            }
        })


        .state('app.map', {
         url: '/map',
         templateUrl: 'app/map/views/index.html',
         controller: function ($scope, $stateParams) {
             $scope.mapType = $stateParams.mapType;
         },
         params:{
             mapType:null
         },
         resolve: {
             deps: ['$ocLazyLoad', 'uiLoad',
               function ($ocLazyLoad, uiLoad) {
                   return uiLoad.load([
                 'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                 //'vendor/leaflet/leaflet.css',
                 'vendor/leaflet/leaflet.js',
                 'vendor/echarts/echarts.js',
                   ]).then(function () {
                       return uiLoad.load([
                  'vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                    'vendor/leaflet/wicket/wicket.js',
                    'vendor/leaflet/wicket/wicket-leaflet.js',
                       'vendor/leaflet/iconLayer/iconLayers.js',
                       'vendor/leaflet/iconLayer/iconLayers.css',
                       'https://cdn.bootcss.com/leaflet.markercluster/0.5.0/leaflet.markercluster.js',
                       'https://cdn.bootcss.com/leaflet.markercluster/0.5.0/MarkerCluster.css',
                       'https://cdn.bootcss.com/leaflet.markercluster/0.5.0/MarkerCluster.Default.css',
                         'vendor/leaflet/label/leaflet.label.js',
                       'vendor/leaflet/label/leaflet.label.css',
                         'vendor/leaflet/heatmap/leaflet-heat.js',
                         'https://cdn.bootcss.com/leaflet.draw/0.4.9/leaflet.draw.css',
                         'https://cdn.bootcss.com/leaflet.draw/0.4.9/leaflet.draw-src.js',
                         'vendor/leaflet/awesome-marker/leaflet.awesome-markers.css',
                         'vendor/leaflet/awesome-marker/leaflet.awesome-markers.js',
                       ]).then(function () {
                           return $ocLazyLoad.load([
                                'angular-nicescroll', 'vr.directives.slider', 'infinite-scroll', 'ui.select', 'app/common/controller/ModalDialogCtrl.js',
 'app/systemmanage/governments/Services/GovernmentService.js',
                                'app/property/Services/PropertyService.js',
                           'app/map/controllers/MapController.js',
                           'app/map/services/MapService.js'
                           ]);
                       });
                   });
               }
             ]
         }
     })
        .state('app.officeadjust', {
            url: '/officeadjust',
            templateUrl: 'app/officeadjust/views/index.html',
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                    'http://cdn.bootcss.com/leaflet/0.7.7/leaflet.css',
                    'vendor/leaflet/leaflet.js',
                    'vendor/echarts/echarts.js',
                      ]).then(function () {
                          return uiLoad.load([
                       'vendor/leaflet/leaflet-tilelayer-wmts-src.js',
                       'vendor/leaflet/wicket/wicket.js',
                       'vendor/leaflet/wicket/wicket-leaflet.js',
                          'vendor/leaflet/iconLayer/iconLayers.js',
                          'vendor/leaflet/iconLayer/iconLayers.css',
                          'https://cdn.bootcss.com/leaflet.markercluster/0.5.0/leaflet.markercluster.js',
                          'https://cdn.bootcss.com/leaflet.markercluster/0.5.0/MarkerCluster.css',
                          'https://cdn.bootcss.com/leaflet.markercluster/0.5.0/MarkerCluster.Default.css',
                            'vendor/leaflet/label/leaflet.label.js',
                          'vendor/leaflet/label/leaflet.label.css',
                            'vendor/leaflet/awesome-marker/leaflet.awesome-markers.css',
                            'vendor/leaflet/awesome-marker/leaflet.awesome-markers.js',
                          ]).then(function () {
                              return $ocLazyLoad.load([
                                   'angular-nicescroll', 'vr.directives.slider', 'infinite-scroll', 'ui.select',
                                   'app/common/controller/ModalDialogCtrl.js',
                                   'app/systemmanage/governments/Services/GovernmentService.js',
                                   'app/officeadjust/controllers/AdjustController.js',
                                   'app/map/services/MapService.js'
                              ]);
                          });
                      });
                  }
                ]
            }
        })

        .state('app.systemmanage', {
            abstract: true,
            url: '',
            template: '<div ui-view></div>',
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js']);
                }]
            }
        })

        .state('app.systemmanage.government', {
            url: '/systemmanage/government',
            templateUrl: 'app/systemmanage/governments/views/list.html',
            resolve: {
                deps: ['$ocLazyLoad',
                    function ($ocLazyLoad) {
                        return $ocLazyLoad.load(['angular-nicescroll', 'angularBootstrapNavTree', 'w5c.validator']).then(
                            function () {
                                return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js', 'app/systemmanage/governments/Services/GovernmentService.js',
                                                                        'app/systemmanage/governments/Controllers/GovernmentListCtrl.js']);
                            }
                        );
                    }
                ]
            },
            needAdmin: true
        })
           .state('app.systemmanage.government_edit', {
               url: '/systemmanage/government/edit',
               templateUrl: 'app/systemmanage/governments/views/inforedit.html',
               resolve: {
                   deps: ['$ocLazyLoad',
                     function ($ocLazyLoad) {

                         return $ocLazyLoad.load([
                           'vendor/libs/webuploader/webuploader.js',
                           'vendor/libs/webuploader/webuploader.css'
                         ]).then(function () {
                             return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select']).then(function () {
                                 return $ocLazyLoad.load([
                             'app/systemmanage/accounts/Services/AccountService.js',
                                 'app/systemmanage/governments/Services/GovernmentService.js',
                               //  'app/systemmanage/accounts/Controllers/AccountCreateOrEditCtrl.js',
                                   'app/systemmanage/governments/Controllers/InforEditCtrl.js'])
                             });
                         });
                     }
                   ]
               }
           })


        .state('app.systemmanage.account', {
            abstract: true,
            url: '',
            template: '<div ui-view></div>',
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js']);
                }]
            },
            needAdmin: true
        })
        .state('app.systemmanage.account.list', {
            url: '/systemmanage/accounts',
            templateUrl: 'app/systemmanage/accounts/views/list.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['app/systemmanage/accounts/Services/AccountService.js', 'app/systemmanage/accounts/Controllers/AccountListCtrl.js'])
                  }
                ]
            },
            needAdmin: true
        })
        .state('app.systemmanage.account.create', {
            url: '/systemmanage/accounts/create',
            templateUrl: 'app/systemmanage/accounts/views/create.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['toaster', 'ui.select', 'w5c.validator']).then(function () {
                          return $ocLazyLoad.load(['app/systemmanage/accounts/Services/AccountService.js',
                              'app/systemmanage/governments/Services/GovernmentService.js',
                              'app/systemmanage/accounts/Controllers/AccountCreateOrEditCtrl.js']);
                      });
                  }
                ]
            },
            needAdmin: true
        })
        .state('app.systemmanage.account.edit', {
            url: '/systemmanage/accounts/:id/edit',
            templateUrl: 'app/systemmanage/accounts/views/create.html',
            controller: function ($scope, $stateParams) {
                $scope.accountId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator']).then(function () {
                          return $ocLazyLoad.load(['app/systemmanage/accounts/Services/AccountService.js',
                              'app/systemmanage/governments/Services/GovernmentService.js',
                              'app/systemmanage/accounts/Controllers/AccountCreateOrEditCtrl.js']);
                      });
                  }
                ]
            },
            needAdmin: true
        })
          .state('app.systemmanage.account.resetpwd', {
              url: '/systemmanage/accounts/resetpwd?:name',
              templateUrl: 'app/systemmanage/accounts/views/resetpwd.html',
              controller: function ($scope, $stateParams) {
                  $scope.accountName = $stateParams.name;
              },
              resolve: {
                  deps: ['$ocLazyLoad',
                    function ($ocLazyLoad) {

                        return $ocLazyLoad.load([
                          'vendor/libs/webuploader/webuploader.js',
                          'vendor/libs/webuploader/webuploader.css'
                        ]).then(function () {
                            return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator', 'fancyboxplus', 'angular-nicescroll', 'ui.select']).then(function () {
                                return $ocLazyLoad.load([
                            'app/systemmanage/accounts/Services/AccountService.js',
                                'app/systemmanage/governments/Services/GovernmentService.js',
                              //  'app/systemmanage/accounts/Controllers/AccountCreateOrEditCtrl.js',
                                  'app/systemmanage/accounts/Controllers/ResetpwdCtrl.js'])
                            });
                        });
                    }
                  ]
              }
          })
        .state('app.systemmanage.account.detail', {
            url: '/systemmanage/accounts/:id',
            templateUrl: 'app/systemmanage/accounts/views/detail.html',
            controller: function ($scope, $stateParams) {
                $scope.accountId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return $ocLazyLoad.load(['toaster']).then(function () {
                          return $ocLazyLoad.load(['app/systemmanage/accounts/Services/AccountService.js',
                              'app/systemmanage/governments/Services/GovernmentService.js',
                              'app/systemmanage/accounts/Controllers/AccountCtrl.js']);
                      });
                  }
                ]
            },
            needAdmin: true
        })


        //.state('app.systemmanage.settings', {
        //    url: '/systemmanage/settings',
        //    templateUrl: 'app/property/views/create.html',
        //    resolve: {
        //        deps: ['$ocLazyLoad', 'uiLoad',
        //          function ($ocLazyLoad, uiLoad) {
        //              return uiLoad.load([
        //                  'http://webapi.amap.com/maps?v=1.3&key=1e4ff737e5e9e2f63cc3333316b38a7c'
        //              ]).then(function () {
        //                  return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator']).then(function () {
        //                      return $ocLazyLoad.load(['app/properties/storecateory/Services/StoreCategoryService.js',
        //                          'app/property/Services/StoreService.js', 'app/property/Controller/StoreCreateOrEditCtrl.js']);
        //                  });
        //              });
        //          }
        //        ]
        //    }
        //})
        //.state('app.systemmanage.logs', {
        //    url: '/systemmanage/logs',
        //    templateUrl: 'app/property/views/create.html',
        //    controller: function ($scope, $stateParams) {
        //        $scope.storeId = $stateParams.id;
        //    },
        //    resolve: {
        //        deps: ['$ocLazyLoad', 'uiLoad',
        //          function ($ocLazyLoad, uiLoad) {
        //              return uiLoad.load([
        //                  'http://webapi.amap.com/maps?v=1.3&key=1e4ff737e5e9e2f63cc3333316b38a7c'
        //              ]).then(function () {
        //                  return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator']).then(function () {
        //                      return $ocLazyLoad.load(['app/properties/storecateory/Services/StoreCategoryService.js',
        //                          'app/property/Services/StoreService.js', 'app/property/Controller/StoreCreateOrEditCtrl.js']);
        //                  });
        //              });
        //          }
        //        ]
        //    }
        //})

        //#endregion

        .state('access', {
            abstract: true,
            url: '/access',
            template: '<div ui-view class="fade-in-right-big smooth"></div>'
        })
        .state('access.signin', {
            url: '/signin',
            templateUrl: 'app/access/views/signin.html',
            resolve: {
                deps: ['$ocLazyLoad',
                function ($ocLazyLoad) {
                    return $ocLazyLoad.load(['toaster', 'w5c.validator']).then(function () {
                        return $ocLazyLoad.load('app/access/controller/signinFormController.js');
                    });
                }]
            }
        });
}]);