//var dataTable;

//$(document).ready(function () {
//    loadDataTable();
//});

//function loadDataTable() {
//    dataTable = $('#tblData').DataTable({
//        "ajax": { url: '/admin/kindness/getall' },
//        "columns": [
//            { "data": "name", "width": "10%" },
//            { "data": "floor", "width": "8%" },
//            { "data": "section", "width": "8%" },
//            { "data": "level", "width": "8%" },
//            { "data": "position", "width": "8%" },
//            { "data": "applicant", "width": "8%" },
//            { "data": "relation", "width": "10%" },
//            { "data": "mobile_Tel", "width": "15%" },
//            {
//                data: 'id',
//                "render": function (data) {
//                    return `<div class="w-75 btn-group" role="group">
//                     <a href="/admin/kindness/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
//                     <a onClick=Delete('/admin/kindness/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
//                    </div>`
//                },
//                "width": "25%"
//            }
//        ]
//    });
//}

//function Delete(url) {
//    Swal.fire({        
//        title: 'Are you sure? 確認刪除?',
//        text: "You won't be able to revert this! 無法回復!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    dataTable.ajax.reload();
//                    toastr.success(data.message);
//                }
//            })
//        }
//    })
//        }
