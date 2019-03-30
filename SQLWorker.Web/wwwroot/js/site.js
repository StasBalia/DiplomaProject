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
    setInterval(updateScriptInfo, 5000);
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


function updateScriptInfo() {
    $.ajax({
        url: '/Script/GetTasksForUser',
        type: 'GET',
        success: function (data) {
            var str = "";
            for (var i = 0; i < data.length; i++) {
                str += '<tr align="center">';
                str += '<td style="width:45%"><a href ="Script/GetScriptInfo?guid=' + data[i].id + '" target="_blank">' + data[i].scriptSource.name + '</td>';
                str += '<td style="width:15%;font-size:10px;">' + data[i].resultFileExtension + '</td>';
                str += '<td style="width:40%;">';
                console.log("Task state = " + data[i].taskState);
                switch (data[i].taskState) {
                    case "Queued":
                        str += '<i class="fa fa-hand-paper"></i>';
                        break;
                    case "Started":
                        str += '<i class="fa fa-spinner fa-pulse"></i>';
                        break;
                    case "Error":
                        str += '<i class="fa fa-times"></i>';
                        break;
                    case "Success":
                        str += '<a href="Script/Download?savedPath=' + data[i].downloadPath + data[i].downloadName + '&fileName=' + data[i].downloadName + '&fileType=' + data[i].resultFileExtension +'" target="_blank"><i class="fa fa-download"></i></a>';
                        break;
                }
                str += '</td>';
                str += '</tr>';
            }
            document.getElementById("partBody").innerHTML = str;
        }
    });
}