//===================================================================================================================

/*const { each } = require("jquery");*/

//===================================================================================================================
$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() >= 45) {
            $('#main_nav').addClass('fixed-top');
        } else {
            $('#main_nav').removeClass('fixed-top');
        }
    });
});

//===================================================================================================================
//===================================================================================================================
function close_msg(obj) {
    $(obj).parent().hide();
}

//===================================================================================================================
//===================================================================================================================
function showAlert(title, message, mode) {
    $('#messageTitle').text(title);
    $('#messageBody').html(message);

    if (mode.toLowerCase() === 'error') {
        document.getElementById("modalHeader").style.backgroundColor = '#f57e7e';
        document.getElementById("modalHeader").style.color = '#ffffff';
        document.getElementById("okbutton").className = 'btn btn-danger';
    }
    else if (mode.toLowerCase() === 'success') {
        document.getElementById("modalHeader").style.backgroundColor = '#00ff21';
        document.getElementById("modalHeader").style.color = '#ffffff';
        document.getElementById("okbutton").className = 'btn btn-success';
    }
    else if (mode.toLowerCase() === 'warning') {
        document.getElementById("modalHeader").style.backgroundColor = '#ffd800';
        document.getElementById("modalHeader").style.color = '#010101';
        document.getElementById("okbutton").className = 'btn btn-warning';
    }
    else if (mode.toLowerCase() === 'info') {
        document.getElementById("modalHeader").style.backgroundColor = '#0094ff';
        document.getElementById("modalHeader").style.color = '#ffffff';
        document.getElementById("okbutton").className = 'btn btn-primary';
    }
    else {
        document.getElementById("modalHeader").style.backgroundColor = '#a8caee';
        document.getElementById("modalHeader").style.color = '#010101';
        document.getElementById("okbutton").className = 'btn btn-primary';
    }

    $('#myModal').modal('show');
};

//===================================================================================================================
//===================================================================================================================
$(document).ready(function () {
    const notyf = new Notyf({
        position: {
            x: "left",
            y: "top",
        },
    });

    var wmsg = $("#notify_message_wellcome").val();
    if (wmsg != null && wmsg != undefined && wmsg.length > 0) {
        notyf.success(wmsg);
        $("#notify_message_wellcome").val('');
    }
    var smsg = $("#notify_message_success").val();
    if (smsg != null && smsg != undefined && smsg.length > 0) {
        notyf.success(smsg);
        $("#notify_message_success").val('');
    }

    var emsg = $("#notify_message_error").val();
    if (emsg != null && emsg != undefined && emsg.length > 0) {
        notyf.error(emsg);
        $("#notify_message_error").val('');
    }

});

//===================================================================================================================
//===================================================================================================================
function fnBuyProduct(btn) {
   
    var packageID = $(btn).data('id');
    var productID = $(btn).data('pid');
    var packageCount = $(btn).data('packagecount');
    if (packageCount > 1) {
        
        fnShowBuyPackagePartial(productID, 'buyModalContent');
        $('#buyProductModal').modal("show");
    }
    else {
        fnAddToCart(btn, "", packageID);
        //$(btn).text("لغو خرید");
    }
}

//===================================================================================================================
//===================================================================================================================


//===================================================================================================================
//===================================================================================================================
function fnShowBuyPackagePartial(pid, containerId) {
    var obj = new Object();
    obj.productID = pid;
    $.ajax({
        url: "/Home/GetProductDetail",
        type: 'POST',
        contentType:
            "application/json;charset=utf-8",
        data: JSON.stringify(obj),
        success: function (data) {

            if (data != null && data != "") {
                $('#' + containerId).html(data);
            }
        },
        error: function () {
        }
    });
}

//===================================================================================================================
//===================================================================================================================
function fnResendSMS(guid) {
    var obj = $('#resend_sms_button');
    if (obj.hasClass('disabled'))
        return;
    else {
        $.ajax({
            url: "/Account/ResendAuthCodeSMS",
            type: 'POST',
            contentType:
                "application/json;charset=utf-8",
            data: JSON.stringify({ 'guid': guid }),
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data.Status === 'Success') {
                    $('#message_div').empty();
                    $('#message_div').append('<div class="success_message">' + data.Message + '</div>');
                    $('#resend_sms_button').addClass('disabled');
                    AllSeconds = 120;
                }
                else if (data.Status === 'Error') {
                    $('#message_div').empty();
                    $('#message_div').append('<div class="error_message">' + data.Message + '</div>');
                }
            },
            error: function (er) {
                $('#message_div').empty();
                $('#message_div').append('<div class="error_message">' + 'ارسال پیامک با خطا مواجه شد' + '</div>');
            }
        });
    }
};

