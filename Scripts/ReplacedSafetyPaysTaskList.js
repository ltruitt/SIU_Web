function DontValidate() {
    $("#aspnetForm").validate().cancelSubmit = true;
}


$(document).ready(function () {

    ///////////////////////////////////////////////////////////////////
    // Expand / Contract Task Definition Field On Focus Enter / Exit //
    ///////////////////////////////////////////////////////////////////
    var refTaskDefinition = $("#TaskDefinition");

    refTaskDefinition.blur(function () {
        refTaskDefinition.height(18);
    });

    refTaskDefinition.focus(function () {
        refTaskDefinition.height(200);
    });

    //var validator = $("#aspnetForm").validate({
    //    wrapper: 'none',
    //    errorClass: "alertmsg",
    //    validClass: "ValidationSuccess",
    //    onsubmit: true,
    //    xonfocusout: true,
    //    focusInvalid: false,
    //    highlight: function (element, errorClass) {
    //        $(element).removeClass("DataInputCss").addClass("ValidationError");
    //    },

    //    unhighlight: function (element, errorClass) {
    //        $(element).removeClass("ValidationError").addClass("DataInputCss");
    //    }
    //});


    //$("#txtDueDate").rules('add', { required: true, messages: { required: 'Task Due Date Is Required'} });
    //$("#txtDueDate").rules('add', { minlength: 8, messages: { required: 'Task Due Date Format is MM/DD/YYYY'} });
    //$("#TaskDefinition").rules('add', { required: true, messages: { required: 'Task Definition Is Required'} });
    //$("#txtAssignedName").rules('add', { required: true, messages: { required: 'Assign The Task To Someone'} });

});