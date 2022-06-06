$(document).ready(function () {
    console.log("aqui")
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
    
    table = $("#tbUsuarios").DataTable({
        language: {
            url: BASE_URL+"assets/js/plugins/DataTables/Languages/portugues-brasil.json",
            select: { rows: { _: "%d linhas selecionadas", 1: "1 linha selecionada", 0: "" } }
        },
        ajax: {
            url: BASE_URL+"Usuarios/getAllUsers",
            dataSrc: "",
            type: "post",
            dataType: "json"
        },
        order: [[0, "asc"]],       
        paging: true,
        searching: true,
        ordering: true,        
        columns: [ 
            { data: "usuario", title: "Nome do Usuário" },
            { data: "email", title: "E-Mail"},
            { data: "data_cadastro", title: "Data de Cadastro"},
            { data: "data_login", title: "Ultimo Login"}           
        ],
        "columnDefs": [
            { "className": "text-center", "targets": "_all" }        
        ],
    });
});