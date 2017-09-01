'use strict';

/*
author:Yuwei Dai
date:2016.1.21
desc:自定义UI组件
*/
angular.module('qm.ui', ['qm.ui.tables', 'qm.ui.webupload']);
//表格组件
angular.module("qm.ui.tables", []).constant('tableConfig', {
    //每行自动生成 checkbox，
    enableSelected: true,
    enableSearch: true,
    page: {
        length: 15,
        start: 0,
        index: 0
    },
    pageSizeOptions: [15, 50, 100, 200]
}).controller('TableController', ['$http', '$scope', '$attrs', 'tableConfig', function ($http, $scope, $attrs, tableConfig) {
    console.log($scope.advanceSearch);
    if ($scope.tableConfig == null) $scope.tableConfig = tableConfig;
    //页面总数
    $scope.tableConfig.page.total = 0;
    $scope.tableConfig.page.allRecords = 0;
    //页面集合
    $scope.tableConfig.page.list = [];

    //重置参数
    $scope.reset = false;

    //排序
    $scope.order = {};
    $scope.rows = [];

    //设置参数
    $scope.setParams = function (reset) {
        if ($scope.reset) reset = true;
        if (reset) $scope.tableConfig.page.index = 0;

        $scope.tableConfig.page.start = $scope.tableConfig.page.index * $scope.tableConfig.page.length;

        var sortConditions = "";

        angular.forEach($scope.columns, function (key, value) {
            //排序集合对象
            if (key.dir) {
                sortConditions += key.name + "," + key.dir + ";";
            }
        });

        //var paramss = angular.copy($scope.params);
        //angular.extend(paramss, { sort: sortConditions, pageSize: $scope.tableConfig.page.length, pageIndex: $scope.tableConfig.page.index });

        if ($scope.params.query == undefined || $scope.params.query == null || $scope.params.query == "")
            delete $scope.params.query;

        $scope.params.sort = sortConditions;
        $scope.params.pageSize = $scope.tableConfig.page.length;
        $scope.params.pageIndex = $scope.tableConfig.page.index;

        $scope.ajax();
    };

    $scope.setParams();

    //翻页
    //pageIndex 从0开始
    $scope.flipPage = function ($pageIndex) {
        if ($pageIndex < 0 || $pageIndex >= $scope.tableConfig.page.total) return;
        $scope.tableConfig.page.index = $pageIndex;
        $scope.setParams();
    };

    //设置列排序
    $scope.setOrder = function (column, reset) {

        //如果当前列处于排序中，直接在升序 降序中切换。反之，遍历取消当前排序的列，当前列默认先升序
        if (column.dir == "asc" || column.dir == "desc") {
            column.dir = column.dir == "asc" ? "desc" : "asc";
        }
        else {
            angular.forEach($scope.columns, function (key, value) {
                if (key.dir == "asc" || key.dir == "desc")
                    key.dir = ""

                if (column.name == key.name) key.dir = "asc";
            });
        }
        $scope.setParams(reset);
    };

    //#region 行选择

    //选中行数目
    $scope.selectedRowCount = 0;
    //选中所有
    $scope.selectAll = false;

    //批量设置行选中
    $scope.setRowSelectiton = function (selected) {
        $scope.selectAll = selected;
        angular.forEach($scope.rows, function (item, value) {
            item.selected = selected;
        });

        $scope.selectedRowCount = selected ? $scope.rows.length : 0;
    };

    //选中切换
    $scope.switchSelection = function () {
        var count = 0;
        angular.forEach($scope.rows, function (item, value) {
            item.selected = !item.selected;
            if (item.selected) count++;
        });
        $scope.selectedRowCount = count;
    };

    //监测每行选中状态变化
    $scope.selectedRowChange = function (row) {
        if (row.selected)
            $scope.selectedRowCount++;
        else
            $scope.selectedRowCount--;
    };

    //监测选中数目
    $scope.$watch('selectedRowCount', function (newValue, oldValue) {
        if ($scope.rows.length == 0) return;
        $scope.selectAll = $scope.selectedRowCount == $scope.rows.length;
    });

    //#endregion

    //监测参数变化，深度监测
    //$scope.$watch("params", function (newValue, oldValue) {

    //}, true);

    $scope.$watch("response", function (newValue, oldValue) {
        //$scope.processData();
        if ($scope.response == null || $scope.response == undefined) return;
        $scope.tableConfig.page.total = Math.ceil($scope.response.paging.total / tableConfig.page.length);
        $scope.tableConfig.page.allRecords = $scope.response.paging.total;

        //计算页面集合         
        var i = -2;
        $scope.tableConfig.page.list = [];

        //最多显示5页
        while ($scope.tableConfig.page.list.length < 5 && $scope.tableConfig.page.list.length < $scope.tableConfig.page.total) {

            var p = $scope.tableConfig.page.index + i;
            if (p > 0) {
                $scope.tableConfig.page.list.push(p);
            }

            i++;
        }

        $scope.rows = $scope.response.data;
        //$scope.selectedRowCount = 0;
    }, true);
}]).directive('qmtable', function () {
    return {
        restrict: 'AE',
        scope: {
            processing: "=",
            params: "=",
            response: "=",
            columns: "=",
            ajax: "&",
            tableConfig: "=",
            extButtons: "=",
            tableEidtAndDelete: "=",
            showAdvance: "=",
            advanceSearch: "&",  //传递方法
            reset: "=",  //强制重置参数
        },
        replace: true,
        templateUrl: 'app/common/directives/qm-ui/table.html',
        controller: "TableController"
    };
});

