$(document).ready(function () {
    $("#selector").flatpickr();
    table = $("#tbModalidades").DataTable({
        language: {
            url: BASE_URL+"assets/js/plugins/DataTables/Languages/portugues-brasil.json",
            select: { rows: { _: "%d linhas selecionadas", 1: "1 linha selecionada", 0: "" } }
        },
        ajax: {
            url: BASE_URL+"Modalidades/getModalidade",
            dataSrc: "",
            type: "post",
            dataType: "json"
        },
        order: [[0, "asc"]],       
        paging: true,
        searching: true,
        ordering: true,        
        columns: [ 
            { data: "nome", title: "Nome da modalidade" },
            { data: "data", title: "Ocorre em"},
            { data: "descricao", title: "Descrição"}           
        ],
        "columnDefs": [
            // { "width": "5%", "targets": 0},
            // { "width": "15%", "targets": 6},
            // { "width": "15%", "targets": 5},
            // { "width": "15%", "targets": 1},
            // { "width": "10%", "targets": 2},
            // { "width": "14%", "targets": 3},
            // { "width": "10%", "targets": 7},
            // { "width": "7%", "className": "text-left", "targets": 4},
            { "className": "text-center", "targets": "_all" } 
            // {
            //     "data": null,
            //     "defaultContent": '',
            //     "orderable": false,
            //     "className": 'select-checkbox', targets: 0
            // }         
        ],
    }); 
     
    $("#frmModalidade").submit( function(e){
        e.preventDefault();    
        var data = $(this).serialize();
        data = new FormData($("#frmModalidade").get(0));
        
        $.ajax({
            type: "POST",
            url: BASE_URL +'Modalidades/cadastraModalidade',
            cache: false,
            contentType: false,
            processData: false,
            dataType: "json",
            data: data,
            success: function (rst){
                $('.page-loader-wrapper').fadeOut();
                if(rst.rst === true)
                {
                    swal.fire({
                        title: "Sucesso",
                        icon: "success",
                        confirmButtonText: "Ok",
                        html: rst.msg,
                    }).then((data) => {
                        $("#modalModalidade").modal("hide");
                        window.location.reload();
                    });
                }
                else
                {
                    swal("Erro",rst.msg, "warning");                                                   
                }
            }
        });
    });

    $("#modalModalidade").on("hidden.bs.modal", function(){
        
        $("#nomeModalidade").val("");
        $("#dataModalidade").val("");
        $("#descricaoModalidade").val("");
    });    
});