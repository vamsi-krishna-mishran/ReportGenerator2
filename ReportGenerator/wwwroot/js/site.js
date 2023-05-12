function sendData(dataArray,url) {
    try {
        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(dataArray), // Replace with your JSON data
            xhrFields: {
                responseType: 'blob' // Set the response type to 'blob'
            },
            success: function (data) {
                var pdfUrl = URL.createObjectURL(data); // Create a URL for the PDF blob
                window.open(pdfUrl); // Open the PDF in a new tab
            },
            complete: function () {
                console.log("call completed..");
            },
            error: function (error) {
                // Handle any errors
                console.error(error);
            }
        });
    }
    catch (error) {
        console.log("error occured" + error);
    }
}
function setPlaceholders() {
    $('input, textarea').each(function () {
        var label = $('label[for="' + $(this).attr('id') + '"]');
        var labelText = label.text();
        $(this).attr('placeholder', labelText);
    });
}


