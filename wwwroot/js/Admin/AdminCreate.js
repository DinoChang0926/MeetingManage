$(document).ready(function () {   
    $.ajax({
        url: "GetRole",
        method: "Get",
        contentType: false,
        processData: false,      
        dataType: "json",
      /*  data: formData,*/
        cache: false,
        async: true,
        success: function (result) {  
            $.each(result, function (i, item) {                 
                $('#RoleSelect').append($('<option>', {
                    value: item.roleValue,
                    text: item.roleName
                }));
            });         
        }
    });
});

function createSelect() {

}
