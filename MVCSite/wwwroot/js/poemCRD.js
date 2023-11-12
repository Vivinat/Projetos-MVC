$(document).ready(function () {
    $(".search-button").click(function () {
        var actionName = $(this).data("action");
        var input = $('.form-control');
        var queryValue = input.val();
        console.log(queryValue);
        console.log(actionName);
        $.post("/Poem/PerformCRD/", { query: queryValue, actionName: actionName }, function (data) {
            // Lógica para manipular a resposta do controlador, se necessário
        });
    });
});