//文件上传组件
angular.module("qm.ui.webupload", []).controller('WebUploaderController', ['$http', '$scope', '$attrs', '$timeout', function ($http, $scope, $attrs, $timeout) {
    //$scope.pictures = [];
    //$scope.files = [];

    //$scope.toaster('success', 'test', '', 1000);
    //$scope.toaster();

    //#region 文件上传模块    
    var imgUploader = null;
    var fileUploader = null;

    $scope.selectedImg = null;

    //上传初始化
    var _uploadInit = function () {

        //删除所有图片
        $scope.removeAll = function (type) {
            if (type == 'img') {
                $scope.pictures = [];
                $scope.selectedImg = null;

                var imgs = imgUploader.getFiles();

                angular.forEach(imgs, function (i, v) {
                    imgUploader.removeFile(i, true);
                });
            }
            else {
                $scope.files = [];
                var files = fileUploader.getFiles();

                angular.forEach(files, function (i, v) {
                    fileUploader.removeFile(i, true);
                });
            }
        };

        //删除文件
        $scope.removeFile = function (file, index) {
            $scope.files.splice(index, 1);

            if (file.fileId != undefined || file.fileId != null)
                fileUploader.removeFile(file.fileId, true);
        };

        //#region 图片操作function

        //删除图片
        $scope.removeImg = function (img, index) {
            $scope.pictures.splice(index, 1);
            if (img.fileId != undefined || img.fileId != null)
            imgUploader.removeFile(img.fileId, true);

            var allFiles = imgUploader.getFiles();
            console.log(allFiles);

            if ($scope.pictures.length > 0)
                $scope.selectPicutre($scope.pictures[0]);
            else $scope.selectedImg = null;
        };

        $scope.selectPicutre = function (img) {
            if ($scope.selectedImg != undefined && $scope.selectedImg != null) {
                $scope.selectedImg.selected = false;
            }

            img.selected = true;

            $scope.selectedImg = img;
        };


        if ($scope.pictures.length > 0)
            $scope.selectPicutre($scope.pictures[0]);

        //#endregion 图片操作function    

        $timeout(function () {
            if ($scope.pictures != null) {
                //图片上传实例化
                if (imgUploader == null) {
                    // 实例化
                    imgUploader = WebUploader.create({
                        // swf文件路径
                        swf: $scope.swfUrl,
                        // 文件接收服务端。
                        server: $scope.pictureUploadUrl,
                        auto: true,
                        // 选择文件的按钮。可选。
                        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                        pick: '#filePicker0',
                        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                        resize: false,
                        accept: {
                            title: 'Images',
                            extensions: 'gif,jpg,jpeg,bmp,png',
                            mimeTypes: 'image/*'
                        },
                        fileNumLimit: 100,//上传数量限制
                        fileSizeLimit: 100 * 2 * 1024 * 1024,//限制上传所有文件大小
                        fileSingleSizeLimit: 2 * 1024 * 1024,//限制上传单个文件大小
                    });

                    imgUploader.on("error", function (type) {
                        console.log(type);
                        if (type == "F_DUPLICATE") {
                            alert( '请不要重复提交');
                        } else if (type == "Q_EXCEED_SIZE_LIMIT") {
                            alert( '所选图片总大小不可超过200M！"');
                        }
                        else if (type == "F_EXCEED_SIZE") {
                            alert( '所选图片单个大小不可超过2M！"');
                        }
                    });
                    // 当有文件添加进来的时候
                    imgUploader.on('fileQueued', function (file) {

                        var img = {
                            fileId: file.id,
                            title: file.name.replace("." + file.ext, ""),
                            alt: "",
                            percentage: 0,
                            uploaded: false
                        };
                        $scope.pictures.push(img);

                        // 创建缩略图
                        // 如果为非图片文件，可以不用调用此方法。
                        // thumbnailWidth x thumbnailHeight 为 100 x 100
                        imgUploader.makeThumb(file, function (error, src) {
                            if (error) {
                                img.src = $rootScope.baseUrl + "img/webUploader/notpreviewed.png";
                            }
                            else {
                                img.src = src;
                            }
                            //非ng环境下，需要手动触发
                            $scope.$apply();
                        }, 200, 200);
                    });
                    //文件上传进度
                    imgUploader.on('uploadProgress', function (file, percentage) {
                        angular.forEach($scope.pictures, function (i, v) {

                            if (v.id == file.id) {
                                v.percentage = percentage;
                                console.log(percentage);

                                $scope.$apply();
                                return;
                            }

                        });


                    });
                    //文件上传结果处理
                    imgUploader.on('uploadSuccess', function (file, response) {
                        angular.forEach($scope.pictures, function (v, i) {
                            if (v.fileId == file.id) {
                                v.percentage = 100;
                                v.uploaded = true;
                                v.error = false;
                                v.status = "上传成功";
                                v.pictureId = response[0].id;
                                v.href = response[0].url;
                                return;
                            }

                        });

                    });

                    imgUploader.on('uploadError', function (file, reason) {


                        angular.forEach($scope.pictures, function (i, v) {

                            if (v.fileId == file.id) {
                                v.percentage = 100;
                                v.uploaded = true;
                                v.error = true;
                                v.status = "上传失败";
                                alert( "上传失败：" + reason);
                                return;
                            }

                        });
                    });
                    imgUploader.on('uploadComplete', function (file) {
                        //非ng环境下，需要手动触发
                        $scope.$apply();
                    });
                }
            }

            if ($scope.files != null) {
                //文件上传实例化
                if (fileUploader == null) {
                    // 实例化
                    fileUploader = WebUploader.create({
                        // swf文件路径
                        swf: $scope.swfUrl,
                        // 文件接收服务端。
                        server: $scope.fileUploadUrl,
                        auto: true,
                        // 选择文件的按钮。可选。
                        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                        pick: '#filePicker1',
                        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                        resize: false,
                        fileNumLimit: 100,//上传数量限制
                        fileSizeLimit: 100 * 5 * 1024 * 1024,//限制上传所有文件大小
                        fileSingleSizeLimit: 5 * 1024 * 1024,//限制上传单个文件大小
                    });

                    fileUploader.on("error", function (type) {
                        console.log(type);
                        if (type == "F_DUPLICATE") {
                            alert( '请不要重复提交');
                        } else if (type == "Q_EXCEED_SIZE_LIMIT") {
                            alert( '所选文件总大小不可超过500M！');
                        }
                        else if (type == "F_EXCEED_SIZE") {
                            alert( '所选文件单个大小不可超过5M！', '', 1000);
                        }
                    });
                    // 当有文件添加进来的时候
                    fileUploader.on('fileQueued', function (file) {

                        var f = {
                            fileId: file.id,
                            title: file.name.replace("." + file.ext, ""),
                            percentage: 0,
                            uploaded: false
                        };
                        $scope.files.push(f);
                    });
                    //文件上传进度
                    fileUploader.on('uploadProgress', function (file, percentage) {
                        angular.forEach($scope.pictures, function (i, v) {
                            if (v.id == file.id) {
                                v.percentage = percentage;
                                $scope.$apply();
                                return;
                            }
                        });
                    });
                    //文件上传结果处理
                    fileUploader.on('uploadSuccess', function (file, response) {
                        angular.forEach($scope.files, function (v, i) {
                            if (v.fileId == file.id) {
                                v.percentage = 100;
                                v.uploaded = true;
                                v.error = false;
                                v.status = "上传成功";
                                v.fileId = response[0].id;
                                v.src = response[0].url;
                                return;
                            }

                        });
                    });
                    fileUploader.on('uploadError', function (file, reason) {
                        angular.forEach($scope.files, function (i, v) {

                            if (v.fileId == file.id) {
                                v.percentage = 100;
                                v.uploaded = true;
                                v.error = true;
                                v.status = "上传失败";
                                alert( "上传失败：" + reason);
                                return;
                            }

                        });
                        $scope.$apply();
                    });
                    fileUploader.on('uploadComplete', function (file) {
                        //非ng环境下，需要手动触发
                        $scope.$apply();
                    });
                }
            }
        });
    };
    ////#endregion

    return _uploadInit();
}]).directive('webuploader', function () {
    return {
        restrict: 'AE',
        scope: {
            swfUrl:"=",
            pictureUploadUrl: "=",
            fileUploadUrl:"=",
            pictures: "=",
            files: "=",
            //toaster: "&",
        },
        replace: true,
        templateUrl: 'app/common/directives/qm-ui/webUploader.html',
        controller: "WebUploaderController"
    };
});