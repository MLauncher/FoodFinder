$(function () {
    var dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 400,
        width: 400,
        title:"Create Review",
        modal: true,
        position: { my: "center", at: "center", within:"#map"},
        buttons: {
            "Add Review": function () { },
            Cancel: function () {
                dialog.dialog("close");
            }
        },
        close: function () {
            form[0].reset();

        }
    });


    var form = dialog.find("form").on("submit", function (event) {
        event.preventDefault();
        
        
       
    });

    var repdialog = $("#reply-dialog").dialog({
        autoOpen: false,
        height: 400,
        width: 400,
        title: "Create Reply",
        modal: true,
        position: { my: "center", at: "center", within: "#map" },
        buttons: {
            "Add Reply": function () { },
            Cancel: function () {
                repdialog.dialog("close");
            }
        }, close: function () {
            form[0].reset();
        }

});
    $(".ui-dialog-buttonpane button:contains('Add Review')").button("disable");
    
  
    $("#createReview").on("click", function () { dialog.dialog("open"); });
    $("#addReplyLink").on("click", function () { repdialog.dialog("open"); });

   


});
