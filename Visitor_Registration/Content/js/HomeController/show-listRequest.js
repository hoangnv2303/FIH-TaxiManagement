var home = {
    init: function () {
        home.regEvents();
    },
    //Show detail images
    regEvents: function () {
        //Show STT
        //debugger;
        var t = $('#dataTable').DataTable({
            "columnDefs": [{
                "searchable": false,
                "orderable": false,
                "targets": 0
            }],
            dom: '<"row"<"col-md-7"l><"col-md-5"Bf>r>t<"row"<"col-md-6"i><"col-md-6"p>>',
            buttons: [
                {
                    text: 'Using Today',
                    action: function (e, dt, node, config) {
                        if (!node.hasClass("Checked")) {
                            var d = new Date(Date.now()),
                                month = '' + (d.getMonth() + 1),
                                day = '' + d.getDate(),
                                year = d.getFullYear();

                            if (month.length < 2)
                                month = '0' + month;
                            if (day.length < 2)
                                day = '0' + day;
                            var dateNow = [year, month, day].join('/')
                            t.column(5).search(dateNow).draw();
                            node.addClass("Checked");
                            node.css("background", "#c4e3ff");
                        }
                        else {
                            t.column(5).search('/').draw();
                            node.removeClass("Checked");
                            node.css("background", "");
                        }
                    }
                }
            ],
            "order": [[1, 'asc']]
        });
        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

        $('#dataTable tbody').on('click', '.btnDetail', function () {
            var id = $(this).data('id');
            var nationalId = $(this).data('nationalid');
            console.log(id);
            window.location.href = "/request-detail/" + id + "/" + nationalId;
        });

        $('.btn-delete').off('click').on('click', function (e) {
            if (confirm('Do you want delete this user?')) {
                e.preventDefault();
                e.stopPropagation(); //
                var btn = $(this);
                var id = btn.data('id');
                var type = $(this).data('type')
                console.log(id)
                $.ajax({
                    data: { id: id, type: type },
                    url: '/Home/Delete',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        //location.reload();
                        console.log(res);
                        //window.location.href = ;
                        window.open("/Home/Index", "_self");
                    }
                })
            }
        });
    }
}
home.init();