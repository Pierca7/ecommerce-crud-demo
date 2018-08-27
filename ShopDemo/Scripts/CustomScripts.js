//Filtrar lista de productos.
if (window.location.href.indexOf("/Plantas/Manage") > -1) {

    let products = document.getElementsByClassName("row");

    $("#Filter").keyup(function () {

        let text = $("#Filter").val().toLowerCase();

        for (var i = 0; i < products.length; i++) {
            var contains = false;
            if ((products[i].id.toLowerCase()).indexOf(text) !== -1) {
                contains = true;
            }

            if (!contains) {
                $(products[i]).addClass("d-none");
            }
            else {
                var check = $(products[i]).hasClass("d-none");
                if (check) {
                    $(products[i]).removeClass("d-none");
                }
            }
        }
    });
}

//Ir arriba.
$("#toUp").click(function () {
    $("body, html").animate({
        scrollTop: "0px"
    }, 300);
});

//Mostrar/Ocultar barra de busqueda.
$('#navbarSupportedContent').on('show.bs.collapse', function () {
    $("#searchbar").addClass("d-none");
    $("#searchbar-sm").removeClass("d-none");
});

$('#navbarSupportedContent').on('hidden.bs.collapse', function () {
    $("#searchbar").removeClass("d-none");
    $("#searchbar-sm").addClass("d-none");
});


