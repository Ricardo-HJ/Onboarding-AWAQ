document.addEventListener("DOMContentLoaded", function () {
    var selects = document.querySelectorAll("form select");
    if (selects) {
        selects.forEach((select) => changeCard(select));
    };

    var inputs = document.querySelectorAll("form input");
    if (inputs) {
        inputs.forEach((inputs) => changeCard(inputs));
    };
})

function changeCard(value) {
    var validClass = ["perfil", "nombre", "correo", "telefono", "pais", "ciudad", "departamento"]
    if (validClass.includes(value.id)) {
        value.addEventListener("change", () => {
            var item = document.querySelectorAll(`.card-colab .${value.id}`);
            item[0].innerHTML = value.value;
        })
    }
}