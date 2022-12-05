let ROUTE;
let HULLNO;
let KR_ID;
let TransportPosition;

$(document).ready(function () {
    let partOfUrl = document.location.pathname;
    let params = partOfUrl.split("/")[3].split("&");
    getRoute(params);
    getTransportPosition(params);
});

function initRoute(Route) {
    console.log(ROUTE);
    let htmlForDataList = "";
    $("#routeName").html(`${Route.TransportTypeObj.Title} №${Route.Number}`);
    $("#endStartRoute").html(`${Route.Stops[0].Title} - ${Route.Stops[Route.Stops.length - 1].Title}`);
    for (let i = 0; i < Route.Stops.length; i++) {
        htmlForDataList += `<div id="${Route.Stops[i].KS_ID}" class="stop-on-route prev-route border-radiused"><div class="stop-name other-text">${Route.Stops[i].Title}</div></div>`;
    };
    $("#stopsList").html(htmlForDataList);
}

function initTransportPosition(transportPosition) {
    let near = "Подъезжает";
    for (let i = 0; i < transportPosition.NextStops.length; i++) {
        let tmpHtml = $(`#${transportPosition.NextStops[i].KS_ID}`).html();
        let time = Math.round(transportPosition.NextStops[i].Time / 60) < 1 ? near : Math.round(transportPosition.NextStops[i].Time / 60);
        tmpHtml += `<div class="time other-text">${time}</div>`;
        $(`#${transportPosition.NextStops[i].KS_ID}`).html(tmpHtml);
        $(`#${transportPosition.NextStops[i].KS_ID}`).removeClass("prev-route");
        $(`#${transportPosition.NextStops[i].KS_ID}`).addClass("next-route");
    }
    //var posArray = $(`#${transportPosition.NextStops[0].KS_ID}`).offsetTop;
    let posGlobal = document.getElementById(`${transportPosition.NextStops[0].KS_ID}`).offsetTop;
    let posIntoDiv = posGlobal - document.getElementById("stopsList").offsetTop;
    document.getElementById("stopsList").scrollTop = posIntoDiv;
}

function getRoute(params) {
    KR_ID = params[0];
    $.ajax({
        url: `/Main/GetRoute/${KR_ID}`,
        method: "get",
        dataType: "html",
        success: function (data) {
            let response = JSON.parse(data);
            if (response.isSuccess) {
                ROUTE = response.data;
                initRoute(response.data);
            }
            else {
                initError(response);
            }            
        }
    });
}

function getTransportPosition(params) {
    HULLNO = params[1];
    $.ajax({
        url: `/Main/GetTransportPosition/${HULLNO}`,
        method: "get",
        dataType: "html",
        success: function (data) {
            let response = JSON.parse(data);
            if (response.isSuccess) {
                TransportPosition = response.data;
                initTransportPosition(response.data);
            }
            else {
                initError(response);
            } 
        }
    });
}