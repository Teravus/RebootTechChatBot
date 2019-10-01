var websocket = {};
var wsUri = "ws://192.168.1.213:8080/echo";
$(document).ready(function () {
	EventStream.SendMessage('This is a test message from overlay controlsssss');
	websocket = new WebSocket(wsUri);
	websocket.onopen = function(evt) { onOpen(evt) };
	websocket.onclose = function(evt) { onClose(evt) };
	websocket.onmessage = function(evt) { onMessage(evt) };
	websocket.onerror = function(evt) { onError(evt) };
});


//websocket.open();

function onOpen(evt)
{
	websocket.send('This is a super duper Websocket message!');
}
function onClose(evt)
{

}
function onMessage(evt)
{
	//var obj = $.parseJSON(evt.data);
	//var messagetype = obj.messagetype;
	//var messageinfo obj.messagedata;
	
	EventStream.SendMessage(evt.data);
}
function onError(evt)
{
	EventStream.SendMessage("OMG, ERROR!, monkaS");
}

 