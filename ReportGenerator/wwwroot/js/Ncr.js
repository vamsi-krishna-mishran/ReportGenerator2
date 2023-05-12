$(function () {
    setPlaceholders();
    $("#capaform").on("submit", function (e) {
        e.preventDefault();
        let data = {};
        $('input[type="checkbox"]:not(:checked)').each(function () {
            let name = $(this).attr("name");
            data[name] = "off";

        });

        const serArray = $("#capaform").serializeArray();
        // console.log(serArray);
        serArray.forEach(el => {
            let x = el.name;
            let y = el.value;
            //if (y == '') y = "not filled";
            data[x] = y;
        })
        console.log(data);
        sendData(data,"/Home/Ncr");
    });
});