//===================================================================================================================
//===================================================================================================================
function fnAddToCart(obj, sumElement, productID) {
    var PackageID = $(obj).data("id");
    var PartIndex = $(obj).data("partid");
    if ($(obj).hasClass('button_disable')) {
        showAlert('خطا', 'برای خرید این پکیج، ابتدا باید پکیج پایه را خریداری نمایید', 'Error');
        return;
    }
    $.ajax({
        url: "/ShopCart/AddToCart/" + PackageID,
        type: "Get"
    }).done(function (result) {
        if (result === -1) {
            //showAlert('خطا')
            return;
        }
        fnShowSumCartPrice(sumElement, productID);

        if (sumElement != null && sumElement != undefined && sumElement != "") {
            $(obj).addClass('display_none');

            if (PartIndex == 0) {
                $('.data_row .cart_td_a_button[data-action="add"]:not([data-partid="0"])').each(function () {
                    $(this).removeClass('display_none').removeClass('button_disable');
                });
            }
            else {
                $('.data_row .cart_td_a_button[data-action="remove"][data-partid="0"]').each(function () {
                    $(this).addClass('button_disable');
                });
            }
            $('#remove_from_cart_' + PackageID).removeClass('display_none').removeClass('button_disable');
        }
        else {
            $(obj).addClass('display_none');
            $('#remove_cart_' + PackageID).removeClass('display_none');
        }
    });
}

//===================================================================================================================
//===================================================================================================================
function fnRemoveFromCart(obj, sumElement, productID) {
    var PackageID = $(obj).data("id");
    var PartIndex = $(obj).data("partid");
    if ($(obj).hasClass('button_disable')) {
        showAlert('خطا', 'برای لغو این پکیج، ابتدا باید سایر پکیج های این محصول را لغو نمایید', 'Error');
        return;
    }
    $.ajax({
        url: "/ShopCart/RemoveFromCart/" + PackageID,
        type: "Get"
    }).done(function (result) {
        if (result === -1) {
            //showAlert('خطا')
            return;
        }

        $.ajax({
            url: "/ShopCart/ShopCartCount",
            type: "POST"
        }).done(function (result) {


            fnShowSumCartPrice(sumElement, productID);
            if (sumElement != null && sumElement != undefined && sumElement != "") {
                $(obj).addClass('display_none');
                $('#add_to_cart_' + PackageID).removeClass('display_none').removeClass('button_disable');
                if (PartIndex == 0) {
                    $('.data_row .cart_td_a_button[data-action="add"]:not([data-partid="0"])').each(function () {
                        $(this).removeClass('display_none').addClass('button_disable');
                    });
                }
                else {
                    var HasOtherPackage = false;

                    $('.data_row .cart_td_a_button[data-action="remove"]:not([data-partid="0"])').each(function () {
                        if ($(this).hasClass('display_none') == false) {
                            HasOtherPackage = true;
                        }
                    });
                    if (HasOtherPackage == false) {
                        $('.data_row .cart_td_a_button[data-action="remove"][data-partid="0"]').each(function () {
                            $(this).removeClass('button_disable');
                        });
                    }
                }
            }
            else {
                $(obj).addClass('display_none');
                $('#add_cart_' + PackageID).removeClass('display_none');
            }
        });
    });
}

//===================================================================================================================
//===================================================================================================================
function fnShowSumCartPrice(sumElement, productID) {
    $.ajax({
        url: "/ShopCart/ShopCartCount",
        type: "POST",
        contentType:
            "application/json;charset=utf-8",
        data: JSON.stringify({ 'productID': productID }),
    }).done(function (result) {
        $("#shop_cart_count").html(result.Count);
        if (sumElement != null && sumElement != "") {
            $("#" + sumElement).text(result.SumPriceText);
        }
        if (result.Count > 0) {
            $('#shopcarticon').removeClass('ti-shopping-cart');
            $('#shopcarticon').addClass('ti-shopping-cart-full');
        }
        else {
            $('#shopcarticon').addClass('ti-shopping-cart');
            $('#shopcarticon').removeClass('ti-shopping-cart-full');
        }
    });
}

//===================================================================================================================
//===================================================================================================================
function fnCollapseNextTR(btn) {
    var id = $(btn).data('id');
    var tr = $('#feature_tr_' + id);
    tr.toggleClass('display_none');
    $('#btn_' + id).toggleClass('fa-chevron-circle-down');
    $('#btn_' + id).toggleClass('fa');
    $('#btn_' + id).toggleClass('ti-arrow-circle-up');
}

//===================================================================================================================
//===================================================================================================================
