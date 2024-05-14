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
    var validClass = ["nombre", "correo", "telefono", "pais", "ciudad", "departamento"]
    if (validClass.includes(value.id)) {
        value.addEventListener("change", () => {
            var item = document.querySelectorAll(`.card-colab .${value.id}`);
            item[0].innerHTML = value.value
        })
    } else if (value.id === "perfil") {
        value.addEventListener("change", () => {
            var item = document.querySelectorAll(`.card-colab .${value.id}`);
            if (value.Files[0]["type"].split("/")[0] !== "image") {
                //
            }
            item[0].src = value.value;
        })
    }
}