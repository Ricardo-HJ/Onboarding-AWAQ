document.addEventListener("DOMContentLoaded", function() {
    var inputs = document.querySelectorAll('.alert');

    if (inputs) {
        inputs.forEach(function (input) {
            var label = input.querySelector('label, span');
            if (label && label.innerText.trim() !== '') {
                input.classList.add('alert-active');
                input.classList.remove('alert');
                
                // Traverse up the DOM tree to find the input label
                var parentDiv = input.parentElement;
                var grandParentDiv = parentDiv.parentElement;
                var inputBoxes = grandParentDiv.querySelectorAll('input, select, .input-file');
                // Change the class of the input label
                if (inputBoxes) {
                    inputBoxes.forEach(function(inputBox) {
                        inputBox.classList.add('box-alert');
                    });
                }
            }
        });
    }
});

function changeClass(inputs) {
    inputs.forEach(function (input) {
        input.classList.add('box-alert');
    });
}


