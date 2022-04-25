var dataTable;
$(document).ready(function () {
    loadDataTable();

});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "userName", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/User/ChangeRole?id=${data}"
                    class="btn btn-warning btn-sm btn4"> <i class="bi bi-pencil-square"></i> &nbsp; Edit </a>
                     <a onClick=Delete('/Admin/User/Delete/${data}')
                     class="btn btn-danger btn-sm btn4"> <i class="bi bi-trash3"></i> &nbsp; Delete </a>`
                },
                "width": "15%"
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
