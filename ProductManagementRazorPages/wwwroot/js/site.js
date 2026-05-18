"use strict";

if (window.signalR) {
    const connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();
    connection.on("LoadAllItems", function () {
        if (location.pathname.toLowerCase() === "/products" || location.pathname.toLowerCase() === "/products/index") {
            location.reload();
        }
    });
    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
}
