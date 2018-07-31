var map, places, infoWindow;
var markers = [];
var autocomplete;
var autoOri;
var autoDest;
var countryRestrict = { 'country': 'us' };
var MARKER_PATH = 'https://developers.google.com/maps/documentation/javascript/images/marker_green';
var hostnameRegexp = new RegExp('^https?://.+?/');
var isCaptchaValid = false;
var CaptchaResonponse = null;
var theReplies = {};
var buttonenable = "disable";
var signedin = false;

var countries = {
    'au': { center: { lat: -25.3, lng: 133.8 }, zoom: 4 },
    'br': { center: { lat: -14.2, lng: -51.9 }, zoom: 3 },
    'ca': { center: { lat: 62, lng: -110.0 }, zoom: 3 },
    'fr': { center: { lat: 46.2, lng: 2.2 }, zoom: 5 },
    'de': { center: { lat: 51.2, lng: 10.4 }, zoom: 5 },
    'mx': { center: { lat: 23.6, lng: -102.5 }, zoom: 4 },
    'nz': { center: { lat: -40.9, lng: 174.9 }, zoom: 5 },
    'it': { center: { lat: 41.9, lng: 12.6 }, zoom: 5 },
    'za': { center: { lat: -30.6, lng: 22.9 }, zoom: 5 },
    'es': { center: { lat: 40.5, lng: -3.7 }, zoom: 5 },
    'pt': { center: { lat: 39.4, lng: -8.2 }, zoom: 6 },
    'us': { center: { lat: 37.1, lng: -95.7 }, zoom: 3 },
    'uk': { center: { lat: 54.8, lng: -4.6 }, zoom: 5 }
};
function initMap() {
    map = new google.maps.Map(document.getElementById('map'),
        {
            zoom: countries['us'].zoom, center: countries['us'].center,
            mapTypeControl: false, panControl: false, zoomControl: false, streetViewControl: false
        }); infoWindow = new google.maps.InfoWindow({ content: document.getElementById('info-content') });
    // Create the autocomplete object and associate it with the UI input control. 
    // Restrict the search to the default country, and to place type "cities". 
    autocomplete = new google.maps.places.Autocomplete( /** @type {!HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'], componentRestrictions: countryRestrict });

    autoOri = new google.maps.places.Autocomplete( /** @type {!HTMLInputElement} */(document.getElementById('origin')),
        { types: ['geocode'], componentRestrictions: countryRestrict });
    autoDest = new google.maps.places.Autocomplete( /** @type {!HTMLInputElement} */(document.getElementById('destination')),
        { types: ['geocode'], componentRestrictions: countryRestrict });

    places = new google.maps.places.PlacesService(map);


    var dService = new google.maps.DirectionsService;
    var dDisplay = new google.maps.DirectionsRenderer;
    dDisplay.setPanel(document.getElementById('right-panel'));


    dDisplay.setMap(map);
    autocomplete.addListener('place_changed', onPlaceChanged); // Add a DOM event listener to react when the user selects a country. 
    document.getElementById('country').addEventListener('change', setAutocompleteCountry);
    document.getElementById("findMe").addEventListener('click', function () { getDirections(dService, dDisplay) });//Adds a listener for Directions Object

  

    
}
// When the user selects a city, get the place details for the city and // zoom the map in on the city. 
function onPlaceChanged() {
    var place = autocomplete.getPlace();
    if (place.geometry) {
        map.panTo(place.geometry.location);
        map.setZoom(15);
        search();
    } else { document.getElementById('autocomplete').placeholder = 'Enter a city'; }
}

// Search for hotels in the selected city, within the viewport of the map. 
function search() {
    var search = {
        bounds: map.getBounds(),
        types: ['lodging']
    };
    places.nearbySearch(search, function (results, status) {
        if (status === google.maps.places.PlacesServiceStatus.OK) {

            clearResults();
            clearMarkers();
            clearReviews();

            // Create a marker for each hotel found, and // assign a letter of the alphabetic to each marker icon.
            for (var i = 0; i < results.length; i++) {
                var markerLetter = String.fromCharCode('A'.charCodeAt(0) + (i % 26));
                var markerIcon = MARKER_PATH + markerLetter + '.png'; // Use marker animation to drop the icons incrementally on the map.
                markers[i] = new google.maps.Marker({ position: results[i].geometry.location, animation: google.maps.Animation.DROP, icon: markerIcon });
                // If the user clicks a hotel marker, show the details of that hotel // in an info 
                window.markers[i].placeResult = results[i];
                google.maps.event.addListener(markers[i], 'click', showInfoWindow);
                //google.maps.event.addListener(markers[i], 'click', slideRight);




                google.maps.event.addListener(markers[i], 'click', showReviewTable);
                //google.maps.event.addListener(markers[i], 'click', getDirections);
                setTimeout(dropMarker(i), i * 100);
                //markers[i].addListener('click', createReviewTable(results[i]));

                addResult(results[i], i);


            }
            //createReviewTable();


        }
    });
}

