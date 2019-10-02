
var OverlayControl = {};

OverlayControl.Communications = function (URI) {
	this.socketstring = URI;
	this.websocket = {};
	this.connected = false;
	this.modulestore = {};
	
}
OverlayControl.Communications.prototype = {
	constructor: OverlayControl.Communications, 

	onMessage: function (evt) { 
		var obj = $.parseJSON(evt.data);
		if (this.modulestore[obj.module] && this.modulestore[obj.module][obj.method]) {
			var handler = this.modulestore[obj.module][obj.method];
			handler(obj.module,obj.method,$.parseJSON(obj.data));
		}
		
	},
	Open: function () {
		var self = this;
		this.websocket = new WebSocket(self.socketstring);
		this.websocket.onopen = function(evt) { self.connected = true; };
		this.websocket.onclose = function(evt) { self.connected = false; };
		this.websocket.onmessage = function(evt) { 
			self.onMessage(evt); 
		};
		this.websocket.onerror = function(evt) { 
			self.connected = false;
			EventStream.SendMessage("OMG, ERROR!, monkaS"); 
		};
	},
	Close: function () {
		this.websocket.close();
	},
	Send: function( module, method, data) {
		var overlaymessage = {
			id : 0,
			module : module, 
			method : method, 
			data : data
		};
		
		websocket.send(overlayMessage);
	},
	IsConnected: function () {
		return this.connected;
	}, 
	SubscribeMessage: function (module, method, d) {
		if (!(this.modulestore[module])) {
			this.modulestore[module] = {};
		}
		this.modulestore[module][method] = d;
	},
	UnsubscribeMessage: function (module, method) {
		if (this.modulestore[module] && this.modulestore[module][method]) {
				delete this.modulestore[module][method];
		}
	}
};

$(document).ready(function () {
	EventStream.SendMessage('This is a test message from overlay controlsssss');
	interval = setInterval(function () {
		$('.media-event-video,.media-event-image,.media-event-audio').each(
			function (index, element) 
			{
				var timedisplayed = parseInt($(element).attr("data-removeat"));
				var nowtime = (new Date()).getTime();
				if (nowtime > timedisplayed)
				{
					$(element).remove();
				}
				
			}
		);
	}, 250);
	
	
	var comm = new OverlayControl.Communications("ws://192.168.1.213:8080/BrowserOverlay");
	OverlayControl.Instance = comm;
	comm.Open();
	
	comm.SubscribeMessage("overlaymessage", "follow", function (module, method, data) {
		EventStream.SendMessage(data.FollowMessage);
	});
	
	comm.SubscribeMessage("overlaymessage", "media", function (module, method, mediadata) {
		var MediaEl;
		switch (mediadata.MediaType)
		{
			case "image":
				MediaEl = "<img src=\"" + mediadata.Media[0].AddressUri + "\" data-removeat=\"" + (parseInt((new Date()).getTime()) + parseInt(mediadata.ObjectRemovalTimeout)) + "\" class=\"media-event-image\" />";
				break;
			case "video":
				MediaEl = "<video width=\"" + mediadata.MediaWidth + "\" height=\"" + mediadata.MediaHeight + "\" data-removeat=\"" + (parseInt((new Date()).getTime()) + parseInt(mediadata.ObjectRemovalTimeout)) + "\" class=\"media-event-video\" controls>";
				$(mediadata.Media).each(function (index, el) 
				{
					MediaEl += "<source src=\"" + el.AddressUri + "\" type=\"" + el.ContentType + "\">";
				});
				MediaEl += "Your browser does not support the video tag.";
				MediaEl += "</video>";
				break;
			case "audio":
				MediaEl = "<audio data-removeat=\"" + (parseInt((new Date()).getTime()) + parseInt(mediadata.ObjectRemovalTimeout)) + "\" class=\"media-event-audio\" controls>";
				$(mediadata.Media).each(function (index, el) 
				{
					MediaEl += "<source src=\"" + el.AddressUri + "\" type=\"" + el.ContentType + "\">";
				});
				MediaEl += "Your browser does not support the video tag.";
				MediaEl += "</audio>";
				break;
		}
		if (MediaEl)
		{
			$(".mediadiv").prepend(MediaEl);
		}
	});
	comm.SubscribeMessage("overlaymessage", "newjavascript", function (module, method, data) {
		var module = data.module;
		var JsURI = data.JSUri;
		$('head').append('<script data-module="'+ module + '" src="' + JsURI + '" type="text/JavaScript"></script>');
		
	});
	comm.SubscribeMessage("overlaymessage", "newstylesheet", function (module, method, data) {
		var module = data.module;
		var cssURI = data.cssURI;
		//<link rel="stylesheet" type="text/css" media="screen" href="css/jquery-ui.css" />
		$('head').append('<link data-module="'+ module + '" rel=\"stylesheet\" type=\"text/css\" media=\"screen\" href="' + cssURI + '" />');
		
	});
	
	$('.draggable').draggable();
	
});



var OverlayMessage = function () {
	this.id = 0;
	this.module = "";
	this.method = "";
	this.data = "";
}
OverlayMessage.prototype = {
	constructor: OverlayMessage
};
 