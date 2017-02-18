function Loading() {
    this.$loading = $('#loading');
}

Loading.prototype.show = function () {
    this.$loading.show();
}

Loading.prototype.hide = function () {
    this.$loading.hide();
}

var loading = new Loading();


//ajax

function Ajax() {
    this.baseUrl = '/api/'
}



Ajax.prototype.get = function (url, data, success, errorMessages) {

    if (arguments.length === 3) {
        errorMessages = success;
        success = data;
    }

    return $.ajax({
        url: url,
        type: 'GET',
        data: typeof data === 'function' ? {} : data,
        success: function (data, status, xhr) {
            //data = "{\"data\":" + data + "}";
            //data = JSON.parse(data).data;
            var error = errors(data, errorMessages);

            if (error.length > 0) {
                alert(error[0].MSG);
                return false;
            }

            return success(data, status, xhr);
        }
    })
};

Ajax.prototype.post = function (url, data, success, errorMessages) {
    return $.ajax({
        url: url,
        type: 'POST',
        data: data,
        success: function (data, status, xhr) {
            data = "{\"data\":" + data + "}";
            data = JSON.parse(data).data;
            var error = errors(data, errorMessages);

            if (error.length > 0) {
                alert(error[0].MSG);
                return false;
            }

            return success(data, status, xhr)
        }
    })
};



$.ajaxSetup({
    timeout: '10000',
    dataType: 'json'
});


$(document).ajaxSend(function (e, xhr, options) {
    //enable loading
    loading.$loading = $('#loading');
    loading.show();


    if (options.type === 'GET') {
        //如果是get 那么直接将cookie加到url中

        options.url += options.url.indexOf('?') > -1 ? '&' : '?';
        options.url += 'cookie=' + $.cookie('des');
    } else if (options.data) {

        if (typeof options.data === 'string') {
            options.data += !!options.data ? '&' : '';
            options.data += 'cookie=' + $.cookie('des');
        }
        
    }

});

var ajax = new Ajax();


/**
 * 返回错误
 * @param data 数据
 * @param errors 对应的错误码
 * @returns boolean
 */
function errors(data, errors) {
    return errors.filter(function (error) {
        return error.CODE == data;
    });
}

$(document).ajaxComplete( function () {
    //disable loading
    loading.hide()
})

//$(document).on('ajaxError', function (xhr, options, error) {
//    alert('发生错误');
//});


//common functions
function QueryString() {
    // This function is anonymous, is executed immediately and
    // the return value is assigned to QueryString!
    var query_string = {};
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        // If first entry with this name
        if (typeof query_string[pair[0]] === 'undefined') {
            query_string[pair[0]] = decodeURIComponent(pair[1]);
            // If second entry with this name
        } else if (typeof query_string[pair[0]] === 'string') {
            var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
            query_string[pair[0]] = arr;
            // If third or later entry with this name
        } else {
            query_string[pair[0]].push(decodeURIComponent(pair[1]));
        }
    }
    return query_string;
};


function TemplateEngine(tpl, data) {
    var reg = /<%([^%>]+)?%>/g,
      regOut = /(^( )?(if|for|else|switch|case|break|{|}))(.*)?/g,
      code = 'var r=[];\n',
      cursor = 0, match;

    var add = function (line, js) {
        js ? (code += line.match(regOut) ? line + '\n' : 'r.push(' + line + ');\n') :
          (code += line != '' ? 'r.push("' + line.replace(/"/g, '\\"') + '");\n' : '');
        return add;
    }
    while (match = reg.exec(tpl)) {
        add(tpl.slice(cursor, match.index))(match[1], true);
        cursor = match.index + match[0].length;
    }
    add(tpl.substr(cursor, tpl.length - cursor));
    code += 'return r.join("");';
    return new Function(code.replace(/[\r\t\n]/g, '')).apply(data);
};

function SubString(str, n) {
    var r = /[^\x00-\xff]/g;
    if (str.replace(r, 'mm').length <= n) {
        return str;
    }
    var m = Math.floor(n / 2);
    for (var i = m; i < str.length; i++) {
        if (str.substr(0, i).replace(r, 'mm').length >= n) {
            return str.substr(0, i) + '...';
        }
    }
    return str;
}

function GenerateDate(str) {
    str = parseInt(str.replace(/[^0-9]/g, '') , 10);
    var date = new Date(str);
    //几月几日发布
    return date.getMonth() + 1 + '月' + date.getDate() + '日' + date.getHours() + ':' + date.getMinutes();
}

