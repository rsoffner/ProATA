"use strict"

const signalrConnection = new signalR.HubConnectionBuilder()
    .withUrl("/messagebroker")
    .configureLogging(signalR.LogLevel.Information)
    .build();

signalrConnection.start().then(function () {
    console.log("SignalR Hub Connected");
}).catch(function (err) {
    return console.error(err.toString());
});

signalrConnection.on("onMessageReceived", function (commandMessage) {
    console.log(commandMessage);
});

$(document).ready(function() {

    $('[name="runBtn"]').click(function () {
        const id = $(this).attr('data-id');

        signalrConnection.invoke("CommandReceived", id, 1).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });

});