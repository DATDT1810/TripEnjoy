// Hàm để lấy giá trị cookie theo tên
function getCookie(name) {
    let cookieArr = document.cookie.split(";");
    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");
        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]); 
        }
    }
    return null; 
}

// Đăng ký sự kiện khi form bình luận được gửi
$("#review-form").submit(function (event) {
    event.preventDefault();

    const rateValue = document.getElementById("rate-value").value;
    const commentContent = document.getElementById("comment-content").value;

    if (rateValue === "0" || commentContent.trim() === "") {
        Swal.fire({
            icon: 'warning',
            title: 'Incomplete Form',
            text: 'Please provide a rating and comment before submitting.',
            confirmButtonText: 'OK'
        });
        return;
    }

    var rateAndCommentDTO = {
        RoomId: $('#review-form input[name="RateAndComment.RoomId"]').val(),
        RateValue: rateValue,
        CommentContent: commentContent,
        ReviewDate: new Date().toISOString()
    };

    var accessToken = getCookie('accessToken');
    if (accessToken == null) {
        Swal.fire({
            icon: 'info',
            title: 'Login Required',
            text: 'You must login to rate and comment!',
            confirmButtonText: 'OK'
        });
        return;
    }

    $.ajax({
        url: "https://localhost:7126/api/Rate/RateAndComment",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(rateAndCommentDTO),
        headers: {
            'Authorization': 'Bearer ' + accessToken
        },
        success: function (response) {
            Swal.fire({
                icon: 'success',
                title: 'Review Submitted',
                text: 'Your review has been posted successfully!',
                confirmButtonText: 'OK'
            });

            const commentData = {
                AccountImage: response.Comment?.AccountImage || "default-image-url",
                AccountName: response.Comment?.FullName || "Anonymous",
                CommentDate: response.Comment?.CommentDate || new Date().toISOString(),
                RateValue: response.Rate?.RateValue || rateValue,
                CommentContent: response.Comment?.CommentContent || commentContent,
            };

            addNewCommentToList(commentData);
        },
        error: function (xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: `An error occurred: ${xhr.responseText || 'Please try again later.'}`,
                confirmButtonText: 'OK'
            });
        }
    });
});


// Hàm thêm bình luận mới vào danh sách bình luận
function addNewCommentToList(comment) {
    const reviewSection = document.getElementById('review-section');
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
    reviewSection.insertAdjacentHTML('beforeend', newCommentHTML); 
    //afterbegin
}


////// Reply
function getCookie(name) {
    let cookieArr = document.cookie.split(";");
    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");
        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]);
        }
    }
    return null;
}


// Đăng ký sự kiện cho nút submit của form reply
$(".reply-form").submit(function (event) {
    event.preventDefault(); // Ngăn không cho form gửi lại

    const form = $(this);
    const roomId = form.find('input[name="CommentReply.RoomId"]').val();
    const replyToCommentId = form.find('input[name="CommentReply.ReplyToComment"]').val();
    const replyContent = form.find('textarea[name="CommentReply.Content"]').val().trim();

    if (!replyContent) {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: 'Reply content cannot be empty!',
            confirmButtonText: 'OK'
        });
        return;
    }

    var accessToken = getCookie('accessToken');
    if (accessToken == null) {
        Swal.fire({
            icon: 'info',
            title: 'Login Required',
            text: 'You must login to reply to comments!',
            confirmButtonText: 'OK'
        });
        return;
    }

    var replyDTO = {
        RoomId: roomId,
        Content: replyContent,
        ReplyToComment: replyToCommentId
    };

    $.ajax({
        url: 'https://localhost:7126/api/Comment/Reply',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(replyDTO),
        headers: {
            'Authorization': 'Bearer ' + accessToken
        },
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Reply posted successfully!',
                    confirmButtonText: 'OK'
                }).then(() => {
                    addNewReplyToList(response.data);
                    form[0].reset();
                    form.hide();
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.message || 'Failed to post reply.',
                    confirmButtonText: 'OK'
                });
            }
        },
        error: function (xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: `An error occurred: ${xhr.responseText || 'Please try again later.'}`,
                confirmButtonText: 'OK'
            });
        }
    });
});

// Hàm để thêm phản hồi mới vào danh sách bình luận
function addNewReplyToList(reply) {
    const replySection = document.querySelector(`.cmt-replies[data-comment-id="${reply.ReplyToComment}"]`);
    const newReplyHTML = `
                <div class="d-flex mt-3">
                    <img src="${reply.AccountImage || 'default-image-url'}" class="img-fluid rounded" style="width: 35px; height: 35px;">
                    <div class="ps-3">
                        <h6>${reply.AccountName || 'Anonymous'} <small class="text-body fw-normal fst-italic">${new Date(reply.CommentDate).toLocaleDateString('vi-VN')}</small></h6>
                        <p class="mb-2">${reply.Content}</p>
                    </div>
                </div>`;
    if (replySection) {
        replySection.insertAdjacentHTML('beforeend', newReplyHTML); 
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Unable to find the reply section to update.',
            confirmButtonText: 'OK'
        });
    }
}
