OverlayControl.Instance.SubscribeMessage('sentimenticon','currentsentiment', function (module, method, data) {
	if (data.sentimentvalue == 0) {
		// load our medium png
		$('.sentimenticon-image').addClass('sentiment-medium');
		$('.sentimenticon-image').removeClass('sentiment-high');
		$('.sentimenticon-image').removeClass('sentiment-low');
	} else if (data.sentimentvalue > 0) {
		// load our high png
		$('.sentimenticon-image').removeClass('sentiment-medium');
		$('.sentimenticon-image').addClass('sentiment-high');
		$('.sentimenticon-image').removeClass('sentiment-low');
	} else if (data.sentimentvalue < 0) {
		$('.sentimenticon-image').removeClass('sentiment-medium');
		$('.sentimenticon-image').removeClass('sentiment-high');
		$('.sentimenticon-image').addClass('sentiment-low');
		// load our low png
	}
	
});
var bodyel = $('body');
bodyel.append('<div class="sentimenticon-image">&nbsp;</div>');
alert("Howdy!");