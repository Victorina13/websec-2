let globalStopsList;
let selectedStop;

$(document).ready(function () {
    getStopsList();
    $("#inputStop").on("input", function () { selectedStop = $(this).val() });
    $("#selectStop").on("click", function () { getSchedule() });
});

function initError(response) {
    $("#listTransport").html(`<div id="err"><h2>Произошла ошибка при выполнении запроса </br>Код ошибки: ${response.errorCode}</h2></div>`);
}

function initStopsList(stopsList) {
    globalStopsList = stopsList;
    let htmlForDataList = "";
    for (let i = 0; i < Object.keys(stopsList).length; ++i) {
        htmlForDataList += `<option>${stopsList[i].Title} (${stopsList[i].Direction})</option>`;
    }
    $("#stops").html(htmlForDataList);
}

function getStopsList() {
    $.ajax({
        url: `/Main/GetStops`,
        method: "get",
        dataType: "html",
        success: function (data) {
            let response = JSON.parse(data);
            if (response.isSuccess) {
                initStopsList(response.data);
            }
            else {
                initError(response);
            }
        }
    })
}

function getSchedule() {
    let infoStopSplitted = selectedStop.split(" (");
    let stopTitle = infoStopSplitted[0];
    let stopDirection = infoStopSplitted[1].substring(0, infoStopSplitted[1].length - 1);
    let KS_ID = Object.values(globalStopsList).filter(stop => stop.Title == stopTitle && stop.Direction == stopDirection)[0].KS_ID;
    $.ajax({
        url: `/Main/GetSchedule/${KS_ID}`,
        method: "get",
        dataType: "html",
        success: function (data) {
            let response = JSON.parse(data);
            if (response.isSuccess) {
                initSchedule(response.data.arrival);
            }
            else {
                initError(response);
            }
        }
    })
}

function initSchedule(data) {

    let htmlForDataList = "";
    htmlForDataList += `<button id="refresh" onclick="getSchedule()">Обновить</button>`;
    htmlForDataList += `<table id="tableId" cellpadding="10">`;
    htmlForDataList += `<tr>`;
    htmlForDataList += `<th>Время, мин</th>`;
    htmlForDataList += `<th>Номер маршрута, вид транспорта</th>`;
    htmlForDataList += `<th>Текущее положение</th>`;
    htmlForDataList += `</tr>`;
    for (let i = 0; i < data.length; i++) {
        htmlForDataList += `<tr>`;
        htmlForDataList += `<td>${data[i].Time}</td>`;
        hrefToRoute = `<a href="https://localhost:7133/Main/Route/${data[i].KR_ID}&${data[i].HullNo}">${data[i].Number} ${data[i].Type}</a>`;
        htmlForDataList += `<td>${hrefToRoute}</br><font size="1">${data[i].ModelTitle} ${data[i].StateNumber}</font></td>`;
        htmlForDataList += `<td>${data[i].RemainingLength}м до ${data[i].NextStopName}</td>`;
        htmlForDataList += `</tr>`;
    }
    htmlForDataList += `</table>`;
    $("#listTransport").html(htmlForDataList); 
}