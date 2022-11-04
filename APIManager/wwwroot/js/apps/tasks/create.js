$(document).ready(function () {
    $('.form-task').parsley()
        .on('form:validate', function (formInstance) {
            var ok = formInstance.isValid();

            if (!ok)
                formInstance.validationResult = false;
        });
});