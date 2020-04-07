$(document).ready(function () {
    var t = $('#dataTable').DataTable();



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


    // Submit all page in datatables js
    $('#requestDetail').submit(function () {
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

    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();



    // Approval
    $('.btnApproval').on("click", function () {
        var comment = $('#comment').val();
        //var requestId = $(this).data('requestId');
        var requestId = $('#requestId').data('id');
        var type = $(this).data('type');
        var process = $(this).data('process');

        var approvalObject = [
            { Id: requestId, type: type, process: process, comment: comment },
        ];
        approval = JSON.stringify({ 'approval': approvalObject });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            url: '/Approval/Approval',
            data: approval,
            success: function () {
                alert('')
            }
        });
        //new PNotify({
        //    title: 'Successfully',
        //    text: 'Approval successfully',
        //    type: 'success',
        //    delay: '2000',
        //});
        window.location.href = "/Home/Index";
    });


    // Swiper time for SEC confirm
    function addZero(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }

    $(document).on('click', '.btnSwipe', function () {
        var thisRow = $(this).closest('tr');

        thisRow.addClass('table-primary');
        thisRow.find('td:nth-child(9) > input').val(addZero(new Date().getHours()) + ':' + addZero(new Date().getMinutes()));
        // add required attribute
        thisRow.find('td:nth-child(10) > input').attr('required', true);
        thisRow.find('td:nth-child(10) > input').removeAttr("disabled", "disabled");
        //thisRow.find("input,button,textarea,select,span").removeAttr("disabled", "disabled");
    });


    $(document).on('click', '.btnLendCard', function (e) {
        var thisRow = $(this).closest('tr');
        var nextRow = $(this).closest('tr').next();

        // Check employeeId
        var EmployeeId = $(this).closest('tr').find('td:nth-child(7)').text();
        $('#submit').prop('disabled', true);
        $.ajax({
            data: { employeeId: EmployeeId },
            url: '/Visitor/CheckEscorter',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.result != "true") {
                    alert('ERROR, This EmployeeId: ' + res.result + ' not exists in AD system, please recheck!');
                    thisRow.removeClass('table-warning');
                    nextRow.removeClass('table-warning');
                    thisRow.find('td:nth-child(10) > input').attr('required', false);
                    thisRow.find("input,button,textarea,select,span").attr("disabled", "disabled");
                    thisRow.find('td:nth-child(10) > input').val('');
                    nextRow.find("input,button,textarea,select,span").attr("disabled", "disabled");
                    nextRow.find('td:nth-child(10) > input').attr('required', false);
                    nextRow.find('td:nth-child(10) > input').val('');
                }
                $('#submit').prop('disabled', false);
            }
        })

        thisRow.addClass('table-warning');
        thisRow.addClass('table-warning');

        var process = $(this).closest('tr').find('td:nth-last-child(2)').text();
        if (process != " ") {
            nextRow.addClass('table-warning');
            nextRow.find('td:nth-child(9) > input').val('00:00');
            nextRow.find('td:nth-child(10) > input').attr('required', true);
            nextRow.find('td:nth-child(10) > input').removeAttr("disabled", "disabled");
        }
        // reset swiper time
        thisRow.find('td:nth-child(9) > input').val('00:00');
        thisRow.find('td:nth-child(10) > input').attr('required', true);
        thisRow.find('td:nth-child(10) > input').removeAttr("disabled", "disabled");
    });

    $(document).on('click', '.btnClear_Swipe', function () {
        var thisRow = $(this).closest('tr');
        var nextRow = $(this).closest('tr').next();
        // remove swiper
        thisRow.removeClass('table-primary');
        thisRow.find("input,button,textarea,select,span").attr("disabled", "disabled");
        thisRow.find('td:nth-child(9) > input').val('00:00');

        thisRow.find('td:nth-child(10) > input').attr('required', false);
        thisRow.find('td:nth-child(10) > input').val('');

        // remove lend card
        thisRow.removeClass('table-warning');
        nextRow.removeClass('table-warning');
        nextRow.find('td:nth-child(10) > input').attr('required', false);
        nextRow.find('td:nth-child(10) > input').val('');
    });



    // check process 1
    // if null -> disable process 2

    $("#dataTable tr").each(function () {
        if ($(this).find('td:nth-last-child(2)').text() == 1
            && $(this).find('input:nth-child(4)').val() == ''
            && $(this).find('td:nth-child(9) > input').val() == '00:00') {
            // find process 2
            $(this).next().find('td:last-child').css("visibility", "hidden");
        }
    })

    // disable if haven't swipe yet 
    $("#dataTable tr > td:nth-child(10) > input[value='']").closest('tr').find("input,button,textarea,select,span").attr("disabled", "disabled");


    // set time
    $(document).on('click', '.btnSetTime', function () {
        $(this).closest('td').find('input').val(addZero(new Date().getHours()) + ':' + addZero(new Date().getMinutes()));
        $(this).closest('td').find('input').removeClass('bg-danger');
    });

    $(document).on('change', '.setTime', function () {
        var getValue = $(this).val() + ':00';
        var datetime = new Date('1970-01-01T' + getValue + 'Z');
        if (datetime.toString() == 'Invalid Date') {  
            $(this).addClass('bg-danger');
            $(this).val('00:00');
        }
        else {
            $(this).removeClass('bg-danger');
        }
    });

});