function ModalOptin(option) {
    $('.modal').modal(option);
}
function changePage(currentPage, param) {
    window.location = "List?" + "currentPage=" + currentPage + param;
}