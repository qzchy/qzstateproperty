"use strict";
//对话框控制器
app.controller('UploadModalDialogCtrl', function ($rootScope, $scope, $timeout, $uibModalInstance) {
    //$wrap = $('#uploader'),
    var fileCount = 0,// 添加的文件数量
    files = [],
    fileSize = 0, // 添加的文件总大小

    ratio = window.devicePixelRatio || 1,   // 优化retina, 在retina下这个值是2

    thumbnailWidth = 110 * ratio,    // 缩略图大小
    thumbnailHeight = 110 * ratio,

    //state = 'pedding',   // 可能有pedding, ready, uploading, confirm, done.

   //上传按钮配置
    uploadBtn = {
        text: '开始上传',
        state: 'pedding', // 可能有pedding, ready, uploading, confirm, done.
        state_class: '',
        disabled: false,
        handler: function () { }
    },

    percentages = {},   // 所有文件的进度信息，key为file id

    info = "",  //信息

    //状态栏配置
    statusBar = {
        visible: true,
        progress_visible: false
    },

    placeholder = {
        visible: true
    },//placeholder 的 element-invisible class

    queue = {
        parentclass: '',
        visible: ''
    },

    supportTransition = (function () {
        var s = document.createElement('p').style,
            r = 'transition' in s ||
                'WebkitTransition' in s ||
                'MozTransition' in s ||
                'msTransition' in s ||
                'OTransition' in s;
        s = null;
        return r;
    })(),

  uploader = null;    // WebUploader实例

    $timeout(function () {

        if (uploader == null) {

            // WebUploader实例

            if (!WebUploader.Uploader.support('flash') && WebUploader.browser.ie) {

                // flash 安装了但是版本过低。
                if (flashVersion) {
                    (function (container) {
                        window['expressinstallcallback'] = function (state) {
                            switch (state) {
                                case 'Download.Cancelled':
                                    alert('您取消了更新！')
                                    break;

                                case 'Download.Failed':
                                    alert('安装失败')
                                    break;

                                default:
                                    alert('安装已成功，请刷新！');
                                    break;
                            }
                            delete window['expressinstallcallback'];
                        };

                        var swf = './expressInstall.swf';
                        // insert flash object
                        var html = '<object type="application/' +
                                'x-shockwave-flash" data="' + swf + '" ';

                        if (WebUploader.browser.ie) {
                            html += 'classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" ';
                        }

                        html += 'width="100%" height="100%" style="outline:0">' +
                            '<param name="movie" value="' + swf + '" />' +
                            '<param name="wmode" value="transparent" />' +
                            '<param name="allowscriptaccess" value="always" />' +
                        '</object>';

                        container.html(html);

                    })($wrap);

                    // 压根就没有安转。
                } else {
                    $wrap.html('<a href="http://www.adobe.com/go/getflashplayer" target="_blank" border="0"><img alt="get flash player" src="http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg" /></a>');
                }

                return;
            } else if (!WebUploader.Uploader.support()) {
                alert('Web Uploader 不支持您的浏览器！');
                return;
            }

            // 实例化
            uploader = WebUploader.create({

                pick: {
                    id: '#filePicker',
                    label: '点击选择图片'
                },
                dnd: '#uploader .queueList',
                paste: document.body,

                accept: {
                    title: 'Images',
                    extensions: 'gif,jpg,jpeg,bmp,png',
                    mimeTypes: 'image/*'
                },

                // swf文件路径
                swf: $rootScope.baseUrl + 'vender/libs/webuploader/Uploader.swf',
                // 文件接收服务端。
                server: $rootScope.apiUrl + 'Media/pictures/Upload?size=500',

                disableGlobalDnd: true,

                chunked: true,
                // server: 'http://webuploader.duapp.com/server/fileupload.php',
                server: 'http://2betop.net/fileupload.php',
                fileNumLimit: 300,
                fileSizeLimit: 5 * 1024 * 1024,    // 200 M

                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false,
            });

            // 添加“添加文件”的按钮，
            uploader.addButton({
                id: '#filePicker2',
                label: '继续添加'
            });

            // 当有文件添加进来时执行，负责view的创建
            function addFile(file) {

                //$prgress = $li.find('p.progress span'),
                //$wrap = $li.find('p.imgWrap'),
                //$info = $('<p class="error"></p>'),

                var showError = function (code) {
                    switch (code) {
                        case 'exceed_size':
                            file.info = '文件大小超出';
                            break;
                        case 'interrupt':
                            file.info = '上传暂停';
                            break;
                        default:
                            file.info = '上传失败，请重试';
                            break;
                    }
                };

                if (file.getStatus() === 'invalid') {
                    showError(file.statusText);
                } else {
                    // @todo lazyload
                    file.text = "预览中";
                    uploader.makeThumb(file, function (error, src) {
                        if (error) {
                            file.text = "不能预览";
                            return;
                        }
                        file.src = src;

                        $scope.$apply(); //更新数据
                    }, thumbnailWidth, thumbnailHeight);

                    percentages[file.id] = [file.size, 0];
                    file.rotation = 0;
                }

                file.on('statuschange', function (cur, prev) {
                    if (prev === 'progress') {
                        $prgress.hide().width(0);
                    } else if (prev === 'queued') {
                        $li.off('mouseenter mouseleave');
                        $btns.remove();
                    }

                    // 成功
                    if (cur === 'error' || cur === 'invalid') {
                        console.log(file.statusText);
                        showError(file.statusText);
                        percentages[file.id][1] = 1;
                    } else if (cur === 'interrupt') {
                        showError('interrupt');
                    } else if (cur === 'queued') {
                        percentages[file.id][1] = 0;
                    } else if (cur === 'progress') {
                        $info.remove();
                        $prgress.css('display', 'block');
                    } else if (cur === 'complete') {
                        $li.append('<span class="success"></span>');
                    }

                    $li.removeClass('state-' + prev).addClass('state-' + cur);
                });

                file.delete = function () {
                    uploader.removeFile(file);
                };

                file.rotate = function (angle) {
                    file.rotation += angle;
                };

                files.push(file);
                $scope.$apply();

                $scope.imgmouseon = function () { };

                //$li.on('mouseenter', function () {
                //    $btns.stop().animate({ height: 30 });
                //});

                //$li.on('mouseleave', function () {
                //    $btns.stop().animate({ height: 0 });
                //});

                //$btns.on('click', 'span', function () {
                //    var index = $(this).index(),
                //        deg;

                //    switch (index) {
                //        case 0:
                //            uploader.removeFile(file);
                //            return;

                //        case 1:
                //            file.rotation += 90;
                //            break;

                //        case 2:
                //            file.rotation -= 90;
                //            break;
                //    }

                //    if (supportTransition) {
                //        deg = 'rotate(' + file.rotation + 'deg)';
                //        $wrap.css({
                //            '-webkit-transform': deg,
                //            '-mos-transform': deg,
                //            '-o-transform': deg,
                //            'transform': deg
                //        });
                //    } else {
                //        $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                //        // use jquery animate to rotation
                //        // $({
                //        //     rotation: rotation
                //        // }).animate({
                //        //     rotation: file.rotation
                //        // }, {
                //        //     easing: 'linear',
                //        //     step: function( now ) {
                //        //         now = now * Math.PI / 180;

                //        //         var cos = Math.cos( now ),
                //        //             sin = Math.sin( now );

                //        //         $wrap.css( 'filter', "progid:DXImageTransform.Microsoft.Matrix(M11=" + cos + ",M12=" + (-sin) + ",M21=" + sin + ",M22=" + cos + ",SizingMethod='auto expand')");
                //        //     }
                //        // });
                //    }$info


                //});
            }

            // 负责view的销毁
            function removeFile(file) {
                var $li = $('#' + file.id);

                delete percentages[file.id];
                updateTotalProgress();
                $li.off().find('.file-panel').off().end().remove();
            }

            function updateTotalProgress() {
                var loaded = 0,
                    total = 0,
                    //spans = $progress.children(),
                    percent;

                $.each(percentages, function (k, v) {
                    total += v[0];
                    loaded += v[0] * v[1];
                });

                percent = total ? loaded / total : 0;

                //spans.eq(0).text(Math.round(percent * 100) + '%');
                //spans.eq(1).css('width', Math.round(percent * 100) + '%');
                updateStatus();
            }

            function updateStatus() {
                var text = '', stats;

                if (uploadBtn.state === 'ready') {
                    text = '选中' + fileCount + '张图片，共' +
                            WebUploader.formatSize(fileSize) + '。';
                } else if (uploadBtn.state === 'confirm') {
                    stats = uploader.getStats();
                    if (stats.uploadFailNum) {
                        text = '已成功上传' + stats.successNum + '张照片至XX相册，' +
                            stats.uploadFailNum + '张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片或<a class="ignore" href="#">忽略</a>'
                    }

                } else {
                    stats = uploader.getStats();
                    text = '共' + fileCount + '张（' +
                            WebUploader.formatSize(fileSize) +
                            '），已上传' + stats.successNum + '张';

                    if (stats.uploadFailNum) {
                        text += '，失败' + stats.uploadFailNum + '张';
                    }
                }

                info = text;
            }

            function setState(val) {
                var file, stats;

                if (val === uploadBtn.state) {
                    return;
                }

                uploadBtn.state_class = 'state-' + val;
                uploadBtn.state = val;

                switch (uploadBtn.state) {
                    case 'pedding':
                        placeholder.visible = false;
                        queue.parentclass = '';
                        queue.visible = false;
                        statusBar.element_invisible = true;
                        uploader.refresh();
                        break;

                    case 'ready':
                        placeholder.visible = true;
                        $('#filePicker2').removeClass('element-invisible');
                        queue.parentclass = 'filled';
                        queue.visible = true;
                        statusBar.element_invisible = false;
                        uploader.refresh();
                        break;

                    case 'uploading':
                        $('#filePicker2').addClass('element-invisible');
                        $progress.show();
                        uploadBtn.text = '暂停上传';
                        break;

                    case 'paused':
                        $progress.show();
                        uploadBtn.text = '继续上传';
                        break;

                    case 'confirm':
                        $progress.hide();
                        uploadBtn.text = '开始上传';
                        uploadBtn.disabled = true;

                        stats = uploader.getStats();
                        if (stats.successNum && !stats.uploadFailNum) {
                            setState('finish');
                            return;
                        }
                        break;
                    case 'finish':
                        stats = uploader.getStats();
                        if (stats.successNum) {
                            alert('上传成功');
                        } else {
                            // 没有成功的图片，重设
                            state = 'done';
                            location.reload();
                        }
                        break;
                }

                updateStatus();

                $scope.$apply();
            }

            uploader.onUploadProgress = function (file, percentage) {
                var $li = $('#' + file.id),
                    $percent = $li.find('.progress span');

                $percent.css('width', percentage * 100 + '%');
                percentages[file.id][1] = percentage;
                updateTotalProgress();
            };

            uploader.onFileQueued = function (file) {
                fileCount++;
                fileSize += file.size;

                if (fileCount === 1) {
                    placeholder.element_invisible = true;
                    statusBar.visible = true;
                }

                addFile(file);
                setState('ready');
                updateTotalProgress();

                $scope.$apply();
            };

            uploader.onFileDequeued = function (file) {
                fileCount--;
                fileSize -= file.size;

                if (!fileCount) {
                    setState('pedding');
                }

                removeFile(file);
                updateTotalProgress();

            };

            uploader.on('all', function (type) {
                var stats;
                switch (type) {
                    case 'uploadFinished':
                        setState('confirm');
                        break;

                    case 'startUpload':
                        setState('uploading');
                        break;

                    case 'stopUpload':
                        setState('paused');
                        break;

                }
            });

            uploader.onError = function (code) {
                alert('Eroor: ' + code);
            };

            function startUpload() {
                if ($(this).hasClass('disabled')) {
                    return false;
                }

                if (state === 'ready') {
                    uploader.upload();
                } else if (state === 'paused') {
                    uploader.upload();
                } else if (state === 'uploading') {
                    uploader.stop();
                }
            }

            function retry() {
                uploader.retry();
            }

            function ignore() {
                alert('todo');
            }


            //$upload.on('click', function () {

            //});

            //$info.on('click', '.retry', function () {
            //    uploader.retry();
            //});

            //$info.on('click', '.ignore', function () {
            //    alert('todo');
            //});

            uploadBtn.state_class = 'state-' + uploadBtn.state;
            updateTotalProgress();
        }

    }, 50);

    $scope.files = files;
    $scope.uploadBtn = uploadBtn;
    $scope.queue = queue;
    $scope.statusBar = statusBar;
    $scope.placeholder = placeholder;

    $scope.info = info;
});