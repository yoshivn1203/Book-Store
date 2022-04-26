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
                "data": { id: "id", lockoutEnd: "lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today)
                    {
                        return `
                    <a onClick=LockUnlock('${data.id}')
                     class="btn btn-danger btn-sm btn4" style="width:90px;"> <i class="fas fa-lock-open"></i> &nbsp; Unlock </a>
                    <a href="/Admin/User/ChangeRole?id=${data.id}"
                    class="btn btn-warning btn-sm btn4" style="width:90px;"> <i class="bi bi-pencil-square"></i> &nbsp; Edit </a>
                     <a onClick=Delete('/Admin/User/Delete/${data.id}')
                     class="btn btn-danger btn-sm btn4" style="width:90px;"> <i class="bi bi-trash3"></i> &nbsp; Delete </a>`
                    }
                    else
                    {
                        return `
                    <a onClick=LockUnlock('${data.id}')
                     class="btn btn-success btn-sm btn4" style="width:90px;"> <i class="fas fa-lock"></i> &nbsp; Lock </a>
                    <a href="/Admin/User/ChangeRole?id=${data.id}"
                    class="btn btn-warning btn-sm btn4" style="width:90px;"> <i class="bi bi-pencil-square"></i> &nbsp; Edit </a>
                     <a onClick=Delete('/Admin/User/Delete/${data.id}')
                     class="btn btn-danger btn-sm btn4" style="width:90px;"> <i class="bi bi-trash3"></i> &nbsp; Delete </a>`
                    }
                    //return `
                    //<a href="/Admin/User/ChangeRole?id=${data.id}"
                    //class="btn btn-warning btn-sm btn4"> <i class="bi bi-pencil-square"></i> &nbsp; Edit </a>
                    // <a onClick=Delete('/Admin/User/Delete/${data.id}')
                    // class="btn btn-danger btn-sm btn4"> <i class="bi bi-trash3"></i> &nbsp; Delete </a>`
                },
                "width": "20%"
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

function LockUnlock(id) {
 
            $.ajax({
                url: '/Admin/User/LockUnlock',
                type: 'POST',
                data: JSON.stringify(id),
                contentType: "application/json",
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




