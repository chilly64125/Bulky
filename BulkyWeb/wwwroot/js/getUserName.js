
$(document).ready(function () {
    getUserName();
});


function getUserName() {
   
    var nameElement = document.getElementById('manage');
    var name = nameElement.innerText;

    $.ajax({
        url: "/customer/eventregistration/getUserName",
        data: {
            user_name: name
              },
    success: function(result) {     
        //$('#output').html("<b>UserName: </b>" + user_name + "<b> Result: </b>" + Result);
        //alert('user_name:' + name + "Result:" + result);
    }  
      });
           

}