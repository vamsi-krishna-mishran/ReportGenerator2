
$(function () {
    initialization();
    setPlaceholders();
    $("#capaform").on("submit", function (e) {
        e.preventDefault();
        let data = {};
        $('input[type="checkbox"]:not(:checked)').each(function () {
            let name = $(this).attr("name");
            data[name]= "off";
            
        });

        
        const serArray = $("#capaform").serializeArray();
       // console.log(serArray);
        serArray.forEach(el => {
            let x = el.name;
            let y = el.value;
          // if (y == '') y = "not filled";
            data[x] = y;
        })
        console.log(data);
        sendData(data,"/Home/Capa");
    });
    $(".next").on("click", function () {
        let id = $(this).attr("id");
        let tabnum = id.at(-1);
        tabnum = +tabnum;
        if (tabnum >= 3) return;
        $(".tabs").hide();
        $(`#tab${tabnum + 1}`).show();
        $(this).attr("id", `ntab${tabnum + 1}`);
        $(".prev").attr("id", `ptab${tabnum + 1}`);

    });
    $(".prev").on("click", function () {
        let id = $(this).attr("id");
        let tabnum = id.at(-1);
        tabnum = +tabnum;
        if (tabnum <=1) return;
        $(".tabs").hide();
        $(`#tab${tabnum - 1}`).show();
        $(this).attr("id", `ntab${tabnum - 1}`);
        $(".next").attr("id", `ptab${tabnum - 1}`);

    });
});
function initialization() {
    $(".tabs").hide();
    $("#tab1").show();
}
