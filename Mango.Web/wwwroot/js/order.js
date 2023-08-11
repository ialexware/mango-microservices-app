var datatTable = null;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    } else {
        if (url.includes("readyforpickup")) {
            loadDataTable("readyforpickup");
        } else {
            if (url.includes("cancelled")) {
                loadDataTable("cancelled");
            } else {
                loadDataTable("");
            }
        }
    }
});
function loadDataTable(status) {
    datatTable = $('#tblData').DataTable({
        "order": [[0, 'desc']],
        "ajax": {
            "url": "/order/GetAll?status=" + status,
        },
        "columns": [
            { "data": "orderHeaderId", "width": "5%" },
            { "data": "email", "width": "25%" },
            { "data": "firstName", "width": "25%" },
            { "data": "lastName", "width": "25%" },
            { "data": "phone", "width": "25%" },
            { "data": "status", "width": "25%" },
            { "data": "orderTotal", "width": "25%" },
            {
                data: "orderHeaderId",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/order/orderDetail?orderId=${data}" class="btn btn-primary mx-2" style="cursor:pointer">
                            <i class="fi bi-pencil-square"></i>
                        </a>
                    </div>`
                },
                "width": "10%"
            }
        ]
    });
}