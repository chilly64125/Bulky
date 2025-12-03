var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/customer/eventregistration/getall' },
        "columns": [
            { data: 'product.title', "width": "10%" },
            { data: 'product.hDate', "width": "5%" },
            { data: 'registrationTime', "width": "5%" },
            //{ data: 'product.listPrice', "width": "5%" },
            //{ data: 'count', "width": "5%" },
            //{ data: 'table', "width": "5%" },
            //{ data: 'vegetarian', "width": "5%" },
            //{ data: 'senior80', "width": "5%" },
            //{ data: 'volunteer', "width": "5%" },
            //{ data: 'preAdult', "width": "5%" },
            //{ data: 'totalNumberJoined', "width": "5%" },
            //{ data: 'paymentAmount', "width": "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group fw-bolder" role="group">
                     <a href="/customer/eventregistration/upsert?id=${data}" class="btn btn-primary mx-2 fw-bolder"> <i class="bi bi-pencil-square"></i> Edit</a>
                     <a onClick=Delete('/customer/eventregistration/delete/${data}') class="btn btn-danger mx-2 fw-bolder"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "10%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: '確認刪除?',
        text: "你將無法還原!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '是的Yes, 刪除delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
$.fn.dataTable.ext.errMode = 'throw';