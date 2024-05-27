document.addEventListener("DOMContentLoaded", function () {
    var preguntasDiv = document.querySelectorAll(".row-preguntas div");
    var cantidadPreguntas = document.querySelectorAll(".row-block .info-p");
    var totalPreguntas = sumQuantity(cantidadPreguntas);

    if (preguntasDiv && cantidadPreguntas && preguntasDiv.length == cantidadPreguntas.length) {
        changeWrapper(preguntasDiv, cantidadPreguntas, totalPreguntas)
    }
});

function sumQuantity(listaCantidad) {
    var total = 0;
    listaCantidad.forEach(function (cantidad) {
        total += parseInt(cantidad.innerHTML);
    })
    return total
}

function changeWrapper(listDiv, listP, total) {
    for (var i = 0; i < listDiv.length; i++) {
        changeWidth(listDiv[i], listP[i].innerHTML, total)
    }
}

function changeWidth(div, quantity, total) {
    div.style.width = `${(quantity / total) * 100}%`
}