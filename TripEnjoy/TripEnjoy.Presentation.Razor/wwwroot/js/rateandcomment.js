// Hàm để lấy giá trị cookie theo tên
function getCookie(name) {
    let cookieArr = document.cookie.split(";");
    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");
        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]); // Giải mã cookie và trả về giá trị
        }
    }
    return null; // Trả về null nếu không tìm thấy cookie
}

// Đăng ký sự kiện khi form bình luận được gửi
$("#review-form").submit(function (event) {
    event.preventDefault(); // Ngăn chặn hành vi gửi form mặc định

    const rateValue = document.getElementById("rate-value").value; // Lấy giá trị đánh giá
    const commentContent = document.getElementById("comment-content").value; // Lấy nội dung bình luận

    // Kiểm tra xem người dùng đã chọn đánh giá và viết bình luận chưa
    if (rateValue === "0" || commentContent.trim() === "") {
        alert("Please provide a rating and comment before submitting."); // Thông báo nếu không có đánh giá hoặc bình luận
        return; // Ngăn không cho gửi form
    }

    // Tạo đối tượng DTO cho yêu cầu
    var rateAndCommentDTO = {
        RoomId: $('#review-form input[name="RateAndComment.RoomId"]').val(),
        RateValue: $('#rate-value').val(),
        CommentContent: $('#comment-content').val(),
        ReviewDate: new Date().toISOString() // Thời gian hiện tại
    };

    var accessToken = getCookie('accessToken'); // Lấy access token từ cookie
    if (accessToken == null) {
        alert("You must login to rate and comment!"); // Thông báo lỗi nếu chưa đăng nhập
        return; // Ngăn không cho gửi form
    }

    // Gửi yêu cầu AJAX
    $.ajax({
        url: "https://localhost:7126/api/Rate/RateAndComment", // Đường dẫn đến API
        type: "POST", // Phương thức POST
        contentType: "application/json", // Kiểu nội dung là JSON
        data: JSON.stringify(rateAndCommentDTO), // Chuyển đổi đối tượng thành JSON
        xhrFields: {
            withCredentials: true // Gửi cookie cùng yêu cầu
        },
        headers: {
            'Authorization': 'Bearer ' + accessToken // Thêm token vào header
        },
        success: function (response) {
            alert("Thanh Cong"); // Thông báo thành công
            addNewCommentToList(response); // Cập nhật danh sách bình luận
        },
        error: function (xhr, status, error) {
            console.error("Error:", error); // Ghi lại lỗi
            console.log("Response Text:", xhr.responseText); // Ghi lại phản hồi từ server
            alert("Error when sending rate and comment: " + xhr.responseText); // Thông báo lỗi
        }
    });
});

// Hàm thêm bình luận mới vào danh sách bình luận
function addNewCommentToList(comment) {
    const reviewSection = document.getElementById('review-section'); // Lấy phần tử nơi sẽ hiển thị bình luận
    const newCommentHTML = `
        <div class="d-flex mb-4">
            <img src="${comment.AccountImage}" class="img-fluid rounded" style="width: 45px; height: 45px;">
            <div class="ps-3">
                <h6>${comment.AccountName} <small class="text-body fw-normal fst-italic">${new Date(comment.CommentDate).toLocaleDateString('vi-VN')}</small></h6>
                <div class="rating mb-2">
                    ${'<small class="fa fa-star text-warning"></small>'.repeat(comment.RateValue)}
                    ${'<small class="fa fa-star text-muted"></small>'.repeat(5 - comment.RateValue)}
                </div>
                <p class="mb-2">${comment.CommentContent}</p>
            </div>
        </div>`;
    reviewSection.insertAdjacentHTML('afterbegin', newCommentHTML); // Thêm bình luận mới vào đầu danh sách
}
  