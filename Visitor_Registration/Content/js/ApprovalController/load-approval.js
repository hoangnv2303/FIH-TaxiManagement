var home = {
    init: function () {
        home.regEvents();
    },
    //Show detail images
    regEvents: function () {
        // Prevent submit without update information
        $('form')
            .each(function () {
                $(this).data('serialized', $(this).serialize())
            })
            .on('change input', function () {
                $(this)
                    .find('input:submit, button:submit')
                    .attr('disabled', $(this).serialize() == $(this).data('serialized'))
                    ;
            })
            .find('input:submit, button:submit')
            .attr('disabled', true)
            ;

        //Show STT
        var t = $('#dataTable').DataTable({
            "columnDefs": [{
                "searchable": false,
                "orderable": false,
                "targets": 0
            }],
            "order": [[1, 'asc']]
        });
        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
            //t.column(10, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //    //cell.innerHTML = i + 1;
            //    console.log(cell.val);
            ////});
        }).draw();

        $('#dataTable tbody').on('click', '.btnDetail', function () {
            var id = $(this).data('id');
            console.log(id);
        });

        // radio button
        $('#dataTable tbody').on('click', '.event-radio', function () {
            var id = $(this).data('id');
            var type = $(this).data('type')
            var input = $('#' + id);
            var comment = $('#comment' + id);

            console.log(id);
            if (type != null) {
                input.val(-1);
                comment.prop('required', true);
                comment.focus();
            }
            else {
                input.val(1);
                comment.prop('required', false);
                //var hoang = "@Html.Hidden(item.Id.ToString())";
                //input.append('<input type="hidden" name="approval[0].Id" value=1/>');
                //$('#addType').html(hoang);
            }
        });

        $('#dataTable tbody').on('click', '.btn-delete', function (e) {
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

        // Submit all page in datatables js
        $('#approval').submit(function () {
            $("#divLoader").show();
            t.search('').draw();

            var form = this;
            // Encode a set of form elements from all pages as an array of names and values
            var params = t.$('input,select,textarea').serializeArray();
            // Iterate over all form elements
            $.each(params, function () {
                // If element doesn't exist in DOM
                if (!$.contains(document, form[this.name])) {
                    // Create a hidden element
                    $(form).append(
                        $('<input>')
                            .attr('type', 'hidden')
                            .attr('name', this.name)
                            .val(this.value)
                    );
                }
            });
            return true;
        });

        $(".btnReset").on('click', function (e) {
            $('.typeApproval').val('');
            $("#approval").trigger('reset');
            $('#approval').find('input:submit').attr('disabled', true);
            $('tr td textarea').prop('required', false);
        });
    }
}
home.init();