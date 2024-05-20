document.addEventListener("DOMContentLoaded", function() {
    var inputs = document.querySelectorAll('.alert');

    if (inputs) {
        inputs.forEach(function(input) {
            var label = input.querySelector('label');
            if (label && label.innerText.trim() !== '') {
                input.classList.add('alert-active');
                input.classList.remove('alert');
                
                // Traverse up the DOM tree to find the input label
                var parentDiv = input.parentElement;
                var grandParentDiv = parentDiv.parentElement;
                var inputBoxes = grandParentDiv.querySelectorAll('input');
                console.log(inputBoxes);
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


