$(function () {
    var username = getUrlVars()["username"];

    // Enable logging for development purpose
    $.connection.hub.logging = true;
    // Declare a proxy to reference the hub.
    var messageHub = $.connection.messageHub;
    // Create a function that the hub can call to broadcast messages.
    messageHub.client.displayMessage = function (sender, message) {
        // Wrap the sender and the message in HTML
        var senderDiv = $('<div />').text(sender).html();
        var messageDiv = $('<div />').text(message).html();
        // Add the message to the page.
        $('#messagelist').append('<li><strong>' + senderDiv
            + '</strong>:&nbsp;&nbsp;' + messageDiv + '</li>');
    };
    
    //$.connection.hub.start().done(function () {
        $.connection.hub.start("~/signalr").done(function () {
       
        messageHub.server.login(username);

        $('#sendmessage').click(function () {
            var msg = $("#message").val();
            messageHub.server.sendMessage(username, msg);
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
    });
});

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
