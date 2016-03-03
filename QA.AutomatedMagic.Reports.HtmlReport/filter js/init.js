$(".btnexp").click(function (e) {
    $(this).closest(".test").children('.tests').toggle(500);
});

$(".btnstep").click(function (e) {
    $(this).closest(".test").children('.steps').toggle(500);
});

$('.btnlog').click(function (e) {
    if ($(this).closest(".step").length > 0)
        $(this).closest(".step").find('.logPanel').first().slideToggle();
    else
        $(this).closest(".test").find('.logPanel').first().slideToggle();
});