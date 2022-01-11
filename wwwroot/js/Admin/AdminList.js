function ModalComfirm(UserId) {
    let userAccountId = "#UserAccount_" + UserId;
    let userAccount = $(userAccountId).text();  
    ModalOptin("show");
    $("#DeleteId").val(UserId);
    $("#Modal_UserComfirm").text("確定要刪除，帳號：" + userAccount + " 嗎?");
}






