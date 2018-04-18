﻿
$(document).ready(function () {
    $('#btn-update-total-qty').on("click", function (e) {
        var token = $('[name=__RequestVerificationToken]').val();
        var data = {
            __RequestVerificationToken: token,
            id: $('#PlateID').val(),
            qty: $('#plate-totalQty').val(),
        };

        $.ajax({
            url: "/StampingPlate/UpdateTotalQty/",
            type: 'POST',
            data: data,
            success: function (response) {
                toast("Total Updated!");
            },
            failure: function (response) {
                toast(response);
            },
            error: function (response) {
                toast(response);
            }
        });
    });
});