//Find direction for origin to destination for Driving
function getDirections(service, renderer) {

    var origin = document.getElementById("origin");
    var destination = document.getElementById("destination");
    console.log("from get Direction")
    var marker = this;
    service.route({
        origin: origin.value,
        destination: destination.value,
        travelMode: 'DRIVING'
    }, function (response, status) {
        if (status = 'OK') {
            //Visually show the route on the map
            renderer.setDirections(response);
        } else {
            window.alert("Direction failed due to " + status);
        }
    }

        )
}


 





//Clear markers on the map
function clearMarkers() {
    for (var i = 0; i < markers.length; i++) {
        if (markers[i]) {
            markers[i].setMap(null);
        }
    }
    markers = [];
} // Set the country restriction based on user input. // Also center and zoom the map on the given country. 
function setAutocompleteCountry() {
    var country = document.getElementById('country').value;
    if (country == 'all') {
        autocomplete.setComponentRestrictions([]);
        map.setCenter({ lat: 15, lng: 0 });
        map.setZoom(2);
    } else {
        autocomplete.setComponentRestrictions({ 'country': country });
        map.setCenter(countries[country].center);
        map.setZoom(countries[country].zoom);
    }
    clearResults();
    clearMarkers();


} function dropMarker(i) {
    return function () {
        markers[i].setMap(map);
    };
} function addResult(result, i) {
    //The Results Table

    var results = document.getElementById('results');
   ///Get Marker Letter icon
    var markerLetter = String.fromCharCode('A'.charCodeAt(0) + (i % 26));
    var markerIcon = MARKER_PATH + markerLetter + '.png';

    //Set the color of the current Table Row to grey or white(alternating)
    var tr = document.createElement('tr'); tr.style.backgroundColor = (i % 2 === 0 ? '#F0F0F0' : '#FFFFFF');
    tr.onclick = function () {
        google.maps.event.trigger(markers[i], 'click');


    };




    var iconTd = document.createElement('td');
    var nameTd = document.createElement('td');
    var icon = document.createElement('img');



    icon.src = markerIcon;
    icon.setAttribute('class', 'placeIcon');
    icon.setAttribute('className', 'placeIcon');

    //console.log("The location of the clicked place" + result.vicinity);

    var name = document.createTextNode(result.name);

    //searchData(tr,result);

    iconTd.appendChild(icon);
    nameTd.appendChild(name);
    tr.appendChild(iconTd);
    tr.appendChild(nameTd);



    results.appendChild(tr);


} function clearResults() {
    var results = document.getElementById('results');
    while (results.childNodes[0]) {
        results.removeChild(results.childNodes[0]);
    }
}

function clearReviews() {

    var reviews = document.getElementById('reviewTable');
    while (reviews.childNodes[0]) {
        reviews.removeChild(reviews.childNodes[0]);
    }
}


