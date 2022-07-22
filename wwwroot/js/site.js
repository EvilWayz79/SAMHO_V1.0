// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {    
    /*--------------------------------------------------------------------------------------------------*/
    /*----------------------------------ESPECIALIDAD----------------------------------------------------*/
    $("#buttonDiv").click(function () {
        $("#EditarEspDiv").empty();
        $("#CrearEspDiv").load("/Admin/CrearEspecialidad");
    });

    $(".editButton").click(function () {
        $("#CrearEspDiv").empty();
        var path = "/Admin/EditarEspecialidad\\?idEspecialidad=" + $(this).attr("value");
        $("#EditarEspDiv").load(path);
    });

    /*--------------------------------------------------------------------------------------------------*/
    /*----------------------------------CREAR HORARIOS--------------------------------------------------*/

    $("#DiasSemana").val("");

    $(".diasCheck").click(function () {
        //var stringTest = console.log($('input[name="diasCheck"]:checked').serialize());

        //var stringTest = $("input[type=checkbox][name=DiasSemana]:checked").val();

        //var stringTest = $.map($('input[name="DiasSemana"]:checked'), function (c) { return c.value; })
        /*
        var diaSeleccionado = $(this).val();
        //var diasEnCampo = $("#DiasSemana").text();
        var diasEnCampo = $("#DiasSemana").val().trim();

        var onOff = $(this).is(":checked");

        if (onOff) {
            diasEnCampo = diasEnCampo + " " + diaSeleccionado;
        }
        else {
            diasEnCampo = diasEnCampo.replace(diaSeleccionado, "");
        }

        //$("#DiasSemana").text(diasEnCampo);
        
        $("#DiasSemana").val(diasEnCampo);*/
    });
    /*--------------------------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------------------------*/
    $("#RolUsuario").ready().change(function () {
        if ($(this).val() != 'Paciente') {
            $("#camposEmpleado").css("display", "block");
        }
        else {
            $("#camposEmpleado").css("display", "none");
        }
    })
    

})

    
