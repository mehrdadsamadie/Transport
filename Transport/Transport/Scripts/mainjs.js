$("#menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("active");
});
//$(function () {
//    $('.sidebar-wrapper .nav li').on('click', function () {
//        $(this).parent().find('li.active').removeClass('active');
//        $(this).addClass('active');
//    });
//});



//$('.sidebar-wrapper .nav li').click(function (e) {
//    e.preventDefault();
//    $('.sidebar-wrapper .nav li').removeClass('active');
//    $(this).addClass('active');

  
//});

//jQuery(document).ready(function ($) {
//    $('.sidebar-wrapper .nav li').find('a[href*="/service/"]').addClass('active');
//    $('.sidebar-wrapper .nav li').find('a[href*="/Driver/"]').addClass('active');
//})
function toDate(dStr, format) {
    var now = new Date();
    if (format == "h:m") {
        now.setHours(dStr.substr(0, dStr.indexOf(":")));
        now.setMinutes(dStr.substr(dStr.indexOf(":") + 1));
        now.setSeconds(0);
        return now;
    } else
        return "Invalid Format";
}