//Creates the Review Table
function createReviewTable(result) {

   
    var slideTitle = document.getElementById("Title");
    slideTitle.innerHTML = "";

    var header = document.createElement("h1");
    header.innerHTML = result.name;
    slideTitle.appendChild(header);

    var slideAddress = document.getElementById("Address");
    slideAddress.innerHTML = "";

    slideAddress.innerHTML = result.vicinity;

    console.log("Review Table started ");


    clearReviews();
    var reviews = document.getElementById('reviewTable');

    var reviewtr = document.createElement('tr');
    var reviewtr2 = document.createElement('tr');
    //reviewtr.style.backgroundColor = (i % 2 === 0 ? '#F0F0F0' : '#FFFFFF');
    var reviewtd = document.createElement('td');
    reviewtd.style.overflow = "scroll";
    var reviewtd2 = document.createElement('td');
    var reviewtd3 = document.createElement('td');

    var form = document.createElement('form');
    form.id = "form-parent";

    var thumbsup = document.createElement("icon");
    thumbsup.className = "glyphicon glyphicon-thumbs-up"

    var thumbsdown = document.createElement("icon");
    thumbsdown.className = "glyphicon glyphicon-thumbs-down"

    var formText = document.createElement('div');
    formText.className = "form-group";

    var formCaptcha = document.createElement('div');
    formCaptcha.className = "form-group";


    var addReviewContainer = document.createElement("div");
    var addReviewText = document.getElementById("createReview");



    
    $(addReviewText).on('click', function () { $("#dialog-form").dialog("open") })
    
    

    var rcontents = document.createElement('a');
    rcontents.id = "NLogged"
    rcontents.href = "Account/Login"
    rcontents.innerHTML = "Login to add a review";

    //var rCaptcha = document.createElement("div");
    //rCaptcha.id = "mycaptcha";
    //formCaptcha.appendChild(rCaptcha);

    



    var rButton = document.createElement('button');

    rButton.id = "sButton";
    rButton.disabled = true;
    rButton.innerText = "Submit";

    var reviewArea = document.getElementById("reviewArea");
    

    jQuery(function ($) {

        $(rButton).on('click', function () {
            console.log("Sending Review");
            var now = makeDate();
            $.ajax({
                url: "NewReviews/makeUserReview",
                type: "POST",

                data:
                    {
                        foodName: result.name,
                        Address: result.vicinity,
                        PicturesPath: "PlaceHolder",
                        Rating: result.rating,
                        content: rTextArea.value,
                        date: now,
                        like: $('input[name="myLikes"]:checked').val(),
                        captcha:CaptchaResonponse

                    },


            })
        })

        $(".ui-dialog-buttonpane button:contains('Add Review')").on('click', function () {
            console.log("Hi from ui button");
            var now = makeDate();
            $.ajax({
                url: "NewReviews/makeUserReview",
                type: "POST",

                data:
                    {
                        foodName: result.name,
                        Address: result.vicinity,
                        PicturesPath: "PlaceHolder",
                        Rating: result.rating,
                        content: reviewArea.value,
                        date: now,
                        like: $('input[name="myLikes"]:checked').val(),
                        captcha: CaptchaResonponse

                    },


            })

            $("#dialog-form").dialog("close");
        })


        $.ajax({
            url: "NewReviews/getReviews2",
            type: "POST",
            data: {
                foodName: result.name,
                Address: result.vicinity,
                PicturesPath: "PlaceHolder",
                Rating: result.rating
            },
            success: function (response) {
                var item = doSomething(response);

                var restartdiv = document.getElementById("rContainer");
                restartdiv.innerHTML = "";

                for (var d = 0; d < item.length; d++) {

                    var divParent = makePanel(item[d]);
                    var linebreak = document.createElement("br");
                   

                    fillsidePanel(item[d]);
                    reviewtd.appendChild(divParent);
                    reviewtd.appendChild(linebreak);

                }

                console.log("In the success function");
                
                slideRight();

                var rHead = document.getElementById("rHead");
                

            }


        })



    })



    console.log("Appending text area and button");




    //reviewtd3.appendChild(rcontents);

    console.log(document.getElementById("signedin"));
    if (document.getElementById("signedin") == null) {




        console.log("Not signed in");
        reviewtd3.appendChild(rcontents);

    }
    else {

        console.log("Signed in ");
        var reviewPanel = document.getElementById("pReviews");

    

        reviewtd3.appendChild(rButton);
        //reviewtd3.appendChild(rButton);

    }
    //reviewtd3.appendChild(rTextArea);
    //reviewtd3.appendChild(rButton);

    console.log("Appending the Review to row");
    reviewtr.appendChild(reviewtd);
    //reviewtr.appendChild(reviewtd2);

    reviewtr2.appendChild(reviewtd3);


    console.log("Final Append");
    reviews.appendChild(reviewtr);
    reviews.appendChild(reviewtr2);


    console.log("Review created");
  




}

var onloadCallback = function () {
    var check = false;

    var captcha2 = document.getElementById("mycaptcha");
    grecaptcha.render(captcha2, {
        'sitekey': '6LcX1RgUAAAAABopjVl_yykutrRaupwD-7TjpS_3',
        'callback': correctCaptcha

    });

};

function doSomething(x) {
    var items = x;
    for (var i = 0; i < items.length; i++) {
        console.log(items[i].rContents + " " + items[i].foodName + " " + items[i].username);

    }
    return items;



}

