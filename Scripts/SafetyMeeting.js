//function PostWatchStart() {
//    var params = '{movieId:"Live Meeting", pos:"0", dur:"0"}';

//    // Make Async Call To Server Showing We Are Looking At A Movie
//    jQuery.ajax(
//            {
//                type: "POST",
//                url: "/Safety/SafetyMeeting.aspx/MovieStart",
//                data: params,
//                contentType: "application/json",
//                dataType: "json"
//            }
//        );
//}



//function PostWatchComplete() {
//    // Create A Parameter To Send To Server
//    var params = '{movieId:"Live Meeting"}';

//    // Make Async Call To Server SHowing We Are Looking At A Movie
//    jQuery.ajax(
//            {
//                type: "POST",
//                url: "/Safety/SafetyMeeting.aspx/MovieComplete",
//                data: params,
//                contentType: "application/json",
//                dataType: "json"
//            }
//        );
//}





//$(document).ready(function () {
//    PostWatchStart();
//});

//$(window).unload(function () {
//    PostWatchComplete();
//});