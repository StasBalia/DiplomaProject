// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function() {
    $("#success-alert").hide();
    $('#fileTree').fileTree(
        {
            root: 'Scripts/',
            script: 'Home/GetFileTree',
            multiFolder: true
        },
        function(file) {
            console.log(file);
            var split = file.split('/');
            $('#launchScriptTitle').text(split[split.length - 1]);
            $('#launchModal').modal('show');
            $('#scriptParams').empty();
            $('#scriptPath').text(file);
            $.ajax({
             url: '/Script/GetParams?path=' + file,
               type: 'GET',
               success: function (data) {
                     for (var i = 0; i < data.length; i++) {
                         $('#scriptParams').append('<div class="form-group"><label for="parameter:' + data[i] + '">' + data[i] + '</label><input type="text" class="form-control" id="parameter:' + data[i] + '"></div>');
                    }
                  }
            });
        });
});

function callLaunch() {
    var paramsArray = $('input[id^="parameter:"]');

    var params = [];

    for (var i = 0; i < paramsArray.length; i++) {
        var obj = {};
        obj["Name"] = paramsArray[i].id.replace('parameter:', '');
        obj["Value"] = paramsArray[i].value;
        params.push(obj);
    }
    
    
    $('#launchModal').modal('hide');
    $("#success-alert").fadeTo(5000, 500).slideUp(500,
        function() {
            $("#success-alert").slideUp(500);
        });
    console.log(JSON.stringify(params));

    var data = {'path':$('#scriptPath').text(), 'parameters':JSON.stringify(params), 'ext':$('#formatSelect').val().toString()};
    
    var jsonData = JSON.stringify(data);
    console.log(jsonData);
    $.ajax({
        url: '/Script/Launch',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: jsonData,
        dataType: "json",
        success: function(data) {
            var response = JSON.parse(data);
            window.open('/Script/Download?savedPath=' + response.SavedPath
            + '&fileName=' + response.FileName + '&fileType=' + response.FileType);
        }
    });
}

function openSource() {
    window.open('/Script/Source?src=' + $('#scriptPath').text());
}