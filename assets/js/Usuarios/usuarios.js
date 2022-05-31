$(document).ready(function () {
    $("#frmUsuario").submit( function(e){
        e.preventDefault();    
        var data = $(this).serialize();
        data = new FormData($("#frmUsuario").get(0));
        
        $.ajax({
            type: "POST",
            url: BASE_URL +'Usuarios/criarCadastro',
            cache: false,
            contentType: false,
            processData: false,
            dataType: "json",
            data: data,
            success: function (rst){
                if(rst.rst === false)
                {
                    swal("Erro!", rst.msg, "warning");
                }
                else
                {
                    swal.fire({                    
                        title: "Sucesso",
                        icon: "success",
                        confirmButtonText: "Ok",
                        html: rst.msg,
                    }).then((data) => {
                        window.location.href = BASE_URL+"Usuarios/index/"+rst.id;
                    })                          
                }
            }
        });
    });
});