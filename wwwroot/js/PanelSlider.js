//$(document).on('click', '.panel-heading span.clickable', function (e) {
//    console.log("panel head");
//    var $this = $(this);
//    if (!$this.hasClass('panel-collapsed')) {//if not hidden
//        $this.parents('.panel').find('.panel-body').slideUp();
//        $this.addClass('panel-collapsed');
//        $this.find('i').removeClass('glyphicon-minus').addClass('glyphicon-plus');
//    } else {//if hidden
//        $this.parents('.panel').find('.panel-body').slideDown();
//        $this.removeClass('panel-collapsed');
//        $this.find('i').removeClass('glyphicon-plus').addClass('glyphicon-minus');
//    }
//});
$(document).on('click', '.panel div.clickable', function (e) {

    console.log("panel Div");
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-minus').addClass('glyphicon-plus');
    } else {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-plus').addClass('glyphicon-minus');
    }
});

$(document).on('click', '#replyPanel', function (e) {

    //console.log("In the replyPanel");
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {// if not hidden
        console.log("Not Hidden:" + $this.find('i'));
        $this.parents('#replyHead').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-minus').addClass('glyphicon-plus');
    } else {//if hidden
        $this.parents('#replyHead').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        console.log("Hidden:" + $this.find('i'));
        $this.find('i').removeClass('glyphicon-plus').addClass('glyphicon-minus');
    }
});

//var alive = false;
//$(document).on('click', '#slide_box', function (e) {
//    console.log("In the event");
//    var $this = $(this);

//    if (alive == false) {
//        $this.delay(400).show(1200,"swing");
//        alive = true;
//    }
//    else {
//        $this.hide("slide", { direction: "left" }, 1200);
//        alive = false
//    }
    

//});

$(document).on('click', '#slide_box', function (e) {
    var $this = $(this);

    var arrow = document.getElementById("slArrow");

    if (arrow.className == "glyphicon glyphicon-chevron-right") {
        var s_Box = document.getElementById("slide_box");
        var i_Box = document.getElementById("container");


        s_Box.style.transitionDuration = "2s";
        s_Box.style.transform = "translateX(300px)";

        i_Box.style.transform = "translateX(300px)";

        arrow.className = "glyphicon glyphicon-chevron-left"
    }
    else {
        var s_Box = document.getElementById("slide_box");
        var i_Box = document.getElementById("container");


        s_Box.style.transitionDuration = "2s";
        s_Box.style.transform = "translateX(1px)";

        i_Box.style.transform = "translateX(-300px)";
        i_Box.style.transitionDuration = "2s";
        arrow.className = "glyphicon glyphicon-chevron-right"
    }
   
}
    );

$(document).on('click', '#sRight', function (e) {
   

    console.log("In the Right method ");
    var r_Box = document.getElementById("rContainer");
    var pos = $(r_Box).scrollLeft();

    $(r_Box).animate({ scrollLeft: pos + 100 },400)
});

$(document).on('click', "#sLeft", function (e) {

    console.log("In the Left method");
    var r_Box = document.getElementById("rContainer");
    var pos = $(r_Box).scrollLeft();

    $(r_Box).animate({ scrollLeft: pos - 100 }, 400)
});
$(document).ready(function () {
    //$('.panel-heading span.clickable').click();
   
    $('.panel div.clickable').click();
    $("#replyParent").click();
    $("slide_box").click();
    $("#sRight").click();
    $("#sLeft").click();
   
  
   

});