function sendReply(reviewBody, review) {

    var replyParent = document.createElement("div");
    var replyText = document.createElement("textarea");
    var replyButton = document.createElement("button");
    replyButton.innerText = "Reply";


    jQuery(function ($) {

        $(replyButton).on('click', function () {
            console.log("In the query");
            $.ajax({
                url: "ReviewReplies/makeReply",
                type: "POST",
                data: {
                    contents: replyText.value,
                    likes: true,
                    reviewId: review.rId

                }

            })
           
        })

 

       


    })


    replyParent.appendChild(replyText);
    replyParent.appendChild(replyButton);

    return replyParent;


}


function fillsidePanel(response) {
    var rContainer = document.getElementById("rContainer");
  
    var rBlock = document.createElement("div");
    rBlock.className = "rBlock";
    rBlock.id = "rBlock";

    var sideuser = document.createElement("p");
    var sReview = document.createElement("p");
   
    sideuser.style.backgroundColor = "lavender";
    sideuser.innerHTML = response.username;
    sReview.innerHTML = response.rContents;


    
   
 
    
    rBlock.appendChild(sideuser);
    rBlock.appendChild(sReview);
    
    if (response.rpList != null) {

        var p = document.createElement("p");
        p.style.color = "blue";
        p.style.cursor = "pointer";
        p.innerHTML = "Click to see all replies";

        
        var newKey = response.rId + "//" +response.username +"//"+ response.rContents;
        theReplies[newKey] = response.rpList;
        p.addEventListener("click", function () { getReplies(newKey) });
       

        rBlock.appendChild(p);
    }
    else {
        var addReplyTxt;
        var signedin = document.getElementById("signedin");
       
        if (signedin != null) {

            var reparea = document.getElementById("replyArea");
            
            addReplyTxt = document.createElement("p");
            addReplyTxt.innerHTML = "Be the first to add a reply";
            $(addReplyTxt).on("click", function () { $("#reply-dialog").dialog("open"); })

            $(".ui-dialog-buttonpane button:contains('Add Reply')").on('click', function () {
                console.log("In the query");
                $.ajax({
                    url: "ReviewReplies/makeReply",
                    type: "POST",
                    data: {
                        contents: reparea.value,
                        likes: true,
                        reviewId: response.rId

                    }

                })
            })


        }
        else {

            addReplyTxt = document.createElement("a");
            addReplyTxt.href = "Account/Login";
            addReplyTxt.innerHTML = "Log in to add a reply";
            
        }
       

        rBlock.appendChild(addReplyTxt);
    }
    rContainer.appendChild(rBlock);

    
}

function getReplies(replies) {


    var expand = document.getElementById("rExpand");
    var header = document.getElementById("rHead");
    var container = document.getElementById("rContainer");
    
    header.style.visibility = "hidden";
    container.style.overflow = null;
    container.style.overflowY = null;
    container.style.visibility = "hidden";
    
   
    $(".rExpand").toggleClass("rExpanded");
   

    
    
    console.log("Identifier " + replies);
    var splits = replies.split("//");
    
    var rId = splits[0];
    var rUser = splits[1];
    var mainContent = splits[2];
    var reparea = document.getElementById("replyArea");

    var fullReviewContainer = document.createElement("div");
    fullReviewContainer.id = "review_container";
    var userp = document.createElement("p");
    userp.innerHTML = rUser;
    userp.style.backgroundColor = "orangered";
    var contentp = document.createElement("p");
    contentp.innerHTML = mainContent;
    var linebreak2 = document.createElement("br");
    linebreak2.style.borderBottom = "solid";
   
    fullReviewContainer.appendChild(userp);
    fullReviewContainer.appendChild(contentp);
    fullReviewContainer.appendChild(linebreak2);

    for (var index = 0; index < theReplies[replies].length; index++) {
        var replydiv = document.createElement("div");
        var rpUser = document.createElement("p");
        var rpContent = document.createElement("p");

        rpUser.innerHTML = theReplies[replies][index].user;
        rpUser.style.backgroundColor = "lavender";
        rpContent.innerHTML = theReplies[replies][index].rpContents;

        replydiv.appendChild(rpUser);
        replydiv.appendChild(rpContent);
        
        fullReviewContainer.appendChild(replydiv);
        
        
     
    }

    
    var addReplyLink = document.getElementById("replyLink");
    $(addReplyLink).on("click", function () { $("#reply-dialog").dialog("open"); })


    $(".ui-dialog-buttonpane button:contains('Add Reply')").on('click', function () {
        console.log("In the query");
        $.ajax({
            url: "ReviewReplies/makeReply",
            type: "POST",
            data: {
                contents: reparea.value,
                likes: true,
                reviewId: rId

            }

        })
    })

    var miniLink = document.createElement("p");
    miniLink.innerHTML = "Go Back";

    miniLink.addEventListener('click', function () { hideReviews() });

    
    fullReviewContainer.appendChild(miniLink);
    expand.appendChild(fullReviewContainer);
    
    console.log("First" + theReplies[replies][0].rpContents);
    //alert("I was clicked");
}

