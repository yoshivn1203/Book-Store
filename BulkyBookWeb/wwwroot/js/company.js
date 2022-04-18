﻿var dataTable;
$(document).ready(function () {
    loadDataTable();

});

function loadDataTable() {
    dataTable = $('#tblData2').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postalCode", "width": "10%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Company/Edit?id=${data}"
                    class="btn btn-warning btn-sm btn4"> <i class="bi bi-pencil-square"></i> &nbsp; Edit </a>
                     <a onClick=Delete('/Admin/Company/Delete/${data}')
                     class="btn btn-danger btn-sm btn4"> <i class="bi bi-trash3"></i> &nbsp; Delete </a>`
                },
                "width": "16%"
            }

        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}