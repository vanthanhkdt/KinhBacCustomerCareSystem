(function ($) {
    $.normal = function () {
    };

    $.fn.sAlert = function (callback) {
        swal({
            title: "Bạn có chắc chắn muốn thêm Tag này không?",
            text: "Tag mới sẽ được thêm vào hệ thống này!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có, thêm ngay!",
            cancelButtonText: "Không, hủy bỏ!",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    callback();
                    //toastr.success("Đã thêm thành công", "Thành công");
                    //swal("Thành công!", "Đã thêm thành công.", "success");
                } else {
                    swal("Đã hủy", "Không có Tag mới nào được thêm.", "error");
                }
            });
    }
}($));