$(document).ready(function () {
    table = $("#tbModalidades").DataTable({
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
            // { "className": "text-center", "targets": "_all" },  
            // {
            //     "data": null,
            //     "defaultContent": '',
            //     "orderable": false,
            //     "className": 'select-checkbox', targets: 0
            // }         
        ],
    });    
});