function hideReviews() {
    
    var reviewPanel = document.getElementById("review_container");
    var rHeader = document.getElementById("rHead");
    var reviewContainer = document.getElementById("rContainer");

    rHeader.style.visibility = "visible";
    reviewContainer.style.visibility = "visible";
    
    $(".rExpanded").empty();
    $(".rExpanded").removeClass("rExpanded");

}

function slideRight() {

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


        //s_Box.style.transitionDuration = "2s";
        //s_Box.style.transform = "translateX(1px)";

        //i_Box.style.transform = "translateX(-300px)";
        //i_Box.style.transitionDuration = "2s";
        arrow.className = "glyphicon glyphicon-chevron-right"
    }
   
}



function makePanel(response) {

        var divParent = document.createElement("div");
        divParent.id = "card-parent";
        divParent.className = "panel panel-default";

        var divHeader = document.createElement("div");

        var spanLeft = document.createElement("span");
        var tImg = document.createElement("i");
        var spanRight = document.createElement("span");
        var minus = document.createElement("i");

        divHeader.className = "panel-heading clickable";
        divHeader.innerHTML = response.username;
        


        spanRight.className = "pull-right";
        minus.className = "glyphicon glyphicon-minus";
        spanLeft.className = "pull-left";
 

        var tCheck;
        if (response.rLike == true) {
            tCheck = "glyphicon glyphicon-thumbs-up";
        }

        else {
            tCheck = "glyphicon glyphicon-thumbs-down";
        }

        tImg.className = tCheck;
        var divBody = document.createElement("div");
        divBody.className = "panel-body";


        var bodyContents = document.createElement("p");
        bodyContents.innerHTML = response.rContents + "<br/><br/>" + response.rDate;

        if (response.rpList != null) {
            var rplParent = document.createElement("div");

            rplParent.className = "panel panel-default";

            rplParent.id = "replyHead";

            var rplHeader = document.createElement("div");
            rplHeader.className = "panel-heading panel-collapsed";

            var $rplHeader = $(rplHeader);


            rplHeader.id = "replyPanel"


            var rplus = document.createElement("i");
            rplus.className = "glyphicon glyphicon-plus";
            var rpRight = document.createElement("span")
            rpRight.className = "pull-right";

            rplHeader.innerHTML = "Replies(" + response.rpList.length + ")";

            rpRight.appendChild(rplus);
            rplHeader.appendChild(rpRight);

            rplParent.appendChild(rplHeader);

            var rplBody = document.createElement("div");
            rplBody.className = "panel-body";
            rplBody.style.display = "none";




            for (var index = 0; index < response.rpList.length; index++) {
                var rpbodyContents = document.createElement("p");

                rpbodyContents.innerHTML = response.rpList[index].rpContents + "<br/><br/>" + response.rpList[index].user;

                rplBody.appendChild(rpbodyContents);
            }



            rplParent.appendChild(rplBody);

            bodyContents.appendChild(rplParent)
        }





        var userReply = sendReply(bodyContents, response);
    
        bodyContents.appendChild(userReply);

        console.log(response.rDate);



        spanLeft.appendChild(tImg);
        spanRight.appendChild(minus);
        divHeader.appendChild(spanLeft);
        divHeader.appendChild(spanRight);
        divBody.appendChild(bodyContents);

        divParent.appendChild(divHeader);
        divParent.appendChild(divBody);

        return divParent;

    }



    //Search the SQL database
    function searchData(tr, result) {
        console.log("In the search method");



        jQuery(function ($) {
            console.log("In the jquery");
            var newFoodTable = {
                foodName: result.name,
                Address: result.vicinity,
                PicturesPath: "Placeholder",
                Rating: result.rating
            }
            $(tr).on('click', function () {
                console.log("In the click event");
                createReviewTable(result);

            });
        });
    }



    // Get the place details for a hotel. Show the information in an info window, 
    // anchored on the marker for the hotel that the user selected. 
    function showInfoWindow() {
        var marker = this;
        places.getDetails({ placeId: marker.placeResult.place_id },
            function (place, status) {
                if (status !== google.maps.places.PlacesServiceStatus.OK) { return; }
                infoWindow.open(map, marker);
                buildIWContent(place);
            });
    }

    function makeDate() {
        var date = new Date();
        var day = date.getDate();        // yields day
        var month = date.getMonth() + 1;    // yields month
        var year = date.getFullYear();  // yields year
        var hour = date.getHours();     // yields hours 
        var minute = date.getMinutes(); // yields minutes
        var second = date.getSeconds(); // yields seconds

        var time = month + "/" + day + "/" + year; //+ " " + hour + ':' + minute + ':' + second;

        console.log(time);

        return time
    }
    function showReviewTable() {

        var marker = this;
        places.getDetails({ placeId: marker.placeResult.place_id },
              function (place, status) {
                  if (status !== google.maps.places.PlacesServiceStatus.OK) { return; }
                  createReviewTable(place);
              });
    }

    function getPlace(){
        var marker = this;
        places.getDetails({ placeId: marker.placeResult.place_id },
              function (place, status) {
                  if (status !== google.maps.places.PlacesServiceStatus.OK) { return; }
                  addReview(place);
              });

    }

    function addReview(result) {

        var reviewArea = document.getElementById("reviewArea");
        console.log("Sending Review");
        var now = makeDate();
        $.ajax({
            url: "NewReviews/makeUserReview",
            type: "POST",

            data:
                {
                    foodName: result.name,
                    Address: result.vicinity,
                    PicturesPath: "PlaceHolder",
                    Rating: result.rating,
                    content: reviewArea.value,
                    date: now,
                    like: $('input[name="myLikes"]:checked').val(),
                    captcha:CaptchaResonponse

                },


        })
    }





    var correctCaptcha = function (response) {

        console.log("In the correct captcha");
        isCaptchaValid = true;
        buttonenable = "enable";
        console.log(isCaptchaValid);

        //var submitButton = document.getElementById("subButton");
        //submitButton.disabled = false;

        $(".ui-dialog-buttonpane button:contains('Add Review')").button("enable");
        CaptchaResonponse = response;
        return response;
    };
    // Load the place information into the HTML elements used by the info window. 
    function buildIWContent(place) {
        document.getElementById('iw-icon').innerHTML = '<img class="hotelIcon" ' + 'src="' + place.icon + '"/>';
        document.getElementById('iw-url').innerHTML = '<b><a href="' + place.url + '">' + place.name + '</a></b>';
        document.getElementById('iw-address').textContent = place.vicinity;
        if (place.formatted_phone_number) {

            document.getElementById('iw-phone-row').style.display = '';
            document.getElementById('iw-phone').textContent = place.formatted_phone_number;

        } else {
            document.getElementById('iw-phone-row').style.display = 'none';
        }
        // Assign a five-star rating to the hotel, using a black star ('&#10029;') 
        // to indicate the rating the hotel has earned, and a white star ('&#10025;') // for the rating points not achieved. 
        if (place.rating) {
            var ratingHtml = '';
            for (var i = 0; i < 5; i++) {
                if (place.rating < (i + 0.5)) {

                    ratingHtml += '&#10025;';
                } else {
                    ratingHtml += '&#10029;';
                }
                document.getElementById('iw-rating-row').style.display = '';
                document.getElementById('iw-rating').innerHTML = ratingHtml;
            }
        } else {
            document.getElementById('iw-rating-row').style.display = 'none';
        } // The regexp isolates the first part of the URL (domain plus subdomain) 
        // to give a short URL for displaying in the info window. 
        if (place.website) {
            var fullUrl = place.website;
            var website = hostnameRegexp.exec(place.website);
            if (website === null) {
                website = 'http://' + place.website + '/'; fullUrl = website;
            }
            document.getElementById('iw-website-row').style.display = '';
            document.getElementById('iw-website').textContent = website;
        } else {
            document.getElementById('iw-website-row').style.display = 'none';
        }
    }