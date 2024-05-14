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
    var validClass = ["nombre", "correo", "telefono", "pais", "ciudad", "departamento"];
    if (validClass.includes(value.id)) {
        value.addEventListener("change", () => {
            var item = document.querySelector(`.card-colab .${value.id}`);
            item.innerHTML = value.value
        })
    } else if (value.id === "file-input") {
        value.addEventListener("change", () => {
            var item = document.querySelector(`.card-colab .perfil`);
            if (value.files["0"]["type"].split("/")[0] !== "image") {
                var label = document.createElement("label");
                label.innerHTML = "Formato de archivo incorrecto ingrese una imagen";
                value.parentElement.appendChild(label);
            } else {
                var child = value.parentElement.children[2];
                if (child) { child.remove(); }
                const image = value.files[0];
                const fileURL = URL.createObjectURL(image);
                item.src = fileURL;
            }
        })
    }
}