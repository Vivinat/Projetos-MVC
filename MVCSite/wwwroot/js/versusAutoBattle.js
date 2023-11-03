
var aliadoNome = "";
var inimigoNome = "";

$(document).ready(function () {

    function atualizarEnfrentando() {
        console.log("Atualizando");
        var enfrentandoElement = document.getElementById("enfrentando");
        if (inimigoNome === "" && aliadoNome === "") {
            enfrentandoElement.textContent = "Aguardando escolha de combatentes...";
        } else {
            enfrentandoElement.textContent = "Enfrentará: " + aliadoNome + " vs " + inimigoNome;
        }
    }

    $(".btn.aliado").click(function () {
        var allyID = $(this).data("allyid");
        $.post("/AutoBattler/SelectAlly", { allyID: allyID }, function (data) {
            aliadoNome = data;
            atualizarEnfrentando();
        });
    });

    $(".btn.inimigo").click(function () {
        var enemyID = $(this).data("enemyid");
        $.post("/AutoBattler/SelectEnemy", { enemyID: enemyID }, function (data) {
            inimigoNome = data;
            atualizarEnfrentando();
        });
    });


    atualizarEnfrentando();

    $(".btn.startClash").click(function () {
        aliadoNome = "";
        inimigoNome = "";
        atualizarEnfrentando();
        $.ajax({
            type: "GET",
            url: "/AutoBattler/Initiate",
            dataType: "json",
            success: function (results) {
                console.log("Deu bom");
                var messageLogDiv = $(".message-log");

                for (var i = 0; i < results.length; i++) {
                    var message = results[i];

                    var messageElement = $("<p>" + message + "</p>");

                    messageLogDiv.append(messageElement);
                }
                $('html, body').animate({ scrollTop: $(document).height() }, 'fast');
            }
        });
    });

});


