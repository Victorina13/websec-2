﻿let globalStopsList;
let selectedStop;
let isAll = true;

$(document).ready(function () {
    getStopsList();
    $("#inputStop").on("input", function () { selectedStop = $(this).val() });
    $("#selectStop").on("click", function () { getSchedule(false) });
    $("#addToFavorite").on("click", function () { actionWithFavorite(true) });
    $("#deleteFromFavorite").on("click", function () { actionWithFavorite(false) });
    $("#changeBetweenAllAndFav").on("click", function () { getFavorites() });
    $("#deleteFromFavorite").hide();
});

function actionWithFavorite(isAdd) {
    if (!selectedStop || typeof selectedStop == "undefined") {
        alert("Остановка не выбрана!");
        return;
    }
    let infoStopSplitted = selectedStop.split(" (");
    let stopTitle = infoStopSplitted[0];
    let stopDirection = infoStopSplitted[1].substring(0, infoStopSplitted[1].length - 1);
    let filterArr = Object.values(globalStopsList).filter(stop => (stop.Title == stopTitle && stop.Direction == stopDirection) || (stop.Title.includes(stopTitle) && stop.Direction.includes(stopDirection)));
    if (filterArr.length == 0) {
        alert("Остановка не выбрана или указано неверное название!");
        return;
    }
    let favorites = JSON.parse(localStorage.getItem("favorites"));
    favorites = favorites == null ? [] : favorites;
    let index = favorites.findIndex(f => f.KS_ID == filterArr[0].KS_ID);
    if (isAdd) {
        if (index == -1) {
            favorites.push(filterArr[0]);
            localStorage.setItem("favorites", JSON.stringify(favorites));
            alert("Остановка успешно добавлена в избранные!");
        }
        else {
            alert("Остановка уже добавлена в избранные!");
        }
    }
    else {
        if (index == -1) {
            alert("Остановки нет в избранных!");
        }
        else {
            favorites.splice(index, 1);
            localStorage.setItem("favorites", JSON.stringify(favorites));
            initFavorite(favorites);
            alert("Остановка успешно удалена из избранных!");
        }
    }
    $("#inputStop").val("");
}

function getFavorites() {
    if (isAll) {
        isAll = false;
        $("#addToFavorite").hide();
        $("#deleteFromFavorite").show()
        $("#changeBetweenAllAndFav").html("Показать все");
        let favorites = JSON.parse(localStorage.getItem("favorites"));
        favorites = favorites == null ? [] : favorites;
        initFavorite(favorites);
    }
    else {
        isAll = true;
        $("#addToFavorite").show();
        $("#deleteFromFavorite").hide()
        $("#changeBetweenAllAndFav").html("Показать избранные");
        initStopsList(null);
    }
    $("#inputStop").val("");
}

function initFavorite(favs) {
    htmlString = "";
    for (let i = 0; i < favs.length; ++i) {
        htmlString += `<option>${favs[i].Title} (${favs[i].Direction})</option>`;
    }
    $("#stops").html(htmlString);
}

function initError(response) {
    $("#listTransport").html(`<div id="err"><h2>Произошла ошибка при выполнении запроса </br>Код ошибки: ${response.errorCode}</h2></div>`);
}

function initStopsList(stopsList) {
    if (!stopsList && globalStopsList) {
        stopsList = globalStopsList;
    }
    globalStopsList = stopsList;
    let htmlForDataList = "";
    for (let i = 0; i < Object.keys(globalStopsList).length; ++i) {
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

function getSchedule(fromRefresh) {
    if (!selectedStop || typeof selectedStop == "undefined") {
        alert("Остановка не выбрана!");
        $("#inputStop").val("");
        return;
    }
    let infoStopSplitted = selectedStop.split(" (");
    let stopTitle = infoStopSplitted.length == 2 ? infoStopSplitted[0] : infoStopSplitted.slice(0, infoStopSplitted.length - 1).join(' (');
    let stopDirection = infoStopSplitted[infoStopSplitted.length - 1].substring(0, infoStopSplitted[infoStopSplitted.length - 1].length - 1);
    let filterArr = Object.values(globalStopsList).filter(stop => stop.Title == stopTitle && stop.Direction == stopDirection);
    if (filterArr.length == 0) {
        alert("Остановка не выбрана или указано неверное название!");
        $("#inputStop").val("");
        return;
    }

    $.ajax({
        url: `/Main/GetSchedule/${filterArr[0].KS_ID}`,
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
    });
    if (!fromRefresh) {
        $("#inputStop").val("");
    }
}

function initSchedule(data) {
    let infoStopSplitted = selectedStop.split(" (");
    let htmlForDataList = "";
    htmlForDataList += `<h2 class="second-text">Остановка: ${infoStopSplitted.length == 2 ? infoStopSplitted[0] : infoStopSplitted.slice(0, infoStopSplitted.length - 1).join(" (")}</h2>`;
    htmlForDataList += `<table id="tableId" cellpadding="10">`;
    htmlForDataList += `<tr>`;
    htmlForDataList += `<th class="other-text">Время, мин</th>`;
    htmlForDataList += `<th class="other-text">Номер маршрута, вид транспорта</th>`;
    htmlForDataList += `<th class="other-text">Текущее положение</th>`;
    htmlForDataList += `</tr>`;
    for (let i = 0; i < data.length; i++) {
        htmlForDataList += `<tr>`;
        htmlForDataList += `<td class="other-text">${data[i].Time}</td>`;
        hrefToRoute = `<a href="/Main/Route/${data[i].KR_ID}&${data[i].HullNo}">${data[i].Number} ${data[i].Type}</a>`;
        htmlForDataList += `<td class="other-text">${hrefToRoute}</br><p class="mini-text">${data[i].ModelTitle} ${data[i].StateNumber}</p></td>`;
        htmlForDataList += `<td class="other-text">${data[i].RemainingLength}м до ${data[i].NextStopName}</td>`;
        htmlForDataList += `</tr>`;
    }
    htmlForDataList += `</table>`;
    $("#listTransport").html(htmlForDataList); 
}