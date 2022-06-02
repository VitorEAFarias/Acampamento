$(document).ready(function(){    
    
    $("#submit").on("submit", function(e){
        e.preventDefault();
        
        var user = $("#user").val();
        var senha = $("#senha").val();  

        console.log(senha)
        if(user != "")
        {
            if(senha != "")
            {           
                var array = {"usuario": user, "senha": senha };        
                $.ajax({
                    type: "post",
                    url: BASE_URL+"Usuarios/login",
                    dataSrc: "",
                    dataType: "json",
                    data: array,
                    success: function(data)
                    {
                        if (data.logged === true)   
                        {
                            swal.fire({                    
                                title: "Sucesso",
                                icon: "success",
                                confirmButtonText: "Ok",
                                html: data.msg,
                            }).then((data) => {
                                window.location.href = BASE_URL+"Dashboard";;
                            })                            
                        }
                        else if(data.logged === false)
                        {      
                            console.log(data)                      
                            swal.fire("Ops!!!", data.error, "warning");
                        }
                    }
                });
            }
            else
            {
                swal.fire("Ops!!!", "Por favor preencha a senha", "warning");
            }
        }
        else
        {
            swal.fire("Ops!!!", "Por favor digite o usuário", "warning");
        }         
    });
});