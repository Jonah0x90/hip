/**
 * Created by Administrator on 2015/9/18.
 */

//标识是否全部安装完成
var isFinished = 0;

//发送请求
var sendFirstRequest;
var sendThirdRequest;

/**请求服务器，获取安装结果**/
var getInstallStatus = function () {
    if (isFinished == 1) {
        clearInterval(sendFirstRequest);
    }
}

var loadImg = function (object, data) {
    var i = 0;
    setInterval(function () {
        i++
        if (i > data) {
            i = data
        }
        var imgLeft = -(i * 44 + (i * 10)) + 'px';
        object.css('background-position', imgLeft + '\t' + '0px');
        object.html(i + '%');
    }, 50)
}

$(document).ready(function () {
    $('.install-content .install-menu .install-btn .build-data')
        .off('click')
        .on('click', function () {
            var self = $(this);
            var dataSourceName = $('#dataSource-name').val();
            var username = $('#data-username').val();
            var password = $('#data-password').val();

            if (dataSourceName.length <= 0) {
                $('.install-content .install-menu .dataSource-name-text').css('color', 'red');
                $('#dataSource-name').focus();
                return;
            }
            else if (username.length <= 0) {
                $('.install-content .install-menu .data-username-text').css('color', 'red');
                $('#data-username').focus();
                return;
            }
            else if (password.length <= 0) {
                $('.install-content .install-menu .data-password-text').css('color', 'red');
                $('#data-password').focus();
                return;
            }

            self.attr('disabled', true);
            self.css('cursor', 'inherit');

            $.post('../sys/Install/UpdateConfig', {
                DataSource: dataSourceName,
                Password: password,
                UserID: username
            }, function (result) {
                if (result.Output.Data.result.msg) {
                    $('body').append($(result.Output.Data.result.msg));
                }
                result.Output.Data.result = result.Output.Data.result.Current;

                if (result.Output.Data.result == 1) {
                    var statOne = $('#stat-one');
                    var circle = statOne.circliful();
                    var sum = 7;
                    //$.getJSON('../sys/Install/GetSum', function (result) {
                    //    if (result.Flag == 1) {
                    //        sum = result.Output.Data;
                    //    }
                    //});


                    $.getJSON('../sys/Install/Excute',
                     function (result) {
                         if (result.Flag == 1) {

                         }
                         //else {
                         //    alert('导入基础数据失败');
                         //    self.attr('disabled', false);
                         //    return;
                         //}

                     });

                    var statId = $('#stat-one').attr('id');
                    var idNum = 1;
                    sendFirstRequest = setInterval(function () {
                        $.getJSON(

                             '../sys/InstallPross/GetProgressing',

                            function (result) {
                                sum = result.sum;
                                var numB = result.current;
                                numB = (numB === null || numB === "undefined") ? 0 : numB;
                                var num = parseFloat(numB) / parseFloat(sum);
                                var speed = 0;
                                if (num > 0) {
                                    var index = num.toString().indexOf('.');
                                    speed = num.toString().substr(index + 1, 2);
                                    if (num === 1) {
                                        speed = 100;
                                    }
                                    else if (speed[0] === "0")
                                        speed = speed[1];
                                }
                                

                                var statHtml = '<div id="' + statId + idNum + '" data-dimension="250" data-text="' + speed + '%' + '" data-info="正在导入..."' +
                                    'data-width="30" data-fontsize="38" data-percent="' + speed + '" data-fgcolor="#61a9dc"' +
                                    'data-bgcolor="#eee" style="margin: 30px auto;"></div>';
                                $('.stat-div').html(statHtml);
                                $('#' + statId + idNum).circliful();

                                if (result.current === sum) {
                                    clearInterval(sendFirstRequest);
                                    $('.install-content .install-menu .install-btn .first-to-next').css('display', 'inline');
                                }

                            });
                        idNum++;
                    }, 1000);

                } else {
                    alert('导入基础数据失败');
                    self.attr('disabled', false);
                }

                //$('.install-content .install-menu .install-btn .first-to-next').css('display', 'inline');
            });
        });

    $('.install-content .install-menu .install-btn .first-to-next')
        .off('click')
        .on('click', function () {
            $('.install-content .install-menu').css('display', 'none');
            $('.install-content .install-second-menu').css('display', 'block');
        });

    $('.install-content .install-second-menu .install-btn .edit-data-btn')
        .off('click')
        .on('click', function () {
            var self = $(this);

            var username = $('.install-second-menu #dataSource-username').val();
            var password = $('.install-second-menu #dataSource-password').val();
            var sid = $('.install-second-menu #dataSource-sid').val();
            var hostName = $('.install-second-menu #dataSource-hostName').val();
            var port = $('.install-second-menu #dataSource-port').val();
            var version = $('.install-second-menu #dataSource-version').val();

            if (username.length <= 0) {
                $('.install-second-menu .dataSource-username-error').css('color', 'red');
                $('.install-second-menu #dataSource-username').focus();
                return;
            }
            else if (password.length <= 0) {
                $('.install-second-menu .dataSource-password-error').css('color', 'red');
                $('.install-second-menu #dataSource-password').focus();
                return;
            }
            else if (sid.length <= 0) {
                $('.install-second-menu .dataSource-sid-error').css('color', 'red');
                $('.install-second-menu #dataSource-sid').focus();
                return;
            }
            else if (hostName.length <= 0) {
                $('.install-second-menu .dataSource-hostName-error').css('color', 'red');
                $('.install-second-menu #dataSource-hostName').focus();
                return;
            }
            else if (port.length <= 0) {
                $('.install-second-menu .dataSource-port-error').css('color', 'red');
                $('.install-second-menu #dataSource-port').focus();
                return;
            }
            else if (version.length <= 0) {
                $('.install-second-menu .dataSource-version-error').css('color', 'red');
                $('.install-second-menu #dataSource-version').focus();
                return;
            }
            self.attr('disabled', true);
            self.css('cursor', 'inherit');

            $.post('../sys/Install/DataSource', {
                Port: port,
                Host: hostName,
                Password: password,
                Provider: version,
                SID: sid,
                UserName: username
            }, function (result) {
                if (result.Flag == 1) {
                    $('.install-content .install-second-menu .install-btn .second-to-next').css('display', 'inline');
                } else {
                    alert('导入第三方数据失败');
                    self.attr('disabled', false);
                }
            });
        });

    $('.install-content .install-second-menu .install-btn .second-to-next')
        .off('click')
        .on('click', function () {
            $('.install-content .install-second-menu').css('display', 'none');
            $('.install-content .install-third-menu').css('display', 'block');
        });

    $('.install-content .install-third-menu .install-btn .import-data-btn')
        .off('click')
        .on('click', function () {
            var self = $(this);
            self.attr('disabled', true);
            self.css('cursor', 'inherit');

            var statThree = $('#stat-three');
            var sum = 1;
            statThree.circliful();

            $.getJSON('../third/third/FetchInpationList', function (result) {
                //if (result.Flag == 0) {

                //} else {
                //    alert('导入第三方数据失败');
                //    self.attr('disabled', false);
                //    return;
                //}
            });

            var statId = $('#stat-three').attr('id');
            var idNum = 1;
            sendThirdRequest = setInterval(function () {
                $.getJSON('../sys/InstallPross/GetProgressing', function (result) {
                    //var speed = (parseFloat(result.Flag) / parseFloat(sum)).toFixed(2) * 100;
                    //statThree.attr('data-text',speed+'%');
                    var numB = result.current;
                    sum = result.sum;
                    numB = (numB === null || numB === "undefined") ? 0 : numB;
                    var num = parseFloat(numB) / parseFloat(sum);
                    var speed = 0;
                    if (num > 0) {
                        var index = num.toString().indexOf('.');
                        speed = num.toString().substr(index + 1, 2);
                        if (num === 1) {
                            speed = 100;
                        }
                        else if (speed[0] === "0")
                            speed = speed[1];
                    }

                    var statHtml = '<div id="' + statId + idNum + '" data-dimension="250" data-text="' + speed + '%' + '" data-info="正在导入..."' +
                        'data-width="30" data-fontsize="38" data-percent="' + speed + '" data-fgcolor="#61a9dc"' +
                        'data-bgcolor="#eee" style="margin: 30px auto;"></div>';
                    $('.stat-div-third').html(statHtml);
                    $('#' + statId + idNum).circliful();

                    if (numB === sum) {
                        clearInterval(sendThirdRequest);
                        $('.install-content .install-third-menu .install-btn .finish-btn').css('display', 'inline');
                    }
                });
            }, 1000);
            //var box = $('#install-box-three');
            //box.css('display','block');
            //loadImg(box,100);
            //setInterval(function () {
            //    $('.install-content .install-third-menu .install-btn .finish-btn').css('display','inline');
            //},5000);
        });

    $('.install-content .install-third-menu .install-btn .finish-btn')
        .off('click')
        .on('click', function () {
            window.close();
        });

    // 文本框内容发生变化事件
    $('.input-data')
        .off('keyup')
        .on('keyup', function () {
            var self = $(this);
            self.parent().next().css('color', 'gray');
        });

});