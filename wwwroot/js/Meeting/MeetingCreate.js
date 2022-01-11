$(document).ready(function () {   
    $.ajax({
        url: "GetRoom",
        method: "Get",
        contentType: false,
        processData: false,      
        dataType: "json",
      /*  data: formData,*/
        cache: false,
        async: true,
        success: function (result) {  
            $.each(result, function (i, item) {                 
                $('#RoomSelect').append($('<option>', {
                    value: item.value,
                    text: item.description
                }));
            });         
        }
    });
});

function createSelect() {

}
